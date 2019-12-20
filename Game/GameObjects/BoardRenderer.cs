using OpenTK;
using OpenTK.Graphics;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;
using BattleshipClient.Engine;

namespace BattleshipClient.Game.GameObjects
{
    class BoardRenderer : GameObject
    {
        public Board BoardReference;

        private MeshRenderer oceanPlaneRenderer;
        private MeshRenderer smallGridRenderer;
        private MeshRenderer largeGridRenderer;

        public BoardRenderer(Board boardReference)
        {
            BoardReference = boardReference;
        }
        public override void OnAdded()
        {
            Mesh planeModel = Assets.Get<Mesh>("plane");
            oceanPlaneRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(0.4f, 0.7f, 0.9f, 0.4f)
                }
            };
            oceanPlaneRenderer.Transform = new Transform()
            {
                localScale = Vector3.One * BoardReference.FullSideLength
            };
            smallGridRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(1f, 1f, 1f, 0.02f),
                    Texture = Assets.Get<Texture>("grid"),
                    Tiling = new Vector2(BoardReference.FullSideLength, BoardReference.FullSideLength)
                }
            };
            smallGridRenderer.Transform = new Transform()
            {
                localPosition = new Vector3(0, 0.01f, 0),
                localScale = Vector3.One * BoardReference.FullSideLength
            };
            largeGridRenderer = new MeshRenderer(planeModel, Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Opaque = false,
                    Color = new Color4(1f, 1f, 1f, 0.02f),
                    Texture = Assets.Get<Texture>("largeGrid"),
                    Tiling = new Vector2(BoardReference.SideLength, BoardReference.SideLength)
                }
            };
            largeGridRenderer.Transform = new Transform()
            {
                localPosition = new Vector3(0, 0.02f, 0),
                localScale = Vector3.One * BoardReference.FullSideLength
            };
        }
        public override void OnRemoved()
        {

        }
        public override void Update()
        {

        }
        public override void Render()
        {
            oceanPlaneRenderer.Render();
            smallGridRenderer.Render();
            largeGridRenderer.Render();
        }
    }
}
