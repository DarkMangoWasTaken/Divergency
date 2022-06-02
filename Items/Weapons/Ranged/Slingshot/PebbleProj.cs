using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Slingshot
{
    public class PebbleProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("deathr");
            Main.projFrames[Projectile.type] = 1;
            //The recording mode
        }

        public override string Texture => "DivergencyMod/Items/Ammo/Pebble";

        private int ProjectileSpawnedCount;
        private int ProjectileSpawnedMax;
        private Projectile ParentProjectile;
        private bool RunOnce = true;
        private bool MouseLeftBool = false;

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.scale = 0.7f;

            Projectile.ownerHitCheck = true;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Timer++;
            if (Timer > 100)
            {
                // Our timer has finished, do something here:
                // Main.PlaySound, Dust.NewDust, Projectile.NewProjectile, etc. Up to you.
            }
            if (Timer >= 30)
            {
                Projectile.velocity.Y += 0.90f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
}