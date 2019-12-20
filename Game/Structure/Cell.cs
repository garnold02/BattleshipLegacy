namespace BattleshipClient.Game.Structure
{
    class Cell
    {
        public bool HasShip => Ship != null;

        Ship Ship { get; set; }
        bool IsHit { get; set; }
    }
}
