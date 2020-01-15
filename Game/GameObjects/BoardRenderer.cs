using OpenTK;
using OpenTK.Graphics;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Engine;
using System.Collections.Generic;
using BattleshipClient.Game.Structure;
using System.Drawing;

namespace BattleshipClient.Game.GameObjects
{
    class BoardRenderer : GameObject
    {
        private readonly MeshRenderer oceanPlaneRenderer;
        private readonly MeshRenderer oceanFloorRenderer;
        private readonly MeshRenderer smallGridRenderer;
        private readonly MeshRenderer largeGridRenderer;
        private readonly MeshRenderer claimMapRenderer;
        private readonly MeshRenderer claimRenderer;
        private readonly ParticleSystem[,] smokeSystems;
        private readonly List<ShipRenderer> sunkShips;
        private float elapsedTime;

        public BoardRenderer(GameContainer container) : base(container)
        {
            Depth = 2;
            smokeSystems = new ParticleSystem[Container.Board.FullSideLength, Container.Board.FullSideLength];
            sunkShips = new List<ShipRenderer>();

            Mesh planeModel = Assets.Get<Mesh>("plane");
            oceanPlaneRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(70, 174, 212, 120),
                },
                Transform = new Transform()
                {
                    localScale = Vector3.One * 500
                }
            };
            oceanFloorRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("oceanFloor"),
                    Tiling = Vector2.One * 50
                },
                Transform = new Transform()
                {
                    localPosition = new Vector3(0, -4, 0),
                    localScale = Vector3.One * 500
                }
            };
            smallGridRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(1f, 1f, 1f, 0.04f),
                    Texture = Assets.Get<Texture>("grid"),
                    Tiling = new Vector2(Container.Board.FullSideLength, Container.Board.FullSideLength)
                },
                Transform = new Transform()
                {
                    localPosition = new Vector3(0, 0.01f, 0),
                    localScale = Vector3.One * Container.Board.FullSideLength
                }
            };
            largeGridRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(1f, 1f, 1f, 0.05f),
                    Texture = Assets.Get<Texture>("largeGrid"),
                    Tiling = new Vector2(Container.Board.SideLength, Container.Board.SideLength)
                },
                Transform = new Transform()
                {
                    localPosition = new Vector3(0, 0.02f, 0),
                    localScale = Vector3.One * Container.Board.FullSideLength
                }
            };
            claimMapRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = true,
                    Color = Color4.Transparent
                },
                Transform = new Transform()
                {
                    localPosition = new Vector3(0, 0.03f, 0),
                    localScale = Vector3.One * Container.Board.FullSideLength
                }
            };
            claimRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(0.1f, 1f, 0.3f, 0.25f),
                    Texture = Assets.Get<Texture>("largeGrid"),
                },
                Transform = new Transform()
                {
                    localPosition = new Vector3(0, 1000000, 0),
                    localScale = Vector3.One * Container.Board.PieceLength
                }
            };
        }
        public override void OnAdded()
        {

        }
        public override void OnRemoved()
        {

        }
        public override void Update(float delta)
        {
            oceanPlaneRenderer.SetUniformFloat("time", elapsedTime);
            elapsedTime += delta;
        }
        public override void Render()
        {
            RenderSunkShips();

            oceanFloorRenderer.Render();
            oceanPlaneRenderer.Render();
            smallGridRenderer.Render();
            largeGridRenderer.Render();
            claimMapRenderer.Render();
            claimRenderer.Render();
        }
        public void SetClaimPosition(int x, int y)
        {
            claimRenderer.Transform.localPosition = new Vector3((x + 0.5f) * Container.Board.PieceLength - Container.Board.FullSideLength / 2, 0.035f, (y + 0.5f) * Container.Board.PieceLength - Container.Board.FullSideLength / 2);
        }
        public void SetClaimMapTexture(Bitmap bitmap)
        {
            if (claimMapRenderer.Material.Texture == null)
            {
                claimMapRenderer.Material.Texture = Texture.Load(bitmap);
            }
            else
            {
                claimMapRenderer.Material.Texture.Update(bitmap);

            }
            claimMapRenderer.Material.Color = new Color4(255, 255, 255, 64);
        }
        public void SetSmoke(int x, int y, bool on)
        {
            if (on)
            {
                if (smokeSystems[x, y] == null)
                {
                    smokeSystems[x, y] = new ParticleSystem(Container)
                    {
                        Transform = new Transform()
                        {
                            localPosition = new Vector3(x - Container.Board.FullSideLength / 2 + 0.5f, 0, y - Container.Board.FullSideLength / 2 + 0.5f)
                        },
                        Frequency = 4,
                        ParticleProperties = new ParticleProperties()
                        {
                            TextureName = "smoke",
                            Lifetime = 6,
                            StartScale = 0.2f,
                            EndScale = 0.8f,
                            StartColor = new Color4(100, 100, 100, 0),
                            MiddleColor = new Color4(100, 100, 100, 255),
                            EndColor = new Color4(160, 160, 160, 0),
                            ColorBlendSeparator = 0.1f,
                            ConstantForce = new Vector3(0, 1, 0),
                            ForceProbability = new Vector3(0.1f, 0, 0.1f)
                        }
                    };
                    Container.ObjManager.Add(smokeSystems[x, y]);
                }
            }
            else
            {
                if (smokeSystems[x, y] != null)
                {
                    Container.ObjManager.Remove(smokeSystems[x, y]);
                    smokeSystems[x, y] = null;
                }
            }
        }
        public void AddSunkShip(Ship ship)
        {
            ShipRenderer renderer = new ShipRenderer(Container.Board);
            renderer.SetProperties(ship);
            renderer.SetSunk();
            sunkShips.Add(renderer);
        }
        private void RenderSunkShips()
        {
            foreach (ShipRenderer renderer in sunkShips)
            {
                renderer.Render();
            }
        }
    }
}
