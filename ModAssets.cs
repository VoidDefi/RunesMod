using ReLogic.Content;
using Terraria.ModLoader;

namespace RunesMod
{
    internal class ModAssets
    {
        public static string Assets => "RunesMod/Assets/";

        public static string Effects => "Effects/";

        public static string Textures => "Textures/";

        public static string MaskTextures => "Textures/Masks/";

        public static string ShieldMaskTextures => "Textures/Masks/Shields/";

        public static string NoiseTextures => "Textures/Noises/";

        public static string UITextures => "Textures/UI/";

        public static string NPCTextures => "Textures/NPCs/";

        public static string BossTextures => "Textures/NPCs/Bosses/";

        public static string ConcentrationUITextures => "Textures/UI/ConcentrationBar/";

        public static string KnowledgesUI => "Textures/UI/Knowledges/";

        public static Asset<T> Request<T>(string path, string name, AssetRequestMode mode = AssetRequestMode.ImmediateLoad) where T : class
        {
            return ModContent.Request<T>(Assets + path + name, mode);
        }

        public static Asset<T> RequestAsync<T>(string path, string name) where T : class
        {
            return ModContent.Request<T>(Assets + path + name, AssetRequestMode.AsyncLoad);
        }
    }
}
