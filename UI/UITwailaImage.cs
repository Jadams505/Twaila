using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Twaila.Context;
using Twaila.Graphics;
using Twaila.Util;

namespace Twaila.UI
{
    public class UITwailaImage : UITwailaElement
    {
        internal TwailaTexture image;

        public UITwailaImage()
        {
            image = new TwailaTexture(TextureAssets.Buff[BuffID.Confused].Value);
        }

        public override Vector2 GetContentSize()
        {
            return new Vector2(image.Width, image.Height);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (image?.Texture != null)
            {
                base.DrawSelf(spriteBatch);
            }
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            spriteBatch?.Draw(image.Texture, new Vector2(drawDim.X, drawDim.Y),
                new Rectangle(0, 0, drawDim.Width, drawDim.Height), Color.White * Opacity, 0, 
                Vector2.Zero, image.Scale, 0, 0);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            spriteBatch?.Draw(image.Texture, new Vector2(drawDim.X, drawDim.Y),
                new Rectangle(0, 0, drawDim.Width, drawDim.Height), Color.White * Opacity, 0, Vector2.Zero, image.Scale, 0, 0);
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            float scale = CalculatedScale();
            if(image.Drawer != null)
            {
                image.Drawer.Invoke(spriteBatch, new Point(drawDim.X, drawDim.Y));
            }
            else
            {
                spriteBatch?.Draw(image.Texture, new Vector2(drawDim.X, drawDim.Y), 
                    new Rectangle(0, 0, drawDim.Width, drawDim.Height), Color.White * Opacity, 0, Vector2.Zero, scale, 0, 0);
            }
        }

        public Rectangle DrawDimensions()
        {
            Rectangle rec = new Rectangle(GetDimensions().ToRectangle().X, GetDimensions().ToRectangle().Y, (int)image.Width, (int)image.Height);
            switch (DrawMode)
            {
                case DrawMode.Trim:
                    if (image.Height < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - image.Height) / 2;
                    }
                    rec.Width = (int)MathHelper.Min(GetInnerDimensions().Width / image.Scale, image.Texture.Width);
                    rec.Height = (int)MathHelper.Min(GetInnerDimensions().Height / image.Scale, image.Texture.Height);
                    return rec;
                case DrawMode.Shrink:
                    if (image.Texture.Height * CalculatedScale() < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - image.Texture.Height * CalculatedScale()) / 2;
                    }
                    rec.Width = image.Texture.Width;
                    rec.Height = image.Texture.Height;
                    return rec;
                case DrawMode.Overflow:
                    if (image.Height < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - image.Height) / 2;
                    }
                    rec.Width = image.Texture.Width;
                    rec.Height = image.Texture.Height;
                    return rec;
            }
            return rec;
        }

        public float CalculatedScale()
        {
            float scaleX = 1;
            if(image.Width > GetInnerDimensions().Width)
            {
                scaleX = GetInnerDimensions().Width / image.Width;
            }
            float scaleY = 1;
            if(image.Height > GetInnerDimensions().Height)
            {
                scaleY = GetInnerDimensions().Height / image.Height;
            }
            return System.Math.Min(scaleX, scaleY) * image.Scale;
        }

        public void SetImage(TwailaTexture texture)
        {
            image = texture;
            if(image?.Texture == null)
            {
                image = new TwailaTexture(TextureAssets.Buff[BuffID.Confused].Value);
            }
        }
    }
}
