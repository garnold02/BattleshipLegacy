using OpenTK;

namespace BattleshipClient.Game.RegularObjects.ParticleSystems
{
    struct ParticleData
    {
        public static int SizeInBytes => sizeof(float) * 5 + Vector3.SizeInBytes * 2 + Vector4.SizeInBytes * 3;

        public bool IsAlive => Lifetime >= 0;
        public float StartLifetime { get; set; }
        public float Lifetime { get; set; }
        public float ColorBlendSeparator { get; set; }
        public Vector3 StartPosition { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector4 StartColor { get; set; }
        public Vector4 MiddleColor { get; set; }
        public Vector4 EndColor { get; set; }
        public float StartScale { get; set; }
        public float EndScale { get; set; }
    }
}
