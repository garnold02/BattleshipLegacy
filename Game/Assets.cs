using System.IO;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using BattleshipClient.Engine.Rendering;

namespace BattleshipClient.Game
{
    class Assets
    {
        private static Dictionary<string, object> assets;
        public static void LoadAll()
        {
            assets = new Dictionary<string, object>();

            LoadMeshes();
            LoadTextures();
            LoadShaders();
        }
        public static T Get<T>(string name)
        {
            object obj = assets[name];

            return (T)obj;
        }

        private static void LoadMeshes()
        {
            string[] filePaths = Directory.GetFiles("../assets/meshes/", "*.obj");
            foreach (string filePath in filePaths)
            {
                string name = Path.GetFileNameWithoutExtension(filePath);

                Mesh mesh = Mesh.Load(filePath);
                assets.Add(name, mesh);
            }
        }
        private static void LoadTextures()
        {
            string[] filePaths = Directory.GetFiles("../assets/textures/", "*.png");
            foreach (string filePath in filePaths)
            {
                string name = Path.GetFileNameWithoutExtension(filePath);

                Texture texture = Texture.Load(filePath);
                assets.Add(name, texture);
            }
        }
        private static void LoadShaders()
        {
            string[] filePaths = Directory.GetFiles("../assets/shaders/", "*.glsl");
            foreach (string filePath in filePaths)
            {
                string name = Path.GetFileNameWithoutExtension(filePath);

                ShaderType type = ShaderType.VertexShader;
                switch (name[0])
                {
                    case 'f':
                        type = ShaderType.FragmentShader;
                        break;
                }
                Shader shader = new Shader(filePath, type);
                assets.Add(name, shader);
            }
        }
    }
}
