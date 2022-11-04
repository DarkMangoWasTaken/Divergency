using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using DivergencyMod.Items.Weapons.Magic.Invoker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using DivergencyMod.Base;
using IL.Terraria.GameContent.RGB;
using Terraria.ModLoader.Config;

namespace DivergencyMod.Items.Weapons.Melee.LivingCoreSpear
{
    public class LivingCoreSpearDash : ModProjectile
    {
        public override string Texture => "DivergencyMod/Items/Weapons/Melee/LivingCoreSpear/LivingCoreSpear";


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Spear");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 18;
            Projectile.width = 36;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;

            Projectile.rotation = (MathF.PI / 4 * 3);
            Projectile.velocity = new Vector2(0, 0);
        }

        private int projectileChargeLoopTime = 40; // the amount of frames between each charge step

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

        private Vector2 getOffset()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 position = player.RotatedRelativePoint(player.MountedCenter);

            Vector2 retVec;

            if (Timer > 0)
                retVec = new Vector2(0, 0); // holding offset
            else
                retVec = new Vector2(30 * Projectile.direction, 0); // holding offset

            return position + retVec;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Player player = Main.player[Projectile.owner];
            hitbox.X += 33 * player.direction;
        }

        public override bool? CanDamage()
        {
            if (Timer < 0)
                return true;

            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = (MathF.PI / 4) + (MathF.PI / 2) * Projectile.direction;

            if (Timer >= 0)
            {
                if (Charges != 4) // max 4
                    Timer++;
            }
            else
            {
                if (AttackTimer == 0)
                {
                    player.velocity.X = ((Charges + 1) * 6) * player.direction;

                    if (Charges == 4)
                        player.SetImmuneTimeForAllTypes(14);
                }
                AttackTimer++;

                if (AttackTimer == 10)
                    player.velocity.X *= 0.4f;
                else if (AttackTimer >= 12)
                {
                    Projectile.Kill();
                }

            }
   
            if (player.noItems || player.CCed || player.dead || !player.active)
                Projectile.Kill();

            if (Main.mouseLeftRelease && Timer >= 10) // 10 frame delay on click attack
                Timer *= -1;

            Projectile.Center = getOffset();

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Texture2D ChargeTexture = ModContent.Request<Texture2D>("DivergencyMod/Items/Weapons/Melee/LivingCoreSpear/LivingCoreSpearCharged").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            Texture2D usedTexture = ThisChargeTimer < (projectileChargeLoopTime-5) ? texture : ChargeTexture;
            SpriteEffects flipped = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            Main.EntitySpriteDraw(usedTexture, Projectile.Center - Main.screenPosition,
            sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, flipped, 0); // drawing the sword itself
            
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            return false;

        }

       
    }


    
}