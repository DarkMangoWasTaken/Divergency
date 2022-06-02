using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Doorlauncher

{
    public class Smoke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BewitchedSpikyBall");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.width = 0;
            Projectile.height = 0;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.alpha += 5;
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}