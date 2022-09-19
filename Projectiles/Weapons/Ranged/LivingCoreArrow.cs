using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using System;

namespace DivergencyMod.Projectiles.Weapons.Ranged
{
    internal class LivingCoreArrow : ModProjectile
    {
        private static float size = 40f;
        private static float maxRotateVal = 0.1f;

        public override void SetDefaults()
        {
            Projectile.aiStyle = Terraria.ID.ProjAIStyleID.Arrow;
            Projectile.damage = 10;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.timeLeft = 240;

            Projectile.maxAI = 3;

            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
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
                            Projectile.ai[0] = 16;
                            Projectile.ai[1] = 1;
                            Projectile.ai[2] = 6;

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

                    Mod.Logger.Info(Projectile.rotation);

                    Projectile.position -= (Projectile.rotation - MathF.PI / 2).ToRotationVector2() * 20f; // size

                    Projectile.rotation += Projectile.ai[0]/4;

                    Projectile.position += (Projectile.rotation - MathF.PI / 2).ToRotationVector2() * 20f; // size

                    if (Projectile.ai[0] == 0)
                    {
                        Projectile.ai[1] = 2;

                        NPC closestNPC = FindClosestNPC(2000f);

                        float rot = 0;
                        if (closestNPC != null)
                            rot = (closestNPC.position - Projectile.position).ToRotation();

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
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center); // maby proritise boss * (target.boss ? 1 : 10);

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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = -1;

            Projectile.position += oldVelocity;
            Projectile.velocity = new Vector2(0, 0);
            Projectile.tileCollide = false;

            return false;
        }
    }
}
