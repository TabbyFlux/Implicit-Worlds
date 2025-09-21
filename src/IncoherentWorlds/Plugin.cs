global using BepInEx;
global using BepInEx.Logging;
global using On;
global using System;
global using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "because nobody fucking cares lmao")]
#pragma warning restore CS0618

namespace IncoherentWorlds
{
    [BepInDependency(RegionKit.API.Core.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "incoherentworlds.itstabby";
        public const string PLUGIN_NAME = "Incoherent Worlds";
        public const string PLUGIN_VERSION = "1.0.0";
        public static new ManualLogSource Logger;
        private bool isInit;
        internal static Plugin instance;
        public void OnEnable()
        {
            instance = this;
            Logger = base.Logger;
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
        }
        public void OnDisable()
        {
            Logger = null;
            On.RainWorld.OnModsInit -= RainWorld_OnModsInit;
            IWEnums.UnregisterValues();
            IWHooks.Undo();
            instance = null;
        }
        private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            try
            {
                bool isInit = this.isInit;
                if (!isInit)
                {
                    IWEnums.RegisterValues();
                    IWHooks.Apply();
                    this.isInit = true;
                    UnityEngine.Debug.Log($"[IW]: inited: {this.isInit}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}