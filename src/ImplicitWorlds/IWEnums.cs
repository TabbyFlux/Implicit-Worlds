namespace ImplicitWorlds
{
    public class IWEnums
    {
        public class RoomEffectType
        {
            public static RoomSettings.RoomEffect.Type NarrowHorizon;
            public static RoomSettings.RoomEffect.Type StellarSky;
            public static RoomSettings.RoomEffect.Type IntegralField;
            public static void RegisterValues()
            {
                NarrowHorizon = new RoomSettings.RoomEffect.Type("NarrowHorizon", true);
                StellarSky = new RoomSettings.RoomEffect.Type("StellarSky", true);
                IntegralField = new RoomSettings.RoomEffect.Type("IntegralField", true);
            }
            public static void UnregisterValues()
            {
                if (NarrowHorizon != null) { NarrowHorizon.Unregister(); NarrowHorizon = null; }
                if (StellarSky != null) { StellarSky.Unregister(); StellarSky = null; }
                if (IntegralField != null) { IntegralField.Unregister(); IntegralField = null; }
            }
        }
    }
}