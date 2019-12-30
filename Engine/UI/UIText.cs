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
            renderers.ForEach(r => r.Delete());
            renderers.Clear();
            int x = 0;

            foreach (char chr in Text)
            {
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
        }
    }
}
