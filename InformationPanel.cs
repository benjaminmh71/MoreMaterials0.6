using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SimulationScripts.BibiteScripts;
using SettingScripts;
using MoreMaterials;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using Mono.Cecil.Cil;
using System.Linq;
using UIScripts;
using UIScripts.InfoHandles;
using UIScripts.UIReferences;
using Utility;
using ScriptHelpers;
using System.Collections;
using System.ComponentModel;

namespace MoreMaterials
{
    [HarmonyPatch(typeof(InformationPanel))]
    internal static class InformationPanelPatch
    {
        [HarmonyPatch("InitPanel")]
        [HarmonyPrefix]
        private static void InitPrefix(InformationPanel __instance)
        {
            __instance.countInfos = new WedgeInfo[]
            {
                new WedgeInfo
                {
                    label = "Bibites",
                    color = Color.white
                },
                new WedgeInfo
                {
                    label = "Eggs",
                    color = new Color(0.8352941f, 0.8431373f, 0.8431373f)
                },
                new WedgeInfo
                {
                    label = "Plants",
                    color = new Color(0.1529412f, 0.6745098f, 0.1333333f)
                },
                new WedgeInfo
                {
                    label = "Meats",
                    color = new Color(0.8431373f, 0.1490196f, 0.2392157f)
                },
                new WedgeInfo
                {
                    label = "Roots",
                    color = new Color(1f, 0.7098f, 0.4588f)
                },
                new WedgeInfo
                {
                    label = "Fruits",
                    color = new Color(0.2863f, 0.2745f, 0.7765f)
                },
                new WedgeInfo
                {
                    label = "Fungus",
                    color = new Color(0.6667f, 0.5725f, 0.5255f)
                }
            };

            __instance.energyInfos = new WedgeInfo[]
            {
                new WedgeInfo
                {
                    label = "Free",
                    color = new Color(0.6665637f, 0.735849f, 0.253382f)
                },
                new WedgeInfo
                {
                    label = "Bibites",
                    color = Color.white
                },
                new WedgeInfo
                {
                    label = "Eggs",
                    color = new Color(0.8352941f, 0.8431373f, 0.8431373f)
                },
                new WedgeInfo
                {
                    label = "Plants",
                    color = new Color(0.1529412f, 0.6745098f, 0.1333333f)
                },
                new WedgeInfo
                {
                    label = "Meats",
                    color = new Color(0.8431373f, 0.1490196f, 0.2392157f)
                },
                new WedgeInfo
                {
                    label = "Roots",
                    color = new Color(0.8431373f, 0.1490196f, 0.2392157f)
                },
                new WedgeInfo
                {
                    label = "Fruits",
                    color = new Color(0.8431373f, 0.1490196f, 0.2392157f)
                },
                new WedgeInfo
                {
                    label = "Fungus",
                    color = new Color(0.8431373f, 0.1490196f, 0.2392157f)
                }
            };

            WedgeInfoHandle rootHandle = UnityEngine.Object.Instantiate(__instance.plantCount, 
                __instance.plantCount.transform.parent);
            rootHandle.name = "rootHandle";
            WedgeInfoHandle fruitHandle = UnityEngine.Object.Instantiate(__instance.plantCount,
                __instance.plantCount.transform.parent);
            fruitHandle.name = "fruitHandle";
            WedgeInfoHandle fungusHandle = UnityEngine.Object.Instantiate(__instance.plantCount,
                __instance.plantCount.transform.parent);
            fungusHandle.name = "fungusHandle";
            WedgeInfoHandle rootEnergyHandle = UnityEngine.Object.Instantiate(__instance.plantEnergy,
                __instance.plantEnergy.transform.parent);
            rootEnergyHandle.name = "rootEnergyHandle";
            WedgeInfoHandle fruitEnergyHandle = UnityEngine.Object.Instantiate(__instance.plantEnergy,
                __instance.plantEnergy.transform.parent);
            fruitEnergyHandle.name = "fruitEnergyHandle";
            WedgeInfoHandle fungusEnergyHandle = UnityEngine.Object.Instantiate(__instance.plantEnergy,
                __instance.plantEnergy.transform.parent);
            fungusEnergyHandle.name = "fungusEnergyHandle";

            rootHandle.InitWedgeInfo(__instance.countInfos[4], "", 0);
            fruitHandle.InitWedgeInfo(__instance.countInfos[5], "", 0);
            fungusHandle.InitWedgeInfo(__instance.countInfos[6], "", 0);
            rootEnergyHandle.InitWedgeInfo(__instance.energyInfos[5], "E", 0);
            fruitEnergyHandle.InitWedgeInfo(__instance.energyInfos[6], "E", 0);
            fungusEnergyHandle.InitWedgeInfo(__instance.energyInfos[7], "E", 0);
        }

        [HarmonyPatch("UpdateInformation")]
        [HarmonyPostfix]
        private static void UpdatePostfix(InformationPanel __instance, int newPlantCount, int newMeatCount, int newBibiteCount, 
            int newEggCount, float pelletSpawnerE, float pelletE, float bibiteE, float eggE, float plantE, float meatE, 
            Dictionary<string, TagInfo> tags)
        {
            float rootCount = 1f;
            float fruitCount = 2f;
            float fungusCount = 3f;
            float rootEnergy = 4f;
            float fruitEnergy = 5f;
            float fungusEnergy = 6f;

            __instance.totalCount.UpdateValue((float)(newBibiteCount + newPlantCount + newEggCount + newMeatCount + 
                rootCount + fruitCount + fungusCount));
            __instance.countPieChart.UpdateValues(new float[]
            {
                (float)newBibiteCount,
                (float)newEggCount,
                (float)newPlantCount,
                (float)newMeatCount,
                rootCount,
                fruitCount,
                fungusCount
            });

            __instance.totalEnergy.UpdateValue(pelletSpawnerE + bibiteE + eggE + meatE + plantE + 
                rootEnergy + fruitEnergy + fungusEnergy);
            __instance.energyPieChart.UpdateValues(new float[] { pelletSpawnerE, bibiteE, eggE, plantE, meatE,
            rootEnergy, fruitEnergy, fungusEnergy});

            Transform countSection = __instance.gameObject.transform.Find("Entities section").Find("CountSection").
                Find("Entities section (1)");
            Transform energySection = __instance.gameObject.transform.Find("Entities section").Find("EnergySection").
                Find("Entities section (1)");
            WedgeInfoHandle rootHandle = countSection.Find("rootHandle").gameObject.GetComponent<WedgeInfoHandle>();
            WedgeInfoHandle fruitHandle = countSection.Find("fruitHandle").gameObject.GetComponent<WedgeInfoHandle>();
            WedgeInfoHandle fungusHandle = countSection.Find("fungusHandle").gameObject.GetComponent<WedgeInfoHandle>();
            WedgeInfoHandle rootEnergyHandle = energySection.Find("rootEnergyHandle").gameObject.GetComponent<WedgeInfoHandle>();
            WedgeInfoHandle fruitEnergyHandle = energySection.Find("fruitEnergyHandle").gameObject.GetComponent<WedgeInfoHandle>();
            WedgeInfoHandle fungusEnergyHandle = energySection.Find("fungusEnergyHandle").gameObject.GetComponent<WedgeInfoHandle>();

            rootHandle.UpdateValue(rootCount);
            fruitHandle.UpdateValue(fruitCount);
            fungusHandle.UpdateValue(fungusCount);
            rootEnergyHandle.UpdateValue(rootEnergy);
            fruitEnergyHandle.UpdateValue(fruitEnergy);
            fungusEnergyHandle.UpdateValue(fungusEnergy);
        }
    }
}