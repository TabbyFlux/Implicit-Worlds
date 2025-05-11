using BepInEx;
using System;

namespace IncoherentWorlds
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "incoherentworlds.itstabby";
        public const string PLUGIN_NAME = "Incoherent Worlds";
        public const string PLUGIN_VERSION = "1.0.0";
        public void OnEnable()
        {
            IWEnums.RegisterValues();
        }
        public void OnDisable()
        {
            IWEnums.UnregisterValues();
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