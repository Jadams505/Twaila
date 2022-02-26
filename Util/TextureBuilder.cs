using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;

namespace Twaila.Util
{
    public class TextureBuilder
    {
        private class Component
        {
            public Rectangle BoundingBox;
            public Texture2D Texture;
            public Point Position;

            public Component(Rectangle boundingBox, Texture2D texture, Point position)
            {
                BoundingBox = boundingBox;
                Texture = texture;
                Position = position;
            }
        }

        private List<Component> _components;

        public TextureBuilder()
        {
            _components = new List<Component>();
        }

        public void AddComponent(Rectangle boundingBox, Texture2D texture, Point position)
        {
            Component comp = new Component(boundingBox, texture, position);
            _components.Add(comp);    
        }

        public Texture2D Build(GraphicsDevice graphicsDevice)
        {
            int smallestX = _components[0].Position.X, biggestX = smallestX + _components[0].BoundingBox.Width;
            int smallestY = _components[0].Position.Y, biggestY = smallestY + _components[0].BoundingBox.Height;
            for(int i = 1; i < _components.Count; ++i)
            {
                Component c = _components[i];
                if(c.Position.X < smallestX)
                {
                    smallestX = c.Position.X;
                }
                if(c.Position.X + c.BoundingBox.Width > biggestX)
                {
                    biggestX = c.Position.X + c.BoundingBox.Width;
                }
                if(c.Position.Y < smallestY)
                {
                    smallestY = c.Position.Y;
                }
                if(c.Position.Y + c.BoundingBox.Height > biggestY)
                {
                    biggestY = c.Position.Y + c.BoundingBox.Height;
                }
            }
            Texture2D texture = new Texture2D(graphicsDevice, biggestX - smallestX, biggestY - smallestY);
            Shift(smallestX < 0 ? Math.Abs(smallestX) : 0, smallestY < 0 ? Math.Abs(smallestY) : 0);
            Color[] data = new Color[texture.Width * texture.Height];
            foreach(Component comp in _components)
            {
                if (comp.BoundingBox.X + comp.BoundingBox.Width > comp.Texture.Width || comp.BoundingBox.Y + comp.BoundingBox.Height > comp.Texture.Height)
                {
                    return null;
                }
                Populate(comp, data, texture.Width);
            }
            texture.SetData(data);
            return texture;
        }

        private void Shift(int xOffset, int yOffset)
        {
            if(xOffset == 0 && yOffset == 0)
            {
                return;
            }
            foreach(Component c in _components)
            {
                c.Position.X += xOffset;
                c.Position.Y += yOffset;
            }
        }

        private void Populate(Component comp, Color[] toPopulate, int width)
        {
            Color[] data = new Color[comp.BoundingBox.Width * comp.BoundingBox.Height];
            comp.Texture.GetData(0, comp.BoundingBox, data, 0, data.Length);
            for(int i = 0; i < data.Length; ++i)
            {
                int row = i / comp.BoundingBox.Width;
                int col = i % comp.BoundingBox.Width;
                if(data[i].A != 0)
                {
                    int populateIndex = (comp.Position.Y + row) * width + comp.Position.X + col;
                    toPopulate[populateIndex] = data[i];
                }
            }
        }
    }
}
