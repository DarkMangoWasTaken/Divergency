using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Localization.GameCulture;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Melee.RootBreaker
{
    public class Rootbreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rootbreaker");
            DisplayName.AddTranslation((int)CultureName.German, "Wurzelbrecher");

            Tooltip.SetDefault("Hitting enemies weakens them"
                + "\nIf an enemy is weakened enough, they will be launched by the hammer"
                + "\nWeakened enemies die instantly");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 15;
            Item.knockBack = 14f;
            Item.noMelee = true;

            Item.shoot = ProjectileType<RootBreakerProj>();
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;

            Item.width = Item.height = 16;
            Item.scale = 1f;

            Item.useTime = Item.useAnimation = 100;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Quest;
            Item.questItem = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 35f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }

        public int attackRotation = 1;
        public int itemComboCounter = 1;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int attackDirection = attackRotation;
            attackRotation = -attackRotation;

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1, attackDirection);
            Item.shoot = ProjectileType<RootBreakerProj>();

            return false;
        }
    }

    public class RootBreakerProj : ModProjectile
    {
        public override string Texture => "DivergencyMod/Items/Weapons/Melee/RootBreaker/Rootbreaker";

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 52;
            Projectile.scale = 1.3f;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = 0;
            Projectile.timeLeft = 72;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public virtual float Lerp(float value) => value == 1f ? 1f : (value == 1f ? 1f : (float)Math.Pow(2, value * 10f - 10f) / 2f);

        public override void AI()
        {
            AttachToPlayer();
        }

        public override bool ShouldUpdatePosition() => false;

        public int SwingTime = 70;
        public float holdOffset = 60f;

        public void AttachToPlayer()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }

            int dir = (int)Projectile.ai[1];
            float swingProgress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float defRot = Projectile.velocity.ToRotation();
            float endSet = ((MathHelper.PiOver2) / 0.2f);
            float start = defRot - endSet;
            float end = defRot + endSet;
            float rotation = dir == 1 ? start.AngleLerp(end, swingProgress) : start.AngleLerp(end, 1f - swingProgress);
            Vector2 position = player.RotatedRelativePoint(player.MountedCenter);

            position += rotation.ToRotationVector2() * holdOffset;

            Projectile.Center = position;
            if (dir == -1)
            {
                Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;
            }
            if (dir == 1)
            {
                Projectile.rotation = (position - player.Center).ToRotation() - MathHelper.PiOver4;
            }

            player.heldProj = Projectile.whoAmI;
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = rotation * player.direction;
            player.itemTime = player.itemAnimation = 2;

            Projectile.netUpdate = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player Player = Main.player[Projectile.owner];

            if (target.knockBackResist > 0f && target.life <= 50)
            {
                Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 10;
                target.knockBackResist = 0.5f;
            }

            //add here only when item active then triggered
            if (target.lifeMax <= 500)
            {
                target.GetGlobalNPC<LivingCoreHammerNPC>().triggered = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int dir = (int)Projectile.ai[1];
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            SpriteEffects spriteEffects = SpriteEffects.None;
            float rotation = Projectile.rotation;

            Player player = Main.player[Projectile.owner];

            if (dir == 1)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, rotation, origin, Projectile.scale, spriteEffects, 0);

            return false;
        }
    }

    public class LivingCoreHammerNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool triggered;
        public float timer = 0;

        public override bool PreAI(NPC npc)
        {
            if (triggered && npc.life <= 50)
            {
                npc.rotation += (npc.velocity.Length() * 0.05f) * npc.direction;
                npc.aiStyle = 0;

                timer++;

                if (timer >= 5f)
                {
                    if (npc.collideX || npc.collideY)
                    {
                        Player Player = Main.LocalPlayer;

                        Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 10;

                        SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, npc.position);

                        npc.StrikeNPC(9999, 10, 10, false, false, true);

                        for (int i = 0; i < Main.rand.Next(3, 4); i++)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-15, -10)), ProjectileType<LivingStone>(), npc.damage, 1f, Main.myPlayer);
                        }
                    }
                }

                return false;
            }

            return base.PreAI(npc);
        }
    }
}