using Microsoft.Xna.Framework;
using RunesMod.Cooldowns;
using RunesMod.Items.SlotProtectors;
using RunesMod.MagicSchools;
using RunesMod.MagicSchools.Other;
using RunesMod.ModUtils;
using RunesMod.Projectiles.Magic.Elements.Water;
using RunesMod.Spells;
using RunesMod.Spells.Defense.Elements.Fire;
using RunesMod.Spells.Elements.Earth;
using RunesMod.Spells.Elements.Electric;
using RunesMod.Spells.Elements.Fire;
using RunesMod.Spells.Elements.Ice;
using RunesMod.Spells.Elements.Water;
using RunesMod.Spells.Test;
using RunesMod.UI;
using RunesMod.UI.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RunesMod.Systems
{
    public class SpellSystem : ModPlayer
    {
        #region Blood Magic

        public float bloodLevel = 0f;

        public float maxCurrentBloodLevel = 100f;

        public bool AddBlood(int useLife)
        {
            bloodLevel += useLife * 100f;
            maxCurrentBloodLevel = MathF.Ceiling(bloodLevel / 100f) * 100f;

            Player.HurtInfo hurt = new();
            hurt.Damage = useLife;
            hurt.Dodgeable = false;
            hurt.DamageSource = new();

            Player.Hurt(hurt);

            Player.SetImmuneTimeForAllTypes(0);

            return Player.statLife > 0;

            /*
            
            Player.statLife -= useLife;

            Player.lifeRegenTime = 0;

            if (Player.statLife <= 0) 
            { 
                Player.statLife = 0;
                Player.KillMe(PlayerDeathReason.ByOther(3), 10.0, 0);

                return false;
            }

            return true;

            */
        }

        #endregion

        public Concentration concentration = new();

        public int notUseSpellTimer = 0;

        public override void OnEnterWorld()
        {
            SpellHotBar hotBar = UIManager.GetUserInterface(SpellHotBar.SpellHotBarInterface).CurrentState as SpellHotBar;
            if (hotBar != null)
            {
                hotBar.ClearSlots();
                hotBar.HoldIndex = null;

                /*
                hotBar.AddSlot(SpellLoader.GetSpell<FireBallSpell>());
                hotBar.AddSlot(SpellLoader.GetSpell<FireShieldSpell>());
                hotBar.AddSlot(SpellLoader.GetSpell<StalactiteSpell>());
                hotBar.AddSlot(SpellLoader.GetSpell<FrostyBreathSpell>());
                hotBar.AddSlot(SpellLoader.GetSpell<LightingSpell>());
                hotBar.AddSlot(SpellLoader.GetSpell<WaterSpitSpell>());
                */

                /*
                hotBar.AddSlot(SpellLoader.GetSpell<TestSpell1>());
                hotBar.AddSlot(SpellLoader.GetSpell<TestSpell2>());
                hotBar.AddSlot(SpellLoader.GetSpell<TestSpell3>());
                hotBar.AddSlot(SpellLoader.GetSpell<TestSpell4>());
                hotBar.AddSlot(SpellLoader.GetSpell<TestSpell5>());
                */
            }
        }

        public override void ResetEffects()
        {
            concentration.MaxConcentration = 1;
        }

        public override void PostUpdate()
        {
            if (Main.netMode == NetmodeID.Server) return;

            if (notUseSpellTimer >= 60 * 10 && bloodLevel > 0)
            {
                bloodLevel--;
            }

            notUseSpellTimer++;

            if (Main.mouseLeft && Player.controlUseItem)
            {
                SpellHotBar hotBar = UIManager.GetUserInterface(SpellHotBar.SpellHotBarInterface).CurrentState as SpellHotBar;
                Spell spell = hotBar.GetSpell();

                TryCastSpell(spell);
            }
        }

        public override void UpdateDead()
        {
            concentration.ClearSlots();
            bloodLevel = 0;
            maxCurrentBloodLevel = 100f;
            notUseSpellTimer = 60 * 10;
        }

        public bool TryCastSpell(Spell spell)
        {
            if (spell == null) return false;

            notUseSpellTimer = 0;

            if (Player.dead || Player.CCed) return false;

            //*
            for (int i = 0; i < SpellLoader.spells.Count; i++)
            {
                SpellLoader.spells[i].Initialize();
            }
            //*/

            Item slotProtector = Player.HeldItem.ModItem is SlotProtector ? Player.HeldItem : null;
            bool canUseSpell = !Player.HeldItem.active || slotProtector != null;

            if (slotProtector == null)
            {
                slotProtector = new Item();
                slotProtector.SetDefaults(ModContent.ItemType<NoneProtector>());
            }

            SlotProtector protector = slotProtector?.ModItem as SlotProtector;

            if (Player.ItemTimeIsZero && canUseSpell && (protector?.CanUseSpell(Player, spell) == true))
            {
                if (spell == null) return false;
                if (!spell.IsInitialize)
                    spell.Initialize();

                int? schoolIndex = spell.magicSchools?.FirstOrDefault(type => !MagicSchoolLoader.GetSchool(type).NeedVoidMana);

                if (schoolIndex < 0)
                {
                    if (!Player.PlayerStates().TrueMagic)
                        return false;
                }

                Vector2 velocity = Player.Center.DirectionTo(Main.MouseWorld) * spell.projectileSpeed;

                if (!spell.CanCasting(Player, velocity))
                    return false;

                List<Cooldown> cooldowns = Player.CooldownSystem().Cooldowns;

                if (cooldowns != null)
                {
                    int index = cooldowns.FindIndex(cooldown =>
                    { 
                        if(cooldown is ShieldCooldown)
                        {
                            ShieldCooldown shieldCooldown = cooldown as ShieldCooldown;

                            return shieldCooldown.Shield?.Spell?.Type == spell.Type;
                        }

                        return false;
                    });

                    if (index >= 0) return false;
                }

                if (spell.sustainable && spell.occupiedSlotLength > 0)
                {
                    if (!concentration.HasSlot(spell))
                        concentration.AddSlot(spell);

                    else return false;
                }

                if (spell.costType == CostTypes.Mana)
                {
                    if (Player.CheckMana(spell.cost, true, false))
                        Player.manaRegenDelay = Player.maxRegenDelay;

                    else return false;
                }

                else if (spell.costType == CostTypes.Life && spell.magicSchools.Contains(MagicSchoolLoader.SchoolType<BloodMagic>()))
                {
                    if (bloodLevel < spell.bloodCost)
                    {
                        if (!AddBlood(spell.cost))
                        {
                            return false;
                        }
                    }

                    bloodLevel -= spell.bloodCost;
                }

                else return false;

                Item itemCrutch = spell.GetCrutchItem(slotProtector);
                Player.ApplyItemTime(itemCrutch);

                Player.CastingAnimationSystem().SetAnimation(itemCrutch.useAnimation, spell.animationStyle);

                if (Main.MouseWorld.X > Player.Center.X) Player.direction = 1;
                else Player.direction = -1;

                spell.OnCasting(Player, velocity);
            }

            return true;
        }
    }
}
