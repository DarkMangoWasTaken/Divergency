using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Doorlauncher
{
    public class FrickinDoorShield : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("deathr");
            Main.projFrames[Projectile.type] = 1;
            //The recording mode
        }

        private bool Collided = false;
        private float Durability = 10f;

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 5;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1200;

            Projectile.ownerHitCheck = true;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.whoAmI != Projectile.whoAmI && proj.active && Projectile.Hitbox.Intersects(proj.Hitbox) && !proj.friendly)
                {
                    proj.Kill();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                        return;
                    }
                    int backGoreType = Mod.Find<ModGore>("FrickinDoor_Back").Type;
                    int frontGoreType = Mod.Find<ModGore>("FrickinDoor_Front").Type;

                    for (int o = 0; o < 2; o++)
                    {
                        Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.Next(0, 0), Main.rand.Next(0, 0)), backGoreType);
                        Durability--;
                        if (Durability == 0f)
                        {
                            Projectile.Kill();
                            Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
                            Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.Next(0, 0), Main.rand.Next(0, 0)), backGoreType);
                            Durability = 10f;
                        }
                    }
                }
            }

            Timer++;
            if (Timer > 100)
            {
                // Our timer has finished, do something here:
                // Main.PlaySound, Dust.NewDust, Projectile.NewProjectile, etc. Up to you.
            }

            Projectile.velocity.Y += 0.95f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player Player = Main.player[Projectile.owner];
            Projectile.Kill();
            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 5;
            // If the NPC dies, spawn gore and play a sound
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player Player = Main.player[Projectile.owner];
            if (!Collided)
            {
                Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 10;
            }
            Collided = true;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            int backGoreType = Mod.Find<ModGore>("FrickinDoor_Back").Type;
            int frontGoreType = Mod.Find<ModGore>("FrickinDoor_Front").Type;

            for (int i = 0; i < 2; i++)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
            }
        }
    }
}