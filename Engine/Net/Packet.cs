using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipClient.Engine.Net
{
    class Packet
    {
        public const ushort BufferSize = 512;

        public PacketType Type { get; }
        public byte Length { get; }
        public byte[] Data { get; }
        public Packet(byte[] rawData)
        {
            Type = (PacketType)rawData[0];
            Length = rawData[1];
            Data = new byte[BufferSize];
            for (int i = 0; i < Length; i++)
            {
                Data[i] = rawData[i];
            }
        }
        public Packet(PacketType type, params Chunk[] chunks)
        {
            Type = type;
            List<byte> bytes = new List<byte>
            {
                (byte)Type
            };
            if (chunks.Length > 0)
            {
                for (int i = 0; i < chunks.Length; i++)
                {
                    Chunk chunkObject = chunks[i];
                    bytes.Add((byte)chunkObject.type);
                    byte[] b = SerializeChunkData(chunkObject);
                    bytes.Add((byte)b.Length);
                    bytes.AddRange(b);
                }
            }
            Length = (byte)(bytes.Count + 1);
            bytes.Insert(1, Length);
            while (bytes.Count < BufferSize)
            {
                bytes.Add(0);
            }
            Data = bytes.ToArray();
        }
        public Chunk[] GetChunks()
        {
            List<Chunk> chunkList = new List<Chunk>();

            if (Data.Length > 4 && Data[2] > 0)
            {
                ChunkType chunkType = (ChunkType)Data[2];
                byte chunkDataLength = Data[3];
                List<byte> chunkByteList = new List<byte>();
                for (int i = 4; i < Length + 1; i++)
                {
                    if (chunkByteList.Count < chunkDataLength)
                    {
                        chunkByteList.Add(Data[i]);
                    }
                    else
                    {
                        chunkList.Add(DeserializeChunk(chunkType, chunkByteList.ToArray()));
                        if (i < Length - 1)
                        {
                            chunkType = (ChunkType)Data[i];
                            chunkDataLength = Data[i + 1];
                            chunkByteList.Clear();
                            i++;
                        }
                    }
                }
            }
            return chunkList.ToArray();
        }
        private Chunk DeserializeChunk(ChunkType type, byte[] data)
        {
            Chunk chunk = null;
            switch (type)
            {
                case ChunkType.Int:
                    chunk = new IntChunk(BitConverter.ToInt32(data, 0));
                    break;
                case ChunkType.Byte:
                    chunk = new ByteChunk(data[0]);
                    break;
                case ChunkType.String:
                    chunk = new StringChunk(Encoding.ASCII.GetString(data));
                    break;
                case ChunkType.Bool:
                    chunk = new BoolChunk(BitConverter.ToBoolean(data, 0));
                    break;
            }
            return chunk;
        }
        private byte[] SerializeChunkData(Chunk chunk)
        {
            byte[] bytes = null;
            if (chunk is IntChunk intChunk)
            {
                bytes = BitConverter.GetBytes(intChunk.Data);
            }
            else if (chunk is ByteChunk byteChunk)
            {
                bytes = new byte[1] { byteChunk.Data };
            }
            else if (chunk is StringChunk stringChunk)
            {
                bytes = Encoding.ASCII.GetBytes(stringChunk.Data);
            }
            else if (chunk is BoolChunk boolChunk)
            {
                bytes = BitConverter.GetBytes(boolChunk.Data);
            }
            return bytes;
        }
    }
}
