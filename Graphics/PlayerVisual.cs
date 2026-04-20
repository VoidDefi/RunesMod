using Microsoft.Xna.Framework;
using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RunesMod.Graphics
{
    public class PlayerVisual : ModPlayer
    {
        private PlayerBuffSystem BuffSystem => Player.GetModPlayer<PlayerBuffSystem>();

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            SetSkinColor(ref drawInfo, 
                         new List<(Color? color, bool onlyHead)>()
                         {
                             (PlayerBuffSystem.GetObsidianSkinColor(Player), false),
                             (PlayerBuffSystem.GetIronSkinColor(Player), false),
                             (PlayerBuffSystem.GetNauseaColor(Player), true)
                         });
        }

        private void SetSkinColor(ref PlayerDrawSet drawInfo, List<(Color? color, bool onlyHead)> colors)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].color == null) { colors.RemoveAt(i); i--; }
            }

            for (int i = 0; i < colors.Count; i++)
            {
                Color? color = colors[i].color;

                if (color.HasValue)
                {
                    Color newColor = color.Value;
                    Color newColorHead = color.Value;
                    Color newColorLegs = color.Value;

                    if (i > 0)
                    {
                        newColor = Color.Lerp(drawInfo.colorBodySkin, newColor, 0.5f);

                        newColorHead = Color.Lerp(drawInfo.colorBodySkin, newColorHead, 0.5f);
                        newColorLegs = Color.Lerp(drawInfo.colorBodySkin, newColorLegs, 0.5f);
                    }

                    drawInfo.colorHead = newColorHead;

                    if (!colors[i].onlyHead)
                    {
                        drawInfo.colorBodySkin = newColor;
                        drawInfo.colorLegs = newColorLegs;
                    }
                }
            }
        }
    }
}
