using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context;
public class LeashedAnchorContext<T> : TileContext where T : TELeashedEntityAnchorWithItem
{
    protected int LeashedItem { get; set; }

    public LeashedAnchorContext(TwailaPoint point) : base(point)
    {

    }

    public static LeashedAnchorContext<T>? CreateLeashedAnchorContext(TwailaPoint pos)
    {
        Point tilePos = pos.BestTilePos();
        Tile tile = Framing.GetTileSafely(tilePos);

        if (!tile.HasTile || tile.TileType >= TileLoader.TileCount)
            return null;

        if (!TileUtil.IsTilePosInBounds(tilePos))
            return null;

        Point targetPos = TileUtil.TileEntityCoordinates(tilePos.X, tilePos.Y, width: 1, height: 1);
        if (!TileEntity.TryGetAt<T>(targetPos.X, targetPos.Y, out var result))
            return null;

        if (TileUtil.IsTileBlockedByAntiCheat(tile, tilePos))
            return null;

        return new LeashedAnchorContext<T>(pos);
    }

    public override bool ContextChanged(BaseContext? other)
    {
        if (other?.GetType() == typeof(LeashedAnchorContext<T>))
        {
            var otherContext = (LeashedAnchorContext<T>)other;
            return LeashedItem != otherContext.LeashedItem;
        }
        return true;
    }

    public override void Update()
    {
        base.Update();
        Content content = TwailaConfig.Instance.DisplayContent;

        LeashedItem = GetCritterItem(BestTilePos.X, BestTilePos.Y);

        if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
        {
            IconGrid.AddIcon(ImageUtil.GetRenderForIconItem(LeashedItem));
        }
        if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
        {
            TextGrid.Add(new UITwailaText(NameUtil.GetNameFromItem(LeashedItem)));
        }
    }

    protected override TwailaRender ItemImage(SpriteBatch spriteBatch)
    {
        return ImageUtil.GetItemTexture(LeashedItem).ToRender()
            .Coalesce(base.ItemImage(spriteBatch));
    }

    protected override string GetName()
    {
        Tile tile = Framing.GetTileSafely(BestTilePos);
        var itemEntry = new DropPlacePair(DropItem: LeashedItem, PlaceItem: LeashedItem);

        string? dropName = NameUtil.GetNameFromItem(itemEntry.DropItem);
        string? placedName = NameUtil.GetNameFromItem(itemEntry.PlaceItem);
        string? mapName = NameUtil.GetNameFromMap(tile, BestTilePos.X, BestTilePos.Y);
        string? internalPrettyName = NameUtil.GetInternalTileName(TileId, fullName: false, pretty: true);
        var displayName = NamingSystem.Instance.GetName(dropName, placedName, mapName, internalPrettyName);

        string? internalName = NameUtil.GetInternalTileName(TileId, false);
        string? fullName = NameUtil.GetInternalTileName(TileId, true);

        TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;

        return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? Language.GetTextValue("Mods.Twaila.Defaults.Name");
    }

    private static int GetCritterItem(int tileX, int tileY)
    {
        Point targetPos = TileUtil.TileEntityCoordinates(tileX, tileY, width: 1, height: 1);
        if (!TileEntity.TryGetAt<T>(targetPos.X, targetPos.Y, out var instance))
        {
            return ItemID.None;
        }

        return instance.ItemType();
    }
}

public static class TELeashedEntityAnchorWithItemExtensions
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name="itemType")]
    internal static extern ref int TELeashedEntityAnchorWithItem_itemType(TELeashedEntityAnchorWithItem self);

    public static int ItemType(this TELeashedEntityAnchorWithItem item) => TELeashedEntityAnchorWithItem_itemType(item);
}
