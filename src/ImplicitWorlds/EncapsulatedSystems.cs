namespace ImplicitWorlds
{
    public class EncapsulatedSystems : BackgroundScene
    {
        public EncapsulatedSystems(Room room, RoomSettings.RoomEffect effect) : base(room)
        {
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(1);
            this.effect = effect;

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
    }
}
