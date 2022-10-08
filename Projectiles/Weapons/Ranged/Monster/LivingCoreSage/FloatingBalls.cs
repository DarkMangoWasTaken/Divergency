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
    internal class FloatingBalls : ModProjectile
    {
        private int Counter { get { return (int)Projectile.ai[1]; } set { Projectile.ai[1] = value; } }
        private bool FistPhase { get { return (int)Projectile.ai[1] > 0; } }


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
            Player plr = Main.CurrentPlayer; //should be the owner entity;

            if (Counter == 0)
            { // first itteration of ai
                int dir = Main.rand.Next(2)*2-1;

                Projectile.position = plr.Center + new Vector2(10f, 0f)* dir;
                Projectile.velocity = new Vector2(dir*8f, (Main.rand.NextFloat()*2-1)*12f);
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

        private void FirstPhaseAI()
        {
            Player plr = Main.CurrentPlayer; //should be the owner entity;

            float speed = Projectile.velocity.Length();

            Vector2 flaotPoint = plr.Center + new Vector2(0f, -160f);
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
                Projectile.ai[2] = plr.Center.X - Projectile.Center.X;
                Projectile.velocity = new Vector2(0, 16f);
            }
        }

        private void SecondPhaseAI()
        {
            if (Projectile.ai[0] != -1)
            {
                Player plr = Main.CurrentPlayer; //should be the owner entity;
                Vector2 targetPosition = Main.npc[plr.MinionAttackTargetNPC].Center; // should be player
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
            }
        }
    }
}
