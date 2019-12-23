using System.Linq;
using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using OpenTK;

namespace BattleshipClient.Game.RegularObjects.ParticleSystems
{
    class ParticleSubpool
    {
        public ParticlePool ParentPool { get; }
        public string TextureName { get; }
        public int MaxParticles { get; }

        private readonly ParticleData[] particleProperties;
        private readonly Vector4[] particleTranslations;
        private readonly Vector4[] particleColors;

        private ParticlePoolRenderer renderer;
        private int firstAvailableSlot = 0;

        public ParticleSubpool(ParticlePool parentPool, string textureName, int maxParticles)
        {
            ParentPool = parentPool;
            TextureName = textureName;
            MaxParticles = maxParticles;

            particleProperties = new ParticleData[MaxParticles];
            particleTranslations = new Vector4[MaxParticles];
            particleColors = new Vector4[MaxParticles];

            renderer = new ParticlePoolRenderer(this);
            firstAvailableSlot = 0;
        }
        public void Update(float delta)
        {
            for (int i = 0; i < MaxParticles; i++)
            {
                ParticleData particleData = particleProperties[i];
                if (particleData.IsAlive)
                {
                    float blend = 1 - particleData.Lifetime / particleData.StartLifetime;

                    Vector3 velocity = particleData.Velocity * delta;
                    float scale = Utility.Lerp(particleData.StartScale, particleData.EndScale, blend);
                    particleTranslations[i] += new Vector4(velocity, 0);
                    particleTranslations[i].W = scale;
                    particleColors[i] = Vector4.Lerp(particleData.StartColor, particleData.EndColor, blend);
                    particleProperties[i].Lifetime -= delta;
                }
                else
                {
                    if (i < firstAvailableSlot)
                    {
                        firstAvailableSlot = i;
                    }
                    particleTranslations[i] = new Vector4(0, -10000, 0, 1);
                }
            }
            renderer.FillBuffers(particleTranslations, particleColors);
        }
        public void Render()
        {
            renderer.Render();
        }
        public void AddParticle(ParticleData particleData)
        {
            particleProperties[firstAvailableSlot] = particleData;
            particleTranslations[firstAvailableSlot] = new Vector4(particleData.StartPosition, particleData.StartScale);
            particleColors[firstAvailableSlot] = particleData.StartColor;
            RecaulculateFirstAvailableSlot();
        }
        private void RecaulculateFirstAvailableSlot()
        {
            for (int i = firstAvailableSlot; i < MaxParticles; i++)
            {
                if (!particleProperties[i].IsAlive)
                {
                    firstAvailableSlot = i;
                    return;
                }
            }
            firstAvailableSlot = 0;
        }
    }
}
