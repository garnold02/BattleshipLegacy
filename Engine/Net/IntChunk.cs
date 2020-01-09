namespace BattleshipClient.Engine.Net
{
    class IntChunk : Chunk
    {
        public int Data => (int)typelessData;
        public IntChunk(int data) : base (ChunkType.Int, data)
        {

        }
    }
}
