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


namespace MoreMaterials
{
    [HarmonyPatch(typeof(BibiteStomach))]
    internal static class BibiteStomachPatch
    {
        [HarmonyPatch("AffinityToMaterial")]
        [HarmonyPrefix]
        private static bool Prefix(BibiteStomach __instance, MatterMaterial mat, ref float __result)
        {
            float diet = (float)Traverse.Create(__instance)
                    .Field("diet").GetValue();

            if (mat == MatterMaterialManager.FindMaterial("Root"))
            {
                __result = 1f - Math.Abs(4f / 3f * diet - 1f/3f);
                return false;
            }
            if (mat == MatterMaterialManager.FindMaterial("Fruit"))
            {
                __result = 1f - Math.Abs(2f * diet - 1f);
                return false;
            }
            if (mat == MatterMaterialManager.FindMaterial("Fungus"))
            {
                __result = 1f - Math.Abs(4f/3f * diet - 1f); ;
                return false;
            }

            return true;
        }
    }
}