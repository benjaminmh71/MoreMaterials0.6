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