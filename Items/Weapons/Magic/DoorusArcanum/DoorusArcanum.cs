using DivergencyMod.Dusts.Particles;
using DivergencyMod.Items.Weapons.Magic.Invoker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Magic.DoorusArcanum
{
    public class RandomBullshit : ModProjectile
    {
        public bool ParticleSpawned = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invoked Projectile");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 100;
            Projectile.scale = 1f;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 5, ParticleManager.NewInstance<InvokedParticle4>(), Color.Purple, 0.1f);
            }
        }

        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.Length() * 0.04f) * Projectile.direction;

            DrawOriginOffsetX = 35;
            Timer++;
            Vector3 RGB = new Vector3(0.46f, 0.26f, 0.71f);
            float multiplier = 1.2f;
            float max = 1f;
            float min = 0.5f;
            RGB *= multiplier;
            if (RGB.X > max)
            {
                multiplier = 0.5f;
            }
            if (RGB.X < min)
            {
                multiplier = 1.5f;
            }
            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;

            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
  

            Player player = Main.player[Projectile.owner];

          
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 14; i++)
            {
               

                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 10, ParticleManager.NewInstance<InvokedParticle4>(), Color.Purple, 0.2f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                // Getting texture of projectile
                Texture2D texture = (Texture2D)Request<Texture2D>(Texture);

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
                float offsetX = 10f;
                origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

                // If sprite is vertical
                // float offsetY = 20f;
                // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);

                // Applying lighting and draw current frame

                Color drawColor = Projectile.GetAlpha(lightColor);
                Main.EntitySpriteDraw(texture,
                    Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                    sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            }

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }
    }



   
    public class DoorusArcanum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doorfrenzy");

            Tooltip.SetDefault("Shoots arcane doors at enemies"
                 + "\n'We've got only one sky...'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 15;
            Item.knockBack = 5f;    
            Item.noMelee = true;
            Item.shoot = ProjectileType<RandomBullshit>();
            Item.shootSpeed = 5f;
            Item.mana = 10;
            Item.width = Item.height = 16;
            Item.scale = 0.6f;
            
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Purple;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(0.2f), ModContent.ProjectileType<RandomBullshit>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.2f), ModContent.ProjectileType<RandomBullshit>(), damage, knockback, player.whoAmI);
            return true;
        }
    }
}