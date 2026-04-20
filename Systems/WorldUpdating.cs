using Microsoft.Xna.Framework;
using RunesMod.Items.Materials;
using RunesMod.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RunesMod.Systems
{
    public class WorldUpdating : ModSystem
    {
        public override void PostUpdateWorld()
        {
            if (!Main.dayTime)
            {
                //Void Shards

                Vector2 position = new Vector2();
                for (int m = 0; m < Main.worldEventUpdates; m++)
                {
                    double num14 = Main.maxTilesX / 4200.0;

                    if (!(Main.rand.Next(8000) < 10.0 * num14))
                    {
                        continue;
                    }
                    int num16 = Main.rand.Next(Main.maxTilesX - 50) + 100;
                    int num2 = 0;

                    for (int i = 0; i < Main.worldSurface; i++)
                    {
                        Tile tile = Framing.GetTileSafely(num16, i);

                        if (tile.HasTile && Main.tileSolid[tile.TileType])
                        {
                            num2 = i - 5;
                            break;
                        }
                    }

                    num16 *= 16;
                    num2 *= 16;

                    position = new(num16, num2);
                    if (Main.expertMode && Main.rand.Next(15) == 0)
                    {
                        int num4 = Player.FindClosest(position, 1, 1);
                        if ((double)Main.player[num4].position.Y < Main.worldSurface * 16.0 && Main.player[num4].afkCounter < 3600)
                        {
                            int num5 = Main.rand.Next(1, 640);
                            position.X = Main.player[num4].position.X + Main.rand.Next(-num5, num5 + 1);
                        }
                    }
                    if (!Collision.SolidCollision(position, 16, 16))
                    {
                        //Item.NewItem(new EntitySource_Misc("FallingStar"), position, ModContent.ItemType<VoidFragment>());
                        Projectile.NewProjectile(new EntitySource_Misc("FallingStar"), position, Vector2.Zero, ModContent.ProjectileType<VoidRift>(), 0, 0f);
                    }
                }
            }
        }
    }
}
