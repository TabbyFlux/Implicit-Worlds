using System;
using UnityEngine;

namespace IncoherentWorlds
{
    public class NarrowHorizon : BackgroundScene //The Narrow Horizon view
    {
        public NarrowHorizon(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            this.effect = effect;
            this.isNarrowHorizon = (effect.type == IWEnums.NarrowHorizon);
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(0);
            if (isNarrowHorizon)
            {
                this.sceneOrigo = base.RoomToWorldPos(room.abstractRoom.size.ToVector2() * 10f);
                Shader.SetGlobalVector(RainWorld.ShadPropMultiplyColor, Color.white);
                Shader.SetGlobalVector(RainWorld.ShadPropAboveCloudsAtmosphereColor, this.atmosphereColor);
                Shader.SetGlobalVector(RainWorld.ShadPropSceneOrigoPosition, this.sceneOrigo);
                Shader.SetGlobalVector(RainWorld.ShadPropMultiplyColor, Color.white);
                this.AddElement(new Simple2DBackgroundIllustration(this, "iwnh_bkg", new Vector2(683f, 384f)));
                this.AddElement(new Building(this, "iwnh_megastructure", new Vector2(-30f, 0f), 1f, 100f, 0f, 4.2f, 200));
            }
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
        public bool isNarrowHorizon;
        public float floorLevel = -2000f;
        public Color atmosphereColor = new Color(0.1f, 0.1f, 0.1f);
        public class Building : BackgroundSceneElement
        {
            private NarrowHorizon auScene
            {
                get
                {
                    return this.scene as NarrowHorizon;
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
            public float getDepthForLayer(float layer)
            {
                return this.depth + layer * this.thickness;
            }
            public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
            {
                sLeaser.sprites = new FSprite[this.layers];
                for(int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    sLeaser.sprites[i] = new FSprite(this.assetName, true);
                    sLeaser.sprites[i].shader = rCam.game.rainWorld.Shaders["AncientUrbanBuilding"];
                    sLeaser.sprites[i].scale = this.scale * (1f / this.getDepthForLayer(1f - (float)i / (float)this.layers));
                    sLeaser.sprites[i].rotation = this.rotation;
                }
                this.AddToContainer(sLeaser, rCam, null);
            }
            public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
            {
                for (int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    float depthForLayer = this.getDepthForLayer(1f - (float)i / (float)this.layers);
                    Vector2 vector = this.scene.DrawPos(this.pos, depthForLayer, camPos, rCam.hDisplace);
                    sLeaser.sprites[i].x = vector.x;
                    sLeaser.sprites[i].y = vector.y;
                    sLeaser.sprites[i].color = new Color(this.auScene.AtmosphereColorAtDepth(depthForLayer), 1f - (float)i / (float)this.layers, 0f);
                }
                base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
            }
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
