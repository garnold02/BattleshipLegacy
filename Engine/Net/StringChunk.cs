using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Engine.Net
{
    class StringChunk : Chunk
    {
        public string Data => typelessData.ToString();
        public StringChunk(string data) : base(ChunkType.String, data)
        {

        }
    }
}
