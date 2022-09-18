using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Projectiles.Weapons.Ranged
{
    internal class LivingCoreTip : ModProjectile
    {
		private static int goingUpFor = 30;

        public override void SetDefaults()
        {
            Projectile.damage = 1;
			Projectile.width = 14;
			Projectile.height = 32;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.aiStyle = -1;

			Projectile.timeLeft = 600;


			Projectile.ai[0] = 0; // pull power
        }

        public override void AI()
		{
			NPC closestNPC = FindClosestNPC(2000f);
			if (closestNPC == null)
			{
				Projectile.Kill(); // some sort of particle explotion
				return;
			}

			if (Projectile.ai[0] < goingUpFor)
				Projectile.ai[0] += 1;
            else
			{
				Projectile.ai[0]++;

				Projectile.velocity *= 1.01f;

				Vector2 addVel = closestNPC.position - Projectile.position;
				addVel.Normalize();

				float power = Projectile.velocity.Length();

				Projectile.velocity += addVel * (Projectile.ai[0]- goingUpFor) / 10;
				Projectile.velocity.Normalize();

				Projectile.velocity *= power;
			}
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
	}
}
