using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
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
        protected string Kills { get; set; }

        protected UITwailaGrid InfoGrid { get; private set; }
        protected UITwailaIconGrid IconGrid { get; private set; }

        public NpcContext(TwailaPoint pos) : base(pos)
        {
            InfoGrid = new UITwailaGrid(TwailaConfig.Instance.DisplayContent.NpcContent.StatsPerRow);
            IconGrid = new UITwailaIconGrid(TwailaConfig.Instance.DisplayContent.NpcContent.IconsPerRow);
            Npc = null;
            Id = "";
            Hp = "";
            Defense = "";
            Damage = "";
            KnockbackTaken = "";
            Kills = "";
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
            TwailaConfig.Content content = TwailaConfig.Instance.DisplayContent;

            IntersectsNPC(Pos.BestPos(), out NPC foundNPC);

            Npc = foundNPC;

            if (Npc == null)
                return;

            if (content.ShowId)
            {
                Id = $"Npc Id: {Npc.type}";
                InfoGrid.Add(new UITwailaText(Id));
            }

            if (content.NpcContent.ShowHp)
            {
                Hp = MathHelper.Clamp(Npc.life, 0, Npc.life).ToString();
                InfoGrid.Add(new UIStatElement(ImageUtil.GetRenderForNpcStat(ImageUtil.NpcStat.Health), Hp));
            }

            if (content.NpcContent.ShowDefense)
            {
                Defense = Npc.defense.ToString();
                InfoGrid.Add(new UIStatElement(ImageUtil.GetRenderForNpcStat(ImageUtil.NpcStat.Defense), Defense));
            }

            if (content.NpcContent.ShowAttack)
            {
                Damage = Npc.damage.ToString();
                InfoGrid.Add(new UIStatElement(ImageUtil.GetRenderForNpcStat(ImageUtil.NpcStat.Attack), Damage));
            }

            if (content.NpcContent.ShowKnockback)
            {
                if (Npc.knockBackResist > 0.8f)
                {
                    KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackHigh").Value;
                }
                else if (Npc.knockBackResist > 0.4f)
                {
                    KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackMedium").Value;
                }
                else if (Npc.knockBackResist > 0f)
                {
                    KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackLow").Value;
                }
                else
                {
                    KnockbackTaken = Language.GetText("BestiaryInfo.KnockbackNone").Value;
                }
                InfoGrid.Add(new UIStatElement(ImageUtil.GetRenderForNpcStat(ImageUtil.NpcStat.Knockback), KnockbackTaken));
            }

            if (content.NpcContent.ShowKills)
            {
                Kills = Main.BestiaryTracker.Kills.GetKillCount(Npc).ToString();
                InfoGrid.Add(new UIStatElement(ImageUtil.GetRenderForNpcStat(ImageUtil.NpcStat.Kill), Kills));
            }

            for(int i = 0; i < Npc.buffType.Length; ++i)
            {
                if (Npc.buffType[i] != 0 && Npc.buffTime[i] > 0)
                {
                    var displayType = TwailaConfig.Instance.DisplayContent.NpcContent.ShowBuffs;
                    int buffType = Npc.buffType[i];
                    if (displayType == TwailaConfig.DisplayType.Icon || displayType == TwailaConfig.DisplayType.Both)
                    {
                        IconGrid.AddIcon(ImageUtil.GetRenderForBuff(buffType));
                    }

                    if (displayType == TwailaConfig.DisplayType.Name || displayType == TwailaConfig.DisplayType.Both)
                    {
                        InfoGrid.Add(new UITwailaText(NameUtil.GetNameForBuff(buffType)));
                    }
                }
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

            InfoElements().ForEach(element => layout.InfoBox.Add(element));

            layout.Mod.SetText(GetMod());
        }

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            RenderBuilder builer = new RenderBuilder();
            if(Npc != null)
            {
                Rectangle drawFrame = new Rectangle(0, 0, Npc.frame.Width, Npc.frame.Height);

                Color drawColor = Npc.color;
                if (drawColor.A == 0)
                {
                    drawColor = Color.White;
                }

                float scale = MathHelper.Clamp(Npc.scale, 0, 1);

                builer.AddImage(ImageUtil.GetNPCTexture(Npc.type), Point.Zero, drawFrame, drawColor, scale);

                return builer.Build();
            }

            return null;
        }

        protected override string GetMod()
        {
            return NameUtil.GetMod(Npc?.ModNPC);
        }

        protected override string GetName()
        {
            if (Npc != null)
            {
                string displayName = Npc.FullName;
                string internalName = NameUtil.GetInternalName(Npc.ModNPC, false);
                string fullName = NameUtil.GetInternalName(Npc.ModNPC, true);

                TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;
                return NameUtil.GetName(nameType, displayName, internalName, fullName);
            }
            return null;
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = new List<UITwailaElement>();

            if(InfoGrid.GridElements.Count > 0)
            {
                elements.Add(InfoGrid);
            }
            if(IconGrid.GridElements.Count > 0)
            {
                elements.Add(IconGrid);
            }
            
            return elements;
        }

        public static bool IntersectsNPC(Point pos, out NPC target)
        {
            foreach(NPC npc in Main.npc)
            {
                if (!npc.active)
                {
                    continue;
                } 

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
