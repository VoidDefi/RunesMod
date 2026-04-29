using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.ModUtils;
using RunesMod.Shields;
using RunesMod.Shields.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.Cooldowns
{
    public class ShieldCooldown : Cooldown
    {
        private Shield shield;

        public Shield Shield 
        {
            get => shield;
            set 
            { 
                shield = value;

                MaxValue = shield.cooldownTime;
                Value = shield.cooldownTime;

                Texture = shield.Spell?.Texture;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Shield != null)
            {
                base.Draw(spriteBatch, position);
            }
        }

        public override void End(Player player)
        {
            if (shield != null && shield.Spell != null)
            {
                player.SpellSystem().TryCastSpell(shield.Spell);//.AddShield(shield.Type, shield.Spell);
            }
        }
    }
}
