using OpenTK;
using OpenTK.Graphics;

namespace BattleshipClient.Engine.Rendering
{
    class Material
    {
        public static Material Default { get; } = new Material();

        public bool Opaque { get; set; } = true;
        public Texture Texture { get; set; } = null;
        public Color4 Color { get; set; } = Color4.White;
        public Vector2 Tiling { get; set; } = new Vector2(1, 1);
    }
}
