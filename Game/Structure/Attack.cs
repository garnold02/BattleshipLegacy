using System.Collections.Generic;

namespace BattleshipClient.Game.Structure
{
    struct StrategyAction
    {
        public Player Owner { get; }
        public ActionType Type { get; }
        public int DestinationX { get; }
        public int DestinationY { get; }
        public bool IsHit { get; }

        public StrategyAction(byte x, byte y, bool hit, Player owner, ActionType type)
        {
            Owner = owner;
            Type = type;
            DestinationX = x;
            DestinationY = y;
            IsHit = hit;
        }
    }
}
