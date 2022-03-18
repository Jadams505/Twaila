﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
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
            image = new TwailaTexture(Main.buffTexture[BuffID.Confused]);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (image?.Texture != null)
            {
                switch (drawMode)
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
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            spriteBatch?.Draw(image.Texture, new Vector2(drawDim.X, drawDim.Y),
                new Rectangle(0, 0, drawDim.Width, drawDim.Height), Color.White, 0, 
                Vector2.Zero, image.Scale, 0, 0);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            spriteBatch?.Draw(image.Texture, new Vector2(drawDim.X, drawDim.Y),
                new Rectangle(0, 0, drawDim.Width, drawDim.Height), Color.White, 0, Vector2.Zero, image.Scale, 0, 0);
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            Rectangle drawDim = DrawDimensions();
            spriteBatch?.Draw(image.Texture, new Vector2(drawDim.X, drawDim.Y),
                new Rectangle(0, 0, drawDim.Width, drawDim.Height), Color.White, 0, Vector2.Zero, CalculatedScale(), 0, 0);
        }

        public Rectangle DrawDimensions()
        {
            Rectangle rec = new Rectangle(GetDimensions().ToRectangle().X, GetDimensions().ToRectangle().Y, image.Width(), image.Height());
            switch (drawMode)
            {
                case DrawMode.Trim:
                    if (image.Height() < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - image.Height()) / 2;
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
                    if (image.Height() < GetInnerDimensions().Height)
                    {
                        rec.Y += (int)(GetInnerDimensions().Height - image.Height()) / 2;
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
            if(image.Width() > GetInnerDimensions().Width)
            {
                scaleX = GetInnerDimensions().Width / image.Width();
            }
            float scaleY = 1;
            if(image.Height() > GetInnerDimensions().Height)
            {
                scaleY = GetInnerDimensions().Height / image.Height();
            }
            return System.Math.Min(scaleX, scaleY) * image.Scale;
        }

        public void SetImage(SpriteBatch spriteBatch, TileContext context, int itemId)
        {
            if (TwailaUI.debugMode)
            {
                image = new TwailaTexture(ImageUtil.GetDebugImage(spriteBatch, context.Tile));
                return;
            }
            if (TwailaConfig.Get().UseItemTextures)
            {
                TwailaTexture item = context.GetItemImage(spriteBatch, itemId);
                image = item.Texture != null ? item : context.GetTileImage(spriteBatch);
            }
            else
            {
                TwailaTexture tile = context.GetTileImage(spriteBatch);
                image = tile.Texture != null ? tile : context.GetItemImage(spriteBatch, itemId);
            }
            if(image?.Texture == null)
            {
                image = new TwailaTexture(Main.buffTexture[BuffID.Confused]);
            }
        }
    }
}
