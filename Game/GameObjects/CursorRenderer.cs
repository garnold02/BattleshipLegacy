using BattleshipClient.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics;

namespace BattleshipClient.Game.GameObjects
{
    class CursorRenderer : GameObject
    {
        private MeshRenderer claimRenderer;
        private ShipRenderer shipRenderer;
        private MeshRenderer stategyCursorRenderer;
        private Renderer currentRenderer;

        public CursorRenderer(GameContainer container) : base(container)
        {
            Depth = 1;
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

        public override void Update()
        {
            SetCurrentRenderer();
            SetRendererPosition();
        }
        private void InitializeRenderers()
        {
            Mesh planeMesh = Assets.Get<Mesh>("plane");
            Shader litShader = Assets.Get<Shader>("f_lit");
            Texture smallGridTexture = Assets.Get<Texture>("grid");
            Texture largeGridTexture = Assets.Get<Texture>("largeGrid");
            claimRenderer = new MeshRenderer(planeMesh, litShader)
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
            stategyCursorRenderer = new MeshRenderer(planeMesh, litShader)
            {
                Material = new Material()
                {
                    Opaque = false,
                    Texture = smallGridTexture,
                    Color = new Color4(1f, 1f, 1f, 0.5f)
                }
            };
            stategyCursorRenderer.Transform.localPosition.Y = 0.03f;
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
                    currentRenderer = stategyCursorRenderer;
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
                            Container.Cursor.ClaimPosition.X * Container.Board.PieceLength - (Container.Board.FullSideLength / 2) + (Container.Board.PieceLength / 2),
                            0.03f, Container.Cursor.ClaimPosition.Y * Container.Board.PieceLength - (Container.Board.FullSideLength / 2) + (Container.Board.PieceLength / 2));
                    break;
                case TurnPhase.ShipPlacement:
                    shipRenderer.SetProperties((int)Container.Cursor.Position.X, (int)Container.Cursor.Position.Y, Container.Cursor.ShipLength, Container.Cursor.IsShipVertical);
                    break;
                case TurnPhase.Strategy:
                    currentRenderer.Transform.localPosition = new Vector3(Container.Cursor.Position.X + 0.5f, 0.03f, Container.Cursor.Position.Y + 0.5f);
                    break;
            }
        }
    }
}
