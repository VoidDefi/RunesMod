using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using System.IO;
using System.Reflection;
using Stubble.Core.Classes;

namespace RunesMod.Systems
{
    public class PotionQualitySystem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public static Type[] Qualities { get; set; } = new Type[10];

        PotionQuality quality = null;

        public PotionQuality Quality => quality;

        public override void SaveData(Item item, TagCompound tag)
        {
            if (quality != null && RunesMod.IsPotion(item.type))
                tag.Add("Quality", (short)quality.type);
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            return;
            if (tag.ContainsKey("Quality") && RunesMod.IsPotion(item.type))
            {
                quality = GetQuality(tag.GetShort("Quality"), item);

                SetItemStat(item, quality);
            }
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(quality?.type ?? -1);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            quality = GetQuality(reader.ReadInt32(), item);
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            PotionQualitySystem result = (PotionQualitySystem)base.Clone(item, itemClone);
            result.quality = quality;
            return result;
        }

        public override bool CanStack(Item destination, Item source)
        {
            return true;

            if (!destination.IsAir || !source.IsAir) 
                return true;

            PotionQualitySystem item1 = destination.GetGlobalItem<PotionQualitySystem>();
            PotionQualitySystem item2 = source.GetGlobalItem<PotionQualitySystem>();

            if (item1?.quality != null && item2?.quality != null)
            {
                if (item1.quality.type == item2.quality.type)
                    return true;
            }

            else return true;

            return false;
        }

        public override bool OnPickup(Item item, Player player)
        {
            return base.OnPickup(item, player);

            if (RunesMod.IsPotion(item.type))
            {
                quality = GetQuality(Main.rand.Next(0, 10), item);

                if (quality != null)
                {
                    Item clone = item.Clone();
                    item.SetDefaults(item.type);

                    item.stack = clone.stack;
                    item.favorited = clone.favorited;

                    SetItemStat(item, quality);
                }
            }

            return base.OnPickup(item, player);
        }

        public override void SetDefaults(Item entity)
        {
            return;

            if (RunesMod.IsPotion(entity.type))
            {
                if (quality == null)
                {
                    quality = GetQuality(0, entity);
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (quality != null)
            {
                if (quality.Description.Value != "")
                    tooltips.Add(new TooltipLine(Mod, "QualityDescription", quality.Description.Value));
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (quality != null)
            {
                quality.OnUseItem(player, item);
            }

            return base.UseItem(item, player);
        }

        private static void SetItemStat(Item item, PotionQuality quality)
        {
            AddQualityToName(item, quality);

            if (item.buffType != 0 && item.buffTime > 0)
                item.buffTime = GetEditingBuffTime(item.buffTime, quality);
        }

        private static void AddQualityToName(Item item, PotionQuality quality)
        {
            if (quality != null)
            {
                string qualityName = quality.Name.Value;
                item.ClearNameOverride();

                if (qualityName != "")
                {
                    item.SetNameOverride(qualityName + " " + item.Name);
                }
            }
        }

        internal static int GetEditingBuffTime(int baseTime, PotionQuality quality, bool addOrSub = true)
        {
            if (quality == null) return baseTime;

            float factor = quality.timeFactor;
            return (int) (addOrSub ? (((float)baseTime) * factor) : (((float)baseTime) / factor));
        }

        public static PotionQuality GetQuality(int type, Item item)
        {
            if (type >= 0 && type < Qualities.Length)
            {
                Type qualityType = (Type)Qualities[type];

                if(qualityType != null)
                {
                    return (PotionQuality)Activator.CreateInstance(qualityType, item);
                }
            }

            return null;
        }

        public static void InitQualities()
        {
            List<Type> result = new List<Type>();

            Type quality = typeof(PotionQuality);
            List<Type> qualities = Assembly.GetAssembly(quality)
                                           .GetTypes()
                                           .Where(type => type.IsSubclassOf(quality))
                                           .ToList();

            foreach (Type type in qualities)
            {
                if (type.IsAbstract)
                    continue;

                result.Add(type);
            }

            Qualities = result.ToArray();
        }
    }
}
