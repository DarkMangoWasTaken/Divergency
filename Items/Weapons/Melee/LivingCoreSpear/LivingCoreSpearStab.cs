using DivergencyMod.Dusts.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ParticleLibrary.Particle;

namespace DivergencyMod.Items.Weapons.Melee.LivingCoreSpear
{
    public class LivingCoreSpearStab : ModProjectile
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

        private int attackActualFrames = 20; // the total number of frames
        private int attackFrames = 3; // the number of frames that will be moving the spear forward
        private int totalFrames = 16; // to total amount of frames the spear will be moving (having a little pause is a good thing, i feel...)
        private int multiplier = 20; // how much to add each attack frame

        private int delay = 75; // 90 frames, 1.5 sec

        private int ThisChargeTimerAI1
        {
            get
            {
                return ThisChargeTimerAI1PreMod % attackActualFrames;
            }
        }

        private int ThisChargeTimerAI1PreMod
        {
            get
            {
                int curFrame = (int)Projectile.ai[1];

                if (curFrame > 4 * attackActualFrames)
                {
                    if  (curFrame > 4 * attackActualFrames + delay)
                    { 
                        curFrame -= delay;
                        ParticleManager.NewParticle(Projectile.Center + new Vector2(Main.rand.NextFloat(10, -10)), Projectile.velocity * 10, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 1f, Projectile.whoAmI, Layer: Layer.BeforeProjectiles);

                    }
                    else
                        curFrame = 4 * attackActualFrames;
                }
                return curFrame;
            }
        }

        private int TotalChargesMade
        {
            get => ((int)ThisChargeTimerAI1PreMod - ((int)ThisChargeTimerAI1PreMod % attackActualFrames)) / attackActualFrames;
        }

        private float Offset
        {
            get
            {
                if (ThisChargeTimerAI1 <= attackFrames)
                    return ThisChargeTimerAI1 * multiplier;
                else
                {
                    int maxOffset = attackFrames * multiplier;
                    int backFrames = totalFrames - attackFrames;

                    int curBackFrame = ThisChargeTimerAI1 - attackFrames;

                    float offset = (float)maxOffset - ((float)maxOffset / (float)backFrames * (float)curBackFrame);

                    offset = MathF.Max(offset, 0);

                    return offset;
                }
            }
        }

        private float ExtraRotationOffset
        {
            get
            {
                float rot = 0;

                if (TotalChargesMade == 1)
                    rot += MathF.PI / 16;
                else if (TotalChargesMade == 2)
                    rot -= MathF.PI / 16;

                return rot;
            }
        }

        private Vector2 getOffset()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 position = player.RotatedRelativePoint(player.MountedCenter);

            Vector2 retVec = new Vector2(0, 0);

            if (Timer >= 0)
            {
                retVec = new Vector2(0, 0); // holding offset
            }
            else
            {
                Mod.Logger.Info(Projectile.direction + " | asdd");
                retVec = new Vector2(Offset * Projectile.direction, MathF.Sin(ExtraRotationOffset) * Offset * Projectile.direction);
            }

            return position + retVec;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Mod.Logger.Info(Projectile.direction + " | asd");
            hitbox.Y += (int)(MathF.Sin(ExtraRotationOffset) * Offset * Projectile.direction);
            hitbox.X += 33 * Projectile.direction;
            // base.ModifyDamageHitbox(ref hitbox);
        }

        public override bool? CanDamage()
        {
            if (ThisChargeTimerAI1 <= attackFrames && Timer < 0)
                return true;

            return false;
        }

        public override void AI()
        {
            Projectile.rotation = (MathF.PI / 4) + (MathF.PI / 2) * Projectile.direction + ExtraRotationOffset;

            if (Timer >= 0)
            {
                if (Charges != 4) // max 4
                    Timer++;
            }
            else
                AttackTimer++;

            //Projectile.Kill();

            Player player = Main.player[Projectile.owner];

            Mod.Logger.Info(Projectile.direction + " | " + player.direction);

            if (player.noItems || player.CCed || player.dead || !player.active)
                Projectile.Kill();

            /*
            if (Main.myPlayer == Projectile.owner)
            {
                player.ChangeDir(Projectile.direction);
            }
            */

            // these 2 lines manage max charge
            if (Main.mouseLeftRelease && Timer >= 10) // 10 frame delay on click attack
                Timer *= -1;

            if (TotalChargesMade == Charges + 1)
                Projectile.Kill();

            Projectile.Center = getOffset();

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = 0;

            /*
            if (player.direction == 1)
            {
                Projectile.rotation = (position - player.Center).ToRotation() + (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + (float)Math.PI * 3 / 4f;
            }
            else 
            {
                Projectile.rotation = (position - player.Center).ToRotation() + (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + (float)MathHelper.PiOver4;
            }
            */
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            //Main.spriteBatch.End();

            //var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/idktrail2").Value;
            //var TrailTex2 = ModContent.Request<Texture2D>("DivergencyMod/Trails/idktrail2").Value;

            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);



            //Main.spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            /*
            
            if (chargereleaseIncrease)
            {
                drawposition += 5;
                backtimer++;
            }
            if (backtimer >= 8 && chargereleaseIncrease)
            {
                drawposition -= 10;

            }

            */

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


            // Main.instance.LoadProjectile(Projectile.type);


            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            //Main.spriteBatch.End();

            //Main.spriteBatch.Begin();

            return false;

        }


    }



}