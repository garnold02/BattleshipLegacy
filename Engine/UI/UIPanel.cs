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
            renderers[0] = new UIRenderer(this)
            {
                Texture = Texture,
                Color = Color4.White
            };
        }
        public override void Arrange()
        {

        }
    }
}
