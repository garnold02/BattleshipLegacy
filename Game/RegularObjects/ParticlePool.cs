using System;
using System.Collections.Generic;
using OpenTK;
using BattleshipClient.Engine;
using BattleshipClient.Game.GameObjects;
using BattleshipClient.Game.RegularObjects.ParticleSystems;

namespace BattleshipClient.Game.RegularObjects
{
    class ParticlePool : RegularObject
    {
        private readonly Random random;
        private readonly Dictionary<string, ParticleSubpool> subpools;
        public ParticlePool(GameContainer container) : base(container)
        {
            random = new Random();
            subpools = new Dictionary<string, ParticleSubpool>();
        }
        public override void Update(float delta)
        {
            foreach (KeyValuePair<string, ParticleSubpool> kvp in subpools)
            {
                kvp.Value.Update(delta);
            }
        }
        public void Render()
        {
            foreach (KeyValuePair<string, ParticleSubpool> kvp in subpools)
            {
                kvp.Value.Render();
            }
        }
        public void AddParticle(ParticleSystem system)
        {
            ParticleProperties properties = system.ParticleProperties;
            ParticleData data = new ParticleData()
            {
                StartLifetime = properties.Lifetime,
                Lifetime = properties.Lifetime,
                ColorBlendSeparator = properties.ColorBlendSeparator,
                StartPosition = system.Transform.Position,
                StartScale = properties.StartScale,
                EndScale = properties.EndScale,
                StartColor = new Vector4(properties.StartColor.R, properties.StartColor.G, properties.StartColor.B, properties.StartColor.A),
                MiddleColor = new Vector4(properties.MiddleColor.R, properties.MiddleColor.G, properties.MiddleColor.B, properties.MiddleColor.A),
                EndColor = new Vector4(properties.EndColor.R, properties.EndColor.G, properties.EndColor.B, properties.EndColor.A),
                Velocity = properties.ConstantForce + new Vector3(properties.ForceProbability.X * RandomFloat(), properties.ForceProbability.Y * RandomFloat(), properties.ForceProbability.Z * RandomFloat())
            };

            ParticleSubpool subpool = FindOrCreateSubpool(properties.TextureName);
            subpool.AddParticle(data);
        }

        private ParticleSubpool FindOrCreateSubpool(string textureName)
        {
            if (!subpools.ContainsKey(textureName))
            {
                subpools.Add(textureName, new ParticleSubpool(this, textureName, 65536));
            }
            return subpools[textureName];
        }
        private float RandomFloat()
        {
            return (float)random.NextDouble() - 0.5f;
        }
    }
}