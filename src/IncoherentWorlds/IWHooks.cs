using RegionKit.Modules.Effects;

namespace IncoherentWorlds
{
    public class IWHooks
    {
        public static void Apply()
        {
            On.BackgroundScene.ctor += BackgroundScene_ctor;
            On.BackgroundScene.Update += BackgroundScene_Update;
            
        }
        public static void Undo()
        {
            On.BackgroundScene.ctor -= BackgroundScene_ctor;
            On.BackgroundScene.Update -= BackgroundScene_Update;
        }
        private static void BackgroundScene_ctor(On.BackgroundScene.orig_ctor orig, BackgroundScene self, Room room)
        {
            orig(self, room);
        }
        private static void BackgroundScene_Update(On.BackgroundScene.orig_Update orig, BackgroundScene self, bool eu)
        {
            orig(self, eu);
        }
        
        //public static RoomSettings.RoomEffect.Type NarrowHorizon = IWEnums.NarrowHorizon;
    }
}
