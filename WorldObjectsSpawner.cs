using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SettingScripts;
using ManagementScripts;
using MoreMaterials;
using UnityEngine;
using System.IO;


namespace MoreMaterials
{
    [HarmonyPatch(typeof(WorldObjectsSpawner))]
    internal static class WorldObjectsSpawnerManagerPatch
    {
        [HarmonyPatch("GeneratePelletOfMatter")]
        [HarmonyPrefix]
        private static bool Prefix(WorldObjectsSpawner __instance, MatterMaterial material, Vector3? pos, Transform holder
            , ref GameObject __result)
        {
            GameObject entry = Traverse.Create(__instance)
                    .Field("plantEntry").GetValue() as GameObject;
            if (material == MatterMaterialManager.FindMaterial("Root"))
            {
                GameObject rootEntry = UnityEngine.Object.Instantiate(entry);
                rootEntry.GetComponent<MatterPellet>().material = MatterMaterialManager.FindMaterial("Root");
                __result = UnityEngine.Object.Instantiate<GameObject>
                    (rootEntry, pos ?? Vector3.zero, Quaternion.identity, (holder != null) ? holder : __instance.freePelletHolder);
                Object.Destroy(rootEntry);
                return false;
            }
            if (material == MatterMaterialManager.FindMaterial("Fruit"))
            {
                GameObject fruitEntry = UnityEngine.Object.Instantiate(entry);
                fruitEntry.GetComponent<MatterPellet>().material = MatterMaterialManager.FindMaterial("Fruit");
                __result = UnityEngine.Object.Instantiate<GameObject>
                    (fruitEntry, pos ?? Vector3.zero, Quaternion.identity, (holder != null) ? holder : __instance.freePelletHolder);
                Object.Destroy(fruitEntry);
                return false;
            }
            if (material == MatterMaterialManager.FindMaterial("Fungus"))
            {
                GameObject fungusEntry = UnityEngine.Object.Instantiate(entry);
                fungusEntry.GetComponent<MatterPellet>().material = MatterMaterialManager.FindMaterial("Fungus");
                __result = UnityEngine.Object.Instantiate<GameObject>
                    (fungusEntry, pos ?? Vector3.zero, Quaternion.identity, (holder != null) ? holder : __instance.freePelletHolder);
                Object.Destroy(fungusEntry);
                return false;
            }

            return true;
        }
    }
}