using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ModLoader;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class TownNpcContext : NpcContext
    {
        protected double Happiness { get; set; }

        protected UIHappinessGrid HappinessGrid { get; set; }

        public TownNpcContext(TwailaPoint pos) : base(pos)
        {
            HappinessGrid = new UIHappinessGrid(width: 4);
        }

        public static TownNpcContext CreateTownNpcContext(TwailaPoint pos)
        {
            if (IntersectsNPC(pos.MouseWorldPos, out var npc) && npc.townNPC)
            {
                return new TownNpcContext(pos);
            }

            return null;
        }

        private static readonly FieldInfo HappinessDatabase = typeof(ShopHelper).GetField("_database", BindingFlags.Instance | BindingFlags.NonPublic);

        public override void Update()
        {
            base.Update();

            if (Npc is null)
                return;

            var config = TwailaConfig.Instance.DisplayContent.NpcContent;

            Happiness = InfoUtil.GetNpcHappiness(Npc, out string number, out string text);

            if(config.ShowHappiness == TwailaConfig.NumberType.Number)
            {
                InfoGrid.Add(new UITwailaText(number));
            }
            else if(config.ShowHappiness == TwailaConfig.NumberType.Text)
            {
                InfoGrid.Add(new UITwailaText(text));
            }
            else if(config.ShowHappiness == TwailaConfig.NumberType.Both)
            {
                InfoGrid.Add(new UITwailaText(text + $" ({Happiness:0.00})"));
            }
            
            var happiness = (PersonalityDatabase)HappinessDatabase?.GetValue(Main.ShopHelper);
            if (happiness != null)
            {
                if (happiness.TryGetProfileByNPCID(Npc.type, out var profile))
                {
                    var list = profile.ShopModifiers;
                    Dictionary<AffectionLevel, UITwailaIconGrid> npcIconGrids = new(4)
                    {
                        {
                            AffectionLevel.Love,
                            new (width: 3, 30f)
                        },
                        {
                            AffectionLevel.Like,
                            new (width: 3, 30f)
                        },
                        {
                            AffectionLevel.Dislike,
                            new (width : 3, 30f)
                        },
                        {
                            AffectionLevel.Hate,
                            new (width : 3, 30f)
                        },
                    };

                    foreach (var preference in list)
                    {
                        if(preference is BiomePreferenceListTrait biomeEntry)
                        {
                            
                            foreach(var biomePreference in biomeEntry.Preferences)
                            {
                                string biomeText = $"{biomePreference.Affection}: {ShopHelper.BiomeNameByKey(biomePreference.Biome.NameKey)}";
                                
                                if(config.ShowBiomePreferences)
                                    InfoGrid.Add(new UITwailaText(biomeText));
                            }
                            
                        }
                        else if(preference is NPCPreferenceTrait npcEntry)
                        {
                            var innerNpc = new NPC();
                            innerNpc.SetDefaults(npcEntry.NpcId);
                            int index = TownNPCProfiles.GetHeadIndexSafe(innerNpc);
                            var render = index != -1 ? TextureAssets.NpcHead[index].Value.ToRender() : ImageUtil.GetRenderForNpc(innerNpc);
                            npcIconGrids[npcEntry.Level].AddIcon(render);
                            
                            string npcText = $"{npcEntry.Level}: {innerNpc.TypeName}";

                            if (config.ShowNpcPreferences == TwailaConfig.DisplayType.Name || config.ShowNpcPreferences == TwailaConfig.DisplayType.Both)
                                InfoGrid.Add(new UITwailaText(npcText));
                        }
                    }

                    if(config.ShowNpcPreferences == TwailaConfig.DisplayType.Icon || config.ShowNpcPreferences == TwailaConfig.DisplayType.Both)
                    {
                        foreach (var grid in npcIconGrids)
                        {
                            if (grid.Value.GridElements.Count > 0)
                                HappinessGrid.AddElementAtAffection(grid.Value, grid.Key);
                        }
                    }
                }
            }
        }

        protected override List<UITwailaElement> InfoElements()
        {
            var elements = base.InfoElements();

            if(HappinessGrid.GridElements.Count > 0)
            {
                elements.Add(HappinessGrid);
            }

            return elements;
        }
    }
}
