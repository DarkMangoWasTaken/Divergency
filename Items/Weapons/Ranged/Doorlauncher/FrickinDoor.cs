using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Doorlauncher
{
    public class FrickinDoor : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("deathr");
            Main.projFrames[Projectile.type] = 1;
            //The recording mode
        }

        private int ProjectileSpawnedCount;
        private int ProjectileSpawnedMax;
        private Projectile ParentProjectile;
        private bool RunOnce = true;
        private bool MouseLeftBool = false;

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.ownerHitCheck = true;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            float Rotation = Projectile.velocity.ToRotation();
            Projectile.rotation += Rotation + MathHelper.ToRadians(90f);

            Timer++;
            if (Timer > 100)
            {
                // Our timer has finished, do something here:
                // Main.PlaySound, Dust.NewDust, Projectile.NewProjectile, etc. Up to you.
            }
            if (Timer >= 15)
            {
                Projectile.velocity.Y += 0.95f;
            }
            if (Timer >= 15)
            {
                Projectile.rotation += (Rotation + MathHelper.ToRadians(90f)) / 10f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player Player = Main.player[Projectile.owner];
            Projectile.Kill();
            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 5;
            // If the NPC dies, spawn gore and play a sound
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player Player = Main.player[Projectile.owner];
            Projectile.Kill();
            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 5;
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
            }
            int backGoreType = Mod.Find<ModGore>("FrickinDoor_Back").Type;
            int frontGoreType = Mod.Find<ModGore>("FrickinDoor_Front").Type;

            for (int i = 0; i < 2; i++)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
            }
            return false;
        }
    }
}