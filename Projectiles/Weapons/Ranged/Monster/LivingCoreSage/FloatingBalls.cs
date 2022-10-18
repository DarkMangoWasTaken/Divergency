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
    internal class FloatingBalls : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BALLS HAHA SEX PENIS HAHA HA SEX");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25; // in SetStaticDefaults()
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        private int Counter { get { return (int)Projectile.ai[1]; } set { Projectile.ai[1] = value; } }
        private bool FistPhase { get { return (int)Projectile.ai[1] >= 0; } }
        private int Timer;

  
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
            Projectile.maxAI = 5;
        }

        public override void AI()
        {
            Timer++;
            if (Timer == 1)
            {
                ParticleManager.NewParticle(Projectile.Center, Projectile.velocity * 3, ParticleManager.NewInstance<PodestProjBase>(), Color.Purple, 0.7f, Projectile.whoAmI, Projectile.whoAmI);

            }
            NPC owner = Main.npc[(int)Projectile.ai[4]];

            if (Counter == 0)
            { // first itteration of ai
                int dir = Main.rand.Next(2)*2-1;

                Projectile.position = owner.Center + new Vector2(10f, 0f)* dir;
                Projectile.velocity = new Vector2(dir*6f, (Main.rand.NextFloat()*2-1)*9f);
                Projectile.velocity.Normalize();
                Projectile.velocity *= 8f;
            }

            if (Counter >= 0)
                Counter++;
            else
                Counter--;

            if (FistPhase)
                FirstPhaseAI();
            else
                SecondPhaseAI();

        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 14; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 10, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 1.2f, Projectile.whoAmI, Projectile.whoAmI);

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
        private void FirstPhaseAI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[4]];

            float speed = Projectile.velocity.Length();

            Vector2 flaotPoint = owner.Center + new Vector2(0f, -160f);
            Vector2 floatPointDiff = flaotPoint - Projectile.Center;

            // apply slow at center
            if ((floatPointDiff).Length() < 120f && Main.rand.NextBool())
                speed *= 0.83f;

            if (speed < 0.02f)
                speed = 0f;


            floatPointDiff.Normalize();
            Projectile.velocity += floatPointDiff;
            Projectile.velocity.Normalize();
            Projectile.velocity *= speed;

            if (speed == 0)
            {
                Counter = -1;
                Projectile.ai[2] = owner .Center.X - Projectile.Center.X;
                Projectile.velocity = new Vector2(0, 10f);
            }
        }

        private void SecondPhaseAI()
        {
            if (Projectile.ai[0] != -1)
            {
                NPC owner = Main.npc[(int)Projectile.ai[4]];
                Vector2 targetPosition = Main.player[(int)Projectile.ai[3]].Center; // should be player
                Vector2 vecForRot = targetPosition - Projectile.Center;

                float rot = vecForRot.ToRotation() + MathHelper.PiOver2;
                targetPosition += new Vector2(MathF.Cos(rot) * Projectile.ai[2], MathF.Sin(rot) * Projectile.ai[2]);

                float speed = Projectile.velocity.Length();
                Vector2 targetPointDiff = targetPosition - Projectile.Center;

                if (Counter == -17)
                {
                    Projectile.ai[0] = -1;
                    Counter = -1;
                }

                targetPointDiff.Normalize();
                Projectile.velocity += targetPointDiff * 5f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= speed;
            }
            else
            {
                if (Counter == -120)
                {
                    Projectile.Kill();
                }
                if (Counter == -30)
                {
                    Projectile.tileCollide = true;
                }
            }
        }   
    }

}
