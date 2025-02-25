using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SimulationScripts.BibiteScripts;
using SettingScripts;
using UIScripts;
using MoreMaterials;
using ManagementScripts;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using ScriptHelpers;
using System.Reflection;


namespace MoreMaterials
{
    [HarmonyPatch(typeof(BrainIconHolder))]
    internal static class BrainIconHolderPatch
    {
        [HarmonyPatch("GetIconOfIndex")]
        [HarmonyPrefix]
        private static bool getIconPrefix(BrainIconHolder __instance, int i, ref Sprite __result)
        {
            if (i < NEATBrainPatch.nInputs && i >= NEATBrainPatch.nInputs-9)
            {
                byte[] imageBytes = File.ReadAllBytes(UnityEngine.Application.dataPath +
                    "/Mods/" + NEATBrainPatch.inputNames[i - NEATBrainPatch.nInputs + 9] + ".png");
                Texture2D imageTexture = new Texture2D(2, 2);
                imageTexture.filterMode = FilterMode.Point;
                ImageConversion.LoadImage(imageTexture, imageBytes);
                __result = Sprite.Create(imageTexture, new Rect(0f, 0f, (float)imageTexture.width, (float)imageTexture.height),
                    new Vector2(0.5f, 0.5f), 1.8f, 0U, 0, Vector4.zero, false);
            } else if (i >= NEATBrainPatch.nInputs)
            {
                __result = __instance.brainIcons[i - 9];
            } else
            {
                __result = __instance.brainIcons[i];
            }

            return false;
        }
    }
}