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
            NEATBrain.NInputs = 41;
            NEATBrain.nodeMaxInov = (long)(41 + NEATBrain.NOutputs + 1);
            BrainUpdater.VersionChanges.Add(
                new VersionBrainChange
                {
                    version = new Utility.Version(100, 1, 0, 0),
                    removedInputs = new NodeChanges[0],
                    addedInputs = new NodeChanges[]
                    {
                        new NodeChanges(41, 9)
                    },
                    addedOutputs = new NodeChanges[0],
                    removedOutputs = new NodeChanges[0]
                });
        }
    }
}