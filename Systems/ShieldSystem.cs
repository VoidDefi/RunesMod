using FullSerializer.Internal;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RunesMod.Cooldowns;
using RunesMod.ModUtils;
using RunesMod.Shields;
using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace RunesMod.Systems
{
    public class ShieldSystem : ModPlayer
    {
        #region IL

        private FieldInfo cancelledInfo;

        private FieldInfo CancelledInfo 
        {
            get
            {
                if (cancelledInfo == null)
                {
                    cancelledInfo = typeof(Player.HurtModifiers)
                        .GetDeclaredFields()
                        .ToList()
                        .Find(f => f.Name == "_cancelled");
                }

                return cancelledInfo;
            }
        }

        public override void Load()
        {
            On_LegacyPlayerRenderer.DrawPlayerFull += ILDrawPlayerFullHook;
            IL_Player.Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float += ILHurtHook;
        }

        private void ILDrawPlayerFullHook(On_LegacyPlayerRenderer.orig_DrawPlayerFull original, LegacyPlayerRenderer self, Camera camera, Player drawPlayer)
        {
            original.Invoke(self, camera, drawPlayer);

            PostDrawPlayerFull(camera, drawPlayer);
        }

        private void ILHurtHook(ILContext context)
        {
            MethodInfo toHurtInfoInfo = typeof(Player.HurtModifiers).GetMethod("ToHurtInfo");
            MethodInfo playerModifyHurtInfo = typeof(PlayerLoader).GetMethod("ModifyHurt", BindingFlags.Public | BindingFlags.Static);
            FieldInfo hurtInfoDamageInfo = typeof(Player.HurtInfo).GetDeclaredFields().ToList().Find(f => f.Name == "_damage");

            var c = new ILCursor(context);

            try
            {
                ILLabel exitIfLabel = context.DefineLabel();

                c.GotoNext(i => i.MatchCall(playerModifyHurtInfo));

                c.Index -= 2;
                
                c.Emit(OpCodes.Ldloca_S, (byte)2); //Load local var: HurtModifiers modifiers
                c.Emit(OpCodes.Ldarg_S, (byte)0); //Load this argument: Player this
                c.Emit(OpCodes.Ldarga_S, (byte)2); //Load in argument: int Damage

                c.EmitDelegate(delegate (ref Player.HurtModifiers hurt, Player player, ref int damage)
                {
                    Type mType = typeof(Player.HurtModifiers);
                    player.GetModPlayer<ShieldSystem>().ModifyHurt(ref hurt, ref damage);
                    return (bool)CancelledInfo.GetValue(hurt);
                });

                c.Emit(OpCodes.Brfalse, exitIfLabel);

                c.Emit(OpCodes.Ldc_R8, 0.0);
                c.Emit(OpCodes.Ret);

                c.MarkLabel(exitIfLabel);
            }

            catch
            {
                context.DumpIL();
            }
        }

        #endregion

        public List<Shield> Shields { get; private set; } = new();

        //public List<>

        public override void PostUpdate()
        {
            if (Main.netMode == NetmodeID.Server) return;

            if (Player.dead)
            {
                for (int i = 0; i < Shields.Count; i++)
                {
                    Shield shield = Shields[i];
                    shield.Destroy();
                    Shields.Remove(shield);
                    i--;
                }
            }

            if (Player.dead || Player.CCed) return;

            for (int i = 0; i < Shields.Count; i++)
            {
                Shield shield = Shields[i];

                if (shield.Player != null)
                {
                    shield.Update();

                    if (shield.strength <= 0 && shield.destroyable)
                    {
                        shield.Destroy();

                        ShieldCooldown cooldown = new();
                        cooldown.Shield = shield;

                        Player.CooldownSystem().Add(cooldown);

                        if (shield.Spell != null)
                        {
                            Player.SpellSystem().concentration.RemoveSlot(shield.Spell);
                        }

                        Shields.Remove(shield);
                        i--;
                    }
                }
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            foreach (Shield shield in Shields)
            {
                if (shield.Player != null)
                {
                    shield.ModifyHitByNPC(npc, ref modifiers);
                }
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            foreach (Shield shield in Shields)
            {
                if (shield.Player != null)
                {
                    shield.ModifyHitByProjectile(proj, ref modifiers);
                }
            }
        }

        public void ModifyHurt(ref Player.HurtModifiers hurt, ref int damage)
        {
            foreach (Shield shield in Shields)
            {
                if (shield.Player != null)
                {
                    if ((bool)CancelledInfo.GetValue(hurt)) continue;

                    shield.ModifyHurt(ref hurt, ref damage);
                }
            }
        } 

        public void PostDrawPlayerFull(Camera camera, Player player)
        {
            List<Shield> shields = player.ShieldSystem().Shields;

            foreach (Shield shield in shields)
            {
                if (shield.Player != null)
                {
                    shield.Draw(camera);
                }
            }
        }

        public void AddShield(Shield shield)
        {
            if (shield == null) return;

            shield.SetDefault();
            shield.strength = shield.maxStrength;
            shield.Player = Player;

            shield.Raised();

            Shields.Add(shield);
        }

        public void AddShield(int type)
        {
            AddShield(ShieldLoader.GetShield(type));
        }

        public void AddShield(int type, Spell spell)
        {
            Shield shield = ShieldLoader.GetShield(type);
            shield.Spell = spell;

            AddShield(shield);
        }
    }
}
