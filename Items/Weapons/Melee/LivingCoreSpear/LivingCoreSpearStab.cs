using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using DivergencyMod.Items.Weapons.Magic.Invoker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Utils;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using DivergencyMod.Base;

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

            private float MovementFactor = 40;
            private float ParticleTimer;
            private bool ProjSpawned = false;
            public int charge;

        public float AttackTimer = 0;
        public int CurrentAttack;
        public bool ProjectileGetBack;
        private bool initializeMouse;
        private Vector2 CurrentMouseWorld;
        private bool initialize = false;
        private bool chargereleaseIncrease;
        public float holdOffset = 1;

        public int Overcharge;
        public override void SetDefaults()
            {
                Projectile.damage = 18;
                Projectile.width = 40;
                Projectile.height = 40;
                Projectile.aiStyle = -1;
                Projectile.friendly = true;
                Projectile.DamageType = DamageClass.Melee;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.scale = 1f;
                Projectile.penetrate = -1;
                Projectile.ownerHitCheck = true;
            
            }

            public float Timer
            {
                get => Projectile.ai[0];
                set => Projectile.ai[0] = value;
            }

        public float swordRotation;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (initialize)
            {
               Main.NewText(DoingAttack);

                {
                    if (Main.mouseLeft && !DoingAttack && Overcharge < 3)
                    {
                        charge++;

                    }
                }
                Vector2 position = player.RotatedRelativePoint(player.MountedCenter);

                if (Main.mouseLeft && !DoingAttack && Overcharge < 3)
                {
                    charge++;

                }
                //handles the attack switching
                if (charge == 60 && !DoingAttack && Overcharge < 3)
                {
                    Overcharge++;
                    charge = 0;

                }

                if (Main.mouseLeftRelease && !Main.mouseLeft)
                {
                    DoingAttack = true;
                }
                if (DoingAttack)
                {
                    Timer++;


                    if (Timer == 30)
                    {
                        Overcharge--;
                        Timer = 0;
                    }
                    if (Overcharge == -1)
                    {
                        Projectile.Kill();
                    }
                    if (Overcharge == 0)
                    {
                        Projectile.GetSpearAttack(0,0,0,0,1);
                    }
                    if (Overcharge == 1)
                    {
                  
                        Projectile.GetSpearAttack(0, 0, 0, 0,2);

                    }
                    if (Overcharge == 2 )
                    {
                        Projectile.GetSpearAttack(0, 0, 0, 0,3);

                    }
                    if (Overcharge == 3)
                    {
                        Projectile.GetSpearAttack(0, 0, 0, 0,4);


                    }
                }
             

               
                if (player.noItems || player.CCed || player.dead || !player.active)
                    Projectile.Kill();
                //if (!player.channel)
                //{
                //    Projectile.Kill();
                //}

                Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, true);

                if (Main.myPlayer == Projectile.owner)
                {
                    player.ChangeDir(Projectile.direction);
                }
                Projectile.velocity = swordRotation.ToRotationVector2();

                Projectile.spriteDirection = player.direction;

               // Projectile.Center = position + Projectile.velocity * MovementFactor;

                position += new Vector2(holdOffset,0);

                player.heldProj = Projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);

                if (player.direction == 1)
                {
                    Projectile.rotation = (position - player.Center).ToRotation() + (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + (float)Math.PI * 3 / 4f;

                }
                else 
                {

                    Projectile.rotation = (position - player.Center).ToRotation() + (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + (float)MathHelper.PiOver4;

                }

            }
            else
            {
                if (player.direction == 1)
                {
                    swordRotation = 0;
                    initialize = true;
                }
                else
                {
                    swordRotation = (float)Math.PI;
                    initialize = true;
                }
            }
            
        }

            public int drawposition = 13;
        public TrailRenderer SwordSlash;
        public TrailRenderer SwordSlash2;
        public TrailRenderer SwordSlash3;
        private int backtimer;
        private byte chargeReleaseMax;
        private bool DoingAttack;

        public override bool PreDraw(ref Color lightColor)
        {

            Player player = Main.player[Projectile.owner];

            Main.spriteBatch.End();




            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/idktrail2").Value;
            var TrailTex2 = ModContent.Request<Texture2D>("DivergencyMod/Trails/idktrail2").Value;

            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);


            if (SwordSlash == null)
            {
                SwordSlash = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(4f), (p) => new Color(100, 255, 100, 10) * (1f - p));
                SwordSlash.drawOffset = Projectile.Size / 1.1f;
            }
            if (SwordSlash2 == null)
            {
                SwordSlash2 = new TrailRenderer(TrailTex2, TrailRenderer.DefaultPass, (p) => new Vector2(2f), (p) => new Color(10, 150, 50, 50) * (1f - p));
                SwordSlash2.drawOffset = Projectile.Size / 1.1f;

            }
          
            

            Main.spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

          
                //SwordSlash.Draw(Projectile.oldPos);
                //SwordSlash2.Draw(Projectile.oldPos);

                //SwordSlash3.Draw(Projectile.oldPos);


            
            if (chargereleaseIncrease)
            {
                drawposition += 5;
                backtimer++;
            }
            if (backtimer >= 8 && chargereleaseIncrease)
            {
                drawposition -= 10;

            }

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            var ChargeTexture = ModContent.Request<Texture2D>("DivergencyMod/Items/Weapons/Melee/LivingCoreSpear/LivingCoreSpearCharged").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);


          
            //chargetex
            if (charge == 55 || charge == 56 || charge == 57 || charge == 58 || charge == 59 || charge == 60)
            {
                Main.EntitySpriteDraw(ChargeTexture,
           player.Center - Main.screenPosition + new Vector2(MovementFactor - 20 , Projectile.gfxOffY),
           sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0); // drawing the sword itself
                Main.instance.LoadProjectile(Projectile.type);
            }
            else
            {
                Main.EntitySpriteDraw(texture,
             player.Center - Main.screenPosition + new Vector2(MovementFactor - 20, Projectile.gfxOffY),
             sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0); // drawing the sword itself
                Main.instance.LoadProjectile(Projectile.type);
            }
        

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.spriteBatch.End();

            Main.spriteBatch.Begin();

            return false;

        }

       
    }


    
}