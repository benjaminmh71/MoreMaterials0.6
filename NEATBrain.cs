using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SimulationScripts.BibiteScripts;
using SettingScripts;
using MoreMaterials;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using Mono.Cecil.Cil;
using System.Linq;
using UIScripts;
using UIScripts.UIReferences;
using Utility;
using ScriptHelpers;

namespace MoreMaterials
{
    [HarmonyPatch(typeof(NEATBrain))]
    internal static class NEATBrainPatch
    {
        [HarmonyPatch("SetBaseNeuronsTypeAndDesc", new Type[] { typeof(NEATBrain.Node[]) })]
        [HarmonyPostfix]
        private static void setBasePostfix(NEATBrain.Node[] nodes)
        {
            for (int i = nInputs - 9; i < nInputs; i++)
            {
                nodes[i].Desc = inputNames[i - nInputs + 9];
            }
        }

        [HarmonyPatch("StartUsedSystems")]
        [HarmonyPrefix]
        private static bool startSeeingPrefix(NEATBrain __instance)
        {
            if (__instance.phero != null && __instance.Nodes[23].NOut + __instance.Nodes[24].NOut +
                __instance.Nodes[25].NOut + __instance.Nodes[26].NOut + __instance.Nodes[27].NOut +
                __instance.Nodes[28].NOut + __instance.Nodes[29].NOut + __instance.Nodes[30].NOut + __instance.Nodes[31].NOut > 0)
            {
                __instance.phero.StartPherosensing();
            }
            if (__instance.fow == null)
            {
                return false;
            }
            int num = 0;
            num += (((NEATBrain.herdingEnabled ? __instance.Nodes[NEATBrain.NInputs + 2].NIn : 0) +
                __instance.Nodes[10].NOut + __instance.Nodes[9].NOut + __instance.Nodes[8].NOut + __instance.Nodes[17].NOut +
                __instance.Nodes[18].NOut + __instance.Nodes[19].NOut > 0) ? 1 : 0);
            num += ((__instance.Nodes[13].NOut + __instance.Nodes[12].NOut + __instance.Nodes[11].NOut
                + __instance.Nodes[32].NOut + __instance.Nodes[33].NOut + __instance.Nodes[34].NOut
                + __instance.Nodes[35].NOut + __instance.Nodes[36].NOut + __instance.Nodes[37].NOut
                + __instance.Nodes[38].NOut + __instance.Nodes[39].NOut + __instance.Nodes[40].NOut > 0) ? 2 : 0);
            num += ((__instance.Nodes[16].NOut + __instance.Nodes[15].NOut + __instance.Nodes[14].NOut > 0) ? 4 : 0);
            __instance.fow.StartSeeing(num);

            return false;
        }

        public static int nInputs = 41;

        public static String[] inputNames =
        {
            "RootCloseness",
            "RootAngle",
            "NRoot",
            "FruitCloseness",
            "FruitAngle",
            "NFruit",
            "FungusCloseness",
            "FungusAngle",
            "NFungus"
        };
    }

    [HarmonyPatch(typeof(NEATBrain))]
    internal static class NInputPatch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var result = new List<MethodBase>();
            result.Add(AccessTools.Method(typeof(NEATBrain), "AddSynapse"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "ComputeActiveNeurons"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "CopyBrain"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "FinalizeEditing"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "FinishRemovalProcess"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "GetHiddenIndex"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "GetInputIndex"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "GetOutputIndex"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "GetOutputIndex"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "GetRandomNodeIndex"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "GetRandomNonInput"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "Output"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "RemoveNeuron"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "RemoveSynapse"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "ResumeBrain"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "SetBaseNeuronsTypeAndDesc", new Type[] { typeof(NEATBrain.Node[]) }));
            result.Add(AccessTools.Method(typeof(NEATBrain), "StartUsedSystems"));
            result.Add(AccessTools.Method(typeof(NEATBrain), "UpdateSenses"));
            result.Add(AccessTools.Method(typeof(BrainSorter), "MakeGroups"));
            result.Add(AccessTools.Method(typeof(BrainUpdater), "UpdateElementsFromVersion"));
            result.Add(AccessTools.Method(typeof(BrainPanelCommon), "FillBrainPanel"));
            result.Add(AccessTools.Method(typeof(BrainPanelCommon), "PlaceNodesAccordingToAnchors"));
            result.Add(AccessTools.Method(typeof(ExpandedBrainPanel), "OpenPanel"));
            result.Add(AccessTools.Method(typeof(SpeciesBrainPanel), "SaveNodePositions"));
            result.Add(AccessTools.Method(typeof(UINeuron), "SetDesc"));
            result.Add(AccessTools.Method(typeof(BibiteTemplateGenePreviewer), "UpdateTemplate"));
            result.Add(AccessTools.Method(typeof(BibiteTemplate), "AlignInnovationsWithSim"));

            return result;
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].ToString() == "ldsfld int SimulationScripts.BibiteScripts.NEATBrain::NInputs")
                {
                    codes[i].opcode = System.Reflection.Emit.OpCodes.Ldc_I4;
                    codes[i].operand = NEATBrainPatch.nInputs;
                }
            }

            return codes.AsEnumerable();
        }
    }
}