using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Slingshot
{
    public class PebbleAim : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BewitchedSpikyBall");
            Main.projFrames[Projectile.type] = 5;
        }

        public bool targeted = false;
        public int lastSelectedItem;

        public override void SetDefaults()
        {
            Projectile.damage = 1;
            Projectile.width = 20;
            Projectile.height = 20;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
        }

        public float MovementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private bool aimed = false;
        private NPC victim;
        private Vector2 AimPos;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            targeted = true;
            if (target.active && !aimed)
            {
                aimed = true;
                victim = target;
            }
            if (targeted)
            {
                victim.defense -= 5;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.HeldItem.type != ModContent.ItemType<Slingshot>())
            {
                victim.defense += 5;
                Projectile.Kill();
            }

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100000;
            if (aimed)
            {
                if (aimed)
                {
                    AimPos = Projectile.position;
                }

                Projectile.Center = victim.Center;
                Projectile.velocity.Y = 0f;
                Projectile.velocity.X = 0f;
                Projectile.damage = 0;
                Projectile.knockBack = 0;
                if (!victim.active)
                {
                    Projectile.Kill();
                }
                if (Projectile.timeLeft == 1)
                {
                    victim.defense += 5;
                }
            }
            else
            {
                Projectile.position = Main.MouseWorld;
                if (Main.mouseRight && !targeted)
                {
                    Projectile.Kill();
                }
            }
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }
    }
}