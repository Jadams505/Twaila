using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.UI;

namespace Twaila.Context
{
    public class NpcContext : BaseContext
    {
        protected NPC Npc { get; set; }

        protected string Id { get; set; }
        protected string Hp { get; set; }
        protected string Defense { get; set; }
        protected string Damage { get; set; }

        public NpcContext(Point pos) : base(pos)
        {
            Update();
        }

        public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(NpcContext))
            {
                NpcContext otherContext = (NpcContext)other;
                return Npc?.type != otherContext.Npc?.type;
            }
            return true;
        }

        public override void Update()
        {
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            IntersectsNPC(new Point(Pos.X, Pos.Y), out NPC foundNPC);

            Npc = foundNPC;

            if (content.ShowId)
            {
                Id = $"Npc Id: {Npc.type}";
            }

            Hp = $"Hp: {Npc.life} / {Npc.lifeMax}";

            Defense = Npc.defense.ToString();

            Damage = $"Attack: {Npc.damage}";

        }

        public override void UpdateOnChange(BaseContext prevContext, Layout layout)
        {
            Update();

            layout.Name.SetText(GetName());

            if (ContextChanged(prevContext))
            {
                Color color = Color.White;
                if (Npc?.color != new Color(0, 0, 0, 0))
                {
                    color = Npc.color;
                }
                layout.Image.SetImage(GetImage(Main.spriteBatch), color);
            }

            InfoElements().ForEach(element => layout.InfoBox.AddAndEnable(element));

            layout.Mod.SetText(GetMod());
        }

        protected override TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            TextureBuilder builer = new TextureBuilder();

            if(Npc != null)
            {
                Rectangle drawFrame = new Rectangle(0, 0, Npc.frame.Width, Npc.frame.Height);

                builer.AddComponent(drawFrame, TextureAssets.Npc[Npc.type].Value, Point.Zero);

                Texture2D texture = builer.Build(spriteBatch.GraphicsDevice);

                return new TwailaTexture(texture);
            }

            return null;
        }

        protected override string GetMod()
        {
            if(Npc != null)
            {
                ModNPC mNpc = NPCLoader.GetNPC(Npc.type);
                if (mNpc != null)
                {
                    return mNpc.Mod.DisplayName;
                }
            }
            return "Terraria";
        }

        protected override string GetName()
        {
            if (Npc != null)
            {
                return Npc.FullName;
            }
            return null;
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = new List<UITwailaElement>();

            if (!string.IsNullOrEmpty(Defense))
            {
                elements.Insert(0, new UIStatElement(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Defense").Value,
                    Defense.ToString()));
            }

            if (!string.IsNullOrEmpty(Damage))
            {
                elements.Insert(0, new TwailaText(Damage));
            }

            if (!string.IsNullOrEmpty(Hp))
            {
                elements.Insert(0, new TwailaText(Hp));
            }

            if (!string.IsNullOrEmpty(Id))
            {
                elements.Insert(0, new TwailaText(Id));
            }

            return elements;
        }

        public static bool IntersectsNPC(Point pos, out NPC target)
        {
            foreach(NPC npc in Main.npc)
            {
                Rectangle npcHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                Rectangle mouseHitbox = new Rectangle(pos.X * 16, pos.Y * 16, 0, 0);
                Rectangle rectangle = new Rectangle((int)(Main.mouseX + Main.screenPosition.X), (int)(Main.mouseY + Main.screenPosition.Y), 1, 1);
                npcHitbox.Intersects(ref rectangle, out bool mouseOver);
                if (mouseOver)
                {
                    target = npc;
                    return true;
                }
            }
            target = null;
            return false;
        }
    }
}
