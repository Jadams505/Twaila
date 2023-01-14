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
        public TwailaRender Render { get; private set; }

        public Color ColorFilter { get; set; }

        public UITwailaImage()
        {
            Render = new TwailaRender(TextureAssets.Buff[BuffID.Confused].ForceVanillaLoad());
        }

        public override Vector2 GetContentSize()
        {
            return new Vector2(Render.Width, Render.Height);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Render != null && Render.CanDraw())
            {
                base.DrawSelf(spriteBatch);
            }
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            Render.Draw(spriteBatch, drawDim, Color.White * Opacity, 1);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            Render.Draw(spriteBatch, drawDim, Color.White * Opacity, 1);
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            float scale = CalculatedScale();
            Render.Draw(spriteBatch, drawDim, Color.White * Opacity, scale);
        }

        public Rectangle DrawDimensions()
        {
            Rectangle rec = new Rectangle((int)GetDimensions().X, (int)GetDimensions().Y, (int)Render.Width, (int)Render.Height);
            switch (DrawMode)
            {
                case DrawMode.Trim:
                    if (Render.Height < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - Render.Height) / 2;
                    }
                    rec.Width = (int)MathHelper.Min(GetInnerDimensions().Width, Render.Width);
                    rec.Height = (int)MathHelper.Min(GetInnerDimensions().Height, Render.Height);
                    return rec;
                case DrawMode.Shrink:
                    if (Render.Height * CalculatedScale() < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - Render.Height * CalculatedScale()) / 2;
                    }
                    rec.Width = (int)Render.Width;
                    rec.Height = (int)Render.Height;
                    return rec;
                case DrawMode.Overflow:
                    if (Render.Height < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - Render.Height) / 2;
                    }
                    rec.Width = (int)Render.Width;
                    rec.Height = (int)Render.Height;
                    return rec;
            }
            return rec;
        }

        public float CalculatedScale()
        {
            float scaleX = 1;
            if(Render.Width > GetInnerDimensions().Width)
            {
                scaleX = GetInnerDimensions().Width / Render.Width;
            }
            float scaleY = 1;
            if(Render.Height > GetInnerDimensions().Height)
            {
                scaleY = GetInnerDimensions().Height / Render.Height;
            }
            return System.Math.Min(scaleX, scaleY);
        }

        public void SetImage(TwailaRender render)
        {
            Render = render;
            if(Render == null || !Render.CanDraw())
            {
                Render = new TwailaRender(TextureAssets.Buff[BuffID.Confused].ForceVanillaLoad());
            }
        }
    }
}
