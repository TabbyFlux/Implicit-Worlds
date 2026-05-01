namespace ImplicitWorlds
{
    public class EncapsulatedSystems : BackgroundScene
    {
        public EncapsulatedSystems(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            this.effect = effect;
            sceneOrigo = new Vector2(room.Width / 2, room.Height / 2);
            Shader.SetGlobalVector(RainWorld.ShadPropMultiplyColor, Color.white);
            Shader.SetGlobalVector(RainWorld.ShadPropSceneOrigoPosition, sceneOrigo);
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(0);

            UnityEngine.Random.state = state;
        }
        public RoomSettings.RoomEffect effect;
        public static float sceneScale = 1f;
        public override void Update(bool eu)
        {
            base.Update(eu);
        }
        public override void Destroy()
        {
            base.Destroy();
        }
        public class Walls : BackgroundSceneElement
        {
            private EncapsulatedSystems esScene
            {
                get
                {
                    return scene as EncapsulatedSystems;
                }
            }
            public Walls(EncapsulatedSystems scene, string assetName, float x, float y, float depth, float scale, float rotation) : base(scene, new Vector2(x * sceneScale, y * sceneScale), depth)
            {
                this.assetName = assetName;
                this.scale = scale * sceneScale;
                this.rotation = rotation;
            }
            public float getDepthForLayer(float layer)
            {
                return depth + layer * thickness;
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[3];
                for (int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    sLeaser.sprites[i] = new FSprite(assetName, true);
                    sLeaser.sprites[i].shader = rCam.game.rainWorld.Shaders["AncientUrbanBuilding"];
                    sLeaser.sprites[i].anchorY = 0f;
                    sLeaser.sprites[i].scale = scale * (1f / getDepthForLayer(1f - (i / 3f)));
                    sLeaser.sprites[i].rotation = rotation;
                }
                AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                for (int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    float depthForLayer = getDepthForLayer(1f - (i / 3f));
                    Vector2 vector = scene.DrawPos(pos, depthForLayer, camPos, rCam.hDisplace);
                    sLeaser.sprites[i].x = vector.x;
                    sLeaser.sprites[i].y = vector.y;
                }
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
            public string assetName;
            public float scale;
            public float rotation;
            public float thickness;
        }
    }
}
