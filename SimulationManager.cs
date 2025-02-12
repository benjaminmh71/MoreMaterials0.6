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
using System.Reflection;


namespace MoreMaterials
{
    [HarmonyPatch(typeof(SimulationManager))]
    internal static class SimulationManagerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static void awakePrefix(SimulationManager __instance)
        {
            NEATBrain.NInputs = 41;
        }
    }
}