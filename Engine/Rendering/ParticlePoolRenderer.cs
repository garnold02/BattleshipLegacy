using BattleshipClient.Game;
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
        private int vao;
        private int instanceBuffer;
        private int program;
        private readonly int uCamTranslationMatrix;
        private readonly int uCamRotationMatrix;
        private readonly int uCamProjectionMatrix;
        public ParticlePoolRenderer(ParticleSubpool pool)
        {
            Subpool = pool;

            GetAssetReferences();
            CreateGL();
            AttachShaders();
            SetupVAO();

            uCamTranslationMatrix = GL.GetUniformLocation(program, "translation");
            uCamRotationMatrix = GL.GetUniformLocation(program, "rotation");
            uCamProjectionMatrix = GL.GetUniformLocation(program, "projection");
        }
        public override void Render()
        {
            GL.DepthMask(false);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VertexBuffer);
            GL.UseProgram(program);

            Matrix4 translationMatrix = Subpool.ParentPool.Container.CameraCtrl.Camera.Transform.TranslationMatrix;
            Matrix4 rotationMatrix = Subpool.ParentPool.Container.CameraCtrl.Camera.Transform.RotationMatrix;
            Matrix4 projectionMatrix = Subpool.ParentPool.Container.CameraCtrl.Camera.PerspectiveMatrix;
            GL.UniformMatrix4(uCamTranslationMatrix, false, ref translationMatrix);
            GL.UniformMatrix4(uCamRotationMatrix, false, ref rotationMatrix);
            GL.UniformMatrix4(uCamProjectionMatrix, false, ref projectionMatrix);

            GL.BindTexture(TextureTarget.Texture2D, texture.glTexture);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.DrawElementsInstanced(PrimitiveType.Triangles, mesh.faces.Length, DrawElementsType.UnsignedInt, mesh.faces, Subpool.MaxParticles);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
            GL.DepthMask(true);
        }
        public override void Delete()
        {

        }
        public void FillBuffer(GlParticleInstance[] instances)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, instanceBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, instances.Length * GlParticleInstance.SizeInBytes, instances, BufferUsageHint.StreamDraw);
        }

        private void GetAssetReferences()
        {
            mesh = Assets.Get<Mesh>("particle");
            texture = Assets.Get<Texture>(Subpool.TextureName);
            vertexShader = Assets.Get<Shader>("v_billboard");
            fragmentShader = Assets.Get<Shader>("f_particle");
        }
        private void CreateGL()
        {
            vao = GL.GenVertexArray();
            instanceBuffer = GL.GenBuffer();
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
        private void SetupVAO()
        {
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VertexBuffer);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector3.SizeInBytes);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector3.SizeInBytes * 2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, instanceBuffer);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, GlParticleInstance.SizeInBytes, ParticleData.SizeInBytes);
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, GlParticleInstance.SizeInBytes, ParticleData.SizeInBytes + Vector4.SizeInBytes);
            GL.VertexAttribDivisor(3, 1);
            GL.VertexAttribDivisor(4, 1);

            GL.BindVertexArray(0);
        }
    }
}
