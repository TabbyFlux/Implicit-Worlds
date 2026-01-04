using System.Collections.Generic;

namespace ImplicitWorlds
{
    public class StellarSky : BackgroundScene
    {
        public StellarSky(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(1);
            this.effect = effect;
            Shader.SetGlobalVector(RainWorld.ShadPropAboveCloudsAtmosphereColor, atmosphereColor);
            Shader.SetGlobalVector(RainWorld.ShadPropMultiplyColor, Color.white);
            perspectiveCenter = new Vector2(room.game.rainWorld.screenSize.x * ConvergenceMult.x, room.game.rainWorld.screenSize.y * ConvergenceMult.y);
            sceneOrigo = ViewOffset;
            sceneScale = ViewScale;
            AddElement(new Simple2DBackgroundIllustration(this, "iwsr_sky", new Vector2(683f, 384f)));
            clouds = new List<StellarCloud>();
            LoadGraphic("clouds1", false, false);
            LoadGraphic("clouds2", false, false);
            LoadGraphic("clouds3", false, false);
            int totalCloseClouds = 7;
            for (int i = 0; i < totalCloseClouds; i++)
            {
                float cloudDepth = (float)i / (float)(totalCloseClouds - 1);
                AddElement(new StellarCloseCloud(this, new Vector2(0f, 0f), cloudDepth, i));
            }
            int totalDistantClouds = 11;
            for (int j = 0;  j < totalDistantClouds; j++)
            {
                float distantCloudDepth = (float)j / (float)(totalDistantClouds - 1);
                AddElement(new StellarDistantCloud(this, new Vector2(0f, -40f * cloudsEndDepth * (1f - distantCloudDepth)), distantCloudDepth, j));
            }
            LoadGraphic("otr_firmamentcloud", false, false);
            float totalSideClouds = 8f;
            int k = 0;
            while (k < totalSideClouds)
            {
                float depth = k / totalSideClouds * endDepth;
                AddElement(new SideCloud(this, CloudPos.x + CloudOffset.x * (k / totalSideClouds), CloudPos.y + CloudOffset.y * (k / totalSideClouds), depth, CloudScale.x, CloudScale.y));
                k++;
            }
            UnityEngine.Random.state = state;
        }
        public RoomSettings.RoomEffect effect;
        private List<StellarCloud> clouds;
        private static float startAltitude = 0f;
        private static float endAltitude = 11400f;
        private static float cloudsStartDepth = 5f;
        private static float cloudsEndDepth = 40f;
        private static float distantCloudsEndDepth = 200f;
        private static float yShift;
        private static Vector2 ViewOffset = new Vector2(-200f, 50f);
        private static Vector2 ConvergenceMult = new Vector2(0.4f, 0.5f);
        private static Vector2 CloudPos = new Vector2(-6000f, 600f);
        private static Vector2 CloudScale = new Vector2(12f, 8f);
        private static Vector2 CloudOffset = new Vector2(20000f, 2000f);
        public Vector2 perspectiveCenter;
        private static float ViewScale = 12f;
        private static float sceneScale = 1f;
        private static float depthScale = 1f;
        private static float endDepth = 160f;
        private static Color atmosphereColor = new Color(0.1254901961f, 0.1254901961f, 0.231372549f);
        public override void Update(bool eu)
        {
            base.Update(eu);
        }
        public override void Destroy()
        {
            base.Destroy();
        }
        public override void AddElement(BackgroundSceneElement element)
        {
            if (element is StellarCloud)
            {
                clouds.Add(element as StellarCloud);
            }
            base.AddElement(element);
        }
        private float CloudDepth(float f)
        {
            return Mathf.Lerp(cloudsStartDepth, cloudsEndDepth, f);
        }
        private float DistantCloudDepth(float f)
        {
            return Mathf.Lerp(cloudsEndDepth, distantCloudsEndDepth, Mathf.Pow(f, 1.5f));
        }
        private float DrawScale(BackgroundSceneElement element)
        {
            return 1f / (element.depth * depthScale + 1f);
        }
        private Vector2 DrawPos(BackgroundSceneElement element, Vector2 camPos, RoomCamera camera)
        {
            Vector2 vector = RoomToWorldPos(camera.pos);
            float num = DrawScale(element);
            float num2 = element.pos.x - (vector.x + camPos.x + sceneOrigo.x);
            float num3 = element.pos.y - (vector.y + camPos.y + sceneOrigo.y);
            return new Vector2(num2 * num + perspectiveCenter.x, num3 * num + perspectiveCenter.y);
        }
        public abstract class StellarCloud : BackgroundSceneElement
        {
            public StellarSky stScene
            {
                get
                {
                    return scene as StellarSky;
                }
            }
            public StellarCloud(StellarSky scene, Vector2 pos, float depth, int index) : base(scene, pos, depth)
            {
                randomOffset = UnityEngine.Random.value;
                this.index = index;
            }
            public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
            {
                skyColor = palette.skyColor;
            }
            public float randomOffset;
            public Color skyColor;
            public int index;
        }
        public class StellarCloseCloud : StellarCloud
        {
            public StellarCloseCloud(StellarSky scene, Vector2 pos, float cloudDepth, int index) : base(scene, pos, scene.CloudDepth(cloudDepth), index)
            {
                this.cloudDepth = cloudDepth;
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[2];
                sLeaser.sprites[0] = new FSprite("pixel", true);
                sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders["Background"];
                sLeaser.sprites[0].anchorY = 1f;
                sLeaser.sprites[0].scaleX = 1400f;
                sLeaser.sprites[0].scaleY = 500f;
                sLeaser.sprites[0].x = 683f;
                sLeaser.sprites[0].y = 0f;
                sLeaser.sprites[1] = new FSprite("clouds" + (index % 3 + 1).ToString(), true);
                sLeaser.sprites[1].shader = rCam.game.rainWorld.Shaders["Cloud"];
                sLeaser.sprites[1].anchorY = 1f;
                AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                float y = scene.RoomToWorldPos(rCam.room.cameraPositions[rCam.currentCameraPosition]).y;
                float num = Mathf.InverseLerp(startAltitude, endAltitude, y);
                float num2 = cloudDepth;
                if (num > 0.5f)
                {
                    num2 = Mathf.Lerp(num2, 1f, Mathf.InverseLerp(0.5f, 1f, num) * 0.5f);
                }
                depth = Mathf.Lerp(cloudsStartDepth, cloudsEndDepth, num2);
                float scaleX = Mathf.Lerp(10f, 2f, num2);
                float posY = base.DrawPos(new Vector2(camPos.x, camPos.y + yShift), rCam.hDisplace).y;
                posY += Mathf.Lerp(Mathf.Pow(cloudDepth, 0.75f), Mathf.Sin(cloudDepth * 3.1415927f), 0.5f) * Mathf.InverseLerp(0.5f, 0f, num) * 600f;
                posY -= Mathf.InverseLerp(0.18f, 0.1f, num) * Mathf.Pow(1f - cloudDepth, 3f) * 100f;
                float scaleY = Mathf.Lerp(1f, Mathf.Lerp(0.75f, 0.25f, num), num2);

                posY += -100f + 200f * num2;
                scaleY = 1f;

                sLeaser.sprites[1].scaleY = scaleY * scaleX;
                sLeaser.sprites[1].scaleX = scaleX;
                //sLeaser.sprites[1].color = atmosphereColor;
                sLeaser.sprites[1].color = new Color(num2 * 0.75f, randomOffset, Mathf.Lerp(scaleY, 1f, 0.5f), 1f);
                sLeaser.sprites[1].x = 683f;
                sLeaser.sprites[1].y = posY - 2f;
                sLeaser.sprites[0].x = 683f;
                sLeaser.sprites[0].y = posY - sLeaser.sprites[1].height;
                //sLeaser.sprites[0].color = atmosphereColor;
                sLeaser.sprites[0].color = Color.Lerp(skyColor, atmosphereColor, num2 * 0.75f);
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
            private float cloudDepth;
        }
        public class StellarDistantCloud : StellarCloud
        {
            public StellarDistantCloud(StellarSky scene, Vector2 pos, float distantCloudDepth, int index) : base(scene, pos, scene.DistantCloudDepth(distantCloudDepth), index)
            {
                this.distantCloudDepth = distantCloudDepth;
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[2];
                sLeaser.sprites[0] = new FSprite("pixel", true);
                sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders["Background"];
                sLeaser.sprites[0].anchorY = 0f;
                sLeaser.sprites[0].scaleX = 1400f;
                sLeaser.sprites[0].x = 683f;
                sLeaser.sprites[0].y = 0f;
                sLeaser.sprites[1] = new FSprite("clouds" + (index % 3 + 1).ToString(), true);
                sLeaser.sprites[1].shader = rCam.game.rainWorld.Shaders["CloudDistant"];
                sLeaser.sprites[1].anchorY = 1f;
                AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                float value = scene.RoomToWorldPos(rCam.room.cameraPositions[rCam.currentCameraPosition]).y + yShift;
                sLeaser.sprites[1].isVisible = true;
                sLeaser.sprites[0].isVisible = true;
                float num = 2f;
                float y = base.DrawPos(new Vector2(camPos.x, camPos.y + yShift), rCam.hDisplace).y;
                float num2 = Mathf.Lerp(0.3f, 0.01f, distantCloudDepth);
                if (index == 8)
                {
                    num2 *= 1.5f;
                }
                sLeaser.sprites[0].scaleY = y - 150f * num * num2;
                sLeaser.sprites[1].scaleY = num2 * num;
                sLeaser.sprites[1].scaleX = num;
                //sLeaser.sprites[1].color = atmosphereColor;
                sLeaser.sprites[1].color = new Color(Mathf.Lerp(0.75f, 0.95f, distantCloudDepth), randomOffset, Mathf.Lerp(num2, 1f, 0.5f), 1f);
                sLeaser.sprites[1].x = 683f;
                sLeaser.sprites[1].y = y - 2f;
                //sLeaser.sprites[0].color = atmosphereColor;
                sLeaser.sprites[0].color = Color.Lerp(skyColor, atmosphereColor, Mathf.Lerp(0.75f, 0.95f, distantCloudDepth));
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
            private float distantCloudDepth;
        }
        public class SideCloud : BackgroundSceneElement
        {
            private StellarSky stScene
            {
                get
                {
                    return scene as StellarSky;
                }
            }
            public SideCloud(StellarSky scene, float x, float y, float depth, float scaleX, float scaleY) : base(scene, new Vector2(x * sceneScale, y * sceneScale), depth)
            {
                this.scaleX = scaleX * sceneScale;
                this.scaleY = scaleY * sceneScale;
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[1];
                sLeaser.sprites[0] = new FSprite("otr_firmamentcloud", true);
                sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders["FirmamentCloud"];
                float num = stScene.DrawScale(this);
                sLeaser.sprites[0].scaleX = num * scaleX;
                sLeaser.sprites[0].scaleY = num * scaleY;
                sLeaser.sprites[0].anchorX = 1f;
                sLeaser.sprites[0].anchorY = 0.25f;
                AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                Vector2 vector = stScene.DrawPos(this, new Vector2(camPos.x, camPos.y), rCam);
                sLeaser.sprites[0].x = vector.x;
                sLeaser.sprites[0].y = vector.y;
                sLeaser.sprites[0].color = new Color(1f, 1f, 1f, Mathf.Min(depth / endDepth, 1f));
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
            private float scaleX;
            private float scaleY;
        }
        public class DistantBuilding : BackgroundSceneElement
        {
            private StellarSky stScene
            {
                get
                {
                    return scene as StellarSky;
                }
            }
            public DistantBuilding(StellarSky scene, string assetName, float x, float y, float depth, float scale, float atmoDepthAdd, float rotation) : base(scene, new Vector2(x * sceneScale, y * sceneScale), depth)
            {
                this.scale = scale * sceneScale;
                this.rotation = rotation;
                this.atmoDepthAdd = atmoDepthAdd;
                this.assetName = assetName;
                scene.LoadGraphic(assetName, true, false);
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[1];
                sLeaser.sprites[0] = new FSprite(assetName, true);
                sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders["DistantBkgObject"];
                sLeaser.sprites[0].anchorY = 0f;
                sLeaser.sprites[0].scale = scale * stScene.DrawScale(this);
                sLeaser.sprites[0].rotation = rotation;
                AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                Vector2 vector = stScene.DrawPos(this, new Vector2(camPos.x, camPos.y + yShift), rCam);
                sLeaser.sprites[0].x = vector.x;
                sLeaser.sprites[0].y = vector.y;
                sLeaser.sprites[0].color = new Color(Mathf.Pow(Mathf.InverseLerp(0f, 600f, depth + atmoDepthAdd), 0.3f) * 0.9f, 0f, 0f);
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
            private string assetName;
            private float scale;
            private float rotation;
            private float atmoDepthAdd;
        }
    }
}
