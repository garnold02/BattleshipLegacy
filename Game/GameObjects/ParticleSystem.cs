using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace BattleshipClient.Game.GameObjects
{
    class ParticleSystem : GameObject
    {
        public ParticleProperties ParticleProperties { get; set; }
        public float Frequency { get; set; }

        private readonly Random random;
        private float particleSpawnTimer;
        public ParticleSystem(GameContainer container) : base(container)
        {
            random = new Random();
            particleSpawnTimer = 0;
        }
        public override void OnAdded()
        {

        }
        public override void OnRemoved()
        {

        }
        public override void Update(float delta)
        {
            if (particleSpawnTimer > 1f / Frequency)
            {
                Container.ParticlePl.AddParticle(this);
                particleSpawnTimer = 0;
            }
            particleSpawnTimer += delta;
        }
        public override void Render()
        {
        }
    }
}
