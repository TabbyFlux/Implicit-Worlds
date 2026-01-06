namespace ImplicitWorlds
{
    public static class IWEnums
    {
        public static class RoomEffectType
        {
            public static RoomSettings.RoomEffect.Type NarrowHorizon;
            public static RoomSettings.RoomEffect.Type StellarSky;
            public static RoomSettings.RoomEffect.Type IntegralField;
            public static RoomSettings.RoomEffect.Type EncapsulatedSystems;
            public static void RegisterValues()
            {
                NarrowHorizon = new RoomSettings.RoomEffect.Type("NarrowHorizon", true);
                StellarSky = new RoomSettings.RoomEffect.Type("StellarSky", true);
                IntegralField = new RoomSettings.RoomEffect.Type("IntegralField", true);
                EncapsulatedSystems = new RoomSettings.RoomEffect.Type("EncapsulatedSystems", true);
            }
            public static void UnregisterValues()
            {
                if (NarrowHorizon != null) { NarrowHorizon.Unregister(); NarrowHorizon = null; }
                if (StellarSky != null) { StellarSky.Unregister(); StellarSky = null; }
                if (IntegralField != null) { IntegralField.Unregister(); IntegralField = null; }
                if (EncapsulatedSystems != null) { EncapsulatedSystems.Unregister(); EncapsulatedSystems = null; }
            }
        }
        public static class RoomPOMObjects
        {
            public static PlacedObject.Type GravityRectZone = new(nameof(POMObjects.GravityRectZoneObjectType.GravityRectZone), true);

            public const string IW_POM_OBJECTS_CATEGORY = "ImplicitWorlds";
            public static void RegisterPOMObjects()
            {
                Pom.Pom.RegisterManagedObject(new POMObjects.GravityRectZoneObjectType());
            }
        }
    }
}