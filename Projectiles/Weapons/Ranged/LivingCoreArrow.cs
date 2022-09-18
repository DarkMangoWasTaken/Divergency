using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace DivergencyMod.Projectiles.Weapons.Ranged
{
    internal class LivingCoreArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = Terraria.ID.ProjAIStyleID.Arrow;
            Projectile.damage = 10;
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 0)
            {
                Projectile.ai[0] -= 1;
                if (Projectile.ai[0] == -91)
                {
                    Projectile.Kill();
                    Terraria.Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -6f), ModContent.ProjectileType<LivingCoreTip>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override bool PreAI()
        {
            if (!(Projectile.ai[0] >= 0))
                AI();

            return Projectile.ai[0] >= 0;
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
