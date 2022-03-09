using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Twaila.Context;
using Twaila.Graphics;

namespace Twaila.UI
{
    public class UITwailaImage : UIElement
    {
        private TwailaTexture _image;
        public UITwailaImage()
        {
            _image = new TwailaTexture(Main.buffTexture[BuffID.Confused]);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(_image?.Texture != null)
            {
                Width.Set(_image.Width(), 0);
                Height.Set(_image.Height(), 0);
                Recalculate();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_image?.Texture != null)
            {
                spriteBatch?.Draw(_image.Texture, new Vector2(GetDimensions().ToRectangle().X, GetDimensions().ToRectangle().Y), 
                    new Rectangle(0, 0, _image.Texture.Width, _image.Texture.Height), Color.White, 0, Vector2.Zero, _image.Scale, 0, 0);
            }
        }

        public void SetImage(SpriteBatch spriteBatch, TileContext context, int itemId)
        {
            if (TwailaConfig.Get().UseItemTextures)
            {
                _image = context.GetItemImage(spriteBatch, itemId) ?? context.GetTileImage(spriteBatch);
            }
            else
            {
                _image = context.GetTileImage(spriteBatch) ?? context.GetItemImage(spriteBatch, itemId);
            }
            if(_image?.Texture == null)
            {
                _image = new TwailaTexture(Main.buffTexture[BuffID.Confused]);
            }
        }
        
    }
}
