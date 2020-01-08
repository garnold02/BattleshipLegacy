using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipClient.Engine.Net
{
    struct Packet
    {
        const ushort BufferSize = 256;

        public CommandType Type { get; }
        public byte Length { get; }
        public byte[] Data { get; }
        public byte[] Bytes
        {
            get
            {
                byte[] bytes = new byte[BufferSize];
                bytes[0] = (byte)Type;
                bytes[1] = Length;
                for (int i = 2; i < Length; i++)
                {
                    bytes[i] = Data[i - 2];
                }
                return bytes;
            }
        }
        public Packet(byte[] rawData)
        {
            Type = (CommandType)rawData[0];
            Length = rawData[1];
            Data = new byte[Length - 2];
            for (int i = 2; i < Length; i++)
            {
                Data[i - 2] = rawData[i];
            }
        }
        public Packet(CommandType type, params PacketChunk[] objects)
        {
            Type = type;
            List<byte> bytes = new List<byte>();
            if (objects.Length > 0)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    PacketChunk chunkObject = objects[i];
                    byte[] b = new byte[0];
                    if (chunkObject.Type == typeof(string))
                    {
                        b = Encoding.ASCII.GetBytes(chunkObject.Data.ToString());
                    }
                    else if (chunkObject.Type == typeof(int))
                    {
                        b = BitConverter.GetBytes((int)chunkObject.Data);
                    }

                    bytes.Add((byte)b.Length);
                    bytes.AddRange(b);
                }
            }
            Length = (byte)(bytes.Count + 2);
            Data = bytes.ToArray();
        }
        public byte[][] GetChunks()
        {
            List<byte[]> chunks = new List<byte[]>();
            if (Data.Length >= 2)
            {
                List<byte> currentByteArray = new List<byte>();
                int currentLength = Data[0];
                for (int i = 1; i < Data.Length; i++)
                {
                    byte b = Data[i];
                    if (currentByteArray.Count == currentLength)
                    {
                        chunks.Add(currentByteArray.ToArray());
                        currentByteArray = new List<byte>();
                        currentLength = b;
                    }
                    else
                    {
                        currentByteArray.Add(b);
                    }
                }
                if (currentByteArray.Count > 0)
                {
                    chunks.Add(currentByteArray.ToArray());
                }
            }
            return chunks.ToArray();
        }
    }
}
