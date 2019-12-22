using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using a = Assimp;

namespace BattleshipClient.Engine.Rendering
{
    class Mesh
    {
        public static Mesh Load(string path)
        {
            Mesh mesh = new Mesh();
            a.AssimpContext context = new a.AssimpContext();
            a.Scene scene = context.ImportFile(path, a.PostProcessSteps.Triangulate | a.PostProcessSteps.SortByPrimitiveType);
            a.Mesh assimpMesh = scene.Meshes[0];

            mesh.vertices = new Vertex[assimpMesh.Vertices.Count];

            for (int i = 0; i < assimpMesh.VertexCount; i++)
            {
                a.Vector3D aPosition = assimpMesh.Vertices[i];
                a.Vector3D aNormal = assimpMesh.Normals[i];
                a.Vector3D aUv = assimpMesh.TextureCoordinateChannels[0][i];

                Vertex vertex = new Vertex()
                {
                    Position = new Vector3(aPosition.X, aPosition.Y, aPosition.Z),
                    Normal = new Vector3(aNormal.X, aNormal.Y, aNormal.Z),
                    Uv = new Vector2(aUv.X, aUv.Y)
                };
                mesh.vertices[i] = vertex;
            }
            mesh.faces = new int[assimpMesh.FaceCount * 3];
            for (int i = 0; i < assimpMesh.FaceCount; i++)
            {
                a.Face aFace = assimpMesh.Faces[i];
                mesh.faces[i * 3 + 0] = aFace.Indices[0];
                mesh.faces[i * 3 + 1] = aFace.Indices[1];
                mesh.faces[i * 3 + 2] = aFace.Indices[2];
            }

            context.Dispose();
            mesh.GenerateBuffers();
            return mesh;
        }

        public Vertex[] vertices;
        public int[] faces;

        public int VertexBuffer { get; private set; }

        public Mesh()
        {
            vertices = new Vertex[0];
            faces = new int[0];
        }
        protected void GenerateBuffers()
        {
            VertexBuffer = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vertex.SizeInBytes, vertices, BufferUsageHint.StaticDraw);
        }
    }
}
