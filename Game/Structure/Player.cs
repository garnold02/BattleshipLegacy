using System.Collections.Generic;
using BattleshipClient.Game.GameObjects;

namespace BattleshipClient.Game.Structure
{
    class Player
    {
        public string Name { get; }
        public byte ID { get; }
        public int Score { get; set; }
        public Board Board { get; }
        public BoardPiece BoardClaim { get; set; }
        public List<Ship> Ships { get; }

        public PlayerRenderer Renderer { get; set; }
        public Player(Board board, string name, byte id)
        {
            Name = name;
            ID = id;
            Board = board;

            Ships = new List<Ship>();
        }
        public void AddShip(Ship ship)
        {
            Ships.Add(ship);
            Renderer.CreateShipRenderer(ship);
        }
        public void AddAttackIndicator(Attack attack)
        {
            Renderer.CreateAttackRenderer(attack);
        }
        public void ClearAttackIndicators()
        {
            Renderer.attackRenderers.Clear();
        }
    }
}
