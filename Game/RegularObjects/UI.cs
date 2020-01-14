using System;
using BattleshipClient.Engine;
using BattleshipClient.Engine.UI;
using OpenTK;
using OpenTK.Input;

namespace BattleshipClient.Game.RegularObjects
{
    class UI : RegularObject
    {
        public ImageFont DefaultFont { get; }
        public UIText CooldownText { get; }
        public UIText ScoreText { get; }
        public UIText OilText { get; }
        public UIPanel UpperButton { get; }
        public UIPanel RightButton { get; }
        public UIPanel LowerButton { get; }
        public UIPanel LeftButton { get; }

        public bool MenuInteractionEnabled { get; set; } = true;
        public Vector2 desiredUpperPosition;
        public Vector2 desiredRightPosition;
        public Vector2 desiredLowerPosition;
        public Vector2 desiredLeftPosition;
        private byte currentButton;

        public UI(GameContainer container) : base(container)
        {
            DefaultFont = new ImageFont("font", 109, 119);
            CooldownText = new UIText(Container.UIManager, DefaultFont)
            {
                Position = new Vector2(0, 1),
                Pivot = new Vector2(0, 1),
                FontSize = 1f,
                Text = "",
            };
            ScoreText = new UIText(Container.UIManager, DefaultFont)
            {
                Position = new Vector2(-1, -1),
                Pivot = new Vector2(-0.5f, -1),
                FontSize = 0.5f,
                Text = ""
            };
            OilText = new UIText(Container.UIManager, DefaultFont)
            {
                Position = new Vector2(1, -1),
                Pivot = new Vector2(0.5f, -1),
                FontSize = 0.5f,
                Text = ""
            };
            UpperButton = new UIPanel(container.UIManager, "upperButton");
            RightButton = new UIPanel(container.UIManager, "rightButton");
            LowerButton = new UIPanel(container.UIManager, "lowerButton");
            LeftButton = new UIPanel(container.UIManager, "leftButton");

            Container.UIManager.Add(CooldownText);
            Container.UIManager.Add(ScoreText);
            Container.UIManager.Add(OilText);
            Container.UIManager.Add(UpperButton);
            Container.UIManager.Add(RightButton);
            Container.UIManager.Add(LowerButton);
            Container.UIManager.Add(LeftButton);
        }
        public override void Update(float delta)
        {
            UpdateMenuVisibility();
            if (Container.TurnManager.IsMenuEnabled)
            {
                UpdateMenuPositions();
                ButtonFunctionality();
            }
        }

        private void UpdateMenuVisibility()
        {
            UpperButton.IsVisible = Container.TurnManager.IsMenuEnabled;
            RightButton.IsVisible = Container.TurnManager.IsMenuEnabled;
            LowerButton.IsVisible = Container.TurnManager.IsMenuEnabled;
            LeftButton.IsVisible = Container.TurnManager.IsMenuEnabled;
        }
        private void UpdateMenuPositions()
        {
            if (MenuInteractionEnabled)
            {
                desiredUpperPosition = Vector2.Zero;
                desiredRightPosition = Vector2.Zero;
                desiredLowerPosition = Vector2.Zero;
                desiredLeftPosition = Vector2.Zero;
            }

            Vector2 clipPosition = Utility.ScreenToClip(Container, Container.MousePosition) * new Vector2(Container.AspectRatio, 1);
            float angle = (float)Math.Atan2(clipPosition.Y, clipPosition.X) - MathHelper.PiOver4;
            if (angle < 0)
            {
                angle += MathHelper.TwoPi;
            }
            if (angle > 0 && angle < MathHelper.PiOver2)
            {
                //Upper quadrant
                if (MenuInteractionEnabled)
                {
                    desiredUpperPosition = new Vector2(0, GetButtonDistance(clipPosition.Y));
                    currentButton = 0;
                }
            }
            else if (angle > MathHelper.PiOver2 && angle < MathHelper.Pi)
            {
                //Left quadrant
                if (MenuInteractionEnabled)
                {
                    desiredLeftPosition = new Vector2(-GetButtonDistance(-clipPosition.X), 0);
                    currentButton = 3;
                }
            }
            else if (angle > MathHelper.Pi && angle < MathHelper.ThreePiOver2)
            {
                //Lower quadrant
                if (MenuInteractionEnabled)
                {
                    desiredLowerPosition = new Vector2(0, -GetButtonDistance(-clipPosition.Y));
                    currentButton = 2;
                }
            }
            else
            {
                //Right quadrant
                if (MenuInteractionEnabled)
                {
                    desiredRightPosition = new Vector2(GetButtonDistance(clipPosition.X), 0);
                    currentButton = 1;
                }
            }

            UpperButton.Position += (desiredUpperPosition - UpperButton.Position) * 0.4f;
            UpperButton.Scale = Vector2.One * Container.Width / 1920;
            UpperButton.Arrange();
            RightButton.Position += (desiredRightPosition - RightButton.Position) * 0.4f;
            RightButton.Scale = Vector2.One * Container.Width / 1920;
            RightButton.Arrange();
            LowerButton.Position += (desiredLowerPosition - LowerButton.Position) * 0.4f;
            LowerButton.Scale = Vector2.One * Container.Width / 1920;
            LowerButton.Arrange();
            LeftButton.Position += (desiredLeftPosition - LeftButton.Position) * 0.4f;
            LeftButton.Scale = Vector2.One * Container.Width / 1920;
            LeftButton.Arrange();
        }
        private void ButtonFunctionality()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left) && MenuInteractionEnabled)
            {
                switch (currentButton)
                {
                    case 0:
                        {
                            Container.TurnManager.BuyStategicOption(ActionType.Repair);
                            UpperButton.Position = new Vector2(0, 0.1f);
                        }
                        break;
                    case 1:
                        {
                            Container.TurnManager.BuyStategicOption(ActionType.Big);
                            RightButton.Position = new Vector2(0.1f, 0);
                        }
                        break;
                    case 2:
                        {
                            Container.TurnManager.Ready();
                            Container.TurnManager.ToggleMenu();
                        }
                        break;
                    case 3:
                        {
                            Container.TurnManager.BuyStategicOption(ActionType.Regular);
                            LeftButton.Position = new Vector2(-0.1f, 0);
                        }
                        break;
                }
            }
        }
        private float GetButtonDistance(float clipPos)
        {
            float a = 6f;
            float b = 2.8f;
            float c = 16f;
            return (float)Math.Pow(2, -Math.Pow((a * clipPos - b), 2)) / c;
        }
    }
}
