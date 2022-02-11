
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;

namespace Twaila.UI
{
    public class UITwailaImage : UIElement
    {
        public Tile Tile { get; private set; }
        public float Scale;
        public int ItemId { get; private set; }
        public UITwailaImage() : this(new Tile())
        {
        }
        public UITwailaImage(Tile tile, int itemId = -1, float scale = 1)
        {
            Tile = tile;
            ItemId = itemId;
            Scale = scale;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            bool drawSuccess = false;
            if (TwailaConfig.Get().UseItemTextures)
            {
                drawSuccess = !DrawFromItemData(spriteBatch) && !DrawFromTileData(spriteBatch) && !DrawFromTile(spriteBatch);
                return;
            }
            drawSuccess = !DrawFromTileData(spriteBatch) && !DrawFromTile(spriteBatch) && !DrawFromItemData(spriteBatch);
        }
        private bool DrawFromTileData(SpriteBatch spriteBatch)
        {
            TileObjectData data = TileObjectData.GetTileData(Tile);
            Texture2D texture = GetTileTexture();
            if (data != null && texture != null && !texture.Equals(TextureManager.BlankTexture))
            {
                SetSizeFromTileData();
                int fullWidth = GetSpriteWidth() + (data.Width * data.CoordinatePadding);
                int fullHeight = GetSpriteHeight() + (data.Height * data.CoordinatePadding);
                CalculatedStyle dim = GetDimensions();
                int frameX = Tile.frameX / fullWidth * fullWidth;
                int frameY = Tile.frameY / fullHeight * fullHeight;
                if (data.Style > data.StyleWrapLimit)
                {
                    if (data.StyleHorizontal)
                    {
                        frameY += fullHeight;
                    }
                    else
                    {
                        frameX += fullWidth;
                    }  
                }
                for (int row = 0; row < data.Height; ++row)
                {
                    for (int col = 0; col < data.Width; ++col)
                    {
                        float drawPosX = (int)Math.Round(dim.X) + col * data.CoordinateWidth;
                        float drawPosY = (int)Math.Round(dim.Y) + row * data.CoordinateHeights[row - 1 >= 0 ? row - 1 : 0];
                        spriteBatch.Draw(texture, new Vector2(drawPosX, drawPosY),
                            new Rectangle(frameX + col * (data.CoordinateWidth + data.CoordinatePadding), 
                            frameY + row * (data.CoordinateHeights[row - 1 >= 0 ? row - 1 : 0] + data.CoordinatePadding),
                            data.CoordinateWidth, data.CoordinateHeights[row]), Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    }
                }
                return true;
            }
            return false;
        }
        private bool DrawFromTile(SpriteBatch spriteBatch)
        {
            
            CalculatedStyle dim = GetDimensions();
            int size = 16;
            int padding = 2;
            Texture2D texture = GetTileTexture();
            if(texture != null && !texture.Equals(TextureManager.BlankTexture))
            {
                SetSizeFromTile();
                for (int row = 0; row < 2; ++row)
                {
                    for (int col = 0; col < 2; ++col)
                    {
                        Vector2 drawPos = new Vector2((int)dim.X + col * size, (int)dim.Y + row * size);
                        Rectangle spriteData = new Rectangle(col * (size + padding), 54 + row * (size + padding), size, size);
                        spriteBatch.Draw(texture, drawPos, spriteData, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    }
                }
                return true;
            }
            return false;
        }
        private void SetSizeFromTile()
        {
            Width.Set(32, 0);
            Height.Set(32, 0);
            Recalculate();
        }
        private Texture2D GetTileTexture()
        {
            ModTile mTile = TileLoader.GetTile(Tile.type);
            if(mTile != null)
            {
                return GetModdedTileTexture();
            }
            return Main.tileTexture[Tile.type];
        }
        private Texture2D GetModdedTileTexture()
        {
            ModTile mTile = TileLoader.GetTile(Tile.type);
            if(mTile != null)
            {
                string texturePath = mTile.HighlightTexture;
                int index = texturePath.IndexOf("_Highlight");
                if(index != -1)
                {
                    try
                    {
                        return ModContent.GetTexture(texturePath.Substring(0, index));
                    }
                    catch (Exception) { }
                }          
            }
            return TextureManager.BlankTexture;
        }
        private void SetSizeFromTileData()
        {
            Width.Set(GetSpriteWidth(), 0);
            Height.Set(GetSpriteHeight(), 0);
            Recalculate();
        }
        private bool DrawFromItemData(SpriteBatch spriteBatch)
        {
            if(ItemId != -1)
            {    
                Texture2D itemTexture = GetItemTexture();     
                if (itemTexture != null && !itemTexture.Equals(TextureManager.BlankTexture))
                {
                    SetSizeFromItemData(itemTexture);
                    spriteBatch.Draw(position: GetDimensions().Position() + itemTexture.Size() * (1f - Scale) / 2f, texture: itemTexture, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: Scale, effects: SpriteEffects.None, layerDepth: 0f);
                    return true;
                } 
            }
            return false;
        }
        private Texture2D GetItemTexture()
        {
            ModTile mTile = TileLoader.GetTile(Tile.type);
            if (mTile != null)
            {
                return GetModdedItemTexture();
            }
            return ItemId == -1 ? TextureManager.BlankTexture : Main.itemTexture[ItemId];
        }
        private Texture2D GetModdedItemTexture()
        {
            ModItem mItem = ModContent.GetModItem(ItemId);
            if (mItem != null)
            {
                try
                {
                    return ModContent.GetTexture(mItem.Texture);
                }
                catch (Exception) { }
            }
            return TextureManager.BlankTexture;
        }
        private void SetSizeFromItemData(Texture2D itemTexture)
        {
            Width.Set(itemTexture.Width, 0);
            Height.Set(itemTexture.Height, 0);
            Recalculate();
        }

        private int GetSpriteHeight()
        {
            TileObjectData data = TileObjectData.GetTileData(Tile);
            if(data != null)
            {
                int height = 0;
                foreach (int i in data.CoordinateHeights)
                {
                    height += i;
                }
                return height;
            }
            return 0;
        }
        private int GetSpriteWidth()
        {
            TileObjectData data = TileObjectData.GetTileData(Tile);
            return data == null ? 0 : data.Width * data.CoordinateWidth;
        }
        public void Set(Tile tile, int itemId = -1, float scale = 1)
        {
            Tile copy = new Tile();
            copy.CopyFrom(tile);
            Tile = copy;
            ItemId = itemId;
            Scale = scale;
        }

    }
}
