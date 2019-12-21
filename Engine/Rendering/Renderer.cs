namespace BattleshipClient.Engine.Rendering
{
    abstract class Renderer
    {
        public Transform Transform { get; set; } = Transform.Identity;
        public abstract void Render();
    }
}
