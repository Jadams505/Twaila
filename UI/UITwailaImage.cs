using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Twaila.Graphics;
using Twaila.Util;

namespace Twaila.UI;

public class UITwailaImage : UITwailaElement
{
    /// <summary>
    /// Seems to fix gaps in textures when drawing with SamplerState.PointClamp
    /// </summary>
    public readonly float MagicScale = 0.01f;
    public TwailaRender Render { get; private set; } = null!;

    public UITwailaImage()
    {
        Render = new TwailaRender(TextureAssets.Buff[BuffID.Confused].ForceVanillaLoad());
    }

    public UITwailaImage(TwailaRender render)
    {
        SetImage(render);
    }

    public override Vector2 GetContentSize()
    {
        return new Vector2(Render.Width, Render.Height);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (Render != null && Render.CanDraw())
        {
            // Fixes gaps in textures taken from TileDefinitionOptionElement
            RasterizerState rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);

            base.DrawSelf(spriteBatch);

            // restore old state taken from GameInterfaceLayer.Draw
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
        }
    }

    protected override void DrawTrimmed(SpriteBatch spriteBatch)
    {
        Rectangle drawDim = DrawDimensions();
        Render.Draw(spriteBatch, drawDim, Color.White * Opacity, 1);
    }

    protected override void DrawOverflow(SpriteBatch spriteBatch)
    {
        Rectangle drawDim = DrawDimensions();
        Render.Draw(spriteBatch, drawDim, Color.White * Opacity, 1);
    }

    protected override void DrawShrunk(SpriteBatch spriteBatch)
    {
        Rectangle drawDim = DrawDimensions();
        float scale = GetDrawScale();
        Render.Draw(spriteBatch, drawDim, Color.White * Opacity, scale + MagicScale);
    }

    public Rectangle DrawDimensions()
    {
        Rectangle dimensions = GetInnerDimensions().ToRectangle();
        Rectangle rec = new Rectangle(dimensions.X, dimensions.Y, (int)Render.Width, (int)Render.Height);
        float drawHeight = 0;
        float drawWidth = 0;
        switch (DrawMode)
        {
            case DrawMode.Trim:
                rec.Width = (int)MathHelper.Min(dimensions.Width, Render.Width);
                rec.Height = (int)MathHelper.Min(dimensions.Height, Render.Height);
                drawHeight = rec.Height;
                drawWidth = rec.Width;
                break;
            case DrawMode.Shrink:
                rec.Width = (int)Render.Width;
                rec.Height = (int)Render.Height;
                drawHeight = Render.Height * GetDrawScale();
                drawWidth = Render.Width * GetDrawScale();
                break;
            case DrawMode.Overflow:
                rec.Width = (int)Render.Width;
                rec.Height = (int)Render.Height;
                drawHeight = rec.Height;
                drawWidth = rec.Width;
                break;
        }
        rec.Y += Math.Max(0, (int)(dimensions.Height - drawHeight) / 2); // center the image vertically
        rec.X += Math.Max(0, (int)(dimensions.Width - drawWidth) / 2);
        return rec;
    }

    public void SetImage(TwailaRender render)
    {
        Render = render;
        if(Render == null || !Render.CanDraw())
        {
            Render = new TwailaRender(TextureAssets.Buff[BuffID.Confused].ForceVanillaLoad());
        }
    }
}
