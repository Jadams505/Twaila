using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using static Twaila.Config.TwailaConfig;

namespace Twaila.Config
{
    public class PositionData
    {
        [DefaultValue(true)]
        public bool UseDefaultPosition;

        [DefaultValue(true)]
        public bool LockPosition;

        [DrawTicks]
        [DefaultValue(HorizontalAnchor.Center)]
        public HorizontalAnchor AnchorX = HorizontalAnchor.Center;

        [DrawTicks]
        [DefaultValue(VerticalAnchor.Top)]
        public VerticalAnchor AnchorY = VerticalAnchor.Top;

        [DefaultValue(0)]
        [Range(0, 2000)]
        public int AnchorPosX;

        [DefaultValue(0)]
        [Range(0, 2000)]
        public int AnchorPosY;

        [DefaultValue(true)]
        public bool ShowUI;

        public PositionData()
        {
            UseDefaultPosition = true;
            LockPosition = true;
            AnchorX = HorizontalAnchor.Center;
            AnchorY = VerticalAnchor.Top;
            AnchorPosX = 0;
            AnchorPosY = 0;
            ShowUI = true;
        }

        public void CopyTo(PositionData other)
        {
            other.UseDefaultPosition = UseDefaultPosition;
            other.LockPosition = LockPosition;
            other.AnchorX = AnchorX;
            other.AnchorY = AnchorY;
            other.AnchorPosX = AnchorPosX;
            other.AnchorPosY = AnchorPosY;
            other.ShowUI = ShowUI;
        }

        public override bool Equals(object obj)
        {
            return obj is PositionData data &&
                   AnchorX == data.AnchorX &&
                   AnchorY == data.AnchorY &&
                   AnchorPosX == data.AnchorPosX &&
                   AnchorPosY == data.AnchorPosY &&
                   ShowUI == data.ShowUI &&
                   LockPosition == data.LockPosition &&
                   UseDefaultPosition == data.UseDefaultPosition;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AnchorX, AnchorY, AnchorPosX, AnchorPosY);
        }
    }
}
