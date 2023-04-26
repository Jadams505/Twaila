using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
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

        public WireContext(TwailaPoint point) : base(point)
        {
            IconGrid = new UITwailaIconGrid(TwailaConfig.Instance.DisplayContent.IconsPerRow)
            {
                MaxSize = 20f,
            };
            WireText = "";
            ActuatorText = "";
        }

        public static WireContext CreateWireContext(TwailaPoint pos)
        {
            Tile tile = Framing.GetTileSafely(pos.BestPos());

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
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            TwailaConfig.Content content = TwailaConfig.Instance.DisplayContent;

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

            InfoElements().ForEach(element => layout.InfoBox.AddAndEnable(element));

            layout.Mod.SetText(GetMod());
        }

        protected override string GetName()
        {
            int itemId = ItemTilePairSystem.GetItemId(Framing.GetTileSafely(Pos.BestPos()), TileType.Tile);
            return NameUtil.GetNameFromItem(itemId);
        }

        protected override string GetMod()
        {
            return "Terraria";
        }

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            return ImageUtil.GetImageForWireAndActuator(spriteBatch, Framing.GetTileSafely(Pos.BestPos())).ToRender();
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = new List<UITwailaElement>();

            if (!string.IsNullOrEmpty(WireText))
            {
                elements.Add(new UITwailaText(WireText));
            }
            if (!string.IsNullOrEmpty(ActuatorText))
            {
                elements.Add(new UITwailaText(ActuatorText));
            }
            if(IconGrid.GridElements.Count > 0)
            {
                elements.Add(IconGrid);
            }
            /*
            if(Icons.IconImages.Count > 0)
            {
                elements.Add(Icons);
            }
            */
            return elements;
        }

        public bool HasWire()
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            return tile.RedWire || tile.BlueWire || tile.YellowWire || tile.GreenWire;
        }
    }
}
