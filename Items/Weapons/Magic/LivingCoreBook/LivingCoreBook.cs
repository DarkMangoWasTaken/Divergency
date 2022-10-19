using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Localization.GameCulture;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Magic.LivingCoreBook
{
    public class LivingCoreBook : ModItem
    {
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) =>
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10)) * (1f - Main.rand.NextFloat(0.5f));

        public override bool AltFunctionUse(Player player) => true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vineboom");

            Tooltip.SetDefault("Summons up to 10 explosive leaves"
                + "\nRight click to detonate instantly");


            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 7;
            Item.knockBack = 5f;
            Item.noMelee = true;

            Item.shoot = ProjectileType<LivingLeaf>();
            Item.shootSpeed = 12f;
            Item.mana = 10;
            Item.width = Item.height = 16;
            Item.scale = 1f;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.DD2_LightningAuraZap;
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == Item.shoot && player.ownedProjectileCounts[Item.shoot] > 0)
                    {
                        Main.projectile[i].timeLeft /= 8;
                    }
                }

                Item.shoot = ProjectileID.None;
                Item.useTime = Item.useAnimation = 40;
                Item.UseSound = SoundID.DD2_KoboldIgnite;
            }
            else
            {
                Item.shoot = ProjectileType<LivingLeaf>();
                Item.useTime = Item.useAnimation = 10;
                Item.UseSound = SoundID.DD2_LightningAuraZap;
            }

            return player.ownedProjectileCounts[Item.shoot] < 10;
        }
    }

    public class LivingLeaf : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25; // in SetStaticDefaults()
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override Color? GetAlpha(Color lightColor) => new(255, 255, 255, 100);

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 16;
            Projectile.scale = 1f;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
        }

        private bool initilize = true;

        public override void AI()
        {
            Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
            float multiplier = 0.2f;
            float max = 1f;
            float min = 0.5f;
            RGB *= multiplier;
            if (RGB.X > max)
            {
                multiplier = 0.3f;
            }
            if (RGB.X < min)
            {
                multiplier = 0.6f;
            }
            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            Projectile.velocity *= 0.98f;

            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.ChlorophyteWeapon, new Vector2(Main.rand.NextFloat(-0.2f, 0.2f)), 0, default, 1f);
            dust.noGravity = true;

            if (initilize)
            {
                int numberDust = 10;

                SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);

                for (int i = 0; i < numberDust; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    dust = Dust.NewDustPerfect(Projectile.Center, DustID.ChlorophyteWeapon, speed * 5, 0, default, 1f);
                    dust.noGravity = true;
                }

                initilize = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/vineboom")
            {
                Volume = 3f,
                PitchVariance = 0.2f,
                MaxInstances = 10

            });
            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8; //abizmal
            ParticleManager.NewParticle(Projectile.Center, new Vector2(0, 0), ParticleManager.NewInstance<LivingCoreExplosionParticle>(), Color.Purple, 1.4f);

            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 10, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, Main.rand.NextFloat(0.5f, 1f));
            }


            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<LivingExplosion>(), Projectile.damage + player.ownedProjectileCounts[Projectile.type] * 10,
                Projectile.knockBack, Projectile.owner);

            int numberDust = 5;

            for (int i = 0; i < numberDust; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 5, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, Main.rand.NextFloat(0.3f, 0.6f));
            }
        }
        public TrailRenderer prim;
        public TrailRenderer prim2;
        public override bool PreDraw(ref Color lightColor)
        {
            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/Trail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(10f) * (1f - p), (p) => Projectile.GetAlpha(Color.LimeGreen) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = Projectile.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(5f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
           
            //prim2.Draw(Projectile.oldPos);


            return true;
        }
    }

    public class LivingExplosion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CrystalLeafShot;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 200;
            Projectile.scale = 1f;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            
            Projectile.timeLeft = 5;
        }
    }
}