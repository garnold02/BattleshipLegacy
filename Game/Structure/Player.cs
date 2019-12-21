using BattleshipClient.Game.GameObjects;
using System.Collections.Generic;

namespace BattleshipClient.Game.Structure
{
    class Player
    {
        public string Name { get; }
        public Board Board { get; }
        public BoardPiece BoardClaim { get; set; }
        public List<Ship> Ships { get; }

        public PlayerRenderer Renderer { get; set; }
        public Player(Board board, string name)
        {
            Name = name;
            Board = board;

            Ships = new List<Ship>();
        }
        public void AddShip(Ship ship)
        {
            Ships.Add(ship);
            Renderer.CreateShipRenderer(ship);
        }
    }
}
