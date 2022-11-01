using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using IL.Terraria.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static ParticleLibrary.Particle;

namespace DivergencyMod.Items.Weapons.Melee.LivingCoreSword

{
    public class LivingCoreSwordProj : ModProjectile
    {
        public static bool swung = false;
        public int SwingTime = 80;
        public float holdOffset = 60f;
        public bool _initialized;
        public override string Texture => "DivergencyMod/Items/Weapons/Melee/LivingCoreSword/LivingCoreSword";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Sword");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 100;

            
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 74;
            Projectile.width = 74;
            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;

        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public virtual float Lerp(float val)
        {
            return val == 1f ? 1f : (val == 1f ? 1f : (float)Math.Pow(2, val * 10f - 10f) / 2f);
        }
      
      

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!_initialized && Main.myPlayer == Projectile.owner)
            {
                SwingTime = (int)(30 / player.GetAttackSpeed(DamageClass.Melee));
                Projectile.alpha = 255;
                Projectile.timeLeft = SwingTime;
                _initialized = true;
                Projectile.netUpdate = true;

            }
            else if (_initialized)
            {
                Projectile.alpha = 0;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10000;
                Timer++;
                if (!player.active || player.dead || player.CCed || player.noItems)
                {
                    return;
                }

                if (Projectile.timeLeft >= 15 && Projectile.timeLeft <= 27)
                for (int i = 0; i < 3; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Unit() * 0.1f;
                    ParticleManager.NewParticle(Projectile.Center + new Vector2(Main.rand.NextFloat(10,-10)), speed * 10, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 1f, Projectile.whoAmI, Layer: Layer.BeforeProjectiles);
                }
                int dir = (int)Projectile.ai[1];
                float swingProgress = Lerp(Utils.GetLerpValue(SwingTime, 0f, Projectile.timeLeft));
                // the actual rotation it should have
                float defRot = Projectile.velocity.ToRotation();
                // starting rotation
                float endSet = ((MathHelper.PiOver2) / 0.2f);
                float start = defRot + endSet;

                // ending rotation
                float end = defRot - endSet;
                // current rotation obv
                float rotation = dir == 1 ? end.AngleLerp(start, swingProgress) : end.AngleLerp(start, 1f - swingProgress);
                // offsetted cuz sword sprite
                Vector2 position = player.RotatedRelativePoint(player.MountedCenter);
                position += rotation.ToRotationVector2() * holdOffset;
                Projectile.Center = position;
                Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;

                player.heldProj = Projectile.whoAmI;
                player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                player.itemRotation = rotation * player.direction;
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.netUpdate = true;
            }
        }
        public override bool ShouldUpdatePosition() => false;


      
        public TrailRenderer SwordSlash;
        public TrailRenderer SwordSlash2;
        public TrailRenderer SwordSlash3;

        public override bool PreDraw(ref Color lightColor)
        {
          

            Main.spriteBatch.End();



          
            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/MotionTrail").Value;
            var TrailTex2 = ModContent.Request<Texture2D>("DivergencyMod/Trails/idktrail").Value;
            var TrailTex3 = ModContent.Request<Texture2D>("DivergencyMod/Trails/gravytrail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
           

            if (SwordSlash == null)
            {
                SwordSlash = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(14f), (p) => new Color(100, 255, 100, 10) * (1f - p));
                SwordSlash.drawOffset = Projectile.Size / 1.9f;
            }
            if (SwordSlash2 == null)
            {
                SwordSlash2 = new TrailRenderer(TrailTex2, TrailRenderer.DefaultPass, (p) => new Vector2(35f), (p) => new Color(10, 150, 50, 50) * (1f - p));
                SwordSlash2.drawOffset = Projectile.Size / 1.9f;

            }
            if (SwordSlash3 == null)
            {
                SwordSlash3 = new TrailRenderer(TrailTex3, TrailRenderer.DefaultPass, (p) => new Vector2(20f), (p) => new Color(10, 255, 50, 40) * (1f - p));
                SwordSlash3.drawOffset = Projectile.Size / 2f;

            }

            Main.spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
          
            if (_initialized && Projectile.timeLeft <= 58)
            {
                SwordSlash.Draw(Projectile.oldPos);
                SwordSlash2.Draw(Projectile.oldPos);
               
                //SwordSlash3.Draw(Projectile.oldPos);


            }


            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);
          

            Main.EntitySpriteDraw(texture,
               Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
               sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0); // drawing the sword itself
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
         
            Main.spriteBatch.End();

            Main.spriteBatch.Begin();

            return false;

        }
        
    }
}