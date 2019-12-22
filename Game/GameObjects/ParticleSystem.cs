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
        protected Vector3 RandomForce => new Vector3((float)(random.NextDouble() - 0.5f) * RandomProbabilities.X, (float)(random.NextDouble() - 0.5f) * RandomProbabilities.Y, (float)(random.NextDouble() - 0.5f) * RandomProbabilities.Z);

        public string TextureName { get; set; }
        public float Intensity { get; set; } = 1;
        public Vector3 StartPosition { get; set; } = Vector3.Zero;
        public Vector3 StartScale { get; set; } = Vector3.One;
        public Vector3 EndScale { get; set; } = Vector3.One;
        public Vector3 ConstantForce { get; set; } = Vector3.Zero;
        public Vector3 RandomProbabilities { get; set; } = Vector3.Zero;
        public Quaternion RotationDelta { get; set; } = Quaternion.Identity;
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public float Lifetime { get; set; } = 1;

        private readonly Random random;
        private readonly List<Particle> livingParticles;
        private readonly List<Particle> deadParticles;
        private float particleSpawnTimer = 0;
        public ParticleSystem(GameContainer container) : base(container)
        {
            Depth = 4;

            random = new Random();
            livingParticles = new List<Particle>();
            deadParticles = new List<Particle>();
        }
        public override void OnAdded()
        {

        }
        public override void OnRemoved()
        {

        }
        public override void Update(float delta)
        {
            if (particleSpawnTimer > 1f / Intensity)
            {
                Particle particle = new Particle(this);
                livingParticles.Add(particle);
                particleSpawnTimer = 0;
            }
            particleSpawnTimer += delta;

            foreach (Particle particle in livingParticles)
            {
                particle.Update(delta);
            }
        }
        public override void Render()
        {
            foreach (Particle particle in livingParticles)
            {
                particle.Render();
            }
            foreach (Particle particle in deadParticles)
            {
                livingParticles.Remove(particle);
            }
        }

        private void RemoveParticle(Particle particle)
        {
            deadParticles.Add(particle);
        }

        class Particle
        {
            private Vector4 StartColorV4 => new Vector4(Owner.StartColor.R, Owner.StartColor.G, Owner.StartColor.B, Owner.StartColor.A);
            private Vector4 EndColorV4 => new Vector4(Owner.EndColor.R, Owner.EndColor.G, Owner.EndColor.B, Owner.EndColor.A);
            private Color4 CurrentColorC4 => new Color4(currentColor.X, currentColor.Y, currentColor.Z, currentColor.W);

            public ParticleSystem Owner { get; }

            private readonly MeshRenderer renderer;
            private Vector3 velocity;
            private Vector4 currentColor;
            private float time;
            public Particle(ParticleSystem system)
            {
                Owner = system;
                velocity = Owner.ConstantForce + Owner.RandomForce;

                renderer = new MeshRenderer(Assets.Get<Mesh>("particle"), Assets.Get<Shader>("f_unlit"))
                {
                    Material = new Material()
                    {
                        Opaque = false,
                        Color = Owner.StartColor,
                        Texture = Assets.Get<Texture>(Owner.TextureName)
                    },
                    Transform = new Transform()
                    {
                        Parent = Owner.Transform,
                        localPosition = Owner.StartPosition,
                        localScale = Owner.StartScale
                    }
                };
            }
            public void Update(float delta)
            {
                float blend = MathHelper.Clamp(time / Owner.Lifetime, 0, 1);

                renderer.Transform.localPosition += velocity * delta;
                renderer.Transform.localRotation = Utility.LookAt(renderer.Transform.Position, Owner.Container.CameraCtrl.Camera.Transform.Position);
                renderer.Transform.localScale = Vector3.Lerp(Owner.StartScale, Owner.EndScale, blend);

                currentColor = Vector4.Lerp(StartColorV4, EndColorV4, blend);
                renderer.Material.Color = CurrentColorC4;

                time += delta;
                if (time > Owner.Lifetime)
                {
                    renderer.Delete();
                    Owner.RemoveParticle(this);
                }
            }
            public void Render()
            {
                renderer.Render();
            }
        }
    }
}
