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


namespace MoreMaterials
{
	[HarmonyPatch(typeof(NEATBrain))]
	internal static class NEATBrainPatch
	{
        [HarmonyPatch("ComputeActiveNeurons")]
		[HarmonyPrefix]
		private static bool computePrefix(NEATBrain __instance)
		{
            for (int i = 0; i < NEATBrain.NInputs + NEATBrain.NOutputs + __instance.nHidden; i++)
            {
                Plugin.Log.LogInfo(__instance.Nodes[i].Desc);
                __instance.Nodes[i].NOut = 0;
            }
        }

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

		private static String[] inputNames =
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
}