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
using UIScripts.UIReferences;
using Utility;
using ScriptHelpers;
using System.Collections;
using System.ComponentModel;

namespace MoreMaterials
{
    [HarmonyPatch(typeof(FieldOfView))]
    internal static class FieldOfViewPatch
    {
            [HarmonyPatch("FindSeenEntities")]
        [HarmonyPrefix]
        private static bool findSeenEntitiesPrefix(FieldOfView __instance)
        {
            if (!__instance.needToSee)
            {
                return false;
            }
            __instance.nBibitesInRange = (__instance.nPlantsInRange = (__instance.nMeatsInRange = 0));
            Transform transform = __instance.transform;
            float num = Mathf.Min(0.25f * __instance.viewRadius, 25f);
            float num2 = __instance.viewAngle / 2f * 0.017453292f;
            if (num2 < 1.9634955f)
            {
                float num3 = Mathf.Min(0f, __instance.viewRadius * Mathf.Cos(num2));
                Vector2 vector = new Vector2(2f * __instance.viewRadius * ((__instance.viewAngle > 180f) ? 1f : Mathf.Sin(num2)), __instance.viewRadius - num3) + 2f * num * Vector2.one;
                Vector2 vector2 = transform.position + (__instance.viewRadius + num3) / 2f * transform.up;
                float num4 = Vector2.SignedAngle(Vector2.up, __instance.transform.up);
                __instance.nInRange = Physics2D.OverlapBoxNonAlloc(vector2, vector, num4, __instance.elementsInViewRadius, __instance.layerMask, 0f);
            }
            else
            {
                __instance.nInRange = Physics2D.OverlapCircleNonAlloc(__instance.transform.position, __instance.viewRadius + num, __instance.elementsInViewRadius, __instance.layerMask, 0f);
            }
            ArrayList seenRoots = new ArrayList();
            ArrayList seenFruits = new ArrayList();
            ArrayList seenFungus = new ArrayList();
            for (int i = 0; i < __instance.nInRange; i++)
            {
                __instance.target = __instance.elementsInViewRadius[i].gameObject;
                __instance.targetLayer = __instance.target.layer;
                if (__instance.targetLayer == __instance.pelletLayer)
                {
                    MatterPellet component = __instance.target.GetComponent<MatterPellet>();
                    if (component.material == __instance.plant && (__instance.targetMask & 2) > 0)
                    {
                        MatterPellet[] array = __instance.seenPlantPellets;
                        int num5 = __instance.nPlantsInRange;
                        __instance.nPlantsInRange = num5 + 1;
                        array[num5] = component;
                    }
                    else if (component.material == __instance.meat && (__instance.targetMask & 4) > 0)
                    {
                        MatterPellet[] array2 = __instance.seenMeatPellets;
                        int num5 = __instance.nMeatsInRange;
                        __instance.nMeatsInRange = num5 + 1;
                        array2[num5] = component;
                    }
                    else if (component.material == MatterMaterialManager.FindMaterial("Root") && (__instance.targetMask & 2) > 0)
                    {
                        seenRoots.Add(component);
                    }
                    else if (component.material == MatterMaterialManager.FindMaterial("Fruit") && (__instance.targetMask & 2) > 0)
                    {
                        seenFruits.Add(component);
                    }
                    else if (component.material == MatterMaterialManager.FindMaterial("Fungus") && (__instance.targetMask & 2) > 0)
                    {
                        seenFungus.Add(component);
                    }
                }
                else if (__instance.targetLayer == __instance.bibiteLayer && __instance.target.transform.parent != __instance.transform)
                {
                    bool flag = false;
                    GameObject gameObject = __instance.target.transform.parent.gameObject;
                    int num6 = 0;
                    while (num6 < __instance.nBibitesInRange && !flag)
                    {
                        flag |= __instance.bibiteWeights[num6].gameObject == gameObject;
                        num6++;
                    }
                    if (!flag)
                    {
                        __instance.bibiteWeights[__instance.nBibitesInRange].gameObject = gameObject;
                        BibiteBody[] array3 = __instance.seenBibites;
                        int num5 = __instance.nBibitesInRange;
                        __instance.nBibitesInRange = num5 + 1;
                        array3[num5] = gameObject.GetComponent<BibiteBody>();
                    }
                }
            }

            int nRoots = 0;
            int nFruits = 0;
            int nFungus = 0;
            float rootWeightSum = 0.0f;
            float fruitWeightSum = 0.0f;
            float fungusWeightSum = 0.0f;
            Vector2 rootDir = Vector2.zero;
            Vector2 fruitDir = Vector2.zero;
            Vector2 fungusDir = Vector2.zero;

            for (int j = 0; j < seenRoots.Count; j++)
            {
                MatterPellet matterPellet = seenRoots[j] as MatterPellet;
                if (!(matterPellet == null) && matterPellet.amount >= 0.01f && !__instance.PelletIsHeld(matterPellet.transform) && __instance.TargetIsInView(matterPellet.transform, matterPellet.radius))
                {
                    float vec = Mathf.Pow(1.05f - __instance.dist2Target / __instance.viewRadius, 2f) * matterPellet.sizeFactor * (1.05f - Mathf.Abs(__instance.angle2Target) / (__instance.viewAngle / 2f));
                    nRoots++;
                    rootDir += __instance.dir2Target * vec;
                    rootWeightSum += vec;
                }
            }
            for (int j = 0; j < seenFruits.Count; j++)
            {
                MatterPellet matterPellet = seenFruits[j] as MatterPellet;
                if (!(matterPellet == null) && matterPellet.amount >= 0.01f && !__instance.PelletIsHeld(matterPellet.transform) && __instance.TargetIsInView(matterPellet.transform, matterPellet.radius))
                {
                    float vec = Mathf.Pow(1.05f - __instance.dist2Target / __instance.viewRadius, 2f) * matterPellet.sizeFactor * (1.05f - Mathf.Abs(__instance.angle2Target) / (__instance.viewAngle / 2f));
                    nFruits++;
                    fruitDir += __instance.dir2Target * vec;
                    fruitWeightSum += vec;
                }
            }
            for (int j = 0; j < seenFungus.Count; j++)
            {
                MatterPellet matterPellet = seenFungus[j] as MatterPellet;
                if (!(matterPellet == null) && matterPellet.amount >= 0.01f && !__instance.PelletIsHeld(matterPellet.transform) && __instance.TargetIsInView(matterPellet.transform, matterPellet.radius))
                {
                    float vec = Mathf.Pow(1.05f - __instance.dist2Target / __instance.viewRadius, 2f) * matterPellet.sizeFactor * (1.05f - Mathf.Abs(__instance.angle2Target) / (__instance.viewAngle / 2f));
                    nFungus++;
                    fungusDir += __instance.dir2Target * vec;
                    fungusWeightSum += vec;
                }
            }

            if (rootWeightSum > 0f)
            {
                rootDir /= rootWeightSum;
            }
            if (fruitWeightSum > 0f)
            {
                fruitDir /= fruitWeightSum;
            }
            if (fungusWeightSum > 0f)
            {
                fungusDir /= fungusWeightSum;
            }

            NEATBrain brain = __instance.gameObject.GetComponent<NEATBrain>();
            brain.Nodes[32].Value = ((rootWeightSum > 0f) ? (1f - Mathf.Min(rootDir.magnitude / __instance.viewRadius, 1f)) : 0f);
            brain.Nodes[33].Value = 2f * Vector2.SignedAngle(__instance.upvect, rootDir) / __instance.viewAngle;
            brain.Nodes[34].Value = (float)nRoots / 4f;
            brain.Nodes[35].Value = ((fruitWeightSum > 0f) ? (1f - Mathf.Min(fruitDir.magnitude / __instance.viewRadius, 1f)) : 0f);
            brain.Nodes[36].Value = 2f * Vector2.SignedAngle(__instance.upvect, fruitDir) / __instance.viewAngle;
            brain.Nodes[37].Value = (float)nFruits / 4f;
            brain.Nodes[38].Value = ((fungusWeightSum > 0f) ? (1f - Mathf.Min(fungusDir.magnitude / __instance.viewRadius, 1f)) : 0f);
            brain.Nodes[39].Value = 2f * Vector2.SignedAngle(__instance.upvect, fungusDir) / __instance.viewAngle;
            brain.Nodes[40].Value = (float)nFungus / 4f;

            return false;
        }
    }
}