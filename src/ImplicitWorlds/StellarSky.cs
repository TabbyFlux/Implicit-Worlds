using System;
using UnityEngine;

namespace ImplicitWorlds
{
    public class StellarSky : BackgroundScene //The Stellar Sky in Stellar Railways
    {
        public StellarSky(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(1);
            this.effect = effect;
            Shader.SetGlobalVector(RainWorld.ShadPropAboveCloudsAtmosphereColor, this.atmosphereColor);
            this.AddElement(new Simple2DBackgroundIllustration(this, "iwsr_sky", new Vector2(683f, 384f)));
            base.LoadGraphic("otr_firmamentcloud", false, false);
            UnityEngine.Random.state = state;
        }
        public RoomSettings.RoomEffect effect;
        public Color atmosphereColor = new Color(28, 38, 62);
    }
}
