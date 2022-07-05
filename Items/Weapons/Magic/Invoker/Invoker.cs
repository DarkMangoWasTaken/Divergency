using DivergencyMod.Dusts.Particles;
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

namespace DivergencyMod.Items.Weapons.Magic.Invoker
{
    public class InvokedProj : ModProjectile
    {
        public bool ParticleSpawned = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invoked Projectile");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 200;
            Projectile.scale = 0.95f;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float Timer2
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 7, ParticleManager.NewInstance<InvokedParticle4>(), Color.Purple, Main.rand.NextFloat(0.5f, 0.7f));
            }
        }

        public override void AI()
        {
            if (Timer == 1)
            {
                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap);
            }
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

            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }

            Player player = Main.player[Projectile.owner];

            ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<InvokedParticle>(), Color.Purple,1);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 14; i++)
            {
                SoundStyle adjusted = SoundID.DD2_EtherianPortalSpawnEnemy with
                {
                    Volume = 0.5f,
                    Pitch = 1f,
                };
                SoundEngine.PlaySound(adjusted);
                SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy with { Volume = 0.75f, Pitch = 1.3f });

                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 10, ParticleManager.NewInstance<InvokedParticle4>(), Color.Purple, Main.rand.NextFloat(0.5f, 0.8f));
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
                float offsetX = 40f;
                origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

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
    
    public class ChargedInvokedProj : ModProjectile
    {
        public bool ParticleSpawned = false;
        public override string Texture => "DivergencyMod/Items/Weapons/Magic/Invoker/InvokedProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invoked Projectile");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 3;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 200;
            Projectile.scale = 1.2f;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float Timer2
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            if (!ParticleSpawned)
            {
                for (int i = 0; i < 5; i++)
                {
                    player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 17;
                }
                //ParticleManager.NewParticle(Projectile.Center, Projectile.velocity * 0, ParticleManager.NewInstance<EyeParticle>(), Color.Purple, 2);
            }
        }

        public override void AI()
        {
            if (Timer == 2)
            {
                SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/InvocationSoundShot")
                {
                    Volume = 0.9f,
                    MaxInstances = 5,
                });
            }

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

            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }

            Player player = Main.player[Projectile.owner];

            ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<InvokedParticle>(), Color.Purple, 1);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                SoundStyle adjusted = SoundID.MenuTick with
                {
                    Volume = 1f,
                    Pitch = 1f
                };
                SoundEngine.PlaySound(adjusted);
                SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy with { Volume = 1f, Pitch = 1.1f });

                ParticleManager.NewParticle(Projectile.Center, speed * 15, ParticleManager.NewInstance<InvokedParticle4>(), Color.Purple, 1f);
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
                float offsetX = 40f;
                origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

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

    public class HeldInvokerProj : ModProjectile
    {
        public override string Texture => "DivergencyMod/Items/Weapons/Magic/Invoker/Invoker";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guide to Invocation");
            Main.projFrames[Projectile.type] = 1;
        }

        private float Combo = 0;

        private float MovementFactor = 30f;
        private float ParticleTimer;
        private bool ProjSpawned = false;

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
                Player player = Main.player[Projectile.owner];

                Timer++;

                for (int i = 0; i < 2; i++)
                {
                    Vector2 velocitypos = Projectile.velocity;

                    ParticleManager.NewParticle(Projectile.Center, velocitypos, ParticleManager.NewInstance<InvokedParticle2>(), Color.Purple, 1f, Projectile.whoAmI);
                }
                if (ProjSpawned)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                        ParticleManager.NewParticle(Projectile.Center, speed * 20, ParticleManager.NewInstance<InvokedParticle2>(), Color.Purple, 0.5f);
                        ParticleTimer = 0;
                    }
                }
                //if (Timer > 2)
                {
                    // Our timer has finished, do something here:
                    // Main.PlaySound, Dust.NewDust, Projectile.NewProjectile, etc. Up to you.
                    //  Timer = 0;
                }
                if (Timer == 1)
                {
                    Projectile.height = Projectile.width = 0;
                }
                if (Timer == 50)
                {
                    SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/InvocationSoundCharge")
                    {
                        Volume = 0.9f,
                        

                    });
                }

                if (Timer == 90)
                {
                    player.HeldItem.mana = 20;

                    for (int i = 0; i < 5; i++)
                    {
                        ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<InvokedParticle3>(), Color.Purple, 1f, 0);
                    }

                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity * 20, ModContent.ProjectileType<ChargedInvokedProj>(), 75, 10, Projectile.owner, Projectile.whoAmI);

                    Projectile.Kill();
                }
                if (Timer >= 30 && Timer <= 89)
                {
                    ProjSpawned = true;
                    player.HeldItem.mana = 10;

                    if (Main.mouseLeftRelease)
                    {
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity * 10, ModContent.ProjectileType<InvokedProj>(), 25, 3, Projectile.owner, Projectile.whoAmI);
                        Projectile.Kill();
                        for (int i = 0; i < 2; i++)
                        {
                            ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<InvokedParticle3>(), Color.Purple, 0.5f, 0);
                        }
                    }
                }

                if (player.noItems || player.CCed || player.dead || !player.active)
                    Projectile.Kill();
                //if (!player.channel)
                //{
                //    Projectile.Kill();
                //}

                Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, true);
                float swordRotation = 0f;
                if (Main.myPlayer == Projectile.owner)
                {
                    player.ChangeDir(Projectile.direction);
                    swordRotation = (Main.MouseWorld - player.Center).ToRotation();
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
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - 30 : 30); // Customization of the sprite position

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw((Texture2D)TextureAssets.Projectile[Projectile.type], Main.player[Projectile.owner].Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            return false;
        }
    }

    public class Invoker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Commandant's Guide to Invocation");

            Tooltip.SetDefault("Requires some charging time in order to shoot"
                 + "\nOvercharge it for more powerful projectiles");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 25;
            Item.knockBack = 5f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<HeldInvokerProj>();
            Item.shootSpeed = 12f;
            Item.mana = 10;
            Item.width = Item.height = 16;
            Item.scale = 1f;
            Item.channel = true;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Purple;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
    }
}