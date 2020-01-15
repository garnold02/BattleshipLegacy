using System.Collections.Generic;
using BattleshipClient.Game.GameObjects;

namespace BattleshipClient.Game.Structure
{
    class Player
    {
        public string Name { get; }
        public byte ID { get; }
        public int Score { get; set; }
        public int Oil { get; set; }
        public Board Board { get; }
        public BoardPiece BoardClaim { get; set; }
        public List<Ship> Ships { get; }
        public Queue<ActionType> Actions { get; }

        public PlayerRenderer Renderer { get; set; }
        public Player(Board board, string name, byte id)
        {
            Name = name;
            ID = id;
            Board = board;

            Ships = new List<Ship>();
            Actions = new Queue<ActionType>();
        }
        public void AddShip(Ship ship)
        {
            Renderer.CreateShipRenderer(ship);
            Ships.Add(ship);
        }
        public void RemoveShip(Ship ship)
        {
            if(Ships.Contains(ship))
            {
                Ships.Remove(ship);
                ship.Renderer.Delete();
            }
        }
        public void AddActionIndicator(StrategyAction action)
        {
            Renderer.CreateActionRenderer(action);
        }
        public void ClearAttackIndicators()
        {
            Renderer.attackRenderers.Clear();
        }
    }
}
