using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace BattleshipClient.Engine.Rendering
{
    class Shader
    {
        private static void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[SHD] {0}", string.Format(message, parameters));
        }

        public int GlShader { get; }
        public ShaderType Type { get; }

        public Shader(string path, ShaderType type)
        {
            Type = type;
            GlShader = GL.CreateShader(Type);

            GL.ShaderSource(GlShader, File.ReadAllText(path));
            GL.CompileShader(GlShader);

            string compileInfo = GL.GetShaderInfoLog(GlShader);
            if (compileInfo.Length > 0)
            {
                Log("[{0}] {1}", path, compileInfo);
            }
            else
            {
                Log("[{0}] Compiled successfully.", path);
            }
        }
    }
}
