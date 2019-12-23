using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game;
using OpenTK;
using System;

namespace BattleshipClient.Engine
{
    class Utility
    {
        public static Vector3 Raycast(Vector3 rayOrigin, Vector3 rayDirection)
        {
            Vector3 planePoint = Vector3.Zero;
            Vector3 planeNormal = Vector3.UnitY;

            if (Vector3.Dot(planeNormal, rayDirection) == 0)
            {
                return new Vector3(0, -1, 0);
            }
            else
            {
                float t = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, rayOrigin)) / Vector3.Dot(planeNormal, rayDirection);
                return rayOrigin + rayDirection * t;
            }
        }
        public static Vector3 ClipToWorldRay(Camera camera, Vector2 clipPosition)
        {
            Matrix4 projectionInv = camera.Matrix.Inverted();

            Vector4 p1 = new Vector4(clipPosition.X, clipPosition.Y, 0, 1) * projectionInv;
            Vector4 p2 = new Vector4(clipPosition.X, clipPosition.Y, 1, 1) * projectionInv;

            Vector3 p1n = p1.Xyz / p1.W;
            Vector3 p2n = p2.Xyz / p2.W;

            Vector3 ray = (p2n - p1n).Normalized();
            return ray;
        }
        public static Vector2 GetMousePositionOnXZPlane(GameContainer container)
        {
            Vector2 clipSpaceMousePosition = new Vector2(container.MousePosition.X / container.Width - 0.5f, (container.Height - container.MousePosition.Y) / container.Height - 0.5f) * 2;
            Vector3 ray = ClipToWorldRay(container.CameraCtrl.Camera, clipSpaceMousePosition);
            Vector3 positionOnPlane = Raycast(container.CameraCtrl.Camera.Transform.Position, ray);

            return new Vector2(positionOnPlane.X, positionOnPlane.Z);
        }
        public static Quaternion LookAt(Vector3 sourcePoint, Vector3 destPoint)
        {
            Vector3 forwardVector = Vector3.Normalize(destPoint - sourcePoint);

            float dot = Vector3.Dot(-Vector3.UnitZ, forwardVector);

            if (Math.Abs(dot - (-1.0f)) < 0.000001f)
            {
                return new Quaternion(Vector3.UnitY.X, Vector3.UnitY.Y, Vector3.UnitY.Z, 3.1415926535897932f);
            }
            if (Math.Abs(dot - (1.0f)) < 0.000001f)
            {
                return Quaternion.Identity;
            }

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Cross(-Vector3.UnitZ, forwardVector);
            rotAxis = Vector3.Normalize(rotAxis);
            return Quaternion.FromAxisAngle(rotAxis, rotAngle);
        }
        public static float Lerp(float a, float b, float t)
        {
            float distance = b - a;
            return a + distance * t;
        }
    }
}
