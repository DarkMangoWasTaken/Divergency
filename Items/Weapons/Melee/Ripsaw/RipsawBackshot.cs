using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Melee.Ripsaw
{
    public class RipsawBackshot : ModProjectile
    {
        public override string Texture => "DivergencyMod/Blank";
        
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 12;
            Projectile.scale = 1f;
            Projectile.hide = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = 0;
            Projectile.timeLeft = 12;
            Projectile.extraUpdates = 5;
        }
    }
}
