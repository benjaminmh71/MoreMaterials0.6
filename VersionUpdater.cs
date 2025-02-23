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

namespace MoreMaterials
{
    [HarmonyPatch(typeof(BibiteTemplate))]
	internal static class VersionPatch
	{
        [HarmonyPatch("SaveState")]
        [HarmonyPostfix]
        private static void Postfix(BibiteTemplate __instance, ref JObject __result)
        {
            Plugin.Log.LogInfo("Running");
            __result["version"] = (new Utility.Version(100, 6, 0, 1)).ToString();
        }
    }

    [HarmonyPatch(typeof(SaveSystem))]
    internal static class SaveVersionPatch
    {
        [HarmonyPatch("SaveBibite")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].ToString() == "call static string UnityEngine.Application::get_version()")
                {
                    codes[i].opcode = System.Reflection.Emit.OpCodes.Ldstr;
                    codes[i].operand = "100.6.0.1";
                }
            }

            return codes.AsEnumerable();
        }
    }
}