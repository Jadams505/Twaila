﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Twaila.Context;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel, IDragable
    {
        private TwailaText Name { get; set; } 
        private TwailaText Mod { get; set; }
        private UITwailaImage Image { get; set; }
        internal TileContext Context { get; set; }
        private bool _dragging;
        private Point _lastMouse;
        private int _tick = 0;

        private Vector2 MaxPanelDimension => new Vector2(TwailaConfig.Get().MaxWidth / 100.0f * Parent.GetDimensions().Width, TwailaConfig.Get().MaxHeight / 100.0f * Parent.GetDimensions().Height);
        private Vector2 MaxPanelInnerDimension => new Vector2(MaxPanelDimension.X - PaddingLeft - PaddingRight, MaxPanelDimension.Y - PaddingTop - PaddingLeft);

        public TwailaPanel()
        {
            Context = new TileContext();
            Name = new TwailaText("Default Name", Main.fontCombatText[0], Color.White, 1f);
            Image = new UITwailaImage();
            Image.MarginRight = 10;
            Mod = new TwailaText("Terraria", Main.fontItemStack, Color.White, 1f);
            Width.Set(0, 0);
            Height.Set(0, 0);
            Top.Set(0, 0);
            Left.Set(PlayerInput.RealScreenWidth / 2, 0);
            Append(Name);
            Append(Mod);
            Append(Image);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            _tick++;
            UpdateFromConfig();
            UpdateSize();
            Drag();
            UpdateAlignment();
        }

        private void UpdateFromConfig()
        {
            TwailaConfig config = TwailaConfig.Get();
            
            Image.drawMode = config.ContentSetting;
            Mod.drawMode = config.ContentSetting;
            Name.drawMode = config.ContentSetting;

            Mod.OverrideTextColor = config.OverrideColor;
            Name.OverrideTextColor = config.OverrideColor;

            BackgroundColor = config.PanelColor.Color;
            BorderColor = Color.Black;
            Image.opacity = 1;
            Mod.opacity = 1;
            Name.opacity = 1;
            if (IsMouseHovering && !IsDragging())
            {
                BackgroundColor *= config.HoverOpacity;
                BorderColor *= config.HoverOpacity;
                Image.opacity = config.HoverOpacity;
                Mod.opacity = config.HoverOpacity;
                Name.opacity = config.HoverOpacity;
                Mod.OverrideTextColor = true;
                Name.OverrideTextColor = true;
            }

            Mod.Color = config.TextColor.Color;
            Name.Color = config.TextColor.Color;

            Mod.TextShadow = config.TextShadow;
            Name.TextShadow = config.TextShadow;

            SetElementState(config.DisplayContent.ShowImage, Image);
            SetElementState(config.DisplayContent.ShowMod, Mod);
            SetElementState(config.DisplayContent.ShowName, Name);
        }

        private void SetElementState(bool shouldShow, UIElement element)
        {
            if (shouldShow && !HasChild(element))
            {
                Append(element);
            }
            if(!shouldShow && HasChild(element))
            {
                RemoveChild(element);
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
            float textHeight = GetDimension(Mod, Mod.GetTextSize().Y) + GetDimension(Name, Name.GetTextSize().Y);
            float imageWidth = GetDimension(Image, Image.image.Width);
            float textWidth = GetDimension(Name, Name.GetTextSize().X) > GetDimension(Mod, Mod.GetTextSize().X) ? 
                GetDimension(Name, Name.GetTextSize().X) : GetDimension(Mod, Mod.GetTextSize().X);
            if(!HasChild(Name) && !HasChild(Mod))
            {
                Image.MarginRight = 0;
            }
            DrawMode drawMode = TwailaConfig.Get().ContentSetting;
            if (drawMode == DrawMode.Shrink)
            {
                imageHeight *= ImageScale(new Vector2(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));
                imageWidth *= ImageScale(new Vector2(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));

                Vector2 maxSize = new Vector2(MaxPanelInnerDimension.X - imageWidth - GetDimension(Image, Image.MarginRight), MaxPanelInnerDimension.Y);
                float nameHeight = GetDimension(Name, Name.GetTextSize().Y) * TextScale(Name, maxSize);
                Name.Height.Set(nameHeight, 0);
                float modHeight = GetDimension(Mod, Mod.GetTextSize().Y) * TextScale(Mod, maxSize);
                textHeight = nameHeight + modHeight;

                float nameWidth = GetDimension(Name, Name.GetTextSize().X) * TextScale(Name, maxSize);
                float modWidth = GetDimension(Mod, Mod.GetTextSize().X) * TextScale(Mod, maxSize);
                textWidth = Math.Max(nameWidth, modWidth);
                Name.Width.Set(nameWidth, 0);
                Mod.Width.Set(modWidth, 0);

                Vector2 remainingSpace = new Vector2(MaxPanelInnerDimension.X - textWidth - GetDimension(Image, Image.MarginRight), MaxPanelInnerDimension.Y);

                imageWidth = GetDimension(Image, Image.image.Width) * ImageScale(remainingSpace);
                imageHeight = GetDimension(Image, Image.image.Height) * ImageScale(remainingSpace);
            }
            else
            {
                if (drawMode == DrawMode.Trim)
                {
                    if (GetDimension(Mod, Mod.GetTextSize().Y) > MaxPanelInnerDimension.Y - GetDimension(Name, Name.GetTextSize().Y))
                    {
                        Mod.Height.Set(0, 0);
                        textHeight = MathHelper.Clamp(textHeight - GetDimension(Mod, Mod.GetTextSize().Y), 0, textHeight);
                    }
                    if (GetDimension(Name, Name.GetTextSize().Y) > MaxPanelInnerDimension.Y)
                    {
                        Name.Height.Set(0, 0);
                        textHeight = MathHelper.Clamp(textHeight - GetDimension(Name, Name.GetTextSize().Y), 0, textHeight);
                    }
                    textWidth = Math.Max(GetDimension(Name, Name.GetTextSize().X), GetDimension(Mod, Mod.GetTextSize().X));
                }
                imageHeight = Math.Min(MaxPanelInnerDimension.Y, imageHeight);
                textHeight = Math.Min(MaxPanelInnerDimension.Y, textHeight);
                imageWidth = Math.Min(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, imageWidth);
                textWidth = Math.Min(MaxPanelInnerDimension.X - imageWidth - GetDimension(Image, Image.MarginRight), textWidth);
                Name.Width.Set(textWidth, 0);
                Mod.Width.Set(textWidth, 0);

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
            Name.Width.Set(Name.GetTextSize().X, 0);
            Name.Height.Set(Name.GetTextSize().Y, 0);
            Mod.Width.Set(Mod.GetTextSize().X, 0);
            Mod.Height.Set(Mod.GetTextSize().Y, 0);
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
            if (GetDimension(text, text.GetTextSize().X) > maxSize.X)
            {
                scaleX = maxSize.X / GetDimension(text, text.GetTextSize().X);
            }
            float scaleY = 1;
            if (GetDimension(text, text.GetTextSize().Y) > maxSize.Y)
            {
                scaleY = maxSize.Y / GetDimension(text, text.GetTextSize().Y);
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
            Name.Top.Set(0, 0);
            Name.Left.Set(GetDimension(Image, Image.Width.Pixels + Image.MarginRight), 0);
            Mod.Top.Set(GetDimension(Name, Name.Height.Pixels), 0);
            Mod.Left.Set(GetDimension(Image, Image.Width.Pixels + Image.MarginRight), 0);
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
            TileContext currentContext = TwailaUI.GetContext(TwailaUI.GetMousePos());
            if(!currentContext.ContentChanged(Context))
            {
                currentContext.SetTileType(Context.TileType);
            }
            else
            {
                _tick = 0;
            }
            if (_tick >= 120)
            {
                CycleType(currentContext);
                //Main.NewText(currentContext.TileType);
                _tick = 0;
            }
            
            if (currentContext.TileType != TileType.Empty && !IsBlockedByAntiCheat(currentContext) && currentContext.ContextChanged(Context))
            {
                int itemId = ItemUtil.GetItemId(currentContext);
                Name.SetText(currentContext.GetName(itemId));
                Mod.SetText(currentContext.GetMod());
                Image.SetImage(spriteBatch, currentContext, itemId);
                Context = currentContext;
            }
        }

        // tile -> wall -> liquid -> tile
        private static void CycleType(TileContext context)
        {
            if(context.TileType == TileType.Empty || (!context.HasTile() && !context.HasLiquid() && !context.HasWall()))
            {
                return;
            }

            TileType type = context.TileType;
            do
            {
                type = Cycle(type);
                context.SetTileType(type);
            } while (context.TileType != type);
        }

        private static TileType Cycle(TileType type)
        {
            if(type == TileType.Tile)
            {
                return TileType.Wall;
            }
            if(type == TileType.Wall)
            {
                return TileType.Liquid;
            }
            if(type == TileType.Liquid)
            {
                return TileType.Tile;
            }
            return TileType.Empty;
        }

        private static bool IsBlockedByAntiCheat(TileContext context)
        {
            if (TwailaConfig.Get().AntiCheat)
            {
                Player player = Main.player[Main.myPlayer];
                if (player.HasBuff(BuffID.Spelunker) && Main.tileSpelunker[context.Tile.type])
                {
                    return false;
                }
                if(player.HasBuff(BuffID.Dangersense) && IsDangersenseTile(context.Tile, context.Pos))
                {
                    return false;
                }
                return !Main.Map.IsRevealed(context.Pos.X, context.Pos.Y);
            }
            return false;
            
        }

        private static bool IsDangersenseTile(Tile tile, Point pos)
        {
            bool dangerTile = tile.type == TileID.PressurePlates || tile.type == TileID.Traps || tile.type == TileID.Boulder ||
                tile.type == TileID.Explosives || tile.type == TileID.LandMine || tile.type == TileID.ProjectilePressurePad || 
                tile.type == TileID.GeyserTrap || tile.type == TileID.BeeHive;
            if (tile.slope() == 0 && !tile.inActive())
            {
                dangerTile = dangerTile || tile.type == TileID.Cobweb || tile.type == TileID.CorruptThorns || 
                    tile.type == TileID.JungleThorns || tile.type == TileID.CrimtaneThorns || tile.type == TileID.Spikes 
                    || tile.type == TileID.WoodenSpikes || tile.type == TileID.HoneyBlock;
                if (!Main.player[Main.myPlayer].fireWalk)
                {
                    dangerTile = dangerTile || tile.type == TileID.Meteorite || tile.type == TileID.Hellstone || tile.type == TileID.HellstoneBrick;
                }
                if (!Main.player[Main.myPlayer].iceSkate)
                {
                    dangerTile = dangerTile || tile.type == TileID.BreakableIce;
                }
            }
            return dangerTile || TileLoader.Dangersense(pos.X, pos.Y, tile.type, Main.player[Main.myPlayer]);
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
