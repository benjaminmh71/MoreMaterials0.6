using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SettingScripts;
using ManagementScripts;
using MoreMaterials;
using UnityEngine;
using System.IO;
using System.Reflection;


namespace MoreMaterials
{
    [HarmonyPatch(typeof(ProceduralSpriteManager))]
    internal static class ProceduralSpriteManagerPatch
    {
        [HarmonyPatch("RequestPelletSpriteOfMaterial")]
        [HarmonyPrefix]
        private static bool Prefix(ProceduralSpriteManager __instance, MatterMaterial material, int sizeIndex
            , ref Sprite __result)
        {
            MethodInfo tryLoadSprite = typeof(ProceduralSpriteManager).GetMethod("TryLoadSprite", BindingFlags.NonPublic | BindingFlags.Static);
            List<SizeFormat> pelletSizes = Traverse.Create(__instance).Field("pelletSizes").GetValue() as List<SizeFormat>;
            Vector2 centerPivot = (Vector2)Traverse.Create(__instance).Field("centerPivot").GetValue();

            if (material == MatterMaterialManager.FindMaterial("Root"))
            {
                float num2 = (float)pelletSizes[sizeIndex].pixelWidth / 5f;
                __result = tryLoadSprite.Invoke(__instance, new object[] {
                    Application.dataPath + "/Mods/", pelletSizes[sizeIndex].name + "_root", centerPivot, num2, true }) as Sprite;
                return false;
            }
            if (material == MatterMaterialManager.FindMaterial("Fruit"))
            {
                float num2 = (float)pelletSizes[sizeIndex].pixelWidth / 5f;
                __result = tryLoadSprite.Invoke(__instance, new object[] {
                    Application.dataPath + "/Mods/", pelletSizes[sizeIndex].name + "_fruit", centerPivot, num2, true }) as Sprite;
                return false;
            }
            if (material == MatterMaterialManager.FindMaterial("Fungus"))
            {
                float num2 = (float)pelletSizes[sizeIndex].pixelWidth / 5f;
                __result = tryLoadSprite.Invoke(__instance, new object[] {
                    Application.dataPath + "/Mods/", pelletSizes[sizeIndex].name + "_fungus", centerPivot, num2, true }) as Sprite;
                return false;
            }

            return true;
        }
    }
}