namespace BattleshipClient.Engine.Net
{
    class ByteChunk : Chunk
    {
        public byte Data => (byte)typelessData;
        public ByteChunk(byte data) : base(ChunkType.Byte, data)
        {

        }
    }
}
