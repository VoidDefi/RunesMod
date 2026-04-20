using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RunesMod.Systems
{
    public class PlayerStates : ModPlayer
    {
        #region Private Fields

        

        #endregion

        #region Properties

        public bool TrueMagic { get; set; } = false;

        #endregion

        #region Stats Sync
        /*
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)ExampleMod.MessageType.ExampleStatIncreasePlayerSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)exampleLifeFruits);
            packet.Write((byte)exampleManaCrystals);
            packet.Send(toWho, fromWho);
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            exampleLifeFruits = reader.ReadByte();
            exampleManaCrystals = reader.ReadByte();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            ExampleStatIncreasePlayer clone = (ExampleStatIncreasePlayer)targetCopy;
            clone.exampleLifeFruits = exampleLifeFruits;
            clone.exampleManaCrystals = exampleManaCrystals;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            ExampleStatIncreasePlayer clone = (ExampleStatIncreasePlayer)clientPlayer;

            if (exampleLifeFruits != clone.exampleLifeFruits || exampleManaCrystals != clone.exampleManaCrystals)
            {
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            }
        }*/

        #endregion

        #region Stats IO

        public override void SaveData(TagCompound tag)
        {
            tag["TrueMagic"] = TrueMagic;
        }

        public override void LoadData(TagCompound tag)
        {
            TrueMagic = tag.GetBool("TrueMagic");
        }

        #endregion
    }
}
