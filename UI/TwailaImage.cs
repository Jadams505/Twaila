using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.UI;

namespace Twaila.UI
{
    public class TwailaImage : UIElement
    {

        public int FrameX, FrameY, SizeX, SizeY, TileWidth, TileHeight;
        public bool MultipleImages { get; private set; }
        public float Scale { get; set; }

        public bool OverrideDraw;
        public int Tree;

        private Texture2D _texture;

        public TwailaImage(Texture2D texture, int frameX, int frameY, int sizeX, int sizeY, int width = 16, int height = 16) : this(texture, width, height)
        {
            FrameX = frameX;
            FrameY = frameY;
            SizeX = sizeX;
            SizeY = sizeY;
            MultipleImages = true;
            Width.Set(sizeX, 0f);
            Height.Set(sizeY, 0f);
        }

        public TwailaImage(Texture2D texture, int width = 16, int height = 16)
        {
            _texture = texture;
            TileWidth = width;
            TileHeight = height;
            MultipleImages = false;
            Width.Set(texture.Width, 0f);
            Height.Set(texture.Height, 0f);
            Scale = 1;
            OverrideDraw = false;
        }

        public TwailaImage() : this(TextureManager.BlankTexture)
        {
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (OverrideDraw)
            {
                DrawTree(spriteBatch);
            }
            else if (MultipleImages)
            {
                for (int i = 0; i < SizeX; ++i)
                {
                    for (int j = 0; j < SizeY; ++j)
                    {
                        spriteBatch.Draw(_texture, new Vector2((int)GetDimensions().X + i * TileWidth, (int)GetDimensions().Y + j * TileHeight),
                            new Rectangle(FrameX + i * (TileWidth + 2), FrameY + j * (TileHeight + 2), TileWidth, TileHeight), Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    }
                }
            }
            else
            {
                spriteBatch.Draw(position: GetDimensions().Position() + _texture.Size() * (1f - Scale) / 2f, texture: _texture, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: Scale, effects: SpriteEffects.None, layerDepth: 0f);
            }
        }

        private void DrawTree(SpriteBatch spriteBatch)
        {
            Scale = 0.5f;
            int size = 20;
            
            Texture2D topTexture = Main.treeTopTexture[Tree + 1];
            Texture2D woodTexture = Tree == -1 ? Main.tileTexture[5] : Main.woodTexture[Tree];
            Texture2D branchTexture = Main.treeBranchTexture[Tree + 1];
            Rectangle top = new Rectangle(82, 0, size * 4, size * 4);
            Rectangle trunk1 = new Rectangle(44, 108, size, size);
            Rectangle trunk2 = new Rectangle(88, 42, size, size);
            Rectangle trunk3 = new Rectangle(66, 66, size, size);
            Rectangle leftBranch = new Rectangle(0, 42, size * 2, size * 2);
            Rectangle rightBranch = new Rectangle(42, 42, size * 2, size * 2);
            Rectangle bottomMiddle = new Rectangle(88, 154, size, size);
            Rectangle bottomLeft = new Rectangle(44, 176, size, size);
            Rectangle bottomRight = new Rectangle(22, 154, size, size);
            
            int unit = (int)Math.Round(16 * Scale);
            CalculatedStyle drawPos = GetDimensions();
            spriteBatch.Draw(topTexture, new Vector2(drawPos.X, drawPos.Y), top, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.X += (int)Math.Round(30 * Scale);
            drawPos.Y += (int)Math.Round(78 * Scale);
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk1, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk2, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(branchTexture, new Vector2(drawPos.X - (int)Math.Round(38 * Scale), drawPos.Y - (int)Math.Round(12 * Scale)), leftBranch, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk3, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(branchTexture, new Vector2(drawPos.X + (int)Math.Round(18 * Scale), drawPos.Y - (int)Math.Round(12 * Scale)), rightBranch, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), bottomMiddle, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X - unit, drawPos.Y), bottomLeft, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X + unit, drawPos.Y), bottomRight, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            
        }

        public void SetImage(Texture2D image, int width = 16, int height = 16)
        {
            _texture = image;
            TileWidth = width;
            TileHeight = height;
            MultipleImages = false;
            Width.Set(image.Width, 0f);
            Height.Set(image.Height, 0f);
            OverrideDraw = false;
            Scale = 1;
            Tree = 0;
            Recalculate();
            
        }

        public void SetImage(Texture2D image, int x, int y, int tilesX, int tilesY, int width = 16, int height = 16)
        {
            SetImage(image, width, height);
            FrameX = x;
            FrameY = y;
            SizeX = tilesX;
            SizeY = tilesY;
            MultipleImages = true;
            Width.Set(SizeX * 16, 0f);
            Height.Set(SizeY * 16, 0f);
        }

    }
}
