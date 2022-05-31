using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Twaila.Context;
using Twaila.Graphics;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel, IDragable
    {
        public TwailaInfoBox InfoBox { get; set; }
        public UITwailaImage Image { get; set; }
        public TileContext Context { get; set; }
        private int pickIndex = 0;
        private bool _dragging;
        private Point _lastMouse;
        private int _tick = 0;

        private Vector2 MaxPanelDimension => new Vector2(TwailaConfig.Get().MaxWidth / 100.0f * Parent.GetDimensions().Width, TwailaConfig.Get().MaxHeight / 100.0f * Parent.GetDimensions().Height);
        private Vector2 MaxPanelInnerDimension => new Vector2(MaxPanelDimension.X - PaddingLeft - PaddingRight, MaxPanelDimension.Y - PaddingTop - PaddingLeft);

        public TwailaPanel()
        {
            Context = new TileContext();
            Image = new UITwailaImage();
            InfoBox = new TwailaInfoBox();
            Image.MarginRight = 10;
            Width.Set(0, 0);
            Height.Set(0, 0);
            Top.Set(0, 0);
            Left.Set(PlayerInput.RealScreenWidth / 2, 0);
            Append(InfoBox);
            //Append(Image);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateFromConfig();
            UpdateSize();
            Drag();
            UpdateAlignment();
        }

        private void UpdateFromConfig()
        {
            TwailaConfig config = TwailaConfig.Get();
            
            Image.DrawMode = config.ContentSetting;
            Image.Opacity = 1;
            InfoBox.ApplyToAll((text) =>
            {
                text.OverrideTextColor = config.OverrideColor;
                text.Opacity = 1;
                text.Color = config.TextColor.Color;
                text.TextShadow = config.TextShadow;
                text.DrawMode = config.ContentSetting;
            });

            BackgroundColor = config.PanelColor.Color;
            BorderColor = Color.Black;
            if (IsMouseHovering && !IsDragging())
            {
                BackgroundColor *= config.HoverOpacity;
                BorderColor *= config.HoverOpacity;
                Image.Opacity = config.HoverOpacity;
                InfoBox.ApplyToAll((text) =>
                {
                    text.OverrideTextColor = true;
                    text.Opacity = config.HoverOpacity;
                });
            }
        }

        private float GetDimension(UIElement childElement, float dimension)
        {
            return HasChild(childElement) ? dimension : 0;
        }

        private void UpdateSize()
        {
            SetPadding(TwailaConfig.Get().PanelPadding);
            SetInitialSizes();
            float imageHeight = GetDimension(Image, Image.Height.Pixels);
            float textHeight = InfoBox.Height.Pixels;
            float imageWidth = GetDimension(Image, Image.image.Width);
            float textWidth = InfoBox.Width.Pixels;
            if(InfoBox.IsEmpty())
            {
                Image.MarginRight = 0;
            }
            DrawMode drawMode = TwailaConfig.Get().ContentSetting;
            if (drawMode == DrawMode.Shrink)
            {
                imageWidth *= ImageScale(new Vector2(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));

                Vector2 maxSize = new Vector2(MaxPanelInnerDimension.X - imageWidth - GetDimension(Image, Image.MarginRight), MaxPanelInnerDimension.Y / InfoBox.NumberOfAppendedElements());
                textHeight = 0;
                InfoBox.ApplyToAll((element) =>
                {
                    float height = element.GetTextSize().Y * TextScale(element, new Vector2(maxSize.X, maxSize.Y));
                    element.Height.Set(height, 0);
                    textHeight += height;
                });
                textWidth = 0;
                InfoBox.ApplyToAll((element) =>
                {
                    float width = element.GetTextSize().X * TextScale(element, maxSize);
                    element.Width.Set(width, 0);
                    if(width > textWidth)
                    {
                        textWidth = width;
                    }
                });

                Vector2 remainingSpace = new Vector2(MaxPanelInnerDimension.X - textWidth - GetDimension(Image, Image.MarginRight), MaxPanelInnerDimension.Y);

                imageWidth = GetDimension(Image, Image.image.Width) * ImageScale(remainingSpace);
                imageHeight = GetDimension(Image, Image.image.Height) * ImageScale(remainingSpace);
            }
            else
            {
                if (drawMode == DrawMode.Trim)
                {
                    float height = 0;
                    for(int i = 0; i < InfoBox.InfoLines.Length; ++i)
                    {
                        if (InfoBox.Appended[i])
                        {
                            TwailaText element = InfoBox.InfoLines[i];
                            if (element.GetTextSize().Y + height < MaxPanelInnerDimension.Y)
                            {
                                height += element.GetTextSize().Y;
                            }
                            else
                            {
                                InfoBox.RemoveElements((InfoType)i);
                            }
                        }
                    }
                    textHeight = height;
                }
                imageHeight = Math.Min(MaxPanelInnerDimension.Y, imageHeight);
                textHeight = Math.Min(MaxPanelInnerDimension.Y, textHeight);
                imageWidth = Math.Min(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, imageWidth);
                textWidth = Math.Min(MaxPanelInnerDimension.X - imageWidth - GetDimension(Image, Image.MarginRight), textWidth);
                InfoBox.ApplyToAll((element) =>
                {
                    element.Width.Set(textWidth, 0);
                });

                Vector2 remainingSpace = new Vector2(MaxPanelInnerDimension.X - textWidth - GetDimension(Image, Image.MarginRight), MaxPanelInnerDimension.Y);

                imageWidth = MathHelper.Clamp(GetDimension(Image, Image.image.Width), 0, remainingSpace.X);
                imageHeight = MathHelper.Clamp(GetDimension(Image, Image.image.Height), 0, remainingSpace.Y);
            }

            float calculatedHeight = imageHeight > textHeight ? imageHeight : textHeight;
            Height.Set(calculatedHeight + PaddingTop + PaddingBottom, 0);
            Image.Height.Set(Math.Max(imageHeight, textHeight), 0);
            
            float calculatedWidth = textWidth + imageWidth + GetDimension(Image, Image.MarginRight) + PaddingLeft + PaddingRight;
            Width.Set(calculatedWidth, 0);
            Image.Width.Set(imageWidth, 0);
        }

        private void SetInitialSizes()
        {
            InfoBox.ApplyToAll((element) =>
            {
                element.Width.Set(element.GetTextSize().X, 0);
                element.Height.Set(element.GetTextSize().Y, 0);
            });
            InfoBox.UpdateDimensions();
            Image.Width.Set(Image.image.Width, 0);
            Image.Height.Set(Image.image.Height, 0);
            Image.MarginRight = 10;
        }

        public float ImageScale(Vector2 maxSize)
        {
            float scaleX = 1;
            if (GetDimension(Image, Image.image.Width) > maxSize.X)
            {
                scaleX = maxSize.X / GetDimension(Image, Image.image.Width);
            }
            float scaleY = 1;
            if (GetDimension(Image, Image.image.Height) > maxSize.Y)
            {
                scaleY = maxSize.Y / GetDimension(Image, Image.image.Height);
            }
            return Math.Min(scaleX, scaleY);
        }

        public float TextScale(TwailaText text, Vector2 maxSize)
        {
            float scaleX = 1;
            if (text.GetTextSize().X > maxSize.X)
            {
                scaleX = maxSize.X / text.GetTextSize().X;
            }
            float scaleY = 1;
            if (text.GetTextSize().Y > maxSize.Y)
            {
                scaleY = maxSize.Y / text.GetTextSize().Y;
            }
            return Math.Min(scaleX, scaleY) * text.Scale;
        }
        
        private void UpdateAlignment()
        {
            if (TwailaConfig.Get().UseDefaultPosition)
            {
                TwailaConfig.Get().AnchorPosX = (int)Parent.GetDimensions().Width / 2;
                TwailaConfig.Get().AnchorPosY = 0;
            }
            UpdatePos();
            Image.Top.Set(0, 0);
            InfoBox.Left.Set(GetDimension(Image, Image.Width.Pixels + Image.MarginRight), 0);
            InfoBox.UpdateVertically();
            Recalculate();
        }

        private void UpdatePos()
        {
            float left = 0;
            switch (TwailaConfig.Get().AnchorX)
            {
                case TwailaConfig.HorizontalAnchor.Left:
                    left = TwailaConfig.Get().AnchorPosX;
                    break;
                case TwailaConfig.HorizontalAnchor.Center:
                    left = TwailaConfig.Get().AnchorPosX - Width.Pixels / 2;
                    break;
                case TwailaConfig.HorizontalAnchor.Right:
                    left = TwailaConfig.Get().AnchorPosX - Width.Pixels;
                    break;
            }
            float top = 0;
            switch (TwailaConfig.Get().AnchorY)
            {
                case TwailaConfig.VerticalAnchor.Top:
                    top = TwailaConfig.Get().AnchorPosY;
                    break;
                case TwailaConfig.VerticalAnchor.Center:
                    top = TwailaConfig.Get().AnchorPosY - Height.Pixels / 2;
                    break;
                case TwailaConfig.VerticalAnchor.Bottom:
                    top = TwailaConfig.Get().AnchorPosY - Height.Pixels;
                    break;
            }
            Left.Set(MathHelper.Clamp(left, 0, Parent.GetDimensions().Width - Width.Pixels), 0);
            Top.Set(MathHelper.Clamp(top, 0, Parent.GetDimensions().Height - Height.Pixels), 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (TwailaConfig.Get().ShowBackground)
            {
                base.DrawSelf(spriteBatch);
            }
        }
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
            if (!IsDragging())
            {
                UpdatePanelContents(spriteBatch);
            }    
        }

        private void UpdatePanelContents(SpriteBatch spriteBatch)
        {
            _tick++;
            TileContext currentContext = TwailaUI.GetContext(TwailaUI.GetMousePos());
            if(currentContext.TileType == TileType.Empty || TileUtil.IsBlockedByAntiCheat(currentContext))
            {
                _tick = 0;
                return;
            }
            if (!currentContext.ContentChanged(Context))
            {
                currentContext.SetTileType(Context.TileType);
            }
            else
            {
                pickIndex = 0;
                _tick = 0;
            }
            Player player = Main.player[Main.myPlayer];
            player.TryGetModPlayer(out TwailaPlayer tPlayer);
            if (_tick >= TwailaConfig.Get().CycleDelay && !tPlayer.CyclingPaused)
            {
                TileUtil.CycleType(currentContext);
                _tick = 0;
            }
            
            

            if(player?.itemAnimation > 0 && currentContext.TileType != TileType.Empty) // attempts to stop rapid updating when mining/hammering
            {
                if(player?.HeldItem.pick > 0 && currentContext.TileType != TileType.Tile)
                {
                    return;
                }
                if(player?.HeldItem.hammer > 0 && currentContext.TileType != TileType.Wall)
                {
                    return;
                }
            }
            Tile tile = Framing.GetTileSafely(currentContext.Pos);
            int itemId = ItemUtil.GetItemId(currentContext);
            InfoBox.RemoveAll();
            if (currentContext.TileType != TileType.Empty && !TileUtil.IsBlockedByAntiCheat(currentContext) && currentContext.ContextChanged(Context))
            {
                if (TwailaConfig.Get().DisplayContent.ShowImage)
                {
                    if (!HasChild(Image))
                    {
                        Append(Image);
                    }
                    Image.SetImage(GetImage(spriteBatch, currentContext, tile, itemId));
                }
                else if (HasChild(Image))
                {
                    RemoveChild(Image);
                }
            }
            SetInfoBoxElements(currentContext, tile, itemId);
            Context = currentContext;
        }

        private void SetInfoBoxElements(TileContext context, Tile tile, int itemId)
        {
            if (TwailaConfig.Get().DisplayContent.ShowName)
            {
                InfoBox.SetAndAppend(InfoType.Name, context.GetName(tile, itemId));
            }
            if (TwailaConfig.Get().DisplayContent.ShowMod)
            {
                InfoBox.SetAndAppend(InfoType.Mod, context.GetMod());
            }
            if (TwailaConfig.Get().DisplayContent.ShowId && InfoUtil.GetId(tile, context.TileType, out int tileId))
            {
                InfoBox.SetAndAppend(InfoType.Id, $"{context.TileType} Id: {tileId}");
            }
            string iconText = "";
            
            if (context.TileType == TileType.Tile && InfoUtil.GetPickaxePower(tile.TileType) > 0)
            {
                Main.player[Main.myPlayer].TryGetModPlayer(out TwailaPlayer tPlayer);
                if (Main.GameUpdateCount % TwailaConfig.Get().CycleDelay == 0 && !tPlayer.CyclingPaused)
                {
                    pickIndex++;
                }
                if (InfoUtil.GetPickInfo(tile, ref pickIndex, out string pickText, out string pickIcon, out int id))
                {
                    if (TwailaConfig.Get().DisplayContent.ShowPickaxe == TwailaConfig.DisplayType.Icon ||
                    TwailaConfig.Get().DisplayContent.ShowPickaxe == TwailaConfig.DisplayType.Both)
                    {
                        iconText += pickIcon;
                    }
                    if (TwailaConfig.Get().DisplayContent.ShowPickaxe == TwailaConfig.DisplayType.Name ||
                        TwailaConfig.Get().DisplayContent.ShowPickaxe == TwailaConfig.DisplayType.Both)
                    {
                        if(id != -1)
                        {
                            InfoBox.SetAndAppend(InfoType.RecommendedPickaxe, NameUtil.GetNameFromItem(id));
                        }
                        else
                        {
                            pickIndex = 0;
                        }
                    }
                    if (TwailaConfig.Get().DisplayContent.ShowPickaxePower)
                    {
                        InfoBox.SetAndAppend(InfoType.PickaxePower, pickText);
                    }
                }
            }

            if (TwailaConfig.Get().DisplayContent.ShowPaint != TwailaConfig.DisplayType.Off && 
                InfoUtil.GetPaintInfo(tile, context.TileType, out string paintText, out string paintIcon))
            {
                if(TwailaConfig.Get().DisplayContent.ShowPaint == TwailaConfig.DisplayType.Icon ||
                    TwailaConfig.Get().DisplayContent.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    iconText += paintIcon;
                }
                if(TwailaConfig.Get().DisplayContent.ShowPaint == TwailaConfig.DisplayType.Name ||
                    TwailaConfig.Get().DisplayContent.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    InfoBox.SetAndAppend(InfoType.PaintText, paintText);
                }
            }
            
            if (!TwailaConfig.Get().AntiCheat || (WiresUI.Settings.DrawWires && !WiresUI.Settings.HideWires))
            {
                if (TwailaConfig.Get().DisplayContent.ShowWire != TwailaConfig.DisplayType.Off && 
                    InfoUtil.GetWireInfo(tile, out string wireText, out string wireIcon))
                {
                    if (TwailaConfig.Get().DisplayContent.ShowWire == TwailaConfig.DisplayType.Icon ||
                        TwailaConfig.Get().DisplayContent.ShowWire == TwailaConfig.DisplayType.Both)
                    {
                        iconText += wireIcon;
                    }
                    if (TwailaConfig.Get().DisplayContent.ShowWire == TwailaConfig.DisplayType.Name ||
                        TwailaConfig.Get().DisplayContent.ShowWire == TwailaConfig.DisplayType.Both)
                    {
                        InfoBox.SetAndAppend(InfoType.WireText, wireText);
                    }
                }
            }
            if (!TwailaConfig.Get().AntiCheat || WiresUI.Settings.HideWires || WiresUI.Settings.DrawWires)
            {
                if (TwailaConfig.Get().DisplayContent.ShowActuator != TwailaConfig.DisplayType.Off && 
                    InfoUtil.GetActuatorInfo(tile, out string actText, out string actIcon))
                {
                    if (TwailaConfig.Get().DisplayContent.ShowActuator == TwailaConfig.DisplayType.Icon ||
                        TwailaConfig.Get().DisplayContent.ShowActuator == TwailaConfig.DisplayType.Both)
                    {
                        iconText += actIcon;
                    }
                    if (TwailaConfig.Get().DisplayContent.ShowActuator == TwailaConfig.DisplayType.Name ||
                        TwailaConfig.Get().DisplayContent.ShowActuator == TwailaConfig.DisplayType.Both)
                    {
                        InfoBox.SetAndAppend(InfoType.ActuatorText, actText);
                    }
                }
            }
            if (iconText != "")
            {
                InfoBox.SetAndAppend(InfoType.InfoIcons, iconText);
            }
        }

        private static TwailaTexture GetImage(SpriteBatch spriteBatch, TileContext context, Tile tile, int itemId)
        {
            if (TwailaConfig.Get().UseItemTextures)
            {
                TwailaTexture item = context.GetImage(spriteBatch, itemId);
                return item?.Texture != null ? item : context.GetImage(spriteBatch, tile);
            }
            else
            {
                TwailaTexture tileTexture = context.GetImage(spriteBatch, tile);
                return tileTexture?.Texture != null ? tileTexture : context.GetImage(spriteBatch, itemId);
            }
        }

        public override void MouseDown(UIMouseEvent evt)
        {   
            _lastMouse = new Point(Main.mouseX, Main.mouseY);
            if (!TwailaConfig.Get().LockPosition)
            {
                Main.LocalPlayer.mouseInterface = true;
                TwailaConfig.Get().UseDefaultPosition = false;
                _dragging = true;
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            _dragging = false;
        }

        public bool IsDragging()
        {
            return _dragging && !Main.ingameOptionsWindow && !Main.hideUI;
        }

        public void Drag()
        {
            if (IsDragging())
            {
                int deltaX = Main.mouseX - _lastMouse.X, deltaY = Main.mouseY - _lastMouse.Y;
                TwailaConfig.Get().AnchorPosX += deltaX;
                TwailaConfig.Get().AnchorPosY += deltaY;
                TwailaConfig.Get().AnchorPosX = (int)MathHelper.Clamp(TwailaConfig.Get().AnchorPosX, 0, Parent.GetDimensions().Width);
                TwailaConfig.Get().AnchorPosY = (int)MathHelper.Clamp(TwailaConfig.Get().AnchorPosY, 0, Parent.GetDimensions().Height);
                _lastMouse.X = Main.mouseX;
                _lastMouse.Y = Main.mouseY;
            }
        }

        public bool ContainsPoint(int x, int y)
        {
            return GetDimensions().ToRectangle().Intersects(new Rectangle(x, y, 0, 0));
        }
    }
}
