using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using DivergencyMod.Players.ComboSystem;
using IL.Terraria.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static ParticleLibrary.Particle;

namespace DivergencyMod.Items.Weapons.Melee.LivingCoreSpear

{
    public class LivingCoreSpearSpin : ModProjectile, IComboProjectile
    {
        public override string Texture => "DivergencyMod/Items/Weapons/Melee/LivingCoreSpear/LivingCoreSpear";

        public int MaxCharges => 4;
        public int CurCharges
        {
            get
            {
                if (Timer < 0)
                    return -1;
                return Charges;
            }
        }

        public int projectileChargeLoopTime => 50;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Sword");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 40;
            Projectile.width = 40;
            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;

        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private int Charges
        {
            get => Math.Abs(((int)Projectile.ai[0] - ((int)Projectile.ai[0] % projectileChargeLoopTime)) / projectileChargeLoopTime);
        }

        private int ThisChargeTimer
        {
            get => Math.Abs((int)Projectile.ai[0] % projectileChargeLoopTime);
        }

        private float AttackTimer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public virtual float Lerp(float val)
        {
            return val == 1f ? 1f : (val == 1f ? 1f : (float)Math.Pow(2, val * 10f - 10f) / 2f);
        }

        public override bool? CanDamage()
        {
            if (Timer < 0)
                return true;

            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.X += (int)(MathF.Cos(Projectile.rotation)*25);
            hitbox.Y += (int)(MathF.Sin(Projectile.rotation)*25);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Timer >= 0)
            {
                if (Charges != 4) // max 4
                    Timer++;

                Vector2 position = player.RotatedRelativePoint(player.MountedCenter);
                Projectile.Center = position;

                if (Main.mouseLeftRelease && Timer >= 20)
                    Timer *= -1;
            }
            else
            {
                AttackTimer++;

                Projectile.rotation += (MathF.PI / 16 * Projectile.direction);

                Vector2 position = player.RotatedRelativePoint(player.MountedCenter);
                Projectile.Center = position;
                Projectile.position += new Vector2(MathF.Cos(Projectile.rotation), MathF.Sin(Projectile.rotation)) * 50;

                if (AttackTimer >= (((float)Charges + 1f) / 5f) * (400))
                    Projectile.Kill();
            }

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
        public override bool ShouldUpdatePosition() => false;

        public TrailRenderer SwordSlash;
        public TrailRenderer SwordSlash2;
        public TrailRenderer SwordSlash3;

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            Rectangle sourceRectangle = new Rectangle(0, 0, 74, 72);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            if (Timer >= 0)
            {
                SpriteEffects spriteEffects = player.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Texture2D ChargeTexture = ModContent.Request<Texture2D>("DivergencyMod/Items/Weapons/Melee/LivingCoreSpear/LivingCoreSpearCharged").Value;

                Texture2D usedTexture = ThisChargeTimer < (projectileChargeLoopTime - 5) ? texture : ChargeTexture;

                Main.EntitySpriteDraw(usedTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                   sourceRectangle, drawColor, 0, origin, Projectile.scale, spriteEffects, 0);


                return false;
            }

            Main.spriteBatch.End();

            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/MotionTrail").Value;
            var TrailTex2 = ModContent.Request<Texture2D>("DivergencyMod/Trails/idktrail").Value;
            var TrailTex3 = ModContent.Request<Texture2D>("DivergencyMod/Trails/gravytrail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);


            if (SwordSlash == null)
            {
                SwordSlash = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(14f), (p) => new Color(100, 255, 100, 10) * (1f - p));
                SwordSlash.drawOffset = Projectile.Size / 1.9f;
            }
            if (SwordSlash2 == null)
            {
                SwordSlash2 = new TrailRenderer(TrailTex2, TrailRenderer.DefaultPass, (p) => new Vector2(35f), (p) => new Color(10, 150, 50, 50) * (1f - p));
                SwordSlash2.drawOffset = Projectile.Size / 1.9f;

            }
            if (SwordSlash3 == null)
            {
                SwordSlash3 = new TrailRenderer(TrailTex3, TrailRenderer.DefaultPass, (p) => new Vector2(20f), (p) => new Color(10, 255, 50, 40) * (1f - p));
                SwordSlash3.drawOffset = Projectile.Size / 2f;

            }

            Main.spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            if (Timer > 0)
            {
                SwordSlash.Draw(Projectile.oldPos);
                SwordSlash2.Draw(Projectile.oldPos);

                //SwordSlash3.Draw(Projectile.oldPos);
            }


            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
               sourceRectangle, drawColor, Projectile.rotation + MathF.PI / 4 * 3, origin, Projectile.scale, SpriteEffects.None, 0); // drawing the sword itself
            Main.instance.LoadProjectile(Projectile.type);
            
            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.spriteBatch.End();

            Main.spriteBatch.Begin();

            return false;

        }

    }
}