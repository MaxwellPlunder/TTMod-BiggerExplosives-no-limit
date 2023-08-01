using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BiggerExplosives.Patches;
using HarmonyLib;
using UnityEngine;

namespace BiggerExplosives
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class BiggerExplosivesPlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        // e.g.
        // com.mynameororg.pluginname
        // Version should be a valid version string.
        // e.g.
        // 1.0.0
        private const string MyGUID = "com.equinox.BiggerExplosives";
        private const string PluginName = "BiggerExplosives";
        private const string VersionString = "1.0.0";

        // Config entry key strings
        // These will appear in the config file created by BepInEx and can also be used
        // by the OnSettingsChange event to determine which setting has changed.
        public static string ExplosionRadiusKey = "ExplosionRadius";
        public static string ExplosionDepthKey = "ExplosionDepth";

        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        // e.g.
        // float myFloat = BiggerExplosivesPlugin.FloatExample.Value;
        // TODO Change this code or remove the code if not required.
        public static ConfigEntry<int> ExplosionRadius;
        public static ConfigEntry<int> ExplosionDepth;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake() {
            ExplosionRadius = Config.Bind("General", ExplosionRadiusKey, 11, new ConfigDescription("Resulting tunnel's width will be = (radius * )2 + 1", new AcceptableValueRange<int>(0, 15)));
            ExplosionDepth = Config.Bind("General", ExplosionDepthKey, 20, new ConfigDescription("Distance from the explosive to dig.", new AcceptableValueRange<int>(1, 30)));

            
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;

            Harmony.CreateAndPatchAll(typeof(ExplodeActionInfoPatch));
        }
    }
}
