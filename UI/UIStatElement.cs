﻿using Microsoft.Xna.Framework;
using System;
using Twaila.Config;
using Twaila.Graphics;

namespace Twaila.UI
{
    public class UIStatElement : UITwailaElement
    {
        public UITwailaImage Icon { get; set; }
        public UITwailaText StatText { get; set; }

        public const float PADDING_RIGHT = 4f;

        public UIStatElement(TwailaRender icon, string text) : base()
        {
            Icon = new UITwailaImage();
            Icon.SetImage(icon);
            Append(Icon);

            StatText = new UITwailaText(text);
            Append(StatText);

            Width.Set(GetContentSize().X, 0);
            Height.Set(GetContentSize().Y, 0);
        }

        public override void Update(GameTime gameTime)
        {
            float scale = DrawMode == DrawMode.Shrink ? GetDrawScale() : 1;

            Icon.Left.Set(0, 0);
            Icon.Top.Set(0, 0);
            Icon.Width.Set((int)(Icon.Render.Width * scale), 0);
            if(DrawMode == DrawMode.Trim && Icon.Width.Pixels > Width.Pixels)
            {
                Icon.Width.Set(Math.Max(Width.Pixels, 0), 0);
            }
            Icon.Height.Set(Height.Pixels * scale, 0);

            StatText.Top.Set(0, 0);
            StatText.Left.Set(Icon.Width.Pixels + PADDING_RIGHT, 0);
            StatText.Width.Set(Math.Max(Width.Pixels - Icon.Width.Pixels - PADDING_RIGHT, 0), 0);
            StatText.Height.Set(Height.Pixels * scale, 0);
        }

        public override Vector2 GetContentSize()
        {
            Vector2 size = Vector2.Zero;

            size.X = Icon.Render.Width + StatText.GetContentSize().X + PADDING_RIGHT;
            size.Y = Math.Max(Icon.Render.Height, StatText.GetContentSize().Y);

            return size;
        }

        public override void ApplyConfigSettings(TwailaConfig config)
        {
            base.ApplyConfigSettings(config);
            StatText.ApplyConfigSettings(config);
            Icon.ApplyConfigSettings(config);
        }

        public override void ApplyHoverSettings(TwailaConfig config)
        {
            base.ApplyHoverSettings(config);
            StatText.ApplyHoverSettings(config);
            Icon.ApplyHoverSettings(config);
        }
    }
}
