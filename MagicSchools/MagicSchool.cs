using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RunesMod.MagicSchools
{
    public class MagicSchool : ModType, ILocalizedModType
    {
        public short Type { get; internal set; }

        public string LocalizationCategory => "MagicSchools";

        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);

        public virtual LocalizedText Tooltip => this.GetLocalization(nameof(Tooltip), PrettyPrintName);

        public string TexturePath => (GetType().Namespace + "." + Name).Replace('.', '/');

        public Asset<Texture2D> Texture
        {
            get
            {
                if (texture == null) texture = ModAssets.Request<Texture2D>(TexturePath, "");
                return texture;
            }
        }

        private Asset<Texture2D> texture = null;

        public virtual bool OnlyCategory => false;

        public virtual DamageClassData DefaultDamage => new();

        public virtual bool NeedVoidMana => true;

        protected override void Register()
        {
            ModTypeLookup<MagicSchool>.Register(this);
            Type = (short)MagicSchoolLoader.ReserveSchoolID();

            SetStaticDefaults();

            MagicSchoolLoader.schools.Add(this);
            Load();
        }

        public virtual bool CanStack(MagicSchool school)
        {
            return true;
        }

        public static void AddToArray(ref int[] magicSchools, int type)
        {
            MagicSchool school = MagicSchoolLoader.GetSchool(type);

            if (school?.OnlyCategory == false)
            {
                List<int> schools = new List<int>();

                if (magicSchools != null)
                {
                    schools = magicSchools.ToList();

                    if (schools.Contains(type))
                    {
                        return;
                    }

                    for (int i = 0; i < schools.Count; i++)
                    {
                        MagicSchool school1 = MagicSchoolLoader.GetSchool(magicSchools[i]);

                        if (!school.CanStack(school1) || !school1.CanStack(school))
                        {
                            Logging.PublicLogger.Info($"RunesMod: {school.FullName} not staking for {school1.FullName}");

                            return;
                        }
                    }
                }

                schools.Add(type);
                magicSchools = schools.ToArray();
            }
        }
    }
}
