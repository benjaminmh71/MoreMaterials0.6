using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace MoreMaterials
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Plugin.Log = base.Logger;
            Harmony harmony = new Harmony(pluginID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private const string pluginID = "bales.thebibites.mutationmanager";

        private const string pluginName = "Mutation Manager";

        private const string version = "1.0.0";

        public static ManualLogSource Log;
    }
}
