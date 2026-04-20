using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RunesMod.MagicSchools;
using RunesMod.UI.Elements;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RunesMod.Spells
{
    public class Spell : ModType, ILocalizedModType
    {
        public bool IsInitialize { get; private set; } = false;

        public short Type { get; internal set; }

        public string LocalizationCategory => "Spells";

        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);

        public virtual LocalizedText Tooltip => this.GetLocalization(nameof(Tooltip), () => "");

        public string TexturePath => (GetType().Namespace + "." + Name).Replace('.', '/');

        public Asset<Texture2D> Texture
        {
            get
            {
                if (texture == null) texture = ModContent.Request<Texture2D>(TexturePath, AssetRequestMode.ImmediateLoad);
                return texture; 
            }
        }

        private Asset<Texture2D> texture = null;

        public int cost;
        public CostTypes costType;

        public int cooldownTime = 2;
        public int animationTime = 2;
        public int animationStyle = -1;
        public bool flipPlayer = true;

        public int projectileType = 0;
        public float projectileSpeed = 1f;

        public int damage = 0;
        public float knockBack = 0;

        public bool sustainable = false;
        public float occupiedSlotLength = 0;

        public ConcentrationBarGradient? concentrationBarGradient = null;

        public int[] magicSchools = null;

        /// <summary>
        /// Используется только для магии крови и при Типе Ценны - жизнь. 
        /// это число отнимается от уровня крови 
        /// </summary>
        public float bloodCost = 1f;

        public void Initialize()
        {
            SetDefaults();
            IsInitialize = true;
        }

        protected virtual void SetDefaults()
        {
            
        }

        protected override void Register()
        {
            ModTypeLookup<Spell>.Register(this);
            Type = (short)SpellLoader.ReserveSpellID();

            _ = DisplayName;
            _ = Tooltip;

            Initialize();
            SpellLoader.spells.Add(this);

            Load();
        }

        public virtual bool CanCasting(Player player, Vector2 velocity)
        {
            return true;
        }

        public virtual void OnCasting(Player player, Vector2 velocity)
        {
            SpawnProjectile(player, velocity);
        }

        public virtual void DrawIcon(SpriteBatch spriteBatch)
        {

        }

        public EntitySource_SpellCast GetSpellCastSource(Player player, string context = null)
        {
            return new EntitySource_SpellCast(player, this, context);
        }

        internal Item GetCrutchItem(Item slotProtector)
        {
            Item crutch = slotProtector.Clone();

            crutch.useTime = (int)(crutch.useTime / 100f * cooldownTime);
            crutch.useAnimation = (int)(crutch.useAnimation / 100f * animationTime);

            return crutch;
        }

        /// <summary>
        /// <inheritdoc cref="Projectile.NewProjectile(Terraria.DataStructures.IEntitySource, Vector2, Vector2, int, int, float, int, float, float, float)"/>
        /// <br/><br/>This particular overload uses a Vector2 instead of X and Y to determine the actual spawn position and a Vector2 to dictate the initial velocity.
        /// </summary>
        protected int SpawnProjectile(Player player, Vector2 velocity = new Vector2(), int type = 0, int newDamage = 0, float newKnockBack = 0, int owner = -1, int ai0 = 0, int ai1 = 0, int ai2 = 0)
        {
            int useType = type <= 0 ? projectileType : type;

            if (useType > 0)
            {
                int useDamage = newDamage <= 0 ? damage : newDamage;
                float useKnockBack = newKnockBack <= 0f ? knockBack : newKnockBack;
                int useOwner = owner <= -1 ? player.whoAmI : owner;

                return Projectile.NewProjectile(GetSpellCastSource(player), player.Center, velocity, useType, useDamage, useKnockBack, useOwner, ai0, ai1, ai2);
            }

            return -1;
        }

        public void AddMagicSchool(int type)
        {
            MagicSchool.AddToArray(ref magicSchools, type);
        }

        public void AddMagicSchool<T>() where T : MagicSchool => AddMagicSchool(MagicSchoolLoader.SchoolType<T>());
    }
}
