using OpenTK;

namespace BattleshipClient.Engine
{
    class Transform
    {
        public static Transform Identity { get => new Transform() { localPosition = new Vector3(0, 0, 0), localRotation = Quaternion.Identity, localScale = new Vector3(1, 1, 1) }; }
        public Transform Parent { get; set; } = null;
        public Matrix4 TranslationMatrix
        {
            get
            {
                if (Parent == null) return LocalTranslationMatrix;
                Vector4 rotatedLocal = new Vector4(localPosition) * Parent.RotationMatrix;
                return (Matrix4.CreateTranslation(Parent.ScaleMatrix.ExtractScale() * rotatedLocal.Xyz)) * Parent.TranslationMatrix;
            }
        }
        public Matrix4 RotationMatrix
        {
            get
            {
                if (Parent == null) return LocalRotationMatrix;
                return LocalRotationMatrix * Parent.RotationMatrix;
            }
        }
        public Matrix4 ScaleMatrix
        {
            get
            {
                if (Parent == null) return LocalScaleMatrix;
                return Parent.ScaleMatrix * Matrix4.CreateScale(localScale);
            }
        }
        public Matrix4 Matrix
        {
            get
            {
                if (Parent == null) return LocalTranslationMatrix;
                return RotationMatrix * ScaleMatrix * TranslationMatrix;
            }
            set
            {
                if (Parent != null) return;
                localPosition = value.ExtractTranslation();
                localRotation = value.ExtractRotation();
                localScale = value.ExtractScale();
            }
        }
        public Vector3 Position { get => TranslationMatrix.ExtractTranslation(); }
        public Quaternion Rotation { get { if (Parent == null) return localRotation; else return localRotation * Parent.Rotation; } }
        public Vector3 Scale { get => ScaleMatrix.ExtractScale(); }
        public Matrix4 LocalTranslationMatrix { get => Matrix4.CreateTranslation(localPosition); }
        public Matrix4 LocalRotationMatrix { get => Matrix4.CreateFromQuaternion(localRotation); }
        public Matrix4 LocalScaleMatrix { get => Matrix4.CreateScale(localScale); }
        public Matrix4 LocalMatrix { get => RotationMatrix * ScaleMatrix * TranslationMatrix; }
        public Vector3 Right { get => (RotationMatrix * new Vector4(1, 0, 0, 0)).Xyz; }
        public Vector3 Up { get => (RotationMatrix * new Vector4(0, 1, 0, 0)).Xyz; }
        public Vector3 Forward { get => (RotationMatrix * new Vector4(0, 0, -1, 0)).Xyz; }
        public Vector3 localPosition = new Vector3(0, 0, 0);
        public Quaternion localRotation = Quaternion.Identity;
        public Vector3 localScale = new Vector3(1, 1, 1);

        public void Rotate(Quaternion rotation)
        {
            localRotation = rotation * this.localRotation;
        }
        public void Rotate(Vector3 rotation)
        {
            localRotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(rotation.X), MathHelper.DegreesToRadians(rotation.Y), MathHelper.DegreesToRadians(rotation.Z)) * this.localRotation;
        }
        public void Rotate(float x, float y, float z)
        {
            localRotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(x), MathHelper.DegreesToRadians(y), MathHelper.DegreesToRadians(z)) * localRotation;
        }
    }
}
