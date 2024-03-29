﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Twaila.Graphics
{
    public class RenderBuilder
    {
        private List<DrawInfo> _drawInstructions;

        public RenderBuilder()
        {
            _drawInstructions = new List<DrawInfo>();
        }

        public void AddImage(Texture2D texture, Point position, Rectangle source, Color color, float scale = 1)
        {
            if(texture != null)
            {
                _drawInstructions.Add(new DrawInfo(texture, position, source, color, scale));
            }
        }

        public void AddImage(DrawInfo info)
        {
            AddImage(info.Texture, info.Position, info.Source, info.Color, info.Scale);
        }

        public void AddImage(Texture2D texture, Point position, Rectangle source, float scale = 1)
        {
            AddImage(texture, position, source, Color.White, scale);
        }

        public TwailaRender Build()
        {
            return new TwailaRender(_drawInstructions);
        }
    }
}
