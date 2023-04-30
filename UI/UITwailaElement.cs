using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace Twaila.UI
{
    public enum DrawMode
    {
        [Label("$Mods.Twaila.Enums.Shrink")] Shrink,
        [Label("$Mods.Twaila.Enums.Trim")] Trim,
        [Label("$Mods.Twaila.Enums.Overflow")] Overflow
    }

    public abstract class UITwailaElement : UIElement
    {
        public DrawMode DrawMode { get; set; }
        public float Opacity { get; set; }

        public UITwailaElement()
        {
            DrawMode = DrawMode.Trim;
            Opacity = 1.0f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            switch (DrawMode)
            {
                case DrawMode.Trim:
                    DrawTrimmed(spriteBatch);
                    break;
                case DrawMode.Shrink:
                    DrawShrunk(spriteBatch);
                    break;
                case DrawMode.Overflow:
                    DrawOverflow(spriteBatch);
                    break;
            }
        }

        public virtual void ApplyConfigSettings(TwailaConfig config)
        {
            DrawMode = config.ContentSetting;
            Opacity = 1;
        }

        public virtual void ApplyHoverSettings(TwailaConfig config)
        {
            Opacity = config.HoverOpacity;
        }

        public Vector2 GetScaleVector(Vector2 maxSize)
        {
            float scaleX = 1;
            if (GetContentSize().X > maxSize.X)
            {
                scaleX = maxSize.X / GetContentSize().X;
            }
            float scaleY = 1;
            if (GetContentSize().Y > maxSize.Y)
            {
                scaleY = maxSize.Y / GetContentSize().Y;
            }
            return new Vector2(scaleX, scaleY);
        }

        public Vector2 GetDrawScaleVector() => GetScaleVector(new Vector2(Width.Pixels, Height.Pixels));

        // Gets the uniform scale it would take to shrink the content to maxSize
        public float GetScale(Vector2 maxSize)
        {
            float scaleX = 1;
            if (GetContentSize().X > maxSize.X)
            {
                scaleX = maxSize.X / GetContentSize().X;
            }
            float scaleY = 1;
            if (GetContentSize().Y > maxSize.Y)
            {
                scaleY = maxSize.Y / GetContentSize().Y;
            }
            return Math.Min(scaleX, scaleY);
        }

        // Gets the uniform scale it would take to shrink the content to this element's dimensions
        public float GetDrawScale() => GetScale(new Vector2(Width.Pixels, Height.Pixels));

        public abstract Vector2 GetContentSize();

        public virtual Vector2 SizePriority() => Vector2.One;

        protected virtual void DrawShrunk(SpriteBatch spriteBatch)
        {

        }

        protected virtual void DrawTrimmed(SpriteBatch spriteBatch)
        {

        }

        protected virtual void DrawOverflow(SpriteBatch spriteBatch)
        {

        }
    }
}
