namespace IncoherentWorlds
{
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