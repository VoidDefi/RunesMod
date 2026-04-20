using JetBrains.Annotations;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace RunesMod.Systems.Knowledges
{
    public interface IKnowledge
    {
        public abstract LocalizedText DisplayName { get; }

        public abstract LocalizedText Tooltip { get; }

        public abstract Asset<Texture2D> Texture { get; }

        public virtual bool CanLearning(Player player)
        {
            return true;
        }

        public abstract bool Compare(IKnowledge knowledge);

        public abstract void SaveData(TagCompound tag);

        public abstract void LoadData(TagCompound tag);

        public virtual void OnLearn(Player player) { }
    }
}
