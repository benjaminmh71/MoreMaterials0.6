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
    [HarmonyPatch(typeof(SimulationManager))]
    internal static class SimulationManagerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static void awakePrefix(SimulationManager __instance)
        {
            foreach (VersionBrainChange v in BrainUpdater.VersionChanges)
            {
                if (v.version == new Utility.Version(100, 6, 0, 1)) return;
            }

            BrainUpdater.VersionChanges.Add(
                new VersionBrainChange
                {
                    version = new Utility.Version(100, 6, 0, 1),
                    removedInputs = new NodeChanges[0],
                    addedInputs = new NodeChanges[]
                    {
                        new NodeChanges(32, 9),
                    },
                    addedOutputs = new NodeChanges[0],
                    removedOutputs = new NodeChanges[0]
                });
        }
    }
}