using BepInEx;
using BepInEx.Logging;
using IL;
using System;

namespace IncoherentWorlds
{
    [BepInDependency(RegionKit.API.Core.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "incoherentworlds.itstabby";
        public const string PLUGIN_NAME = "Incoherent Worlds";
        public const string PLUGIN_VERSION = "1.0.0";
        public static new ManualLogSource Logger;
        bool isInit;
        public void OnEnable()
        {
            Logger = base.Logger;
            On.RainWorld.OnModsInit += OnModsInit;
            IWEnums.RegisterValues();
            IWHooks.Apply();
        }
        public void OnDisable()
        {
            Logger = null;
            On.RainWorld.OnModsInit -= OnModsInit;
            IWEnums.UnregisterValues();
            IWHooks.Undo();
        }
        public void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            if (isInit) return;
            isInit = true;
            Logger.LogDebug("IW init success, right?");
        }
    }
    public class IWEnums
    {
        public static RoomSettings.RoomEffect.Type NarrowHorizon;
        public static RoomSettings.RoomEffect.Type StellarSky;
        public static void RegisterValues()
        {
            NarrowHorizon = new RoomSettings.RoomEffect.Type("NarrowHorizon", true);
            StellarSky = new RoomSettings.RoomEffect.Type("StellarSky", true);
        }
        public static void UnregisterValues()
        {
            if (NarrowHorizon != null) { NarrowHorizon.Unregister(); NarrowHorizon = null; }
            if (StellarSky != null) { StellarSky.Unregister(); StellarSky = null; }
        }
    }
}