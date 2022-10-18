using System;
using System.Collections.Generic;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.Dusts.Particles.CorePuzzleParticles;
using DivergencyMod.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
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
        private int Counter { get { return (int)Projectile.ai[2]; } set { Projectile.ai[2] = value; } }
        private int Direction { get { return (int)Projectile.ai[1] % 4;} }
        private Vector2[] Directions = new Vector2[] {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
        };

        public static int maxAI = 3;

        private int Delay = 60;
        private float StartDistance = 300f;
        private float Speed = 18f;
        private int Timer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BALLS HAHA SEX PENIS HAHA HA SEX");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25; // in SetStaticDefaults()
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
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
        }

        public override void AI()
        {
            Timer++;
            if (Timer == 1)
            {
                ParticleManager.NewParticle(Projectile.Center, Projectile.velocity * 3, ParticleManager.NewInstance<PodestProjBase>(), Color.Purple, 1f, Projectile.whoAmI, Projectile.whoAmI);

            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.spriteDirection = Projectile.direction;
            Player owner  = Main.player[(int)Projectile.ai[0]];

            if (Counter == Delay)
            {
                Counter = -1;
                Projectile.velocity = Directions[Direction] * 14f;

                for (int i = 0; i < 10; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    ParticleManager.NewParticle(Projectile.Center, speed * 10 , ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 0.65f, Projectile.whoAmI, Projectile.whoAmI);

                }
            }
            else if (Counter >= 0)
            {
                Counter++;

                Vector2 dir = Directions[Direction];
                Projectile.rotation = dir.ToRotation()+MathHelper.PiOver2;
                Projectile.Center = owner .Center + -dir * StartDistance;

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
        public TrailRenderer prim;
        public TrailRenderer prim2;
        public override bool PreDraw(ref Color lightColor)
        {
            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/Trail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(30f) * (1f - p), (p) => Projectile.GetAlpha(Color.LimeGreen) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = Projectile.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(15f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
            prim2.Draw(Projectile.oldPos);


            return false;
        }
    }
}
