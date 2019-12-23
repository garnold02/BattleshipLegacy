using System.Collections.Generic;
using BattleshipClient.Engine;
using BattleshipClient.Game.GameObjects;
using BattleshipClient.Game.RegularObjects.ParticleSystems;
using OpenTK;

namespace BattleshipClient.Game.RegularObjects
{
    class ParticlePool : RegularObject
    {
        private readonly Dictionary<string, ParticleSubpool> subpools;
        public ParticlePool(GameContainer container) : base(container)
        {
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
                StartPosition = system.Transform.Position,
                StartScale = properties.StartScale,
                EndScale = properties.EndScale,
                StartColor = new Vector4(properties.StartColor.R, properties.StartColor.G, properties.StartColor.B, properties.StartColor.A),
                EndColor = new Vector4(properties.EndColor.R, properties.EndColor.G, properties.EndColor.B, properties.EndColor.A),
                Velocity = properties.ConstantForce
            };

            ParticleSubpool subpool = FindOrCreateSubpool(properties.TextureName);
            subpool.AddParticle(data);
        }

        private ParticleSubpool FindOrCreateSubpool(string textureName)
        {
            if (!subpools.ContainsKey(textureName))
            {
                subpools.Add(textureName, new ParticleSubpool(this, textureName, 256));
            }
            return subpools[textureName];
        }
    }
}