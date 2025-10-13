using RegionKit.Modules.Effects;

namespace ImplicitWorlds
{
    public class IWHooks
    {
        public static void Apply()
        {
            On.RainWorldGame.Update += TestHook;
        }
        public static void Undo()
        {
            On.RainWorldGame.Update -= TestHook;
        }
        private static void TestHook(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            orig(self);
            if (Input.GetKey(KeyCode.J))
            {
                self.RealizedPlayerFollowedByCamera.room.PlaySound(SoundID.SS_AI_Give_The_Mark_Boom, 0f, 1f, 1f);
                UnityEngine.Debug.Log("[ImplicitWorlds]: sound has been played!");
            }
        }
    }
}
