using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using System;
using ParticleLibrary;
using DivergencyMod.Dusts.Particles;
using Terraria.ID;
using DivergencyMod.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace DivergencyMod.Projectiles.Weapons.Ranged
{
    internal class LivingCoreArrow : ModProjectile
    {
        private static float size = 40f;
        private static float maxRotateVal = 0.1f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Blast");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = Terraria.ID.ProjAIStyleID.Arrow;
            Projectile.damage = 10;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.arrow = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 2;


            Projectile.timeLeft = 240;

            Projectile.maxAI = 3;

            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 dir = Main.rand.NextVector2Unit() * 0.1f;
            

                  ParticleManager.NewParticle(Projectile.Center, dir * 60, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 0.9f);


                    
                }
            }

            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(Projectile.position, Projectile.position - (Projectile.velocity.SafeNormalize(Vector2.Zero) * 24f), 0, DelegateMethods.CastLight);

            if (Projectile.ai[1] == 0)
            {
                if (Projectile.ai[0] < 0)
                {
                    Projectile.ai[0] -= 1;
                    if (Projectile.ai[0] < -20)
                    {
                        Projectile.ai[0]++;
                        //Projectile.Kill();
                        //Terraria.Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -20f), ModContent.ProjectileType<LivingCoreTip>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai1: 20);

                        Projectile.position -= (Projectile.rotation - MathF.PI / 2).ToRotationVector2() * 60f; // size

                        float rotate = -(Projectile.rotation > MathF.PI ? -(Projectile.rotation - MathF.PI) : Projectile.rotation);

                        if (MathF.Abs(rotate) > 0.2f) // maxRotateVal
                            rotate = rotate / MathF.Abs(rotate) * 0.2f; // maxRotateVal

                        Projectile.rotation += rotate;

                        Projectile.rotation = Projectile.rotation > MathF.PI * 2f ? 0 : Projectile.rotation;

                        Projectile.position += (Projectile.rotation - MathF.PI / 2).ToRotationVector2() * 60f; // size

                        if (Projectile.rotation == 0)
                        {
                            Projectile.ai[0] = 300;
                            Projectile.ai[1] = 1;
                            Projectile.ai[2] = 12;

                            Projectile.tileCollide = true; // might not want this, idk
                        }
                    }
                }
            }
            else if (Projectile.ai[1] == 1)
            {
                if (Projectile.ai[2] == 0)
                {
                    Projectile.ai[0]--;

                    Projectile.position -= (Projectile.rotation - MathF.PI / 2).ToRotationVector2() * 24f; // size

                    Projectile.rotation += 1; //Projectile.ai[0]/4;

                    Projectile.position += (Projectile.rotation - MathF.PI / 2).ToRotationVector2() * 24f; // size

                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 dir = Main.rand.NextVector2Unit() * Main.rand.NextFloat();
                    }

                    if (Projectile.ai[0] == 0)
                    {
                        Projectile.ai[1] = 2;

                        NPC closestNPC = FindClosestNPC(2000f);

                        float rot = 0;
                        if (closestNPC != null)
                            rot = (closestNPC.position - Projectile.position).ToRotation();
                        else
                            rot = Main.rand.NextFloat() * MathF.PI*2;

                        Projectile.rotation = rot;

                        Projectile.velocity = Projectile.rotation.ToRotationVector2() * 20f;

                        Projectile.rotation += MathF.PI / 2;
                    }
                }
                else
                    Projectile.ai[2]--;
            }
            //else if (Projectile.ai[1] == 2)


        }

        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];

                if (target.CanBeChasedBy())
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center) * (target.boss ? 0.1f : 1);

                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }

        public override bool PreAI()
        {
            if (Projectile.ai[1] == 0)
            {
                if (!(Projectile.ai[0] >= 0))
                    AI();
                else
                    Projectile.velocity.Y += 0.2f; // drags it down, remove to make it work like normal arrow

                return Projectile.ai[0] >= 0;
            }

            AI();
            return false;
        }
        public TrailRenderer prim;
        public TrailRenderer prim2;
        public override bool PreDraw(ref Color lightColor)
        {
            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/MotionTrail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(10f) * (1f - p), (p) => Projectile.GetAlpha(Color.LimeGreen) * 1f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = Projectile.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(5f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 1f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
            prim2.Draw(Projectile.oldPos);


            return true;
        }
    


    public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 dir = (-oldVelocity).RotatedBy(Main.rand.NextFloat()* MathF.PI - MathF.PI/2);
                dir.Normalize();
                dir *= 0.3f;
                ParticleManager.NewParticle(Projectile.Center, dir * 60, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 0.9f);
            }

            if (Projectile.ai[1] != 0)
                return true;

            Projectile.ai[0] = -1;

            Projectile.position += oldVelocity;
            Projectile.velocity = new Vector2(0, 0);
            Projectile.tileCollide = false;

            return false;
        }
    }


}
