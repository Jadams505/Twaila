using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Twaila.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;
using System;

namespace Twaila.Util
{
    public static class ImageUtil
    {
        public static Texture2D GetImageFromTile(SpriteBatch spriteBatch, Tile tile)
        {
            int size = 16;
            int padding = 2;
            Texture2D texture = GetTileTexture(tile.TileType);

            if (texture != null)
            {
                TextureBuilder builder = new TextureBuilder();
                for (int row = 0; row < 2; ++row)
                {
                    for (int col = 0; col < 2; ++col)
                    {
                        Rectangle copyRectangle = new Rectangle(col * (size + padding), 54 + row * (size + padding), size, size);
                        builder.AddComponent(copyRectangle, texture, new Point(size * col, size * row));
                    }
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static TwailaRender GetWallRenderFromTile(Tile tile)
        {
            if (tile.WallType > 0)
            {
                int size = 32;
                int startX = 324, startY = 108;
                Texture2D texture = GetWallTexture(tile);

                if (texture != null)
                {
                    RenderBuilder builder = new RenderBuilder();
                    Rectangle copyRectangle = new Rectangle(startX, startY, size, size);
                    builder.AddImage(source: copyRectangle, texture: texture, position: Point.Zero);
                    return builder.Build();
                }
            }
            return new TwailaRender();
        }

        public static TwailaRender GetLiquidRenderFromTile(Tile tile)
        {
            if (tile.LiquidAmount > 0)
            {
                int size = 16;
                int startX = 0, startY = 0;
                Texture2D texture = null;
                switch (tile.LiquidType)
                {
                    case LiquidID.Lava:
                        texture = TextureAssets.Liquid[WaterStyleID.Lava].ForceVanillaLoad();
                        break;
                    case LiquidID.Honey:
                        texture = TextureAssets.Liquid[WaterStyleID.Honey].ForceVanillaLoad();
                        break;
                    case LiquidID.Water:
                        texture = TextureAssets.Liquid[Main.waterStyle].ForceVanillaLoad();
                        break;
                }

                if (texture != null)
                {
                    RenderBuilder builder = new RenderBuilder();
                    Rectangle copyRectangle = new Rectangle(startX, startY, size, size);
                    builder.AddImage(source: copyRectangle, texture: texture, position: Point.Zero);
                    return builder.Build();
                }
            }
            return new TwailaRender();
        }

        public static Texture2D GetImageCustom(SpriteBatch spriteBatch, Tile tile)
        {
            return GetImageForCampfire(spriteBatch, tile) ?? GetImageForHerbs(spriteBatch, tile) ??
                GetImageForXmasTree(spriteBatch, tile) ?? TreeUtil.GetImageForBamboo(spriteBatch, tile.TileType)
                ?? TreeUtil.GetImageForSeaweed(spriteBatch, tile.TileType) ?? GetImageForMannequins(spriteBatch, tile)
                ?? GetImageForSnakeRope(spriteBatch, tile.TileType) ?? GetImageForCattail(spriteBatch, tile)
                ?? GetImageForRelic(spriteBatch, tile) ?? GetImageForPylon(spriteBatch, tile) ??
                GetImageForVoidVault(spriteBatch, tile) ?? GetImageForMarbleColumn(spriteBatch, tile);
        }

        /*
            For some reason the frameY for campfires when they are turned off does not match the frameY on
            their spritesheet. It might have something to do with animation frames
        */
        public static Texture2D GetImageForCampfire(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.TileType == TileID.Campfire && !TwailaConfig.Instance.UseItemTextures)
            {
                TileObjectData data = TileObjectData.GetTileData(tile);
                int mutableFrameY = tile.TileFrameY;
                if (tile.TileFrameY >= data.CoordinateFullHeight)
                {
                    mutableFrameY = 288; // this is the correct frameY for the campfire when it is turned off
                }
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, tile.TileFrameX, mutableFrameY, data);
            }
            return null;
        }

        /*
            Becuase herb tiles bloom and unbloom based on various conditions the tileId's do not reflect what texture
            is actually being displayed. Thus those conditions must be explititly checked in order to determine if the tile is
            mature or blooming
            Blinkroot and Shiverthorn are exceptions because once bloomed they never unbloom
        */
        public static Texture2D GetImageForHerbs(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.TileType == TileID.MatureHerbs)
            {
                int mutableId = tile.TileType;
                int style = TileObjectData.GetTileStyle(tile);
                TileObjectData data = TileUtil.GetTileObjectData(tile);
                if (style == 0 && Main.dayTime) // daybloom
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 1 && !Main.dayTime) // moonglow
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0)) // deathweed
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 4 && (Main.raining || Main.cloudAlpha > 0)) // waterleaf
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 5 && !Main.raining && Main.time > 40500) // fireblossom
                {
                    mutableId = TileID.BloomingHerbs;
                }
                return GetImageFromTileObjectData(spriteBatch, mutableId, tile.TileFrameX, tile.TileFrameY, data);
            }
            return null;
        }

        /*
            Christmas trees store extra data in the top left tile to account for decorations
        */
        public static Texture2D GetImageForXmasTree(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.ChristmasTree)
            {
                int mutableFrameY = tile.TileFrameY;
                if(tile.TileFrameX >= 10) // the top left tile's frameX is always 10
                {
                    mutableFrameY = 0; // sets the frameY to what it would be if it had no decorations
                }
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, tile.TileFrameX, mutableFrameY, TileUtil.GetTileObjectData(tile));
            }
            return null;
        }

        public static Texture2D GetImageForMannequins(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.DisplayDoll)
            {
                TileObjectData data = TileUtil.GetTileObjectData(tile);
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, tile.TileFrameX + data.CoordinateFullWidth * 4, tile.TileFrameY, TileUtil.GetTileObjectData(tile));
            }
            return null;
        }

        public static Texture2D GetImageForSnakeRope(SpriteBatch spriteBatch, int tileId)
        {
            if(tileId == TileID.MysticSnakeRope)
            {
                TextureBuilder builder = new TextureBuilder();
                Texture2D texture = GetTileTexture(tileId);
                int size = 16;
                int padding = 2;
                for(int i = 0; i < 3; ++i)
                {
                    builder.AddComponent(new Rectangle(size + padding, 0 + i * (size + padding), size, size), texture, 
                        new Point(0, 0 + i * size));
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForCattail(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.Cattail)
            {
                TextureBuilder builder = new TextureBuilder();
                Texture2D texture = GetTileTexture(tile.TileType);
                int size = 16;
                int padding = 2;
                int bottomStyle = 4;
                int middleStyle = 8;
                int topStyle = 11;

                Point drawPos = Point.Zero;
                builder.AddComponent(new Rectangle((size + padding) * topStyle, tile.TileFrameY, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle((size + padding) * middleStyle, tile.TileFrameY, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle((size + padding) * bottomStyle, tile.TileFrameY, size, size), texture, drawPos);
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForRelic(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.MasterTrophyBase)
            {
                Texture2D baseTexture = GetTileTexture(tile.TileType);
                Texture2D relicTexture = TextureAssets.Extra[198].ForceVanillaLoad();
                int size = 48;
                int padding = 2;
                int frameY = tile.TileFrameX / 54 * (size + padding);

                Point drawPos = Point.Zero;
                TextureBuilder builder = new TextureBuilder();
                builder.AddComponent(new Rectangle(0, frameY, size, size), relicTexture, drawPos);
                drawPos.Y += size;
                for(int i = 0; i < 3; ++i)
                {
                    builder.AddComponent(new Rectangle(i * 18, 54, 16, 16), baseTexture, drawPos);
                    drawPos.X += 16;
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForPylon(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.TeleportationPylon)
            {
                Texture2D baseTexture = GetTileTexture(tile.TileType);
                Texture2D pylonTexture = TextureAssets.Extra[181].ForceVanillaLoad();
                int pylonWidth = 28, pylonHeight = 44;
                int padding = 2;
                int frameX = 90 + (tile.TileFrameX / 54 * (pylonWidth + padding));

                Point drawPos = Point.Zero;
                TextureBuilder builder = new TextureBuilder();             
                int startY = 18;
                int startX = tile.TileFrameX / 54 * 54;
                for (int row = 0; row < 3; ++row)
                {
                    for (int col = 0; col < 3; ++col)
                    {
                        builder.AddComponent(new Rectangle(startX + (col * 18), startY + (row * 18), 16, 16), baseTexture, new Point(16 * col, row * 16));
                    }
                }
                drawPos.X += 10;
                drawPos.Y -= 16;
                builder.AddComponent(new Rectangle(frameX, 0, pylonWidth, pylonHeight), pylonTexture, drawPos);
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForVoidVault(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.VoidVault)
            {
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, 0, 0, TileUtil.GetTileObjectData(tile));
            }
            return null;
        }

        public static Texture2D GetImageForWireAndActuator(SpriteBatch spriteBatch, Tile tile)
        {
            if(!tile.HasTile && tile.WallType == 0 && tile.LiquidAmount <= 0)
            {
                if (tile.HasActuator)
                {
                    return GetItemTexture(ItemID.Actuator);
                }
                if (tile.YellowWire || tile.GreenWire || tile.BlueWire || tile.RedWire)
                {
                    return GetItemTexture(ItemID.Wire);
                } 
            }
            return null;
        }

        public static TwailaRender GetRenderForPlate(int foodId)
        {
            RenderBuilder builder = new RenderBuilder();

            Texture2D foodTexture = GetItemTexture(foodId);
            Rectangle foodBox = ItemID.Sets.IsFood[foodId] ? foodTexture.Frame(horizontalFrames: 1, verticalFrames: 3,
                frameX: 0, frameY: 2) : foodTexture.Frame();

            Texture2D plateTexture = GetTileTexture(TileID.FoodPlatter);
            Rectangle plateBox = plateTexture.Frame(horizontalFrames: 2);

            Point drawPos = Point.Zero;

            builder.AddImage(plateTexture, drawPos, plateBox);
            drawPos.Y += 16;
            drawPos.Y -= foodBox.Height;
            drawPos.X -= (foodBox.Width - 16) / 2;
            builder.AddImage(foodTexture, drawPos, foodBox);

            return builder.Build();
        }

        public static Texture2D GetImageForIconItem(SpriteBatch spriteBatch, int itemId)
        {
            Texture2D texture = GetItemTexture(itemId);
            DrawAnimation animation = Main.itemAnimations[itemId];

            if (animation != null)
            {
                TextureBuilder builer = new TextureBuilder();
                Rectangle box = Main.itemAnimations[itemId].GetFrame(texture);

                builer.AddComponent(box, texture, Point.Zero);

                return builer.Build(spriteBatch.GraphicsDevice);
            }
            return texture;
        }

        public static TwailaRender GetRenderForIconItem(int itemId)
        {
            Texture2D texture = GetItemTexture(itemId);
            DrawAnimation animation = Main.itemAnimations[itemId];

            if (animation != null)
            {
                RenderBuilder builer = new RenderBuilder();
                Rectangle box = Main.itemAnimations[itemId].GetFrame(texture);

                builer.AddImage(source: box, texture: texture, position: Point.Zero);

                return builer.Build();
            }
            return texture.ToRender();
        }

        public static TwailaRender GetRenderForItemFrame(SpriteBatch spriteBatch, Tile tile, int posX, int posY, int itemId)
        {
            RenderBuilder builer = new RenderBuilder();

            Texture2D itemTexture = GetItemTexture(itemId);
            DrawAnimation itemAnimation = Main.itemAnimations[itemId];
            Rectangle itemBox = itemAnimation != null ? itemAnimation.GetFrame(itemTexture, 0) : itemTexture.Frame();

            Texture2D frameTexture = GetImageFromTileDrawing(spriteBatch, tile, posX, posY);
            Rectangle frameBox = frameTexture.Frame();

            Vector2 drawPos = Vector2.Zero;

            float itemFrameSize = 20f;
            float scale = 1f;

            if (itemBox.Width > itemFrameSize || itemBox.Height > itemFrameSize)
            {
                scale = (itemBox.Width <= itemBox.Height) ? (itemFrameSize / itemBox.Height) : (itemFrameSize / itemBox.Width);
            }
            
            builer.AddImage(source: frameBox, texture: frameTexture, position: drawPos.ToPoint());

            drawPos.X += frameBox.Width / 2;
            drawPos.Y += frameBox.Height / 2;
            drawPos.X -= itemBox.Width / 2 * scale;
            drawPos.Y -= itemBox.Height / 2 * scale;

            builer.AddImage(source: itemBox, texture: itemTexture, position: drawPos.ToPoint(), scale: scale);

            return builer.Build();
        }

        public static TwailaRender GetRenderForWeaponRack(SpriteBatch spriteBatch, Tile tile, int posX, int posY, int itemId)
        {
            RenderBuilder builer = new RenderBuilder();

            Texture2D itemTexture = GetItemTexture(itemId);
            DrawAnimation itemAnimation = Main.itemAnimations[itemId];
            Rectangle itemBox = itemAnimation != null ? itemAnimation.GetFrame(itemTexture, 0) : itemTexture.Frame();

            Texture2D rackTexture = GetImageFromTileDrawing(spriteBatch, tile, posX, posY);
            Rectangle rackBox = rackTexture.Frame();

            Vector2 drawPos = Vector2.Zero;

            float rackSize = 40f;
            float scale = 1f;

            if (itemBox.Width > rackSize || itemBox.Height > rackSize)
            {
                scale = (itemBox.Width <= itemBox.Height) ? (rackSize / itemBox.Height) : (rackSize / itemBox.Width);
            }

            builer.AddImage(source: rackBox, texture: rackTexture, position: drawPos.ToPoint());

            drawPos.X += rackBox.Width / 2;
            drawPos.Y += rackBox.Height / 2;
            drawPos.X -= itemBox.Width / 2 * scale;
            drawPos.Y -= itemBox.Height / 2 * scale;

            builer.AddImage(itemTexture, drawPos.ToPoint(), itemBox, Color.White, scale);

            return builer.Build();
        }
        
        public static Texture2D GetImageForMarbleColumn(SpriteBatch spriteBatch, Tile tile)
        {
            int width = 16;
            int height = 18;
            int paddingX = 2;
            int paddingY = 6;
            int startY = 66;
            if (tile.TileType == TileID.MarbleColumn)
            {
                Texture2D texture = GetTileTexture(tile.TileType);
                if (texture != null)
                {
                    TextureBuilder builder = new TextureBuilder();
                    for (int row = 0; row < 2; ++row)
                    {
                        for (int col = 0; col < 2; ++col)
                        {
                            Rectangle copyRectangle = new Rectangle(col * (width + paddingX), startY + row * (height + paddingY), width, height);
                            builder.AddComponent(copyRectangle, texture, new Point(width * col, height * row));
                        }
                    }
                    return builder.Build(spriteBatch.GraphicsDevice);
                }
            }
            return null;
        }

        public static Texture2D GetDebugImage(SpriteBatch spriteBatch, Tile tile)
        {
            TextureBuilder builder = new TextureBuilder();
            Texture2D texture = GetTileTexture(tile.TileType);
            builder.AddComponent(new Rectangle(0, 0, texture.Width, texture.Height), texture, Point.Zero);
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        public static Texture2D GetImageFromTileObjectData(SpriteBatch spriteBatch, int tileId, int frameX, int frameY, TileObjectData data)
        {
            if (data == null)
            {
                return null;
            }
            Texture2D texture = GetTileTexture(tileId);
            if (texture != null)
            {
                TextureBuilder builder = new TextureBuilder();

                frameX = frameX / data.CoordinateFullWidth * data.CoordinateFullWidth;
                frameY = frameY / data.CoordinateFullHeight * data.CoordinateFullHeight;

                int height = 0;
                for (int row = 0; row < data.Height; ++row)
                {
                    for (int col = 0; col < data.Width; ++col)
                    {
                        int width = data.CoordinateWidth;
                        Rectangle copyRectangle = new Rectangle(frameX + (width + data.CoordinatePadding) * col,
                            frameY + height + data.CoordinatePadding * row, width, data.CoordinateHeights[row]);
                        builder.AddComponent(copyRectangle, texture, new Point(width * col, height));
                    }
                    height += data.CoordinateHeights[row];
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageFromTileDrawing(SpriteBatch spriteBatch, Tile tile, int posX, int posY)
        {
            TileObjectData data = TileUtil.GetTileObjectData(tile);
            short tileFx = tile.TileFrameX, tileFy = tile.TileFrameY;
            Main.instance.TilesRenderer.GetTileDrawData(posX, posY, tile, tile.TileType,
                ref tileFx, ref tileFy, out int width, out int height, out int top, out int h, out int addX, out int addY,
                out _, out _, out _, out _);
            if (Main.tileFrame[tile.TileType] == 0) // if the tile is not animated
            {
                tileFx += (short)addX;
                tileFy += (short)addY;
            }
            return GetImageFromTileObjectData(spriteBatch, tile.TileType, tileFx, tileFy, data);
        }

        public enum NpcStat
        {
            Health,
            Attack,
            Defense,
            Crit,
            Kill
        }

        public static TwailaRender GetRenderForNpcStat(NpcStat stat)
        {
            Texture2D texture = stat switch
            {
                NpcStat.Health => ModContent.Request<Texture2D>("Twaila/Assets/Health")?.Value,
                NpcStat.Attack => ModContent.Request<Texture2D>("Twaila/Assets/Attack")?.Value,
                NpcStat.Defense => ModContent.Request<Texture2D>("Twaila/Assets/Defense")?.Value,
                NpcStat.Crit => ModContent.Request<Texture2D>("Twaila/Assets/Crit")?.Value,
                NpcStat.Kill => GetItemTexture(ItemID.Tombstone),
                _ => null
            }; 

            RenderBuilder builder = new RenderBuilder();
            float scale = 16.5f / Math.Max(texture.Width, texture.Height);
            builder.AddImage(texture, Point.Zero, texture.Frame(), scale);

            return builder.Build();
        }

        public static TwailaRender GetRenderForBuff(int type)
        {
            Texture2D texture = TextureAssets.Buff[type].ForceVanillaLoad();

            RenderBuilder builder = new RenderBuilder();
            builder.AddImage(texture, Point.Zero, texture.Bounds, 0.5f);

            return builder.Build();
        }

        public static Texture2D GetTileTexture(int tileId)
        {
            if(tileId >= 0 && tileId < TextureAssets.Tile.Length)
            {
                Main.instance.LoadTiles(tileId);
                return TextureAssets.Tile[tileId].Value;
            }
            return null;
        }

        public static Texture2D GetWallTexture(Tile tile)
        {
            if (tile.WallType >= 0 && tile.WallType < TextureAssets.Wall.Length)
            {
                Main.instance.LoadWall(tile.WallType);
                return TextureAssets.Wall[tile.WallType].Value;
            }
            return null;
        }

        public static Texture2D GetItemTexture(int itemId)
        {
            if (itemId >= 0 && itemId < TextureAssets.Item.Length)
            {
                Main.instance.LoadItem(itemId);
                return TextureAssets.Item[itemId].Value;
            }
            return null;
        }

        public static Texture2D GetArmorTexture(Item item, EquipType equipType)
        {
            switch (equipType)
            {
                case EquipType.Head:
                    Main.instance.LoadArmorHead(item.headSlot);
                    return TextureAssets.ArmorHead[item.headSlot].Value;
                case EquipType.Body:
                    Main.instance.LoadArmorBody(item.bodySlot);
                    return TextureAssets.ArmorBody[item.bodySlot].Value;
                case EquipType.Legs:
                    Main.instance.LoadArmorLegs(item.legSlot);
                    return TextureAssets.ArmorLeg[item.legSlot].Value;
            }
            return null;
        }

		public static Texture2D GetNPCTexture(int npcId)
		{
			if (npcId >= 0 && npcId < TextureAssets.Npc.Length)
			{
				Main.instance.LoadNPC(npcId);
				return TextureAssets.Npc[npcId].Value;
			}
			return null;
		}

		public static Texture2D ForceVanillaLoad(this Asset<Texture2D> asset)
		{
			if(asset == null)
			{
				return null;
			}
			if (asset.State == AssetState.NotLoaded)
			{
				return Main.Assets.Request<Texture2D>(asset.Name, AssetRequestMode.ImmediateLoad).Value;
			}
			return asset.Value;
		}

        public static TwailaRender ToRender(this Texture2D texture)
        {
            return new TwailaRender(texture);
        }
    }
}
