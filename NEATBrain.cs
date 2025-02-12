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
		[HarmonyPatch("Awake")]
		[HarmonyPrefix]
		private static void awakePrefix(NEATBrain __instance)
		{
			
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