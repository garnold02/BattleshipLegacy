using OpenTK;
using OpenTK.Graphics;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Engine;

namespace BattleshipClient.Game.GameObjects
{
    class BoardRenderer : GameObject
    {
        private readonly MeshRenderer oceanPlaneRenderer;
        private readonly MeshRenderer oceanFloorRenderer;
        private readonly MeshRenderer smallGridRenderer;
        private readonly MeshRenderer largeGridRenderer;
        private readonly MeshRenderer claimRenderer;
        private float elapsedTime;

        public BoardRenderer(GameContainer container) : base(container)
        {
            Depth = 2;

            Mesh planeModel = Assets.Get<Mesh>("plane");
            oceanPlaneRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_water"), Assets.Get<Shader>("f_noise"), Assets.Get<Shader>("f_water"))
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
                    localPosition = new Vector3(0, -24, 0),
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
            oceanFloorRenderer.Render();
            oceanPlaneRenderer.Render();
            smallGridRenderer.Render();
            largeGridRenderer.Render();
            claimRenderer.Render();
        }
        public void SetClaimPosition(int x, int y)
        {
            claimRenderer.Transform.localPosition = new Vector3((x + 0.5f) * Container.Board.PieceLength - Container.Board.FullSideLength / 2, 0.025f, (y + 0.5f) * Container.Board.PieceLength - Container.Board.FullSideLength / 2);
        }
    }
}
