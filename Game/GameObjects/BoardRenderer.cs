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

        public BoardRenderer(GameContainer container) : base(container)
        {
            Depth = 2;

            Mesh planeModel = Assets.Get<Mesh>("plane");
            oceanPlaneRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_water"), Assets.Get<Shader>("f_water"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(0.2f, 0.5f, 0.8f, 0.4f),
                    Normal = Assets.Get<Texture>("waterNormal"),
                    Tiling = Vector2.One * 250
                },
                Transform = new Transform()
                {
                    localScale = Vector3.One * 1000
                }
            };
            oceanFloorRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("oceanFloor"),
                    Tiling = Vector2.One * 250
                },
                Transform = new Transform()
                {
                    localScale = Vector3.One * 1000
                }
            };
            smallGridRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(1f, 1f, 1f, 0.03f),
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
                    Color = new Color4(1f, 1f, 1f, 0.04f),
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
            oceanPlaneRenderer.Transform.localPosition = new Vector3(((int)Container.CameraCtrl.CurrentPosition.X / 8) * 8, 0, ((int)Container.CameraCtrl.CurrentPosition.Z / 8) * 8);
            oceanFloorRenderer.Transform.localPosition = new Vector3(((int)Container.CameraCtrl.CurrentPosition.X / 4) * 4, -24, ((int)Container.CameraCtrl.CurrentPosition.Z / 4) * 4);
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
