﻿using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.RegularObjects;
using OpenTK;
using System.Collections.Generic;

namespace BattleshipClient.Engine.UI
{
    abstract class UIElement
    {
        public Vector2 ActualPosition => (Position * new Vector2(Manager.Container.Width / 2, Manager.Container.Height / 2) - Pivot * new Vector2(Scale.X/2,Scale.Y/2));
        public UIManager Manager { get; }

        public Vector2 Pivot { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        protected List<UIRenderer> renderers = new List<UIRenderer>();

        public UIElement(UIManager manager)
        {
            Manager = manager;
        }
        public abstract void Arrange();
        public void Render()
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.Render();
            }
        }
    }
}
