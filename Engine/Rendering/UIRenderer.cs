using BattleshipClient.Engine.UI;
using BattleshipClient.Game;
using BattleshipClient.Game.RegularObjects;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace BattleshipClient.Engine.Rendering
{
    class UIRenderer : Renderer
    {
        public Vector4 Transformation { get; set; }
        public Vector4 UvTransformation { get; set; } = new Vector4(0, 0, 1, 1);
        public Color4 Color { get; set; }
        public Texture Texture { get; set; }

        private UIElement Owner { get; }
        private readonly Mesh mesh;
        private readonly Dictionary<string, int> uniformLocations;
        private int glProgram;
        private int vao;
        public UIRenderer(UIElement owner)
        {
            Owner = owner;
            mesh = Assets.Get<Mesh>("particle");
            uniformLocations = new Dictionary<string, int>();

            SetupProgram();
            SetupVAO();
        }
        public override void Delete()
        {

        }
        public override void Render()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.UseProgram(glProgram);
            GL.BindVertexArray(vao);
            GL.BindTexture(TextureTarget.Texture2D, Texture.glTexture);
            GL.ActiveTexture(TextureUnit.Texture0);

            SetUniformMatrix4("projection", Owner.Manager.Projection);
            SetUniformVector4("color", new Vector4(Color.R, Color.G, Color.B, Color.A));
            SetUniformVector4("transformation", Transformation);
            SetUniformVector4("uvTransformation", UvTransformation);

            GL.DrawElements(PrimitiveType.Triangles, mesh.faces.Length, DrawElementsType.UnsignedInt, mesh.faces);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
            GL.Enable(EnableCap.DepthTest);
        }
        private void SetupProgram()
        {
            Shader vertexShader = Assets.Get<Shader>("v_ui");
            Shader fragmentShader = Assets.Get<Shader>("f_ui");

            glProgram = GL.CreateProgram();
            GL.AttachShader(glProgram, vertexShader.GlShader);
            GL.AttachShader(glProgram, fragmentShader.GlShader);
            GL.LinkProgram(glProgram);
            GL.DetachShader(glProgram, vertexShader.GlShader);
            GL.DetachShader(glProgram, fragmentShader.GlShader);
        }
        private void SetupVAO()
        {
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VertexBuffer);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector3.SizeInBytes);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector3.SizeInBytes * 2);

            GL.BindVertexArray(0);
        }
        public void SetUniformFloat(string name, float value)
        {
            int uniformLocation = FindUniformLocation(name);
            GL.Uniform1(uniformLocation, value);
        }
        public void SetUniformVector2(string name, Vector2 value)
        {
            int uniformLocation = FindUniformLocation(name);
            GL.Uniform2(uniformLocation, value);
        }
        public void SetUniformVector3(string name, Vector3 value)
        {
            int uniformLocation = FindUniformLocation(name);
            GL.Uniform3(uniformLocation, value);
        }
        public void SetUniformVector4(string name, Vector4 value)
        {
            int uniformLocation = FindUniformLocation(name);
            GL.Uniform4(uniformLocation, value);
        }
        public void SetUniformMatrix4(string name, Matrix4 value)
        {
            int uniformLocation = FindUniformLocation(name);
            GL.UniformMatrix4(uniformLocation, false, ref value);
        }

        private int FindUniformLocation(string name)
        {
            if (!uniformLocations.ContainsKey(name))
            {
                //If the uniform was not used previously, find it and add it to the cache
                int uniformLocation = GL.GetUniformLocation(glProgram, name);
                uniformLocations.Add(name, uniformLocation);
            }

            return uniformLocations[name];
        }
    }
}
