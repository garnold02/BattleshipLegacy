using BattleshipClient.Engine.UI;
using BattleshipClient.Game.GameObjects;
using OpenTK;

namespace BattleshipClient.Game.RegularObjects
{
    class UI : RegularObject
    {
        public ImageFont DefaultFont { get; }
        public UIText CooldownText { get; }
        public UIText ScoreText { get; }
        public UIText OilText { get; }
        public UIPanel ActionMenuPanel { get; }
        public UI(GameContainer container) : base(container)
        {
            DefaultFont = new ImageFont("font", 109, 119);
            CooldownText = new UIText(Container.UIManager, DefaultFont)
            {
                Position = new Vector2(0, 1),
                Pivot = new Vector2(0, 1),
                FontSize = 1f,
                Text = "00:00",
            };
            ScoreText = new UIText(Container.UIManager, DefaultFont)
            {
                Position = new Vector2(-1, -1),
                Pivot = new Vector2(-0.5f, -1),
                FontSize = 0.5f,
                Text = "0 pont"
            };
            OilText = new UIText(Container.UIManager, DefaultFont)
            {
                Position = new Vector2(1, -1),
                Pivot = new Vector2(0.5f, -1),
                FontSize = 0.5f,
                Text = "0 liter olaj"
            };
            ActionMenuPanel = new UIPanel(container.UIManager, "menuWheel")
            {
            };

            Container.UIManager.Add(CooldownText);
            Container.UIManager.Add(ScoreText);
            Container.UIManager.Add(OilText);
            //Container.UIManager.Add(ActionMenuPanel);
        }
        public override void Update(float delta)
        {

        }
    }
}
