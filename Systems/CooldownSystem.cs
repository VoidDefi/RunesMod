using RunesMod.Cooldowns;
using RunesMod.Shields.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace RunesMod.Systems
{
    public class CooldownSystem : ModPlayer
    {
        public List<Cooldown> Cooldowns { get; private set; }

        public override void PreUpdate()
        {
            if (Player.dead) Cooldowns?.Clear();

            if (Cooldowns == null) return;

            for (int i = 0; i < Cooldowns.Count; i++)
            {
                Cooldown cooldown = Cooldowns[i];
                cooldown.Decrement(Player);

                if (cooldown.Value <= 0)
                {
                    cooldown.End(Player);
                    Cooldowns.Remove(cooldown);
                    i--;
                }
            }
        }

        public void Add(Cooldown cooldown)
        {
            if (Cooldowns == null) Cooldowns = new List<Cooldown>();

            if (!Cooldowns.Contains(cooldown))
            {
                Cooldowns.Add(cooldown);
            }
        }
    }
}
