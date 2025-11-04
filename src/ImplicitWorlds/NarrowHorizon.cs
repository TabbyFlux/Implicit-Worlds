namespace ImplicitWorlds
{
    public class NarrowHorizon : BackgroundScene
    {
        public NarrowHorizon(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(42);
            this.effect = effect;
            sceneOrigo = base.RoomToWorldPos(room.abstractRoom.size.ToVector2() * 10f);
            Shader.SetGlobalVector(RainWorld.ShadPropMultiplyColor, Color.white);
            Shader.SetGlobalVector(RainWorld.ShadPropAboveCloudsAtmosphereColor, atmosphereColor);
            Shader.SetGlobalVector(RainWorld.ShadPropSceneOrigoPosition, sceneOrigo);
            Shader.SetGlobalVector(RainWorld.ShadPropMultiplyColor, Color.white);
            AddElement(new Simple2DBackgroundIllustration(this, "iwnh_bkg", new Vector2(683f, 384f)));
            int num = 32;
            base.LoadGraphic("otr_dustdunes", false, false);
            for (int i = 0; i < num; i++)
            {
                float num2 = Mathf.Pow((float)i / (float)(num - 1), 1.75f);
                float num3 = Mathf.Lerp(startDepth, fogDepth, num2);
                float num4 = Mathf.PerlinNoise(num3 / 4f, 0f) * 2000f * num2;
                AddElement(new NarrowHorizon.DustDune(this, -2000f + num4, 0f, num3, 8000f + num4, 400f));
            }
            //AddElement(new Building(this, "iwnh_megastructure", new Vector2(-30f, 0f), 1f, 60f, 0f, 4.2f, 100));
            UnityEngine.Random.state = state;
        }
        public float AtmosphereColorAtDepth(float depth)
        {
            return Mathf.Clamp(depth / 8.2f, 0f, 1f);
        }
        public override void Update(bool eu)
        {
            base.Update(eu);
        }
        public override void Destroy()
        {
            base.Destroy();
        }
        public RoomSettings.RoomEffect effect;
        private float floorLevel = -2000f;
        private static Color atmosphereColor = new Color(0.1f, 0.1f, 0.1f);
        private static float sceneScale = 1f;
        private static float depthScale = 1f;
        private static float startDepth = 2f;
        private static float fogDepth = 30f;
        public Vector2 perspectiveCenter;
        public Vector2 DrawPos(BackgroundSceneElement element, Vector2 camPos, RoomCamera camera)
        {
            Vector2 vector = base.RoomToWorldPos(camera.pos);
            float num = DrawScale(element);
            float num2 = element.pos.x - (vector.x + camPos.x + sceneOrigo.x);
            float num3 = element.pos.y - (vector.y + camPos.y + sceneOrigo.y);
            return new Vector2(num2 * num + perspectiveCenter.x, num3 * num + perspectiveCenter.y);
        }
        public float DrawScale(BackgroundSceneElement element)
        {
            return 1f / (element.depth * depthScale + 1f);
        }
        public class Building : BackgroundSceneElement
        {
            private NarrowHorizon nhScene
            {
                get
                {
                    return scene as NarrowHorizon;
                }
            }
            public Building(NarrowHorizon scene, string assetName, Vector2 pos, float depth, float scale, float rotation, float thickness, int layers) : base(scene, pos, depth)
            {
                this.assetName = assetName;
                this.depth = depth;
                this.scale = scale;
                this.rotation = rotation;
                this.thickness = thickness;
                this.layers = layers;
                scene.LoadGraphic(assetName, true, false);
            }
            public string assetName;
            public float scale;
            public float rotation;
            public float thickness;
            public int layers;
            public float GetDepthForLayer(float layer)
            {
                return depth + layer * thickness;
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[this.layers];
                for(int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    sLeaser.sprites[i] = new FSprite(this.assetName, true);
                    sLeaser.sprites[i].shader = rCam.game.rainWorld.Shaders["AncientUrbanBuilding"];
                    sLeaser.sprites[i].scale = this.scale * (1f / this.GetDepthForLayer(1f - (float)i / (float)this.layers));
                    sLeaser.sprites[i].rotation = this.rotation;
                }
                this.AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                for (int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    float depthForLayer = GetDepthForLayer(1f - (float)i / (float)this.layers);
                    Vector2 vector = this.scene.DrawPos(pos, depthForLayer, camPos, rCam.hDisplace);
                    sLeaser.sprites[i].x = vector.x;
                    sLeaser.sprites[i].y = vector.y;
                    sLeaser.sprites[i].color = new Color(nhScene.AtmosphereColorAtDepth(depthForLayer), 1f - (float)i / (float)this.layers, 0f);
                }
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
        }
        public class DustDune : BackgroundSceneElement
        {
            private NarrowHorizon nhScene
            {
                get
                {
                    return scene as NarrowHorizon;
                }
            }
            public DustDune(NarrowHorizon scene, float x, float y, float depth, float scaleX, float scaleY) : base(scene, new Vector2(x * sceneScale, y * sceneScale), depth)
            {
                this.scaleX = scaleX * sceneScale;
                this.scaleY = scaleY * sceneScale;
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[1];
                sLeaser.sprites[0] = new FSprite("otr_dustdunes", true);
                sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders["DustDunes"];
                float num = nhScene.DrawScale(this);
                sLeaser.sprites[0].scaleX = scaleX * num / sLeaser.sprites[0].textureRect.width;
                sLeaser.sprites[0].scaleY = scaleY * num / sLeaser.sprites[0].textureRect.height;
                sLeaser.sprites[0].anchorX = 0f;
                sLeaser.sprites[0].anchorY = 0.5f;
                sLeaser.sprites[0].color = new Color(1f, 1f, 1f, Mathf.Min(depth / fogDepth, 1f));
                AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                Vector2 vector = nhScene.DrawPos(this, new Vector2(camPos.x, camPos.y), rCam);
                sLeaser.sprites[0].x = vector.x;
                sLeaser.sprites[0].y = vector.y;
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
            public float scaleX;
            public float scaleY;
        }
        public class Smoke : BackgroundSceneElement
        {
            public Smoke(Watcher.AncientUrbanView scene, Vector2 pos, float depth, int index, float flattened, float alpha, float shaderInputColor, bool shaderType) : base(scene, pos, depth)
            {
                this.flattened = flattened;
                this.alpha = alpha;
                this.shaderInputColor = shaderInputColor;
                this.shaderType = shaderType;
                this.randomOffset = UnityEngine.Random.value;
            }
            private float flattened;
            private float alpha;
            private float shaderInputColor;
            private float randomOffset;
            private bool shaderType;
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[1];
                sLeaser.sprites[0] = new FSprite("smoke1", true);
                sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders[this.shaderType ? "Dust" : "CloudDistant"];
                sLeaser.sprites[0].anchorY = 0f;
                this.AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                float num = 2f;
                float y = base.DrawPos(camPos, rCam.hDisplace).y;
                sLeaser.sprites[0].scaleY = this.flattened * num;
                sLeaser.sprites[0].scaleX = num;
                sLeaser.sprites[0].color = new Color(this.shaderInputColor, this.randomOffset, Mathf.Lerp(this.flattened, 1f, 0.5f), this.alpha);
                sLeaser.sprites[0].x = 683f - rCam.hDisplace;
                sLeaser.sprites[0].y = y;
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
        }
    }
}
