using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System;
using ParticleLibrary;
using DivergencyMod.Dusts.Particles;

namespace DivergencyMod.Items.Accs.Forest
{
    public class HealLeafProjectile : ModProjectile
    {
        public int timer = 1;
        public int setup = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()  //currently a damaging projectile
        {
            Projectile.width = 10;  
            Projectile.height = 10;

            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.ownerHitCheck = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 6000;
            Projectile.scale = 1.1f;
        }
        
    

        public override void AI()
        {
            ParticleManager.NewParticle(Projectile.Center, Projectile.velocity * 1, ParticleManager.NewInstance<LeafTrail>(), Color.Purple, 1);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
              

            Player player = Main.player[Projectile.owner];
         
            if (Projectile.active && player.Hitbox.Intersects(Projectile.Hitbox))
            {
                player.Heal(2);
                player.HealEffect(2);
                Projectile.Kill();
            }
                Projectile.damage = 0;

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }


            setup++;
    
            if (setup == 45)      //change this to increase the time it flies upwards
            {
                Projectile.velocity = Vector2.Zero;//change this to increase the speed of the projectile falling downwards
                Projectile.velocity.Y = 2;
            }
            else if (setup > 45)     //must be the same number as the one above
            {
              

                if (timer < 100)
                {
                    if (timer == 0)
                    {
                        Projectile.velocity = Projectile.velocity.RotatedBy(2);
                        timer++;
                    }

                    Projectile.velocity = Projectile.velocity.RotatedBy(0.02);
                    timer++;
                }
                else if (timer < 200)
                {
                    if (timer == 100)
                    {
                        Projectile.velocity = Projectile.velocity.RotatedBy(-2);
                        timer++;
                    }

                    Projectile.velocity = Projectile.velocity.RotatedBy(-0.02);
                    timer++;
                }
                else if (timer == 200)
                {
                    timer = 0;
                }
            }
        }
    }
}
