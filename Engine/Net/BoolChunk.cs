namespace BattleshipClient.Engine.Net
{
    class BoolChunk : Chunk
    {
        public bool Data => (bool)typelessData;
        public BoolChunk(bool data) : base(ChunkType.Bool, data)
        {

        }
    }
}
