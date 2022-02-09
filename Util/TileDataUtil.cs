using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.ID;
using Twaila.UI;
namespace Twaila.Util
{
    internal class TileDataUtil
    {
        public static void UpdateUI(TwailaPanel panel, Tile tile, bool useItemTextures = false)
        {
            
            switch (tile.type)
            {
                case TileID.Dirt:
                    DefaultFramedTile(tile, panel, ItemID.DirtBlock);
                    break;
                case TileID.Stone:
                    DefaultFramedTile(tile, panel, ItemID.StoneBlock);
                    break;
                case TileID.Grass:
                    DefaultFramedTile(tile, panel, name: "Grass");
                    break;
                case TileID.Plants:
                    if(tile.frameX == 144)
                    {
                        UpdateName(panel, ItemID.Mushroom);
                        UpdateTexture(panel, ItemID.Mushroom, tile, 1, 1, 16, 20);
                    }
                    else if(tile.frameX < 108)
                    {
                        UpdateName(panel, name: "Grass Foilage", overrideName: true);
                        UpdateWithBlockTexture(panel, tile, 1, 1, 16, 20);
                    }
                    else
                    {
                        UpdateName(panel, name: "Flower Foilage", overrideName: true);
                        UpdateWithBlockTexture(panel, tile, 1, 1, 16, 20);
                    }
                    break;
                case TileID.Torches:
                    DetermineTorch(tile, panel);
                    break;
                case TileID.Trees:
                    UpdateName(panel);
                    //UpdateTexture(panel, tile: tile);
                    DetermineTree(tile, panel);
                    break;
                default:
                    DefaultFramedTile(tile, panel, name: "Defualt Name");
                    break;
            }
            panel.Mod.SetText("Terraria");
        }


        private static void DefaultPlaceableTile(int itemId, TwailaPanel panel)
        {
            panel.Image.SetImage(Main.itemTexture[itemId]);
            panel.Name.SetText(Lang.GetItemName(itemId).Value);
            panel.Mod.SetText("Terraria");
        }

        private static void DefaultFramedTile(Tile tile, TwailaPanel panel, int itemId = -1, string name = "")
        {
            if(!UpdateWithItemTexture(itemId, panel))
            {
                panel.Image.SetImage(Main.tileTexture[tile.type] ?? TextureManager.BlankTexture, 0, 54, 2, 2);

            }
            UpdateName(panel, itemId, name);
        }
        private static void UpdateName(TwailaPanel panel, int itemId = -1, string name = "", bool overrideName = false)
        {
            if (overrideName)
            {
                panel.Name.SetText(name);
            }
            else if(itemId != -1)
            {
                UpdateNameFromItem(itemId, panel);
            }
            else
            {
                UpdateNameFromMap(panel, name);
            }
        }

        private static void UpdateNameFromItem(int itemId, TwailaPanel panel)
        {
            panel.Name.SetText(Lang.GetItemNameValue(itemId));
        }

        private static void UpdateNameFromMap(TwailaPanel panel, string fallBackName = "")
        {
            string mapName = Lang.GetMapObjectName(Main.Map[Player.tileTargetX, Player.tileTargetY].Type);
            if (!mapName.Equals(""))
            {
                panel.Name.SetText(mapName);
            }
            else
            {
                panel.Name.SetText(fallBackName);
            }
        }

        private static void UpdateTexture(TwailaPanel panel, int itemId = -1, Tile tile = null, int tileCountX = 1, int tileCountY = 1, int width = 16,
            int height = 16)
        {
            if(!UpdateWithItemTexture(itemId, panel))
            {
                UpdateWithBlockTexture(panel, tile, tileCountX, tileCountY, width, height);
            }
        }

        private static bool UpdateWithItemTexture(int itemId, TwailaPanel panel)
        {
            if (itemId != -1 && TwailaConfig.Get().UseItemTextures)
            {
                panel.Image.SetImage(Main.itemTexture[itemId]);
                return true;
            }
            return false;
        }

        private static void UpdateWithBlockTexture(TwailaPanel panel, Tile tile, int tileCountX = 1, int tileCountY = 1, int width = 16,
            int height = 16)
        {
            panel.Image.SetImage(Main.tileTexture[tile.type], tile.frameX, tile.frameY, tileCountX, tileCountY, width, height);
        }

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
    }
}
