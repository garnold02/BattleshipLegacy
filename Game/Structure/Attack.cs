using System.Collections.Generic;

namespace BattleshipClient.Game.Structure
{
    struct Attack
    {
        public List<Player> Owners { get; }
        public int DestinationX { get; }
        public int DestinationY { get; }
        public bool IsHit { get; }

        public Attack(List<Player> owners, int destX, int destY, bool isHit)
        {
            Owners = owners;
            DestinationX = destX;
            DestinationY = destY;
            IsHit = isHit;
        }
    }
}
