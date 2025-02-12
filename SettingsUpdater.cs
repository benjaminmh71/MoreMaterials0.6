using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using UIScripts.SettingHandles;
using SimulationScripts;
using SettingScripts;
using MoreMaterials;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Reflection;


namespace MoreMaterials
{
	[HarmonyPatch(typeof(SettingsUpdater))]
	internal static class SettingsUpdaterPatch
	{
		[HarmonyPatch("Start")]
		[HarmonyPrefix]
		private static void Prefix(SettingsUpdater __instance)
		{
			MatterMaterialSettings rootSettings =
				MatterMaterialManager.SettingsOfMaterial(MatterMaterialManager.FindMaterial("Root"));
			MatterMaterialSettings fruitSettings =
				MatterMaterialManager.SettingsOfMaterial(MatterMaterialManager.FindMaterial("Fruit"));
			MatterMaterialSettings fungusSettings =
				MatterMaterialManager.SettingsOfMaterial(MatterMaterialManager.FindMaterial("Fungus"));
			MethodInfo updateDecay = typeof(SettingsUpdater).GetMethod("UpdateDecay", BindingFlags.NonPublic | BindingFlags.Instance);
			MethodInfo updateDecayParameters =
				typeof(SettingsUpdater).GetMethod("UpdateDecayParameters", BindingFlags.NonPublic | BindingFlags.Instance);
			MethodInfo updatePhysicalProperties =
				typeof(SettingsUpdater).GetMethod("UpdatePhysicalProperties", BindingFlags.NonPublic | BindingFlags.Instance);

			void updateRootDecay()
			{
				updateDecayParameters.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Root") });
			}
			void updateRootPhysicalProperties()
			{
				updatePhysicalProperties.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Root") });
			}
			void updateFruitDecay()
			{
				updateDecayParameters.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fruit") });
			}
			void updateFruitPhysicalProperties()
			{
				updatePhysicalProperties.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fruit") });
			}
			void updateFungusDecay()
			{
				updateDecayParameters.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fungus") });
			}
			void updateFungusPhysicalProperties()
			{
				updatePhysicalProperties.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fungus") });
			}

			rootSettings.decay.Subscribe(new UnityAction<bool>
				(
				val =>
				{
					updateDecay.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Root"), val });
				}));
			rootSettings.decayRate.Subscribe(new UnityAction(updateRootDecay));
			rootSettings.freshTime.Subscribe(new UnityAction(updateRootDecay));

			fruitSettings.decay.Subscribe(new UnityAction<bool>
				(
				val =>
				{
					updateDecay.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fruit"), val });
				}));
			fruitSettings.decayRate.Subscribe(new UnityAction(updateFruitDecay));
			fruitSettings.freshTime.Subscribe(new UnityAction(updateFruitDecay));

			fungusSettings.decay.Subscribe(new UnityAction<bool>
				(
				val =>
				{
					updateDecay.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fungus"), val });
				}));
			fungusSettings.decayRate.Subscribe(new UnityAction(updateFungusDecay));
			fungusSettings.freshTime.Subscribe(new UnityAction(updateFungusDecay));

			rootSettings.onPhysicalParametersChange.AddListener(new UnityAction(updateRootPhysicalProperties));
			fruitSettings.onPhysicalParametersChange.AddListener(new UnityAction(updateFruitPhysicalProperties));
			fungusSettings.onPhysicalParametersChange.AddListener(new UnityAction(updateFungusPhysicalProperties));
		}

		[HarmonyPatch("OnDestroy")]
		[HarmonyPrefix]
		private static void onDestroyPrefix(SettingsUpdater __instance)
		{
			MatterMaterialSettings rootSettings =
				MatterMaterialManager.SettingsOfMaterial(MatterMaterialManager.FindMaterial("Root"));
			MatterMaterialSettings fruitSettings =
				MatterMaterialManager.SettingsOfMaterial(MatterMaterialManager.FindMaterial("Fruit"));
			MatterMaterialSettings fungusSettings =
				MatterMaterialManager.SettingsOfMaterial(MatterMaterialManager.FindMaterial("Fungus"));
			MethodInfo updateDecay = typeof(SettingsUpdater).GetMethod("UpdateDecay", BindingFlags.NonPublic | BindingFlags.Instance);
			MethodInfo updateDecayParameters =
				typeof(SettingsUpdater).GetMethod("UpdateDecayParameters", BindingFlags.NonPublic | BindingFlags.Instance);

			void updateRootDecay()
			{
				updateDecayParameters.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Root") });
			}
			void updateFruitDecay()
			{
				updateDecayParameters.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fruit") });
			}
			void updateFungusDecay()
			{
				updateDecayParameters.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fungus") });
			}

			rootSettings.decay.UnSubscribe(new UnityAction<bool>
				(
				val =>
				{
					updateDecay.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Root"), val });
				}));
			rootSettings.decayRate.UnSubscribe(new UnityAction(updateRootDecay));
			rootSettings.freshTime.UnSubscribe(new UnityAction(updateRootDecay));

			fruitSettings.decay.UnSubscribe(new UnityAction<bool>
				(
				val =>
				{
					updateDecay.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fruit"), val });
				}));
			fruitSettings.decayRate.UnSubscribe(new UnityAction(updateFruitDecay));
			fruitSettings.freshTime.UnSubscribe(new UnityAction(updateFruitDecay));

			fungusSettings.decay.UnSubscribe(new UnityAction<bool>
				(
				val =>
				{
					updateDecay.Invoke(__instance, new object[] { MatterMaterialManager.FindMaterial("Fungus"), val });
				}));
			fungusSettings.decayRate.UnSubscribe(new UnityAction(updateFungusDecay));
			fungusSettings.freshTime.UnSubscribe(new UnityAction(updateFungusDecay));
		}
	}
}