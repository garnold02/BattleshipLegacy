using BattleshipClient.Engine.UI;
using OpenTK;
using System.Collections.Generic;

namespace BattleshipClient.Game.RegularObjects
{
    class UIManager : RegularObject
    {
        public Matrix4 Projection => Matrix4.CreateOrthographic(Container.Width, Container.Height, 0, 1);

        private readonly List<UIElement> uiElements;
        private readonly List<UIElement> uIElementsAdded;
        private readonly List<UIElement> uIElementsRemoved;
        public UIManager(GameContainer container) : base(container)
        {
            uiElements = new List<UIElement>();
            uIElementsAdded = new List<UIElement>();
            uIElementsRemoved = new List<UIElement>();
        }
        public override void Update(float delta)
        {
            uiElements.AddRange(uIElementsAdded);
            uIElementsAdded.Clear();
            uIElementsRemoved.ForEach(e => uiElements.Remove(e));
            uIElementsRemoved.Clear();
        }
        public void Render()
        {
            foreach (UIElement uiElement in uiElements)
            {
                uiElement.Render();
            }
        }
        public void Arrange()
        {
            foreach (UIElement uiElement in uiElements)
            {
                uiElement.Arrange();
            }
        }
        public void Add(UIElement element)
        {
            uIElementsAdded.Add(element);
        }
        public void Remove(UIElement element)
        {
            uIElementsRemoved.Add(element);
        }
    }
}
