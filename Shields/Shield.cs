using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.Spells;
using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace RunesMod.Shields
{
    public abstract class Shield : ModType
    {
        public static Color AbsorbedColor => new Color(255, 125, 255);

        public short Type { get; internal set; }

        public Player Player { get; set; } = null;

        public Spell Spell { get; set; } = null;

        public int maxStrength = 0;
        public int strength = 0;

        public bool destroyable = false;

        public int cooldownTime = 0;

        public void AbsorbedEffect(int absorbedDamage)
        {
            CombatText.NewText(Player.getRect(), AbsorbedColor, absorbedDamage, false, false);
        }

        public virtual void SetDefault()
        {

        }

        protected override void Register()
        {
            ModTypeLookup<Shield>.Register(this);

            ShieldLoader.AddShield(GetType());

            Load();
        }

        public virtual void Update()
        {

        }

        public virtual void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {

        }

        public virtual void ModifyHitByProjectile(Projectile projectile, ref Player.HurtModifiers modifiers)
        {

        }

        public virtual void ModifyHurt(ref Player.HurtModifiers hurt, ref int damage)
        {

        }

        public virtual void Draw(Camera camera)
        {

        }

        public virtual void Destroy() 
        { 
            
        }

        public virtual void Raised()
        {

        }
    }
}
