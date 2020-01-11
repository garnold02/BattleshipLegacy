using BattleshipClient.Engine;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Game.GameObjects
{
    class Explosion : GameObject
    {
        private readonly ParticleSystem pSystem;
        private float time;
        public Explosion(GameContainer container, Vector3 position) : base(container)
        {
            Transform.localPosition = position;
            pSystem = new ParticleSystem(Container)
            {
                Transform = Transform,
                ParticleProperties = new ParticleProperties
                {
                    TextureName = "smoke",
                    Lifetime = 8,
                    ColorBlendSeparator = 0.25f,
                    StartColor = new Color4(255, 192, 56, 255),
                    MiddleColor = new Color4(64, 57, 52, 255),
                    EndColor = new Color4(64, 57, 52, 0),
                    ConstantForce = new Vector3(0, 1, 0),
                    ForceProbability = new Vector3(2, 1, 2),
                    StartScale = 0.5f,
                    EndScale = 2
                },
                Frequency = 120
            };
            Container.ObjManager.Add(pSystem);
        }

        public override void OnAdded()
        {
        }

        public override void OnRemoved()
        {
        }

        public override void Render()
        {
        }

        public override void Update(float delta)
        {
            time += delta;
            pSystem.Frequency *= 0.9f;
            if (time > 10)
            {
                Container.ObjManager.Remove(pSystem);
                Container.ObjManager.Remove(this);
            }
        }
    }
}
