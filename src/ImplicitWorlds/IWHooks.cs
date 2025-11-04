namespace ImplicitWorlds
{
    public class IWHooks
    {
        public static void Apply()
        {
            On.Room.ctor += NH_ctor;
        }
        public static void Undo()
        {
            On.Room.ctor -= NH_ctor;
        }
        private static void NH_ctor(On.Room.orig_ctor orig, Room self, RainWorldGame game, World world, AbstractRoom absRoom, bool DevUI)
        {
            try
            {
                if (self.fullyLoaded && self.roomSettings.GetEffectAmount(IWEnums.RoomEffectType.NarrowHorizon) > 0f)
                {
                    self.AddObject(new NarrowHorizon(self, self.roomSettings.GetEffect(IWEnums.RoomEffectType.NarrowHorizon)));
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
                UnityEngine.Debug.Log("[ImplicitWorlds]: NH_ctor hook failed!");
            }
            orig(self, game, world, absRoom, DevUI);
        }
    }
}
