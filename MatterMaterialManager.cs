using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using SimulationScripts;
using SettingScripts;
using MoreMaterials;
using UnityEngine;
using System.IO;


namespace MoreMaterials
{
    [HarmonyPatch(typeof(MatterMaterialManager))]
    internal static class MatterMaterialManagerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static void Prefix(MatterMaterialManager __instance)
        {
            MatterMaterial root = ScriptableObject.CreateInstance("MatterMaterial") as MatterMaterial;
            root.Name = "Root";
            root.Cohesiveness = 50f;
            root.MassDensity = 0.025f;
            root.EnergyDensity = 7.5f;
            root.Reactivity = 0.035f;
            root.MinConversionEfficiency = 0.05f;
            root.MaxConversionEfficiency = 0.65f;
            root.decay = false;
            root.freshTime = 100f;
            root.decayRate = 0.005f;
            byte[] rootA = File.ReadAllBytes(UnityEngine.Application.dataPath + "/Mods/medium_root.png");
            Texture2D rootT = new Texture2D(2, 2);
            rootT.filterMode = FilterMode.Point;
            ImageConversion.LoadImage(rootT, rootA);
            root.defaultSprite = Sprite.Create(rootT, new Rect(0f, 0f, (float)rootT.width, (float)rootT.height), new Vector2(0.5f, 0.5f), 1.8f, 0U, 0, Vector4.zero, false);
            MatterMaterial fruit = ScriptableObject.CreateInstance("MatterMaterial") as MatterMaterial;
            fruit.Name = "Fruit";
            fruit.Cohesiveness = 5f;
            fruit.MassDensity = 0.02f;
            fruit.EnergyDensity = 7.5f;
            fruit.Reactivity = 0.05f;
            fruit.MinConversionEfficiency = 0.4f;
            fruit.MaxConversionEfficiency = 0.75f;
            fruit.decay = false;
            fruit.freshTime = 100f;
            fruit.decayRate = 0.005f;
            byte[] fruitA = File.ReadAllBytes(UnityEngine.Application.dataPath + "/Mods/medium_fruit.png");
            Texture2D fruitT = new Texture2D(2, 2);
            fruitT.filterMode = FilterMode.Point;
            ImageConversion.LoadImage(fruitT, fruitA);
            fruit.defaultSprite = Sprite.Create(fruitT, new Rect(0f, 0f, (float)fruitT.width, (float)fruitT.height), new Vector2(0.5f, 0.5f), 1.8f, 0U, 0, Vector4.zero, false);
            MatterMaterial fungus = ScriptableObject.CreateInstance("MatterMaterial") as MatterMaterial;
            fungus.Name = "Fungus";
            fungus.Cohesiveness = 30f;
            fungus.MassDensity = 0.02f;
            fungus.EnergyDensity = 7.5f;
            fungus.Reactivity = 0.055f;
            fungus.MinConversionEfficiency = 0.3f;
            fungus.MaxConversionEfficiency = 0.8f;
            fungus.decay = false;
            fungus.freshTime = 100f;
            fungus.decayRate = 0.005f;
            byte[] fungusA = File.ReadAllBytes(UnityEngine.Application.dataPath + "/Mods/medium_fungus.png");
            Texture2D fungusT = new Texture2D(2, 2);
            fungusT.filterMode = FilterMode.Point;
            ImageConversion.LoadImage(fungusT, fungusA);
            fungus.defaultSprite = Sprite.Create(fungusT, new Rect(0f, 0f, (float)fungusT.width, (float)fungusT.height), new Vector2(0.5f, 0.5f), 1.8f, 0U, 0, Vector4.zero, false);
            MatterMaterialManager.PhysicalMaterials.Add(root);
            MatterMaterialManager.MaterialSettings.Add(new MatterMaterialSettings(root));
            MatterMaterialManager.PhysicalMaterials.Add(fruit);
            MatterMaterialManager.MaterialSettings.Add(new MatterMaterialSettings(fruit));
            MatterMaterialManager.PhysicalMaterials.Add(fungus);
            MatterMaterialManager.MaterialSettings.Add(new MatterMaterialSettings(fungus));
        }
    }
}