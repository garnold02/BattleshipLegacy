using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.RegularObjects;
using OpenTK;
using OpenTK.Graphics;

namespace BattleshipClient.Engine.UI
{
    class UIText : UIElement
    {
        public ImageFont Font { get; set; }
        public float Size { get; set; } = 1;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                Refresh();
            }
        }
        private string text;

        public UIText(UIManager manager, ImageFont font) : base(manager)
        {
            Font = font;
        }
        public override void Arrange()
        {
            Refresh();
        }
        private void Refresh()
        {
            Scale = new Vector2(Text.Length * 1.25f * Font.CharWidth * Size, Font.CharHeight * Size);
            int x = 0;
            int modCount = Text.Length;
            if (renderers.Count < Text.Length)
            {
                x = renderers.Count;
                modCount = renderers.Count;
                for (int i = renderers.Count; i < Text.Length; i++)
                {
                    char chr = Text[i];
                    UIRenderer charRenderer = new UIRenderer(this)
                    {
                        Texture = Font.Texture,
                        Color = Color4.White,
                        Transformation = new Vector4(ActualPosition.X, ActualPosition.Y, 0, 0) + new Vector4((x + 0.5f) * Font.CharWidth * 1.25f - Scale.X * 0.5f, 0, Font.CharWidth, Font.CharHeight) * Size * 0.5f,
                        UvTransformation = Font.GetCharacterBox(chr)
                    };
                    renderers.Add(charRenderer);
                    x++;
                }
                x = 0;
            }
            else if (renderers.Count > text.Length)
            {
                for (int i = Text.Length; i < renderers.Count; i++)
                {
                    renderers[i].Delete();
                    renderers.RemoveAt(i);
                }
            }
            for (int i = 0; i < modCount; i++)
            {
                char chr = Text[i];

                UIRenderer charRenderer = renderers[i];
                charRenderer.Texture = Font.Texture;
                charRenderer.Color = Color4.White;
                charRenderer.Transformation = new Vector4(ActualPosition.X, ActualPosition.Y, 0, 0) + new Vector4((x + 0.5f) * Font.CharWidth * 1.25f - Scale.X * 0.5f, 0, Font.CharWidth, Font.CharHeight) * Size * 0.5f;
                charRenderer.UvTransformation = Font.GetCharacterBox(chr);
                x++;
            }
        }
    }
}
