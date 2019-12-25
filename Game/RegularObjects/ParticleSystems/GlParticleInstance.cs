using OpenTK;

namespace BattleshipClient.Game.RegularObjects.ParticleSystems
{
    struct GlParticleInstance
    {
        public static int SizeInBytes => ParticleData.SizeInBytes + Vector4.SizeInBytes * 2;

        public ParticleData ParticleData { get; set; }
        public Vector4 Transformation { get; set; }
        public Vector4 Color { get; set; }
    }
}
