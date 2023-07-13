using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using static Twaila.Config.TwailaConfig;

namespace Twaila.Config
{
    public class PanelContextData
    {
        [DefaultValue(true)]
        public bool SyncPositionData = true;

        [SeparatePage]
        public PositionData ClosedInventory = new PositionData()
        {
            UseDefaultPosition = true,
            LockPosition = true,
            AnchorX = HorizontalAnchor.Center,
            AnchorY = VerticalAnchor.Top,
            AnchorPosX = 0,
            AnchorPosY = 0,
            ShowUI = true,
        };

        [SeparatePage]
        public PositionData OpenInventory = new PositionData()
        {
            UseDefaultPosition = true,
            LockPosition = true,
            AnchorX = HorizontalAnchor.Center,
            AnchorY = VerticalAnchor.Top,
            AnchorPosX = 0,
            AnchorPosY = 0,
            ShowUI = true,
        };

        [SeparatePage]
        public PositionData InFullscreenMap = new PositionData()
        {
            UseDefaultPosition = true,
            LockPosition = true,
            AnchorX = HorizontalAnchor.Center,
            AnchorY = VerticalAnchor.Top,
            AnchorPosX = 0,
            AnchorPosY = 0,
            ShowUI = true,
        };

        public PanelContextData()
        {
            SyncPositionData = true;
        }

        public PositionData GetActiveContext()
        {
            if (SyncPositionData)
                return ClosedInventory;

            if (Main.mapFullscreen)
                return InFullscreenMap;

            if (Main.playerInventory)
                return OpenInventory;

            return ClosedInventory;
        }

        public override bool Equals(object obj)
        {
            return obj is PanelContextData data &&
                   ClosedInventory == data.ClosedInventory &&
                   OpenInventory == data.OpenInventory &&
                   InFullscreenMap == data.InFullscreenMap &&
                   SyncPositionData == data.SyncPositionData;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ClosedInventory, OpenInventory, InFullscreenMap, SyncPositionData);
        }
    }
}
