using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Twaila.Config;

namespace Twaila.UI
{
    public class UIHappinessGrid : UITwailaGrid
    {
        public UITwailaGrid HateColumn { get; set; }
        public UITwailaGrid DislikeColumn { get; set; }
        public UITwailaGrid LikeColumn { get; set; }
        public UITwailaGrid LoveColumn { get; set; }

        public static Color LoveColor => TwailaConfig.Instance.DisplayContent.NpcContent.HappinessColors.LoveColor.Color;
        public static Color LikeColor => TwailaConfig.Instance.DisplayContent.NpcContent.HappinessColors.LikeColor.Color;
        public static Color DislikeColor => TwailaConfig.Instance.DisplayContent.NpcContent.HappinessColors.DislikeColor.Color;
        public static Color HateColor => TwailaConfig.Instance.DisplayContent.NpcContent.HappinessColors.HateColor.Color;

        public int OutlineSize { get; set; } = 3; 

        public UIHappinessGrid(int width) : base(width)
        {
            SmartRows = false;
            RowPadding = Math.Max(10, OutlineSize * 2);

            LoveColumn = new(width: 1) { SmartRows = false };
            Add(LoveColumn);

            LikeColumn = new(width: 1) { SmartRows = false };
            Add(LikeColumn);

            DislikeColumn = new(width: 1) { SmartRows = false };
            Add(DislikeColumn);

            HateColumn = new(width: 1) { SmartRows = false };
            Add(HateColumn);
        }

        public override Vector2 GetContentSize()
        {
            Vector2 baseSize = base.GetContentSize();
            foreach (var element in HappinessColumns())
            {
                if (element.grid.GridElements.Count > 0)
                {
                    return baseSize += new Vector2(OutlineSize * 2, OutlineSize * 2);
                }
            }

            return baseSize;
        }

        public IEnumerable<(UITwailaGrid grid, Color color)> HappinessColumns()
        {
            yield return (LoveColumn, LoveColor);
            yield return (LikeColumn, LikeColor);
            yield return (DislikeColumn, DislikeColor);
            yield return (HateColumn, HateColor);
        }

        public void AddElementAtAffection(UITwailaElement element, AffectionLevel level)
        {
            switch (level)
            {
                case AffectionLevel.Hate:
                    HateColumn.Add(element);
                    break;
                case AffectionLevel.Dislike:
                    DislikeColumn.Add(element);
                    break;
                case AffectionLevel.Like:
                    LikeColumn.Add(element);
                    break;
                case AffectionLevel.Love:
                    LoveColumn.Add(element);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var element in Children)
            {
                element.Left.Pixels += OutlineSize;
                element.Top.Pixels += OutlineSize;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {

            var columns = HappinessColumns().ToList();
            float scale = DrawMode == DrawMode.Shrink ? GetDrawScale() : 1f;

            int width = (int)(OutlineSize * scale);
            for (int i = 0; i < columns.Count; ++i)
            {
                var element = columns[i];
                if (element.grid.GridElements.Count > 0 && element.grid.Width.Pixels > 0 && element.grid.Height.Pixels > 0)
                {
                    Rectangle rec = element.grid.GetDimensions().ToRectangle();

                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rec.X - width, rec.Y - width, width, rec.Height + 2 * width), element.color * Opacity); // left vertical
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rec.X, rec.Y - width, rec.Width, width), element.color * Opacity); // top horizontal
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rec.X - width, rec.Y + rec.Height + width, rec.Width + 2 * width, width), element.color * Opacity); // bottom horizontal
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rec.X + rec.Width, rec.Y - width, width, rec.Height + 2 * width), element.color * Opacity); // right vertical
                }
            }

            base.DrawSelf(spriteBatch);
        }

    }
}
