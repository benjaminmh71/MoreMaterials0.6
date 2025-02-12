using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using UIScripts.SettingHandles;
using SimulationScripts;
using SettingScripts;
using MoreMaterials;
using UnityEngine;
using System.IO;


namespace MoreMaterials
{
	[HarmonyPatch(typeof(MenuDynamicSettingsManager))]
	internal static class MenuDynamicSettingsManagerPatch
	{
		[HarmonyPatch("Awake")]
		[HarmonyPrefix]
		private static void Prefix(MenuDynamicSettingsManager __instance)
		{
			SettingsGroupHandle materialParameters = Traverse.Create(__instance)
					.Field("MaterialParameters").GetValue() as SettingsGroupHandle;
			if (materialParameters.Settings.Count < 9)
			{
				materialParameters.Settings.Add(new MatterMaterialSettingsHandle(MatterMaterialManager.SettingsOfMaterial(
					MatterMaterialManager.FindMaterial("Root"))));
				materialParameters.Settings.Add(new MatterMaterialSettingsHandle(MatterMaterialManager.SettingsOfMaterial(
					MatterMaterialManager.FindMaterial("Fruit"))));
				materialParameters.Settings.Add(new MatterMaterialSettingsHandle(MatterMaterialManager.SettingsOfMaterial(
					MatterMaterialManager.FindMaterial("Fungus"))));
			}
		}
	}
}