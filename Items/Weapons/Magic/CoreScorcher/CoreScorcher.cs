using DivergencyMod.Dusts.Particles;
using Microsoft.Xna.Framework;
using ParticleLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Localization.GameCulture;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Magic.CoreScorcher
{
    public class CoreScorcher : ModItem
    {


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core Scorcher");

            Tooltip.SetDefault("Fires a deadly Living Core flame"
                + "\nInflicts the Dryads buffs/debuffs on enemy/player contact");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 25;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.crit = 0;
            Item.shoot = ProjectileType<CoreFlame>();
            Item.shootSpeed = 12f;
            Item.mana = 3;
            Item.width = Item.height = 16;
            Item.scale = 1f;
            Item.useTime = Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            
            Item.autoReuse = true;
            Item.useTurn = false;
            
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Green;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item34 with { Volume = 1f, Pitch = Main.rand.NextFloat(0.5f, 2f), MaxInstances = 400 });

            for (int i = 0; i < 5; i++)
            {
                ParticleManager.NewParticle(position, velocity, ParticleManager.NewInstance<ResetParticle>(), Color.Purple, 0.85f);
            }


            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 75f;
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5)) * (1f - Main.rand.NextFloat(0.35f));

                position += Offset;
            
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -5);
        }
    }

    public class CoreFlame : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor) => new(255, 255, 255, 100);
        public override string Texture => "DivergencyMod/Items/Weapons/Magic/Invoker/InvokedProj";

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = Projectile.height = 60;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.alpha = 255;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 30;
            Projectile.hide = true;
            Projectile.CritChance = 0;

        }
       



        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.active && player.Hitbox.Intersects(Projectile.Hitbox))
            {
                player.AddBuff(BuffID.DryadsWard, 180, true, false);
                
            }
            Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
            float multiplier = 1f;
            float max = 2f;
            float min = 1f;
            RGB *= multiplier;
            if (RGB.X > max)
            {
                multiplier = 0.5f;
            }
            if (RGB.X < min)
            {
                multiplier = 0.6f;
            }
            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.DryadsWardDebuff, 120, false);
        }



    }

    
}