using OpenTK;

namespace BattleshipClient.Engine.Rendering
{
    struct Vertex
    {
        public static int SizeInBytes { get => Vector3.SizeInBytes + Vector3.SizeInBytes + Vector2.SizeInBytes; }
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 Uv { get; set; }
    }
}
