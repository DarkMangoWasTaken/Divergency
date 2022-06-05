using DivergencyMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Accs

{
    public class AuraProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Aura");
            Main.projFrames[Projectile.type] = 7;
        }

        private float MovementFactor = 0.1f;

        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 100;
            Projectile.height = 100;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        
        public override void AI()
        {

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;

            Player player = Main.player[Projectile.owner];
            if (player.CCed || player.dead || !player.active)
                Projectile.Kill();

            player.GetModPlayer<Aura>().increasedLifeRegen = 5;
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            float swordRotation = 0f;
            if (Main.myPlayer == Projectile.owner)
            {
                swordRotation = (Main.MouseWorld - player.Center).ToRotation();
            }
            Projectile.velocity = swordRotation.ToRotationVector2();

            Projectile.Center = playerCenter + Projectile.velocity * MovementFactor;// customization of the hitbox position

            Projectile.Center = playerCenter;// customization of the hitbox position

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 7)
                {
                    Projectile.frame = 0;
                    
                }
            }
         
            
     
                   
                    
                
         
            

            for (int i = 0; i < 5; i++)
            {
                if (Projectile.frame == 1)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                    Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Center, ModContent.DustType<FrickinFlameDust>(), speed * 2, Scale: 1f);
                    d.noGravity = true;
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