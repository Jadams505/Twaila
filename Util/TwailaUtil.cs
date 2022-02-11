using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.UI;
namespace Twaila.Util
{
    internal class TwailaUtil
    {

        public static void UpdateUI(TwailaPanel panel, Tile tile)
        {
            UpdateName(panel, tile, itemId: FindItemId(tile), name: "Default Name");
            UpdateModName(panel, tile);
            panel.Image.Set(tile, FindItemId(tile));
        }

        private static int FindItemId(Tile tile)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            int style = TileObjectData.GetTileStyle(tile);
            if(mTile == null)
            {
                Item item = new Item();
                for (int i = 0; i < ItemID.Count; ++i)
                {
                    item.SetDefaults(i);
                    if (item.createTile == tile.type && (style == -1 || item.placeStyle == style))
                    {
                        return i;
                    }
                }
                return -1;
            }
            return mTile.drop;    
        }
        private static void UpdateName(TwailaPanel panel, Tile tile, int itemId = -1, string name = "", bool overrideName = false)
        {
            if (overrideName)
            {
                panel.Name.SetText(name);
                return;
            }
            if(TileLoader.GetTile(tile.type) == null) // vanilla
            {
                if(UpdateNameFromItem(itemId, panel) || UpdateNameFromMap(panel, tile))
                {
                    return;
                }
            }
            else if (UpdateNameFromMap(panel, tile) || UpdateNameFromItem(itemId, panel))
            {
                return;
            }
            panel.Name.SetText(name);
        }

        private static bool UpdateNameFromItem(int itemId, TwailaPanel panel)
        {
            if(itemId == -1)
            {
                return false;
            }
            ModItem item = ModContent.GetModItem(itemId);
            if(item == null) // vanilla
            {
                panel.Name.SetText(Lang.GetItemNameValue(itemId));
                return true;
            }
            string name = item.DisplayName.GetDefault();
            if(name == null || name.Equals(""))
            {
                return false;
            }
            panel.Name.SetText(name);
            return true;
        }

        private static bool UpdateNameFromMap(TwailaPanel panel, Tile tile)
        {
            string mapName = Lang.GetMapObjectName(Main.Map[Player.tileTargetX, Player.tileTargetY].Type);
            if (!mapName.Equals(""))
            {
                panel.Name.SetText(mapName);
                return true;
            }
            return false;
        }

        private static bool UpdateModName(TwailaPanel panel, Tile tile)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            panel.Mod.SetText(mTile == null ? "Terraria" : mTile.mod.DisplayName);
            return true;
        }

        /*
        private static void DetermineTorch(Tile tile, TwailaPanel panel)
        {
            int[] torchIds = {ItemID.Torch, ItemID.BlueTorch, ItemID.RedTorch, ItemID.GreenTorch, ItemID.PurpleTorch, ItemID.WhiteTorch,
                ItemID.YellowTorch, ItemID.DemonTorch, ItemID.CursedTorch, ItemID.IceTorch, ItemID.OrangeTorch, ItemID.IchorTorch, ItemID.UltrabrightTorch,
                ItemID.BoneTorch, ItemID.RainbowTorch, ItemID.PinkTorch};
            UpdateName(panel, itemId: torchIds[tile.frameY / 22]);
            UpdateTexture(panel, itemId: torchIds[tile.frameY / 22], tile, 1, 1, 20, 20);
            panel.Image.FrameX = 0; // uses only the first torch texture from the sprite sheet
        }

        private static void DetermineTree(Tile tile, TwailaPanel panel)
        {
            int? type = GetTreeType();
            if(type != null)
            {
                panel.Image.Tree = type.Value;
                panel.Image.Height.Set(74, 0);
                panel.Image.Width.Set(40, 0);
                panel.Image.OverrideDraw = true;
            }
        }

        

        private static int? GetTreeType()
        {
            int x = Player.tileTargetX, y = Player.tileTargetY;
            Tile tile = Main.tile[x, y];
            if (tile.frameX == 66 && tile.frameY <= 45)
            {
                x++;
            }
            if (tile.frameX == 88 && tile.frameY >= 66 && tile.frameY <= 110)
            {
                x--;
            }
            if (tile.frameX == 22 && tile.frameY >= 132 && tile.frameY < 198)
            {
                x--;
            }
            if (tile.frameX == 44 && tile.frameY >= 132)
            {
                x++;
            }
            while (Main.tile[x, y].type == 5 && Main.tile[x, y].active())
            {
                y += 1;
            }
            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return null;
            }
            switch (Main.tile[x, y].type)
            {
                case 23:
                    return 0;
                case 60:
                    if (!(y > Main.worldSurface))
                    {
                        return 1;
                    }
                    return 5;
                case 70:
                    return 6;
                case 109:
                    return 2;
                case 147:
                    return 3;
                case 199:
                    return 4;
                case 2:
                    return -1;
                default:
                    return null;
            }
        }
        */
    }
}
