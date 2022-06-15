using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.UI;

namespace Twaila.Context
{
    public class NpcContext : BaseContext
    {
        protected int NpcId { get; set; }
        protected int NpcIndex { get; set; }

        protected string Id { get; set; }

        public NpcContext(Point pos) : base(pos)
        {
            Update();
        }

        public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(NpcContext))
            {
                NpcContext otherContext = (NpcContext)other;
                return NpcId != otherContext.NpcId;
            }
            return true;
        }

        public override void Update()
        {
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;
            int type = 0;
            int i = 0;
            while(type == 0 && i < Main.npc.Length)
            {
                NPC npc = Main.npc[i];
                Rectangle npcHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.frame.Width, npc.frame.Height);
                Rectangle mouseHitbox = new Rectangle(Pos.X * 16, Pos.Y * 16, 0, 0);
                npcHitbox.Intersects(ref mouseHitbox, out bool mouseOver);
                if (mouseOver)
                {
                    type = npc.type;
                    NpcIndex = i;

                    if (content.ShowId)
                    {
                        Id = $"Npc Id: {NpcId}";
                    }
                }
                i++;
            }
            NpcId = type;
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

        protected override TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            TextureBuilder builer = new TextureBuilder();

            NPC npc = Main.npc[NpcIndex];

            Rectangle drawFrame = new Rectangle(0, 0, npc.frame.Width, npc.frame.Height);

            builer.AddComponent(drawFrame, TextureAssets.Npc[NpcId].Value, Point.Zero);

            Texture2D texture = builer.Build(spriteBatch.GraphicsDevice);

            return new TwailaTexture(texture);
        }

        protected override string GetMod()
        {
            ModNPC mNpc = NPCLoader.GetNPC(NpcId);
            if(mNpc != null)
            {
                return mNpc.Mod.DisplayName;
            }
            return "Terraria";
        }

        protected override string GetName()
        {
            NPC npc = Main.npc[NpcIndex];
            if (npc != null)
            {
                return npc.GivenOrTypeName;
            }
            return null;
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = new List<UITwailaElement>();

            if (!string.IsNullOrEmpty(Id))
            {
                elements.Insert(0, new TwailaText(Id));
            }

            return elements;
        }

        public static bool IntersectsNPC(Point pos)
        {
            foreach(NPC npc in Main.npc)
            {
                Rectangle npcHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.frame.Width, npc.frame.Height);
                Rectangle mouseHitbox = new Rectangle(pos.X * 16, pos.Y * 16, 0, 0);
                npcHitbox.Intersects(ref mouseHitbox, out bool mouseOver);
                if (mouseOver)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
