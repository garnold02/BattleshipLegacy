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

        private readonly GlParticleInstance[] particleInstances;

        private readonly ParticlePoolRenderer renderer;
        private int firstAvailableSlot = 0;

        public ParticleSubpool(ParticlePool parentPool, string textureName, int maxParticles)
        {
            ParentPool = parentPool;
            TextureName = textureName;
            MaxParticles = maxParticles;
            particleInstances = new GlParticleInstance[MaxParticles];
            renderer = new ParticlePoolRenderer(this);
            firstAvailableSlot = 0;
        }
        public void Update(float delta)
        {
            for (int i = 0; i < MaxParticles; i++)
            {
                ParticleData particleData = particleInstances[i].ParticleData;
                if (particleData.IsAlive)
                {
                    float blend = 1 - particleData.Lifetime / particleData.StartLifetime;

                    Vector3 velocity = particleData.Velocity * delta;
                    float scale = Utility.Lerp(particleData.StartScale, particleData.EndScale, blend);
                    particleInstances[i].Transformation = new Vector4(particleInstances[i].Transformation.Xyz + velocity, scale);
                    if (blend < particleData.ColorBlendSeparator)
                    {
                        particleInstances[i].Color = Vector4.Lerp(particleData.StartColor, particleData.MiddleColor, blend / particleData.ColorBlendSeparator);
                    }
                    else
                    {
                        particleInstances[i].Color = Vector4.Lerp(particleData.MiddleColor, particleData.EndColor, blend / (1f - particleData.ColorBlendSeparator));
                    }
                    particleData.Lifetime -= delta;

                    particleInstances[i].ParticleData = particleData;
                }
                else
                {
                    if (i < firstAvailableSlot)
                    {
                        firstAvailableSlot = i;
                    }
                    particleInstances[i].Transformation = new Vector4(0, -10000, 0, 1);
                }
            }
            renderer.FillBuffer(particleInstances);
        }
        public void Render()
        {
            renderer.Render();
        }
        public void AddParticle(ParticleData particleData)
        {
            particleInstances[firstAvailableSlot] = new GlParticleInstance()
            {
                ParticleData = particleData,
                Transformation = new Vector4(particleData.StartPosition, 1),
                Color = particleData.StartColor
            };
            RecaulculateFirstAvailableSlot();
        }
        private void RecaulculateFirstAvailableSlot()
        {
            for (int i = firstAvailableSlot; i < MaxParticles; i++)
            {
                if (!particleInstances[i].ParticleData.IsAlive)
                {
                    firstAvailableSlot = i;
                    return;
                }
            }
            firstAvailableSlot = 0;
        }
    }
}
