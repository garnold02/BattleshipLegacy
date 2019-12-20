﻿using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
namespace BattleshipClient.Engine.Rendering
{
    /// <summary>
    /// 2D texture.
    /// </summary>
    public class Texture
    {
        #region Static
        public static Texture Load(string path, TextureFilteringMode filteringMode = TextureFilteringMode.Nearest)
        {
            Bitmap bitmap = new Bitmap(path);

            Texture texture = new Texture()
            {
                Width = bitmap.Width,
                Height = bitmap.Height,
                Path = path,
                Filtering = filteringMode
            };
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bitmap.UnlockBits(data);

            texture.glTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture.glTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filteringMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)filteringMode);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            return texture;
        }
        #endregion
        public string Path { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public TextureFilteringMode Filtering { get; private set; }

        internal int glTexture;
    }

    public enum TextureFilteringMode
    {
        Nearest = 9728,
        Linear = 9729,
    }
}
