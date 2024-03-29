﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace Twaila.Graphics
{
    public class TwailaRender
    {
        public List<DrawInfo> Info { get; private set; }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public TwailaRender(List<DrawInfo> info) : this()
        {
            if(info == null || info.Count == 0)
            {
                return;
            }
            Info = info;

            int smallestX = Info[0].Position.X, biggestX = smallestX + (int)Info[0].Size().X;
            int smallestY = Info[0].Position.Y, biggestY = smallestY + (int)Info[0].Size().Y;
            for (int i = 1; i < info.Count; ++i)
            {
                DrawInfo draw = Info[i];
                if (draw.Position.X < smallestX)
                {
                    smallestX = draw.Position.X;
                }
                if (draw.Position.X + draw.Size().X > biggestX)
                {
                    biggestX = draw.Position.X + (int)draw.Size().X;
                }
                if (draw.Position.Y < smallestY)
                {
                    smallestY = draw.Position.Y;
                }
                if (draw.Position.Y + draw.Size().Y > biggestY)
                {
                    biggestY = draw.Position.Y + (int)draw.Size().Y;
                }
            }
            Shift(smallestX < 0 ? Math.Abs(smallestX) : 0, smallestY < 0 ? Math.Abs(smallestY) : 0);

            Width = biggestX - smallestX;
            Height = biggestY - smallestY;
        }

        public TwailaRender(Texture2D texture, float scale = 1) : this()
        {
            if (texture != null)
            {
                DrawInfo info = new DrawInfo(texture, Point.Zero, new Rectangle(0, 0, texture.Width, texture.Height), scale);
                Info.Add(info);

                Width = texture.Width * scale;
                Height = texture.Height * scale;
            }
        }

        public TwailaRender()
        {
            Info = new List<DrawInfo>();
        }

        public bool CanDraw()
        {
            if(Info == null || Info.Count == 0)
            {
                return false;
            }
            foreach(DrawInfo info in Info)
            {
                if(info.Texture == null)
                {
                    return false;
                }
            }
            return true;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
        {
            Draw(spriteBatch, bounds, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle bounds, Color color)
        {
            Draw(spriteBatch, bounds, color, 1);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle bounds, Color color, float scale)
        {
            foreach (DrawInfo draw in Info)
            {
                Vector2 drawPos = new Vector2(bounds.X + draw.Position.X * scale, bounds.Y + draw.Position.Y * scale);
                Rectangle source = draw.Source;

                if (draw.Position.X + draw.Size().X * scale > bounds.Width)
                {
                    int sizeX = (int)((bounds.Width - draw.Position.X) / draw.Scale);
                    source.Width = sizeX;
                }

                if (draw.Position.Y + draw.Size().Y * scale > bounds.Height)
                {
                    int sizeY = (int)((bounds.Height - draw.Position.Y) / draw.Scale);
                    source.Height = sizeY;
                }

                if(source.Width > 0 && source.Height > 0)
                {
                    spriteBatch.Draw(draw.Texture, drawPos, source, draw.Color.MultiplyRGBA(color), 0, Vector2.Zero, draw.Scale * scale, 0, 0);
                }
            }
        }

        private void Shift(int xOffset, int yOffset)
        {
            foreach (DrawInfo draw in Info)
            {
                draw.Position = new Point(draw.Position.X + xOffset, draw.Position.Y + yOffset);
            }
        }
    }
}
