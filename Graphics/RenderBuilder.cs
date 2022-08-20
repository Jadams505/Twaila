using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace Twaila.Graphics
{
    public class RenderBuilder
    {
        public class DrawInfo
        {
            public Texture2D Texture { get; set; }
            public Point Position { get; set; }
            public Rectangle Source { get; set; }
            public Color Color { get; set; }
            public float Scale { get; set; }

            public DrawInfo(Texture2D texture, Point position, Rectangle source, Color color, float scale = 1)
            {
                Texture = texture;
                Position = position;
                Source = source;
                Color = color;
                Scale = scale;
            }

            public DrawInfo(Texture2D texture, Point position, Rectangle source, float scale = 1) : 
                this(texture, position, source, Color.White, scale)
            {

            }

            public Vector2 Size()
            {
                return new Vector2(Source.Width * Scale, Source.Height * Scale);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, Position.ToVector2(), Source, Color, 0, Vector2.Zero, Scale, 0, 0);
            }
        }

        private List<DrawInfo> _drawInstructions;

        public RenderBuilder()
        {
            _drawInstructions = new List<DrawInfo>();
        }

        public void AddInstruction(Texture2D texture, Point position, Rectangle source, Color color, float scale = 1)
        {
            _drawInstructions.Add(new DrawInfo(texture, position, source, color, scale));
        }

        public void AddInstruction(Texture2D texture, Point position, Rectangle source, float scale = 1)
        {
            AddInstruction(texture, position, source, Color.White, scale);
        }

        public void Draw(SpriteBatch spriteBatch, Point position)
        {
            int smallestX = _drawInstructions[0].Position.X, biggestX = smallestX + (int)_drawInstructions[0].Size().X;
            int smallestY = _drawInstructions[0].Position.Y, biggestY = smallestY + (int)_drawInstructions[0].Size().Y;
            for (int i = 1; i < _drawInstructions.Count; ++i)
            {
                DrawInfo info = _drawInstructions[i];
                if (info.Position.X < smallestX)
                {
                    smallestX = info.Position.X;
                }
                if (info.Position.X + info.Size().X > biggestX)
                {
                    biggestX = info.Position.X + (int)info.Size().X;
                }
                if (info.Position.Y < smallestY)
                {
                    smallestY = info.Position.Y;
                }
                if (info.Position.Y + info.Size().Y > biggestY)
                {
                    biggestY = info.Position.Y + (int)info.Size().Y;
                }
            }
            Shift(smallestX < 0 ? Math.Abs(smallestX) : 0, smallestY < 0 ? Math.Abs(smallestY) : 0);

            foreach (DrawInfo draw in _drawInstructions)
            {
                Point drawPos = new Point(position.X + draw.Position.X, position.Y + draw.Position.Y);
                spriteBatch.Draw(draw.Texture, drawPos.ToVector2(), draw.Source, draw.Color, 0, Vector2.Zero, draw.Scale, 0, 0);
            }
        }

        private void Shift(int xOffset, int yOffset)
        {
            foreach (DrawInfo draw in _drawInstructions)
            {
                draw.Position = new Point(draw.Position.X + xOffset, draw.Position.Y + yOffset);
            }
        }
    }
}
