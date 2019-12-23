using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Engine
{
    struct ParticleProperties
    {
        public string TextureName { get; set; }
        public float Lifetime { get; set; }
        public float StartScale { get; set; }
        public float EndScale { get; set; }
        public Vector3 ConstantForce { get; set; }
        public Vector3 ForceProbability { get; set; }
        public Color4 StartColor { get; set; }
        public Color4 EndColor { get; set; }
    }
}
