using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Engine.UI
{
    class ImageFont
    {
        public Texture Texture { get; }
        public int CharWidth { get; }
        public int CharHeight { get; }
        private int CharCountX { get; }
        private int CharCountY { get; }
        private float CharRatioX { get; }
        private float CharRatioY { get; }
        public ImageFont(string texture, int charWidth, int charHeight)
        {
            Texture = Assets.Get<Texture>(texture);
            CharWidth = charWidth;
            CharHeight = charHeight;
            CharCountX = Texture.Width / charWidth;
            CharCountY = Texture.Height / charHeight;
            CharRatioX = 1f / CharCountX;
            CharRatioY = 1f / CharCountY;
        }

        public Vector4 GetCharacterBox(char chr)
        {
            int chrNormalized = chr - 32;
            int x = chrNormalized % CharCountX;
            int y = (CharCountY - 1) - (chrNormalized / CharCountX);

            return new Vector4(x * CharRatioX, y * CharRatioY, CharRatioX, CharRatioY);
        }
    }
}
