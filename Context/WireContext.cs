using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Twaila.Graphics;
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

        protected TwailaIconLine Icons { get; set; }

        public WireContext(Point point) : base(point)
        {
            Update();
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
            Tile tile = Framing.GetTileSafely(Pos);
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            HasActuator = tile.HasActuator;
            RedWire = tile.RedWire;
            BlueWire = tile.BlueWire;
            YellowWire = tile.YellowWire;
            GreenWire = tile.GreenWire;
            Icons = new TwailaIconLine();

            if(!TwailaConfig.Get().AntiCheat || (WiresUI.Settings.DrawWires && !WiresUI.Settings.HideWires))
            {
                if (InfoUtil.GetWireInfo(tile, out string wireText, out int[] wireIcons))
                {
                    if (content.ShowWire == TwailaConfig.DisplayType.Icon || content.ShowWire == TwailaConfig.DisplayType.Both)
                    {
                        foreach (int icon in wireIcons)
                        {
                            if(icon > 0)
                            {
                                Icons.IconImages.Add(ImageUtil.GetItemTexture(icon));
                            }
                        }
                    }
                    if (content.ShowWire == TwailaConfig.DisplayType.Name || content.ShowWire == TwailaConfig.DisplayType.Both)
                    {
                        WireText = wireText;
                    }
                }
            }
            if (!TwailaConfig.Get().AntiCheat || WiresUI.Settings.HideWires || WiresUI.Settings.DrawWires)
            {
                if (InfoUtil.GetActuatorInfo(tile, out string actText, out int actIcon))
                {
                    if (content.ShowActuator == TwailaConfig.DisplayType.Icon || content.ShowActuator == TwailaConfig.DisplayType.Both)
                    {
                        if (actIcon > 0)
                        {
                            Icons.IconImages.Add(ImageUtil.GetItemTexture(actIcon));
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
            int itemId = ItemUtil.GetItemId(Framing.GetTileSafely(Pos), TileType.Tile);
            return NameUtil.GetNameFromItem(itemId);
        }

        protected override string GetMod()
        {
            return "Terraria";
        }

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            return new TwailaRender(ImageUtil.GetImageForWireAndActuator(spriteBatch, Framing.GetTileSafely(Pos)));
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = new List<UITwailaElement>();

            if (!string.IsNullOrEmpty(WireText))
            {
                elements.Add(new TwailaText(WireText));
            }
            if (!string.IsNullOrEmpty(ActuatorText))
            {
                elements.Add(new TwailaText(ActuatorText));
            }
            if(Icons.IconImages.Count > 0)
            {
                elements.Add(Icons);
            }

            return elements;
        }

        public bool HasWire()
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return tile.RedWire || tile.BlueWire || tile.YellowWire || tile.GreenWire;
        }
    }
}
