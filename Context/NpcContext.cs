using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class NpcContext : BaseContext
    {
        protected NPC Npc { get; set; }

        protected string Id { get; set; }
        protected string Hp { get; set; }
        protected string Defense { get; set; }
        protected string Damage { get; set; }
        protected string KnockbackTaken { get; set; }

        public NpcContext(TwailaPoint pos) : base(pos)
        {
			Npc = null;
			Id = "";
			Hp = "";
			Defense = "";
			Damage = "";
			KnockbackTaken = "";
        }

		public static NpcContext CreateNpcContext(TwailaPoint pos)
		{
			if (IntersectsNPC(pos.BestPos(), out _))
			{
				return new NpcContext(pos);
			}

			return null;
		}

		public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(NpcContext))
            {
                NpcContext otherContext = (NpcContext)other;
				if(Npc?.type == otherContext.Npc.type)
				{
					return Npc?.netID != otherContext.Npc?.netID;
				}
            }
            return true;
        }

        public override void Update()
        {
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            IntersectsNPC(Pos.BestPos(), out NPC foundNPC);

            Npc = foundNPC;

            if (content.ShowId)
            {
                Id = $"Npc Id: {Npc.type}";
            }

            Hp = MathHelper.Clamp(Npc.life, 0, Npc.life).ToString();

            Defense = Npc.defense.ToString();

            Damage = Npc.damage.ToString();

            if(Npc.knockBackResist > 0.8f)
            {
                KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackHigh").Value;
            }
            else if(Npc.knockBackResist > 0.4f)
            {
                KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackMedium").Value;
            }
            else if(Npc.knockBackResist > 0f)
            {
                KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackLow").Value;
            }
            else
            {
                KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackNone").Value;
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

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            RenderBuilder builer = new RenderBuilder();

            if(Npc != null)
            {
                Rectangle drawFrame = new Rectangle(0, 0, Npc.frame.Width, Npc.frame.Height);

				Color drawColor = Npc.color;

				if(drawColor.A == 0)
				{
					drawColor = Color.White;
				}

				builer.AddImage(ImageUtil.GetNPCTexture(Npc.type), Point.Zero, drawFrame, drawColor, Npc.scale);

				return builer.Build();
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
                return Npc.FullName; // remember to check for internal names
            }
            return null;
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = new List<UITwailaElement>();

            if (!string.IsNullOrEmpty(Id))
            {
                elements.Add(new TwailaText(Id));
            }

            if (!string.IsNullOrEmpty(Hp))
            {
                elements.Add(new UIStatElement(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_HP").Value, Hp.ToString()));
            }

            if (!string.IsNullOrEmpty(Defense))
            {
                elements.Add(new UIStatElement(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Defense").Value, Defense.ToString()));
            }

            if (!string.IsNullOrEmpty(Damage))
            {
                elements.Add(new UIStatElement(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Attack").Value,
                    Damage.ToString()));
            }

            if (!string.IsNullOrEmpty(KnockbackTaken))
            {
                elements.Add(new UIStatElement(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Knockback").Value,
                    KnockbackTaken));
            }
            
            return elements;
        }

        public static bool IntersectsNPC(Point pos, out NPC target)
        {
            foreach(NPC npc in Main.npc)
            {
                Rectangle npcHitbox = new Rectangle((int)npc.TopLeft.X - 16, (int)npc.TopLeft.Y - 16, npc.frame.Width, npc.frame.Height);
                Rectangle mouseHitbox = new Rectangle(pos.X * 16, pos.Y * 16, 0, 0);
				mouseHitbox.Intersects(ref npcHitbox, out bool mouseOver);
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
