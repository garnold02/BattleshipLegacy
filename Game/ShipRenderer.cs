using BattleshipClient.Engine;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;
using OpenTK;

namespace BattleshipClient.Game
{
    class ShipRenderer
    {
        public Board Board { get; }
        public Vector3 Position;
        public int Length { get; private set; }
        public bool IsVertical { get; private set; }

        private MeshRenderer frontRenderer;
        private MeshRenderer middleRenderer;
        private MeshRenderer backRenderer;
        public ShipRenderer(Board board)
        {
            Board = board;

            frontRenderer = new MeshRenderer(Assets.Get<Mesh>("ship_front"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("ship")
                }
            };
            middleRenderer = new MeshRenderer(Assets.Get<Mesh>("ship_middle"), Assets.Get<Shader>("f_lit"))
            {
                Material = new Material()
                {
                    Texture = Assets.Get<Texture>("ship")
                }
            };
            backRenderer = new MeshRenderer(Assets.Get<Mesh>("ship_back"), Assets.Get<Shader>("f_lit"))
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
            IsVertical = IsVertical;

            AdjustTransforms();
        }
        public void Render()
        {
            frontRenderer.Render();
            for (int i = 0; i < Length - 2; i++)
            {
                middleRenderer.Transform.localPosition = new Vector3(Position.X + (IsVertical ? 0 : i), Position.Y, Position.Z + (IsVertical ? i : 0));
                middleRenderer.Render();
            }
            backRenderer.Render();
        }

        private void AdjustTransforms()
        {
            frontRenderer.Transform.localPosition = Position;
            frontRenderer.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? MathHelper.Pi / 2 : 0, 0);

            middleRenderer.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? MathHelper.Pi / 2 : 0, 0);

            backRenderer.Transform.localPosition = new Vector3(Position.X + (IsVertical ? 0 : Length), Position.Y, Position.Z + (IsVertical ? Length : 0));
            backRenderer.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? MathHelper.Pi / 2 : 0, 0);
        }
    }
}
