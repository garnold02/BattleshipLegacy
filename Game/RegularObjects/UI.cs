using BattleshipClient.Engine.UI;
using OpenTK;

namespace BattleshipClient.Game.RegularObjects
{
    class UI : RegularObject
    {
        public ImageFont DefaultFont { get; }
        public UIText CooldownText { get; }

        public UI(GameContainer container) : base(container)
        {
            DefaultFont = new ImageFont("font", 109, 119);
            CooldownText = new UIText(Container.UIManager, DefaultFont)
            {
                Position = new Vector2(0, 1),
                Pivot = new Vector2(0, 1),
                Size = 1f,
                Text = "00:00",
            };

            Container.UIManager.Add(CooldownText);
        }
        public override void Update(float delta)
        {

        }
    }
}
