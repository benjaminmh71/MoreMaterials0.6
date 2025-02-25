using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SimulationScripts.BibiteScripts;
using SettingScripts;
using ManagementScripts;
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
using System.Reflection.Emit;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MoreMaterials
{
    [HarmonyPatch(typeof(SaveController))]
    internal static class SaveControllerPatch
    {
        [HarmonyPatch("SavePath", MethodType.Getter)]
        [HarmonyPostfix]
        private static void Postfix(ref string __result)
        {
            __result = Path.Combine(UnityEngine.Application.persistentDataPath, "MoreMaterials0.6.0Data/Savefiles");
        }
    }

    [HarmonyPatch(typeof(SaveSystem))]
    internal static class SaveSystemPathPatch
    {
        [HarmonyPatch("savedBibitePath", MethodType.Getter)]
        [HarmonyPostfix]
        private static void Postfix(ref string __result)
        {
            __result = Path.Combine(UnityEngine.Application.persistentDataPath, "MoreMaterials0.6.0Data/Bibites/");
        }
    }
}