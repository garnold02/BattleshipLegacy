namespace BattleshipClient.Engine.Net
{
    abstract class Chunk
    {
        public ChunkType type;
        protected object typelessData;

        public Chunk(ChunkType type, object data)
        {
            this.type = type;
            this.typelessData = data;
        }
    }
}
