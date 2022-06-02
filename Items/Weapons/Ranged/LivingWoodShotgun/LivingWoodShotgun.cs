using DivergencyMod.Items.Ammo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Localization.GameCulture;

namespace DivergencyMod.Items.Weapons.Ranged.LivingWoodShotgun
{
    public class LivingWoodShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Wood Shotgun");

            Tooltip.SetDefault("Converts Wooden Bullets to Living Wood Bullets"
                + "\n'The confusing sequal!'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 4;
            Item.crit = 20;
            Item.knockBack = 2f;
            Item.noMelee = true;

            Item.useAmmo = ModContent.ItemType<WoodenBullet>();
            Item.shoot = ModContent.ProjectileType<LivingWoodBullet>();
            Item.shootSpeed = 19f;

            Item.width = Item.height = 16;
            Item.scale = 1f;

            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item167;
            Item.autoReuse = false;
            Item.useTurn = false;

            Item.value = Item.sellPrice(999, 0, 0, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 10f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 5 + Main.rand.Next(3);

            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(20));

                float scale = 1f - (Main.rand.NextFloat() * 0.3f);
                perturbedSpeed *= scale;

                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }

            const int numberDust = 10;

            for (int i = 0; i < numberDust; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(20));

                float scale = 1f - (Main.rand.NextFloat() * 0.75f);
                perturbedSpeed *= scale;

                Dust dust = Dust.NewDustPerfect(position, DustID.Grass, perturbedSpeed, 0, default, 1f);
                Gore.NewGore(null, position, velocity, GoreID.TreeLeaf_Normal, 1.1f);
                dust.noGravity = true;
            }

            return false;
        }
    }

    public class LivingWoodBullet : ModProjectile
    {
        public override string Texture => "DivergencyMod/Items/Weapons/Ranged/WoodenShotgun/WoodenBulletProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forest Ripper");
            DisplayName.AddTranslation((int)CultureName.German, "Waldrei�er");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            DrawOriginOffsetX = 8;
            Projectile.width = Projectile.height = 4;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 0;
            Projectile.extraUpdates = 2;
        }

        private bool target = false;

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.WoodFurniture, 0, 0, 100, Color.SandyBrown, 1f);
                dust.noGravity = true;
                dust.velocity *= 2f;
                dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.WoodFurniture, 0f, 0f, 1000, Color.SandyBrown, 1f);
            }
        }

        public override void AI()
        {
            int thej = 0;
            if (Projectile.velocity.Y == thej)
            {
                Projectile.Kill();
            }

            Vector2 move = Vector2.Zero;
            float distance = 100f;

            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].immortal && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.ChlorophyteWeapon, Vector2.Zero, 0, default, 1.5f);
                dust.noGravity = true;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                Projectile.spriteDirection = Projectile.direction;
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }

                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (2 * Projectile.velocity + move) / 10f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
                else
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                Projectile.spriteDirection = Projectile.direction;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.ChlorophyteWeapon, Vector2.Zero, 0, default, 1.5f);
                dust.noGravity = true;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 30f)
            {
                vector *= 20f / magnitude;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}