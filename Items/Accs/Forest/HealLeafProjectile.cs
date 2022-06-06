using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System;

namespace DivergencyMod.Items.Accs.Forest
{
    public class HealLeafProjectile : ModProjectile
    {
        public int timer = 1;

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
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.velocity.Y = 2f;          //change this to increase the fall speed of the projectile
        }

        public override void AI()
        {
            base.AI();

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }



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
