using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Projectiles.Weapons.Ranged.Monster.LivingCoreSage
{
    internal class DirectionalAttack : ModProjectile
    {
        private int Counter { get { return (int)Projectile.ai[1]; } set { Projectile.ai[1] = value; } }
        private int Direction { get { return (int)Projectile.ai[2] % 4;} }
        private Vector2[] Directions = new Vector2[] {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
        };

        private int Delay = 60;
        private float StartDistance = 140f;
        private float Speed = 18f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 35;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.maxAI = 3;
        }

        public override void AI()
        {
            Player plr = Main.CurrentPlayer; //Main.player[(int)Projectile.ai[0]];

            if (Counter == Delay)
            {
                Counter = -1;
                Projectile.velocity = Directions[Direction] * 18f;

                // loads of particles (this is release)
            }
            else if (Counter >= 0)
            {
                Counter++;

                Vector2 dir = Directions[Direction];
                Projectile.rotation = dir.ToRotation()+MathHelper.PiOver2;
                Projectile.Center = plr.Center + -dir * StartDistance;

                // this is stand still
            }
            else if (Counter < 0)
            {
                Counter--;
                if (Counter < -120)
                    Projectile.Kill();

                // this is stand released
            }
        }
    }
}
