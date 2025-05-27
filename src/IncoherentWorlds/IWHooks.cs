using System;
using On;
using RegionKit.Modules.Effects;

namespace IncoherentWorlds
{
    public class IWHooks
    {
        public static void Apply()
        {
            On.BackgroundScene.ctor += IWHooks.BackgroundScene_ctor;
            //On.BackgroundScene.Update += IWHooks.BackgroundScene_Update;
        }
        public static void Undo()
        {
            On.BackgroundScene.ctor -= IWHooks.BackgroundScene_ctor;
            //On.BackgroundScene.Update -= IWHooks.BackgroundScene_Update;
        }
        public static void BackgroundScene_ctor(On.BackgroundScene.orig_ctor orig, BackgroundScene self, Room room)
        {
            orig(self, room);
            if (room.roomSettings.IsEffectInRoom(IWEnums.NarrowHorizon))
            {
                room.AddObject(new NarrowHorizon(room, new RoomSettings.RoomEffect(IWEnums.NarrowHorizon, 0f, false)));
            }
        }
        //public static readonly RoomSettings.RoomEffect.Type NarrowHorizon = IWEnums.NarrowHorizon;
    }
}
