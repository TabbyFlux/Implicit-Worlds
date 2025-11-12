namespace ImplicitWorlds
{
    public class IWHooks
    {
        public static void Apply()
        {
            On.Room.Loaded += Room_Loaded;
        }
        public static void Undo()
        {
            On.Room.Loaded -= Room_Loaded;
        }
        private static void Room_Loaded(On.Room.orig_Loaded orig, Room self)
        {
            orig(self);
            try
            {
                if (self.game != null)
                {
                    for (int i = 0; i < self.roomSettings.effects.Count; i++)
                    {
                        if (self.roomSettings.effects[i].type == IWEnums.RoomEffectType.NarrowHorizon)
                        {
                            self.AddObject(new NarrowHorizon(self, self.roomSettings.effects[i]));
                            UnityEngine.Debug.Log("[ImplicitWorlds]: Narrow Horizon view added!");
                        }
                        else if (self.roomSettings.effects[i].type == IWEnums.RoomEffectType.StellarSky)
                        {
                            self.AddObject(new StellarSky(self, self.roomSettings.effects[i]));
                            UnityEngine.Debug.Log("[ImplicitWorlds]: Stellar Sky view added!");
                        }
                        else if (self.roomSettings.effects[i].type == IWEnums.RoomEffectType.IntegralField)
                        {
                            self.AddObject(new IntegralField(self, self.roomSettings.effects[i]));
                            UnityEngine.Debug.Log("[ImplicitWorlds]: Integral Field view added!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
            }
        }
    }
}
