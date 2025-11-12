namespace ImplicitWorlds
{
    public class NarrowHorizon : BackgroundScene
    {
        public NarrowHorizon(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(0);
            this.effect = effect;
            //sceneOrigo = base.RoomToWorldPos(room.abstractRoom.size.ToVector2() * 10f);
            sceneOrigo = ViewOffset;
            perspectiveCenter = new Vector2(room.game.rainWorld.screenSize.x * ConvergenceMult.x, room.game.rainWorld.screenSize.y * ConvergenceMult.y);
            sceneScale = ViewScale;
            depthScale = ViewDepthMultiplier;
            Shader.SetGlobalVector(RainWorld.ShadPropAboveCloudsAtmosphereColor, atmosphereColor);
            AddElement(new Simple2DBackgroundIllustration(this, "iwnh_bkg", new Vector2(683f, 384f)));
            int totalDunes = 128;
            base.LoadGraphic("otr_dustdunes", false, false);
            for (int i = 0; i < totalDunes; i++)
            {
                float interpolation = Mathf.Pow((float)i / (float)(totalDunes - 1), 1.75f);
                float depth = Mathf.Lerp(startDepth, fogDepth, interpolation);
                float offsetX = Mathf.PerlinNoise(depth / 4f, 0f) * 2000f * interpolation;
                AddElement(new NarrowHorizon.SandDune(this, posX + offsetX, 0f, depth, 8000f + offsetX, 600f));
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
        private float posX = -2000f;
        private static Color atmosphereColor = new Color(1f, 0.666f, 0.435f);
        private static Vector2 ConvergenceMult = new Vector2(0.4f, 0.5f);
        private static Vector2 ViewOffset = new Vector2(0f, 0f);
        private static float ViewScale = 10f;
        private static float ViewDepthMultiplier = 8f;
        private static float sceneScale = 1f;
        private static float depthScale = 1f;
        private static float startDepth = 2f;
        private static float fogDepth = 80f;
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
                sLeaser.sprites = new FSprite[layers];
                for(int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    sLeaser.sprites[i] = new FSprite(assetName, true);
                    sLeaser.sprites[i].shader = rCam.game.rainWorld.Shaders["AncientUrbanBuilding"];
                    sLeaser.sprites[i].scale = scale * (1f / GetDepthForLayer(1f - (float)i / (float)layers));
                    sLeaser.sprites[i].rotation = rotation;
                }
                this.AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                for (int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    float depthForLayer = GetDepthForLayer(1f - (float)i / (float)layers);
                    Vector2 vector = scene.DrawPos(pos, depthForLayer, camPos, rCam.hDisplace);
                    sLeaser.sprites[i].x = vector.x;
                    sLeaser.sprites[i].y = vector.y;
                    sLeaser.sprites[i].color = new Color(nhScene.AtmosphereColorAtDepth(depthForLayer), 1f - (float)i / (float)layers, 0f);
                }
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
        }
        public class SandDune : BackgroundSceneElement
        {
            private NarrowHorizon nhScene
            {
                get
                {
                    return scene as NarrowHorizon;
                }
            }
            public SandDune(NarrowHorizon scene, float x, float y, float depth, float scaleX, float scaleY) : base(scene, new Vector2(x * sceneScale, y * sceneScale), depth)
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
                sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders[shaderType ? "Dust" : "CloudDistant"];
                sLeaser.sprites[0].anchorY = 0f;
                AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                float num = 2f;
                float y = base.DrawPos(camPos, rCam.hDisplace).y;
                sLeaser.sprites[0].scaleY = flattened * num;
                sLeaser.sprites[0].scaleX = num;
                sLeaser.sprites[0].color = new Color(shaderInputColor, randomOffset, Mathf.Lerp(flattened, 1f, 0.5f), alpha);
                sLeaser.sprites[0].x = 683f - rCam.hDisplace;
                sLeaser.sprites[0].y = y;
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
        }
    }
}
