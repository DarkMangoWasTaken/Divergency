using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Melee.RootBreaker
{
    public class LivingStone : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.StoneBlock;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 16;
            Projectile.scale = 1f;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.Length() * 0.1f) * Projectile.direction;
            Projectile.velocity.Y += 0.5f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.LocalPlayer;

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 3;

            for (int j = 0; j < 10; j++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                Dust.NewDustPerfect(Projectile.Center, DustID.Stone, speed * 2, 0, default, 1f);
            }
        }
    }
}