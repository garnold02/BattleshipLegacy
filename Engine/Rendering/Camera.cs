using System;
using OpenTK;

namespace BattleshipClient.Engine.Rendering
{
    class Camera
    {
        public Transform Transform { get; set; } = Transform.Identity;
        public float FieldOfView { get; set; }
        public float NearClip { get; set; }
        public float FarClip { get; set; }

        public Matrix4 PerspectiveMatrix { get; private set; }
        public Matrix4 Matrix { get; private set; }
        public Matrix4 InvertedMatrix { get; private set; }

        public Camera(float fov, float near, float far)
        {
            FieldOfView = fov;
            NearClip = near;
            FarClip = far;
            UpdateCache();
        }
        public void UpdateCache()
        {
            PerspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(FieldOfView * ((float)Math.PI / 180f), Root.GameContainer.AspectRatio, NearClip, FarClip);
            Matrix = (Transform.TranslationMatrix.Inverted() * Transform.RotationMatrix) * PerspectiveMatrix;
            InvertedMatrix = Matrix.Inverted();
        }
    }
}
