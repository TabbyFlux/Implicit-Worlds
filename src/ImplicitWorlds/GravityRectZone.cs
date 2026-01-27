using RegionKit.Modules.ShelterBehaviors;
using RWCustom;

namespace ImplicitWorlds.POMObjects
{
    public class GravityRectZoneObjectType : Pom.Pom.ManagedObjectType
    {
        public GravityRectZoneObjectType() : base(nameof(GravityRectZone), IWEnums.RoomPOMObjects.IW_POM_OBJECTS_CATEGORY, typeof(GravityRectZone), typeof(GravityRectZoneData), typeof(GravityRectZoneRepresentation))
        {
        }
        public override UpdatableAndDeletable MakeObject(PlacedObject pObj, Room room)
        {
            return new GravityRectZone(pObj, room);
        }
        public class GravityRectZone : UpdatableAndDeletable
        {
            public GravityRectZone(PlacedObject pObj, Room room)
            {
                this.pObj = pObj;
                this.room = room;
                //this.pObj.active = true;
                room.AddObject(this);
                UnityEngine.Debug.Log("[IW]: GravityRectZone created!");
            }
            public IntRect Rect
            {
                get
                {
                    IntRect initialRect = IntRect.MakeFromIntVector2(((GravityRectZoneData)pObj.data).intV2Rect);
                    IntRect resultRect = new IntRect((int)(pObj.data.owner.pos.x / 20), (int)(pObj.data.owner.pos.y / 20), (int)(pObj.data.owner.pos.x / 20) + initialRect.right, (int)(pObj.data.owner.pos.y / 20) + initialRect.top);
                    //UnityEngine.Debug.Log($"[IW]: Rect is calculated! Left: {resultRect.left}, Bottom: {resultRect.bottom}, Right: {resultRect.right}, Top: {resultRect.top}; Height: {resultRect.Height}, Width: {resultRect.Width}, Area: {resultRect.Area}");
                    return resultRect;
                }
            }
            public float InitialGravity
            {
                get
                {
                    float result = 1f;
                    bool isZeroG = false;
                    for (int i = 0; i < room.roomSettings.effects.Count; i++)
                    {
                        if (room.roomSettings.effects[i].type == RoomSettings.RoomEffect.Type.ZeroG)
                        {
                            isZeroG = true;
                            result = room.roomSettings.effects[i].amount;
                        }
                        else if (room.roomSettings.effects[i].type == RoomSettings.RoomEffect.Type.BrokenZeroG)
                        {
                            isZeroG = true;
                            result = room.roomSettings.effects[i].amount;
                        }
                    }
                    if (isZeroG)
                    {
                        return result;
                    }
                    else
                    {
                        return 1f;
                    }
                }
            }
            public override void Update(bool eu)
            {
                base.Update(eu);
                for (int i = 0; i < room.physicalObjects.Length; i++)
                {
                    for (int j = 0; j < room.physicalObjects[i].Count; j++)
                    {
                        for (int k = 0; k < room.physicalObjects[i][j].bodyChunks.Length; k++)
                        {
                            bool wasInsideRect = false;
                            Vector2 vector = room.physicalObjects[i][j].bodyChunks[k].ContactPoint.ToVector2();
                            Vector2 pos = room.physicalObjects[i][j].bodyChunks[k].pos + vector * (room.physicalObjects[i][j].bodyChunks[k].rad + 30f);
                            if (Rect.Contains(room.GetTilePosition(pos)) && room.physicalObjects[i][j] is Player && wasInsideRect == false)
                            {
                                room.gravity = ((GravityRectZoneData)pObj.data).GetValue<float>("value");
                                UnityEngine.Debug.Log("[IW]: Player inside rect!");
                                wasInsideRect = true;
                            }
                            if (!Rect.Contains(room.GetTilePosition(pos), false) && wasInsideRect)
                            {
                                room.gravity = 1f; //InitialGravity;
                                wasInsideRect = false;
                            }
                        }
                    }
                }
            }
            public readonly PlacedObject pObj;
        }
        public class GravityRectZoneData : Pom.Pom.ManagedData
        {
            [BackedByField("rect")]
            public IntVector2 intV2Rect;

            public static Pom.Pom.ManagedField[] managedFields = new Pom.Pom.ManagedField[]
            {
                new Pom.Pom.FloatField("value", 0f, 1f, 0f, 0.01f, control: Pom.Pom.ManagedFieldWithPanel.ControlType.slider, displayName: "Value"),
                new Pom.Pom.IntVector2Field("rect", new IntVector2(3, 3), controlType: Pom.Pom.IntVector2Field.IntVectorReprType.rect)
            };
            public GravityRectZoneData(PlacedObject owner) : base(owner, managedFields)
            {
            }
        }
        public class GravityRectZoneRepresentation : Pom.Pom.ManagedRepresentation
        {
            public GravityRectZoneRepresentation(PlacedObject.Type type, DevInterface.ObjectsPage objPage, PlacedObject pObj) : base(type, objPage, pObj)
            {
            }
            public override void Update()
            {
                base.Update();
            }
        }
    }
}
