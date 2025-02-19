using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SimulationScripts.BibiteScripts;
using SettingScripts;
using MoreMaterials;
using ManagementScripts;
using UnityEngine;
using System.IO;
using System;
using ScriptHelpers;
using System.Reflection;


namespace MoreMaterials
{
	[HarmonyPatch(typeof(BrainUpdater))]
	internal static class BrainUpdaterPatch
	{
		/*[HarmonyPatch("Awake")]
		[HarmonyPrefix]
		private static void awakePrefix(BrainUpdater __instance)
		{
			
		}*/
	}
}