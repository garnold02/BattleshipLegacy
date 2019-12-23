using BattleshipClient.Game;
using BattleshipClient.Game.RegularObjects;
using BattleshipClient.Game.RegularObjects.ParticleSystems;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BattleshipClient.Engine.Rendering
{
    class ParticlePoolRenderer : Renderer
    {
        public ParticleSubpool Subpool { get; }

        private Mesh mesh;
        private Texture texture;
        private Shader vertexShader;
        private Shader fragmentShader;
        private int translationBuffer;
        private int colorBuffer;
        private int program;
        private readonly int uProjectionMatrix;
        public ParticlePoolRenderer(ParticleSubpool pool)
        {
            Subpool = pool;

            GetAssetReferences();
            CreateGL();
            AttachShaders();

            uProjectionMatrix = GL.GetUniformLocation(program, "projection");
        }
        public override void Render()
        {
            int size = Vertex.SizeInBytes;

            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VertexBuffer);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, size, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, size, Vector3.SizeInBytes);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, size, Vector3.SizeInBytes * 2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, translationBuffer);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, Vector4.SizeInBytes, 0);
            GL.VertexAttribDivisor(3, 1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VertexBuffer);
            GL.UseProgram(program);

            Matrix4 projectionMatrix = Subpool.ParentPool.Container.CameraCtrl.Camera.Matrix;
            GL.UniformMatrix4(uProjectionMatrix, false, ref projectionMatrix);

            GL.BindTexture(TextureTarget.Texture2D, texture.glTexture);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.DrawElementsInstanced(PrimitiveType.Triangles, mesh.faces.Length, DrawElementsType.UnsignedInt, mesh.faces, Subpool.MaxParticles);
        }
        public override void Delete()
        {

        }
        public void FillBuffers(Vector4[] translations, Vector4[] colors)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, translationBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, translations.Length * Vector4.SizeInBytes, translations, BufferUsageHint.StreamDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * Vector4.SizeInBytes, colors, BufferUsageHint.StreamDraw);
        }

        private void GetAssetReferences()
        {
            mesh = Assets.Get<Mesh>("plane");
            texture = Assets.Get<Texture>(Subpool.TextureName);
            vertexShader = Assets.Get<Shader>("v_billboard");
            fragmentShader = Assets.Get<Shader>("f_particle");
        }
        private void CreateGL()
        {
            translationBuffer = GL.GenBuffer();
            colorBuffer = GL.GenBuffer();
            program = GL.CreateProgram();
        }
        private void AttachShaders()
        {
            GL.AttachShader(program, vertexShader.GlShader);
            GL.AttachShader(program, fragmentShader.GlShader);
            GL.LinkProgram(program);
            GL.DetachShader(program, vertexShader.GlShader);
            GL.DetachShader(program, fragmentShader.GlShader);
        }
    }
}
