using BattleshipClient.Engine;
using BattleshipClient.Game.Structure;

namespace BattleshipClient.Game.GameObjects
{
    abstract class GameObject
    {
        public Board Board { get; }
        public Transform Transform { get; set; } = Transform.Identity;

        public GameObject(Board board)
        {
            Board = board;
        }
        public abstract void OnAdded();
        public abstract void OnRemoved();
        public abstract void Update();
        public abstract void Render();
    }
}
