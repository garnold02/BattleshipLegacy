using BattleshipClient.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics;

namespace BattleshipClient.Game.GameObjects
{
    class CursorRenderer : GameObject
    {
        private MeshRenderer claimRenderer;
        private ShipRenderer shipRenderer;
        private MeshRenderer strategyCursorRenderer;
        private Renderer currentRenderer;

        public CursorRenderer(GameContainer container) : base(container)
        {
            Depth = 3;
            InitializeRenderers();
        }

        public override void OnAdded()
        {

        }

        public override void OnRemoved()
        {

        }

        public override void Render()
        {
            currentRenderer?.Render();
        }

        public override void Update(float delta)
        {
            SetCurrentRenderer();
            SetRendererPosition();
        }
        private void InitializeRenderers()
        {
            Mesh planeMesh = Assets.Get<Mesh>("plane");
            Shader vertexShader = Assets.Get<Shader>("v_neutral");
            Shader fragmentShader = Assets.Get<Shader>("f_lit");
            Texture smallGridTexture = Assets.Get<Texture>("grid");
            Texture largeGridTexture = Assets.Get<Texture>("largeGrid");
            claimRenderer = new MeshRenderer(planeMesh, vertexShader, fragmentShader)
            {
                Material = new Material()
                {
                    Opaque = false,
                    Texture = largeGridTexture,
                    Color = new Color4(0.25f, 1f, 0.5f, 0.5f)
                }
            };
            claimRenderer.Transform.localPosition.Y = 0.03f;
            claimRenderer.Transform.localScale = Vector3.One * Container.Board.PieceLength;
            shipRenderer = new ShipRenderer(Container.Board);
            strategyCursorRenderer = new MeshRenderer(planeMesh, vertexShader, fragmentShader)
            {
                Material = new Material()
                {
                    Opaque = false,
                    Texture = smallGridTexture,
                    Color = new Color4(1f, 1f, 1f, 0.5f)
                }
            };
            strategyCursorRenderer.Transform.localPosition.Y = 0.03f;
        }
        private void SetCurrentRenderer()
        {
            switch (Container.TurnManager.Phase)
            {
                case TurnPhase.LandClaiming:
                    currentRenderer = claimRenderer;
                    break;
                case TurnPhase.ShipPlacement:
                    currentRenderer = shipRenderer;
                    break;
                case TurnPhase.Strategy:
                    currentRenderer = strategyCursorRenderer;
                    break;
                default:
                    currentRenderer = null;
                    break;
            }
        }
        private void SetRendererPosition()
        {
            switch (Container.TurnManager.Phase)
            {
                case TurnPhase.LandClaiming:
                    currentRenderer.Transform.localPosition =
                        new Vector3(
                            Container.CursorCtrl.ClaimPosition.X * Container.Board.PieceLength - (Container.Board.FullSideLength / 2) + (Container.Board.PieceLength / 2),
                            0.04f, Container.CursorCtrl.ClaimPosition.Y * Container.Board.PieceLength - (Container.Board.FullSideLength / 2) + (Container.Board.PieceLength / 2));
                    break;
                case TurnPhase.ShipPlacement:
                    shipRenderer.SetProperties((int)Container.CursorCtrl.Position.X, (int)Container.CursorCtrl.Position.Y, Container.CursorCtrl.ShipLength, Container.CursorCtrl.IsShipVertical);
                    break;
                case TurnPhase.Strategy:
                    strategyCursorRenderer.Transform.localPosition = new Vector3(Container.CursorCtrl.Position.X - Container.Board.FullSideLength/2 + 0.5f, 0.04f, Container.CursorCtrl.Position.Y - Container.Board.FullSideLength / 2 + 0.5f);
                    break;
            }
        }
    }
}
