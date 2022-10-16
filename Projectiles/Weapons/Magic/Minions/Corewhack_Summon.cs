using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Projectiles.Weapons.Magic.Minions
{
    public class Corewhack_Summon : ModProjectile
    {
        private bool CanShoot
        {
            get { return Projectile.ai[0] == 0; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0.5f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] > 0)
                Projectile.ai[0]--;

            Projectile.timeLeft = 100;
            Player owner  = Main.player[Projectile.owner];

            if (!owner .HasBuff<Buffs.CorewhackBuff>())
            {
                Projectile.Kill();
            }

            int target = owner .MinionAttackTargetNPC;
            Vector2 offest = Vector2.Zero;
            if (target == -1)
                offest = owner .Center;
            else
                offest = Main.npc[target].Center;

            int totalMinions = owner .ownedProjectileCounts[Projectile.type];

            int minionPos = owner .numMinions; // Projectile.minionPos + 1

            float minionOffset = (float)minionPos / (float)totalMinions * MathF.PI*2 + (float)Main.time/100f;

            Projectile.rotation = minionOffset;

            Projectile.Center = offest + minionOffset.ToRotationVector2()*(60f+(4f*totalMinions));

            Vector2 vel = -Projectile.rotation.ToRotationVector2() * 10f;

            if (target != -1 && CanShoot && Collision.CanHit(Projectile, Main.npc[target]))
            {

                // shoot target

                Projectile.ai[0] = 30;

                Terraria.Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, Terraria.ID.ProjectileID.CrystalBullet, Projectile.damage, 0f, Projectile.owner);
            }
        }
    }
    /*
    public class Corewhack_Summon_Shot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }
    }*/
}