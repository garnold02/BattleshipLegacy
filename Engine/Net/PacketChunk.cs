using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Engine.Net
{
    struct PacketChunk
    {
        public Type Type { get; set; }
        public object Data { get; set; }

        public PacketChunk(Type type, object data)
        {
            Type = type;
            Data = data;
        }
    }
}
