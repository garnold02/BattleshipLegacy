using BattleshipClient.Game;
using BattleshipClient.Game.Structure;
using OpenTK;

namespace BattleshipClient.Engine.Rendering
{
    class ShipRenderer : Renderer
    {
        public Board Board { get; }
        public Vector3 Position;
        public int Length { get; private set; }
        public bool IsVertical { get; private set; }

        private readonly MeshRenderer frontRenderer;
        private readonly MeshRenderer middleRenderer;
        private readonly MeshRenderer backRenderer;
        public ShipRenderer(Board board)
        {
            Board = board;

            frontRenderer = new MeshRenderer(Assets.Get<Mesh>("ship_front"), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("ship")
                }
            };
            middleRenderer = new MeshRenderer(Assets.Get<Mesh>("ship_middle"), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("ship")
                }
            };
            backRenderer = new MeshRenderer(Assets.Get<Mesh>("ship_back"), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("ship")
                }
            };
        }
        public void SetProperties(Ship from)
        {
            SetProperties(from.PositionX, from.PositionY, from.Length, from.IsVertical);
        }
        public void SetProperties(int positionX, int positionY, int length, bool isVertical)
        {
            Position = new Vector3(positionX - Board.FullSideLength / 2, 0, positionY - Board.FullSideLength / 2);
            Length = length;
            IsVertical = isVertical;

            AdjustTransforms();
        }
        public override void Render()
        {
            frontRenderer.Render();
            for (int i = 0; i < Length - 2; i++)
            {
                middleRenderer.Transform.localPosition = new Vector3(Position.X + 0.5f + (IsVertical ? 0 : i + 1), Position.Y, Position.Z + 0.5f + (IsVertical ? i + 1 : 0));
                middleRenderer.Render();
            }
            backRenderer.Render();
        }
        public override void Delete()
        {
            frontRenderer.Delete();
            middleRenderer.Delete();
            backRenderer.Delete();
        }

        private void AdjustTransforms()
        {
            frontRenderer.Transform.localPosition = new Vector3(Position.X + 0.5f, Position.Y, Position.Z + 0.5f);
            frontRenderer.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? -MathHelper.Pi / 2 : 0, 0);

            middleRenderer.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? -MathHelper.Pi / 2 : 0, 0);

            backRenderer.Transform.localPosition = new Vector3(Position.X + 0.5f + (IsVertical ? 0 : Length - 1), Position.Y, Position.Z + 0.5f + (IsVertical ? Length - 1 : 0));
            backRenderer.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? -MathHelper.Pi / 2 : 0, 0);
        }
    }
}
