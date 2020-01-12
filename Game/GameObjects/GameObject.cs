using BattleshipClient.Engine;
using BattleshipClient.Game.Structure;

namespace BattleshipClient.Game.GameObjects
{
    abstract class GameObject
    {
        public GameContainer Container { get; }
        public Transform Transform { get; set; } = Transform.Identity;
        public int Depth { get; set; } = 0;

        public GameObject(GameContainer container)
        {
            Container = container;
        }
        public abstract void OnAdded();
        public abstract void OnRemoved();
        public abstract void Update(float delta);
        public abstract void Render();
    }
}
