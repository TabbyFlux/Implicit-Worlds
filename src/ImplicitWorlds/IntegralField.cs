namespace ImplicitWorlds
{
    public class IntegralField : BackgroundScene
    {
        public IntegralField(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(0);
            this.effect = effect;
            perspectiveCenter = new Vector2(room.game.rainWorld.screenSize.x * ConvergenceMult.x, room.game.rainWorld.screenSize.y * ConvergenceMult.y);
            sceneOrigo = ViewOffset;
            sceneScale = ViewScale;
            depthScale = ViewDepthMultiplier;
            UnityEngine.Random.state = state;
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
        private static Vector2 ConvergenceMult = new Vector2(0.4f, 0.5f);
        private static Vector2 ViewOffset = new Vector2(0f, 0f);
        private static float ViewScale = 10f;
        private static float ViewDepthMultiplier = 8f;
        private static float sceneScale = 1f;
        private static float depthScale = 1f;
        private static float startDepth = 0f;
        private static float endDepth = 80f;
        public Vector2 perspectiveCenter;
    }
}
