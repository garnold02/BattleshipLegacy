using System.Collections.Generic;

namespace BattleshipClient.Game.Structure
{
    struct Attack
    {
        public List<Player> Owners { get; }
        public List<StrategyOption> Types { get; }
        public int DestinationX { get; }
        public int DestinationY { get; }
        public bool IsHit { get; }

        public Attack(List<Player> owners, List<StrategyOption> types, int destX, int destY, bool isHit)
        {
            Types = types;
            Owners = owners;
            DestinationX = destX;
            DestinationY = destY;
            IsHit = isHit;
        }
    }
}
