using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Magic.LivingLeafStorm
{
    public class LivingLeafStorm : ModItem
    {
        // You can use a vanilla texture for your item by using the format: "Terraria/Item_<Item ID>".
        // You can use a vanilla texture for your item by using the format: "Terraria/Item_<Item ID>".

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leafthingy Yeeter");
            Tooltip.SetDefault(@"Yeet");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 7;
            Item.knockBack = 5f;
            Item.noMelee = true;

            Item.shoot = ProjectileType<YeeterProj>();
            Item.shootSpeed = 12f;
            Item.mana = 10;
            Item.width = Item.height = 16;
            Item.scale = 1f;
            Item.noUseGraphic = true;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            //Item.UseSound = SoundID.DD2_LightningAuraZap;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.channel = true;

            Item.value = Item.sellPrice(999, 0, 0, 0);
            Item.rare = ItemRarityID.Master;
        }

        // Because this weapon fires a holdout Projectile, it needs to block usage if its Projectile already exists.
    }

    public class YeeterProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("auroranproj");
            Main.projFrames[Projectile.type] = 1;
        }

        private float Combo = 0;

        private float MovementFactor = 63f;

        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 595;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.2f;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            {
                Timer++;
                //if (Timer > 2)
                {
                    // Our timer has finished, do something here:
                    // Main.PlaySound, Dust.NewDust, Projectile.NewProjectile, etc. Up to you.
                    //  Timer = 0;
                }
                if (Timer == 1)
                {
                    Projectile.damage = 0;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.position.X, Projectile.position.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<YeeterProj2>(), 4, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
                }

                Player player = Main.player[Projectile.owner];
                if (player.noItems || player.CCed || player.dead || !player.active)
                    Projectile.Kill();

                Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, true);
                float swordRotation = 0f;
                if (Main.myPlayer == Projectile.owner)
                {
                    player.ChangeDir(Projectile.direction);
                    swordRotation = (Main.MouseWorld - player.Center).ToRotation();
                    if (!player.channel)
                        Projectile.Kill();
                }
                Projectile.velocity = swordRotation.ToRotationVector2();

                Projectile.spriteDirection = player.direction;
                if (Projectile.spriteDirection == 1)
                    Projectile.rotation = Projectile.velocity.ToRotation();
                else
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;

                Projectile.Center = playerCenter + Projectile.velocity * MovementFactor;// customization of the hitbox position

                player.heldProj = Projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - 80 : 80); // Customization of the sprite position

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw((Texture2D)TextureAssets.Projectile[Projectile.type], Main.player[Projectile.owner].Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            return false;
        }
    }

    public class YeeterProj2 : ModProjectile
    {
        private float Charge = 0;

        private Projectile ParentProjectile;
        public bool MouseLeftPressed = false;
        public bool hit = false;

        public float Timer3
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public float Timer4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BewitchedSpikyBall");
            Main.projFrames[Projectile.type] = 9;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 5;
            Projectile.width = 15;
            Projectile.height = 15;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 2f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Charge == 1)
            {
                Projectile.Kill();
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Charge == 1)
            {
                Projectile.velocity = Projectile.velocity * -0.4f;

                // If the Projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - Projectile.oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -Projectile.oldVelocity.X / 10;
                }

                // If the Projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(Projectile.velocity.Y - Projectile.oldVelocity.Y) > float.Epsilon)
                {
                    Projectile.velocity.Y = -Projectile.oldVelocity.Y;
                }
                hit = true;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            // Set both direction and spriteDirection to 1 or -1 (right and left respectively)
            // Projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            // Adding Pi to rotation if facing left corrects the drawing

            float swordRotation = 0f;
            if (Main.myPlayer == Projectile.owner)
            {
                swordRotation = (Main.MouseWorld - player.Center).ToRotation();
            }
            if (Charge == 0)
            {
                Projectile.velocity = swordRotation.ToRotationVector2();
            }
            if (hit)
            {
                float Rotation = Projectile.velocity.ToRotation();
                Projectile.rotation += Rotation + MathHelper.ToRadians(90f);
            }

            Projectile.spriteDirection = player.direction;
            if (Projectile.spriteDirection == 1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            }

            Timer4++;
            if (Charge == 1)
            {
                if (Timer4 >= 100)
                {
                    Projectile.velocity.Y += 0.4f;
                }
            }

            if (Projectile.damage < 50)
            {
                Timer3++;
            }
            else
            {
                Timer3 = 3;
            }
            if (Timer3 == 6)
            {
                Timer3 = 0;
            }
            ParentProjectile = Main.projectile[(int)Projectile.ai[0]];
            if (Main.mouseLeft)
            {
                if (Charge == 0)
                {
                    if (Timer3 == 5)
                    {
                        Projectile.damage += 1;
                    }
                }
                if (!MouseLeftPressed)
                {
                    Projectile.Center = ParentProjectile.Center;
                }
            }
            if (!Main.mouseLeft)
            {
                Charge = 1;
                if (!MouseLeftPressed)
                {
                    if (Projectile.damage! <= 16)
                    {
                        Projectile.Kill();
                    }

                    Projectile.velocity = 0.7f * Projectile.damage * (Vector2.Normalize(Projectile.DirectionTo(Main.MouseWorld)));
                    MouseLeftPressed = true;
                }
            }
            if (MouseLeftPressed)
            {
                Projectile.velocity = Projectile.velocity;
            }
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 9)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            const int numberDust = 40;

            for (int i = 0; i < numberDust; i++)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(20));

                    float scale = 1f - (Main.rand.NextFloat() * 0.75f);
                    perturbedSpeed *= scale;

                    Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.WoodFurniture, 0, 0, 100, default, 2f);
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.WoodFurniture, 0f, 0f, 1000, default, 2f);
                }
            }
            for (int i = 0; i < 12; i++)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    Gore.NewGore(null, Projectile.Center, speed * 2, GoreID.TreeLeaf_Normal, 1.1f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // Getting texture of Projectile
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
            float offsetX = 12f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

            // If sprite is vertical
            // float offsetY = 20f;
            // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);

            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }
    }
}