using BattleshipClient.Game;
using BattleshipClient.Game.Structure;
using OpenTK;

namespace BattleshipClient.Engine.Rendering
{
    class ShipRenderer : Renderer
    {
        public Board Board { get; }
        public Ship Ship { get; private set; }
        public Vector3 Position;
        public int Length { get; private set; }
        public bool IsVertical { get; private set; }
        public bool[] HitValues { get; private set; }
        private MeshRenderer[] renderers;
        public ShipRenderer(Board board)
        {
            Board = board;
        }
        public void SetProperties(Ship from)
        {
            Ship = from;
            bool[] hitValues = new bool[from.Length];
            for (int i = 0; i < from.Length; i++)
            {
                hitValues[i] = from.Cells[i].IsHit;
            }
            SetProperties(from.PositionX, from.PositionY, from.Length, from.IsVertical, hitValues);
        }
        public void SetProperties(int positionX, int positionY, int length, bool isVertical, bool[] hitValues)
        {
            Position = new Vector3(positionX - Board.FullSideLength / 2, -0.5f, positionY - Board.FullSideLength / 2);
            Length = length;
            IsVertical = isVertical;
            HitValues = hitValues;

            AdjustRenderers();
        }
        public override void Render()
        {
            for (int i = 0; i < Length; i++)
            {
                renderers[i].Transform.localPosition = new Vector3(Position.X + 0.5f + (IsVertical ? 0 : i), Position.Y, Position.Z + 0.5f + (IsVertical ? i : 0));
                renderers[i].Render();
            }
        }
        public override void Delete()
        {
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.Delete();
            }
        }

        private void AdjustRenderers()
        {
            renderers = new MeshRenderer[Length];
            if (Length == 1)
            {
                MeshRenderer r = new MeshRenderer(Assets.Get<Mesh>("oil_rig"), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
                {
                    Material = new Material()
                    {
                        Opaque = false,
                        Texture = Assets.Get<Texture>("oilRig")
                    }
                };
                r.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? MathHelper.Pi : -MathHelper.Pi / 2, 0);
                renderers[0] = r;
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    MeshRenderer r;
                    if (i == 0)
                    {
                        string modelName = HitValues[i] ? "ship_front_ruined" : "ship_front";
                        string textureName = HitValues[i] ? "shipFrontRuined" : "shipFront";
                        r = new MeshRenderer(Assets.Get<Mesh>(modelName), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
                        {
                            Material = new Material()
                            {
                                Opaque = true,
                                Texture = Assets.Get<Texture>(textureName)
                            }
                        };
                        r.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? MathHelper.Pi : -MathHelper.Pi / 2, 0);
                    }
                    else if (i == Length - 1)
                    {
                        string modelName = HitValues[i] ? "ship_back_ruined" : "ship_back";
                        string textureName = HitValues[i] ? "shipBackRuined" : "shipBack";
                        r = new MeshRenderer(Assets.Get<Mesh>(modelName), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
                        {
                            Material = new Material()
                            {
                                Texture = Assets.Get<Texture>(textureName)
                            }
                        };
                        r.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? MathHelper.Pi : -MathHelper.Pi / 2, 0);
                    }
                    else
                    {
                        string modelName = HitValues[i] ? "ship_middle_ruined" : "ship_middle";
                        string textureName = HitValues[i] ? "shipMiddleRuined" : "shipMiddle";
                        r = new MeshRenderer(Assets.Get<Mesh>(modelName), Assets.Get<Shader>("v_neutral"), Assets.Get<Shader>("f_lit"))
                        {
                            Material = new Material()
                            {
                                Texture = Assets.Get<Texture>(textureName)
                            }
                        };
                        r.Transform.localRotation = Quaternion.FromEulerAngles(0, IsVertical ? MathHelper.Pi : -MathHelper.Pi / 2, 0);
                    }
                    renderers[i] = r;
                }
            }
        }
    }
}
