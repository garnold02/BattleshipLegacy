using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game;
using BattleshipClient.Game.RegularObjects;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace BattleshipClient.Engine.UI
{
    class UIPanel : UIElement
    {
        public Texture Texture { get; set; }
        public UIPanel(UIManager manager, string textureName = "blank") : base(manager)
        {
            Texture = Assets.Get<Texture>(textureName);
            renderers = new List<UIRenderer>()
            {
                new UIRenderer(this)
                {
                    Texture = Texture,
                    Color = Color4.White,
                    UvTransformation=new Vector4(0,0,1,1)
                }
            };
            Arrange();
        }
        public override void Arrange()
        {
            renderers[0].Transformation = new Vector4(ActualPosition.X, ActualPosition.Y, Scale.X * Texture.Width / 2, Scale.Y * Texture.Height / 2);
        }
    }
}
