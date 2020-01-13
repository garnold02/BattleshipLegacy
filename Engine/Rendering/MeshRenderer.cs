using BattleshipClient.Game;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace BattleshipClient.Engine.Rendering
{
    class MeshRenderer : Renderer
    {
        public Mesh Mesh { get; set; }
        public Material Material { get; set; } = Material.Default;

        public bool UseCustomProjection { get; set; }
        public Matrix4 CustomProjection { get; set; }

        private readonly List<Shader> attachedShaders;
        private readonly Dictionary<string, int> uniformLocations;
        private int glProgram;
        private int vao;

        public MeshRenderer(Mesh mesh, params Shader[] shaders)
        {
            Mesh = mesh;
            attachedShaders = new List<Shader>();
            uniformLocations = new Dictionary<string, int>();

            AttachShader(Assets.Get<Shader>("v_projection"));
            if (shaders.Length > 0)
            {
                foreach (Shader shader in shaders)
                {
                    AttachShader(shader);
                }
                Apply();
            }

            SetupVAO();
        }
        public void AttachShader(Shader shader)
        {
            attachedShaders.Add(shader);
        }
        public void Apply()
        {
            glProgram = GL.CreateProgram();
            foreach (Shader shader in attachedShaders)
            {
                GL.AttachShader(glProgram, shader.GlShader);
            }
            GL.LinkProgram(glProgram);
            foreach (Shader shader in attachedShaders)
            {
                GL.DetachShader(glProgram, shader.GlShader);
            }

            attachedShaders.Clear();
        }
        public override void Render()
        {
            if (!Material.Opaque)
            {
                GL.DepthMask(false);
            }
            GL.BindVertexArray(vao);
            GL.UseProgram(glProgram);

            //Set uniforms
            SetUniformMatrix4("translation", Transform.TranslationMatrix);
            SetUniformMatrix4("rotation", Transform.RotationMatrix);
            SetUniformMatrix4("scale", Transform.ScaleMatrix);

            Matrix4 projection = UseCustomProjection ? CustomProjection : Root.GameContainer.CameraCtrl.Camera.Matrix;
            SetUniformMatrix4("projection", projection);

            //Material
            SetUniformVector4("matColor", new Vector4(Material.Color.R, Material.Color.G, Material.Color.B, Material.Color.A));
            SetUniformVector2("matTiling", Material.Tiling);

            if (Material.Texture != null)
            {
                SetUniformInt("tex0", 0);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, Material.Texture.glTexture);
                SetUniformFloat("useTexture", 1);
            }
            else
            {
                SetUniformFloat("useTexture", 0);
            }
            if (Material.Normal != null)
            {
                SetUniformInt("tex1", 1);

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, Material.Normal.glTexture);
                SetUniformFloat("useNormal", 1);
            }
            else
            {
                SetUniformFloat("useNormal", 0);
            }

            //Light
            SetUniformFloat("lightAmbient", 0.25f);
            SetUniformVector3("lightDirection", new Vector3(-0.7071f, -0.7071f, -0.7071f));
            SetUniformVector4("lightColor", Vector4.One);

            GL.DrawElements(PrimitiveType.Triangles, Mesh.faces.Length, DrawElementsType.UnsignedInt, Mesh.faces);

            GL.UseProgram(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);

            GL.DepthMask(true);
        }
        public override void Delete()
        {
            GL.DeleteProgram(glProgram);
        }
        public void SetUniformInt(string name, int value)
        {
            int uniformLocation = FindUniformLocation(name);
            GL.Uniform1(uniformLocation, value);
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
            GL.UseProgram(glProgram);
            if (!uniformLocations.ContainsKey(name))
            {
                //If the uniform was not used previously, find it and add it to the cache
                int uniformLocation = GL.GetUniformLocation(glProgram, name);
                uniformLocations.Add(name, uniformLocation);
            }

            return uniformLocations[name];
        }
        private void SetupVAO()
        {
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Mesh.VertexBuffer);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector3.SizeInBytes);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector3.SizeInBytes * 2);

            GL.BindVertexArray(0);
        }
    }
}
