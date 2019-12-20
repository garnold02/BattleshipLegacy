using BattleshipClient.Engine;

namespace BattleshipClient.Game.GameObjects
{
    abstract class GameObject
    {
        public Transform Transform { get; set; } = Transform.Identity;

        public abstract void OnAdded();
        public abstract void OnRemoved();
        public abstract void Update();
        public abstract void Render();
    }
}
