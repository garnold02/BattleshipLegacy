namespace BattleshipClient.Game.Structure
{
    class Player
    {
        public string Name { get; }
        public BoardPiece BoardClaim { get; set; }
        public Player(string name)
        {
            Name = name;
        }
    }
}
