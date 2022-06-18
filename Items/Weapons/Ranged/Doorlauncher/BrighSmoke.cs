using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Doorlauncher
{
    public class BrightSmoke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BewitchedSpikyBall");
            Main.projFrames[Projectile.type] = 4;
        }

        public bool MoveBack = false;

        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 595;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
        }

        public float MovementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
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