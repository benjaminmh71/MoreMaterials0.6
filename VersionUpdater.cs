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
            __result["version"] = (new Utility.Version(100, 6, 0, 1)).ToString();
        }
    }

        [HarmonyPatch(typeof(SaveSystem))]
    internal static class SaveVersionTranspiler
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var result = new List<MethodBase>();
            result.Add(AccessTools.Method(typeof(SaveSystem), "SaveBibite"));
            result.Add(AccessTools.Method(typeof(SaveSystem), "SaveEgg"));
            var startGameType = Assembly.GetAssembly(typeof(SaveSystem)).GetTypes().First(x => x.Name == "<CreateSave>d__21");
            var moveNextMethod = startGameType.GetMethod("MoveNext", AccessTools.all);
            result.Add(moveNextMethod);
            result.Add(AccessTools.PropertyGetter(typeof(Utility.Version), "Present"));

            return result;
        }

        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            String version = (new Utility.Version(100, 6, 0, 1)).ToString();
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].ToString() == "call static string UnityEngine.Application::get_version()")
                {
                    Plugin.Log.LogInfo("Found");
                    codes[i].opcode = System.Reflection.Emit.OpCodes.Ldstr;
                    codes[i].operand = version;
                }
            }

            return codes.AsEnumerable();
        }
    }
}