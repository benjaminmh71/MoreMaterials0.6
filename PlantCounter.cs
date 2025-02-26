using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SimulationScripts.BibiteScripts;
using SettingScripts;
using ManagementScripts;
using MoreMaterials;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;
using System.Reflection;
using Mono.Cecil.Cil;
using System.Linq;
using UIScripts;
using UIScripts.UIReferences;
using Utility;
using ScriptHelpers;
using System.Reflection.Emit;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MoreMaterials
{
    [HarmonyPatch(typeof(PlantCounter))]
    internal static class PlantCounterPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static bool awakePrefix(PlantCounter __instance)
        {
            if (__instance.pellet == null) return false;

            if (__instance.pellet.material.Name == "Root")
            {
                MatterMaterialManager.FindDefaultMaterial("rootCounter").MassDensity += 1f;
                __instance.pellet.AfterEnergyChange.AddListener(new UnityAction<float>(__instance.ChangeCount));
                return false;
            }

            if (__instance.pellet.material.Name == "Fruit")
            {
                MatterMaterialManager.FindDefaultMaterial("fruitCounter").MassDensity += 1f;
                __instance.pellet.AfterEnergyChange.AddListener(new UnityAction<float>(__instance.ChangeCount));
                return false;
            }

            if (__instance.pellet.material.Name == "Fungus")
            {
                MatterMaterialManager.FindDefaultMaterial("fungusCounter").MassDensity += 1f;
                __instance.pellet.AfterEnergyChange.AddListener(new UnityAction<float>(__instance.ChangeCount));
                return false;
            }

            return true;
        }

        [HarmonyPatch("ChangeCount")]
        [HarmonyPrefix]
        private static bool changeCountPrefix(PlantCounter __instance, float biomassDifference)
        {
            if (__instance.pellet.material.Name == "Root")
            {
                MatterMaterialManager.FindDefaultMaterial("rootCounter").EnergyDensity += biomassDifference;
                return false;
            }

            if (__instance.pellet.material.Name == "Fruit")
            {
                MatterMaterialManager.FindDefaultMaterial("fruitCounter").EnergyDensity += biomassDifference;
                return false;
            }

            if (__instance.pellet.material.Name == "Fungus")
            {
                MatterMaterialManager.FindDefaultMaterial("fungusCounter").EnergyDensity += biomassDifference;
                return false;
            }

            return true;
        }

        [HarmonyPatch("OnDestroy")]
        [HarmonyPrefix]
        private static bool onDestroyPrefix(PlantCounter __instance)
        {
            if (__instance.pellet == null) return false;

            if (__instance.pellet.material.Name == "Root")
            {
                MatterMaterialManager.FindDefaultMaterial("rootCounter").MassDensity -= 1f;
                __instance.ChangeCount(-__instance.pellet.energy);
                __instance.pellet.AfterEnergyChange.RemoveListener(new UnityAction<float>(__instance.ChangeCount));
                return false;
            }

            if (__instance.pellet.material.Name == "Fruit")
            {
                MatterMaterialManager.FindDefaultMaterial("fruitCounter").MassDensity -= 1f;
                __instance.ChangeCount(-__instance.pellet.energy);
                __instance.pellet.AfterEnergyChange.RemoveListener(new UnityAction<float>(__instance.ChangeCount));
                return false;
            }

            if (__instance.pellet.material.Name == "Fungus")
            {
                MatterMaterialManager.FindDefaultMaterial("fungusCounter").MassDensity -= 1f;
                __instance.ChangeCount(-__instance.pellet.energy);
                __instance.pellet.AfterEnergyChange.RemoveListener(new UnityAction<float>(__instance.ChangeCount));
                return false;
            }

            return true;
        }

        [HarmonyPatch("ResetCount")]
        [HarmonyPrefix]
        private static void resetPrefix()
        {
            /*MatterMaterialManager.FindDefaultMaterial("rootCounter").MassDensity = 0f;
            MatterMaterialManager.FindDefaultMaterial("fruitCounter").MassDensity = 0f;
            MatterMaterialManager.FindDefaultMaterial("fungusCounter").MassDensity = 0f;
            MatterMaterialManager.FindDefaultMaterial("rootCounter").EnergyDensity = 0f;
            MatterMaterialManager.FindDefaultMaterial("fruitCounter").EnergyDensity = 0f;
            MatterMaterialManager.FindDefaultMaterial("fungusCounter").EnergyDensity = 0f;*/
        }
    }
}