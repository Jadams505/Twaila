﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class WireContext : BaseContext
    {
        protected bool HasActuator { get; set; }
        protected bool RedWire { get; set; }
        protected bool BlueWire { get; set; }
        protected bool YellowWire { get; set; }
        protected bool GreenWire { get; set; }

        protected string WireText { get; set; }
        protected string ActuatorText { get; set; }

        protected UITwailaIconGrid IconGrid { get; set; }

        protected UITwailaGrid TextGrid { get; set; }

        protected Point BestTilePos => Pos.BestTilePos(); 

        public WireContext(TwailaPoint point) : base(point)
        {
            IconGrid = new UITwailaIconGrid(TwailaConfig.Instance.DisplayContent.IconsPerRow, 20f);

            TextGrid = new UITwailaGrid(TwailaConfig.Instance.DisplayContent.TextsPerRow)
            {
                SmartRows = true,
            };
            WireText = "";
            ActuatorText = "";
        }

        public static WireContext CreateWireContext(TwailaPoint pos)
        {
            Point tilePos = pos.BestTilePos();
            Tile tile = Framing.GetTileSafely(tilePos);

            if(tile.TileType >= TileLoader.TileCount)
                return null;

            if (!TileUtil.IsTilePosInBounds(tilePos))
                return null;

            bool noTile = !tile.HasTile && tile.WallType <= 0 && tile.LiquidAmount <= 0;
            bool hasWire = tile.RedWire || tile.BlueWire || tile.YellowWire || tile.GreenWire;
            bool canSeeWire = WiresUI.Settings.DrawWires && !WiresUI.Settings.HideWires;
            bool canSeeActuator = WiresUI.Settings.HideWires || WiresUI.Settings.DrawWires; // literally only necessary for the actuation rod
            
            if (noTile)
            {
                if (hasWire && (!TwailaConfig.Instance.AntiCheat.HideWires || canSeeWire))
                {
                    return new WireContext(pos);
                }
                if (tile.HasActuator && (!TwailaConfig.Instance.AntiCheat.HideWires || canSeeActuator))
                {
                    return new WireContext(pos);
                }
            }
            return null;
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(WireContext))
            {
                WireContext otherContext = (WireContext)other;
                return otherContext.HasActuator != HasActuator;
            }
            return true;
        }

        public override void Update()
        {
            Tile tile = Framing.GetTileSafely(BestTilePos);
            Content content = TwailaConfig.Instance.DisplayContent;

            HasActuator = tile.HasActuator;
            RedWire = tile.RedWire;
            BlueWire = tile.BlueWire;
            YellowWire = tile.YellowWire;
            GreenWire = tile.GreenWire;
            //Icons = new TwailaIconLine();

            if(!TwailaConfig.Instance.AntiCheat.HideWires || (WiresUI.Settings.DrawWires && !WiresUI.Settings.HideWires))
            {
                if (InfoUtil.GetWireInfo(tile, out string wireText, out int[] wireIcons))
                {
                    if (content.ShowWire == TwailaConfig.DisplayType.Icon || content.ShowWire == TwailaConfig.DisplayType.Both)
                    {
                        foreach (int icon in wireIcons)
                        {
                            if(icon > 0)
                            {
                                IconGrid.AddIcon(ImageUtil.GetItemTexture(icon).ToRender());
                            }
                        }
                    }
                    if (content.ShowWire == TwailaConfig.DisplayType.Name || content.ShowWire == TwailaConfig.DisplayType.Both)
                    {
                        WireText = wireText;
                        TextGrid.Add(new UITwailaText(WireText));
                    }
                }
            }
            if (!TwailaConfig.Instance.AntiCheat.HideWires || WiresUI.Settings.HideWires || WiresUI.Settings.DrawWires)
            {
                if (InfoUtil.GetActuatorInfo(tile, out string actText, out int actIcon))
                {
                    if (content.ShowActuator == TwailaConfig.DisplayType.Icon || content.ShowActuator == TwailaConfig.DisplayType.Both)
                    {
                        if (actIcon > 0)
                        {
                            IconGrid.AddIcon(ImageUtil.GetItemTexture(actIcon).ToRender());
                        }
                    }
                    if (content.ShowActuator == TwailaConfig.DisplayType.Name || content.ShowActuator == TwailaConfig.DisplayType.Both)
                    {
                        ActuatorText = actText;
                        TextGrid.Add(new UITwailaText(ActuatorText));
                    }
                }
            }
        }

        public override void UpdateOnChange(BaseContext prevContext, Layout layout)
        {
            Update();

            layout.Name.SetText(GetName());

            if (ContextChanged(prevContext))
            {
                layout.Image.SetImage(GetImage(Main.spriteBatch));
            }

            InfoElements().ForEach(element => layout.InfoBox.Add(element));

            layout.Mod.SetText(GetMod());
        }

        protected override string GetName()
        {
            int itemId = ItemTilePairSystem.GetItemId(Framing.GetTileSafely(BestTilePos), TileType.Tile);
            return NameUtil.GetNameFromItem(itemId);
        }

        protected override string GetMod()
        {
            return "Terraria";
        }

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            return ImageUtil.GetImageForWireAndActuator(spriteBatch, Framing.GetTileSafely(BestTilePos)).ToRender();
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = new List<UITwailaElement>();

            if(TextGrid.GridElements.Count > 0)
            {
                elements.Add(TextGrid);
            }
            if(IconGrid.GridElements.Count > 0)
            {
                elements.Add(IconGrid);
            }
            return elements;
        }

        public bool HasWire()
        {
            Tile tile = Framing.GetTileSafely(BestTilePos);
            return tile.RedWire || tile.BlueWire || tile.YellowWire || tile.GreenWire;
        }
    }
}
