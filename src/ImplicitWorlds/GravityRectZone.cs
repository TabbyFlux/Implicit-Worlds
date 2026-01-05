using RWCustom;

namespace ImplicitWorlds.POMObjects
{
    public class GravityRectZone : UpdatableAndDeletable
    {
        public static void RegisterObject()
        {
            Pom.Pom.RegisterFullyManagedObjectType(managedFields, typeof(GravityRectZone), "GravityRectZone", IWEnums.RoomPOMObjects.IW_POM_OBJECTS_CATEGORY);
        }
        public GravityRectZone(Room room, PlacedObject pObj)
        {
            this.room = room;
            this.pObj = pObj;
            this.data = pObj.data as Pom.Pom.ManagedData;
            room.AddObject(this);
        }
        public float value
        {
            get
            {
                return data.GetValue<float>("Value");
            }
        }
        public IntRect Rect
        {
            get
            {
                rect = data.GetValue<IntVector2>("rect");
                return new IntRect(Math.Min((int)data.owner.pos.x, (int)data.owner.pos.x + rect.x), Math.Min((int)data.owner.pos.y, (int)data.owner.pos.y + rect.y), Math.Max((int)data.owner.pos.x, (int)data.owner.pos.x + rect.x), Math.Max((int)data.owner.pos.y, (int)data.owner.pos.y + rect.y));
            }
        }
        public override void Update(bool eu)
        {
            base.Update(eu);
            for (int i = 0; i < room.physicalObjects.Length; i++)
            {
                for (int j = 0; j < room.physicalObjects[i].Count; j++)
                {
                    if (Rect.Includes(room.GetTilePosition(room.physicalObjects[i][j].bodyChunks[0].pos)) && room.physicalObjects[i][j] is Player)
                    {
                        room.gravity = value;
                    }
                }
            }
        }
        internal static Pom.Pom.ManagedField[] managedFields = new Pom.Pom.ManagedField[]
        {
            new Pom.Pom.FloatField("Value", 0f, 1f, 0f, 0.01f, control: Pom.Pom.ManagedFieldWithPanel.ControlType.slider, null),
            new Pom.Pom.IntVector2Field("rect", new IntVector2(3, 3), controlType: Pom.Pom.IntVector2Field.IntVectorReprType.rect)
        };
        public Pom.Pom.ManagedData data;
        public PlacedObject pObj;
        public IntVector2 rect;
    }
}
