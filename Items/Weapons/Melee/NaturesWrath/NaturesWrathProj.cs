using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Melee.NaturesWrath
{
    public class NaturesWrathProj : ModProjectile
    {
        public override string Texture => "DivergencyMod/Items/Weapons/Melee/NaturesWrath/NaturesWrath";

        public int combowombo;
        public static bool swung = false;
        public int SwingTime = 50;
        public float holdOffset = 63f;
        public bool SoundPlay = false;
        public bool SoundPlayed = false;

        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 60;
            Projectile.width = 80;
            Projectile.friendly = true;
            Projectile.scale = 1.2f;
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;
            AttachToPlayer();
        }

        public override bool ShouldUpdatePosition() => false;

        public void AttachToPlayer()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.type == ModContent.ProjectileType<NaturesWrathProj2>() && proj.active && Projectile.Hitbox.Intersects(proj.Hitbox) && proj.friendly)
                {
                    proj.Kill();
                    player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8;
                    SoundPlay = true;
                }
            }
            int dir = (int)Projectile.ai[1];
            float swingProgress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            // the actual rotation it should have
            float defRot = Projectile.velocity.ToRotation();
            // starting rotation
            float endSet = ((MathHelper.PiOver2) / 0.192f);
            float start = defRot - endSet;

            // ending rotation
            float end = defRot + endSet;
            // current rotation obv
            float rotation = dir == 1 ? start.AngleLerp(end, swingProgress) : start.AngleLerp(end, 1f - swingProgress);
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (SoundPlay && !SoundPlayed)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);
                SoundPlayed = true;
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }

    public class NaturesWrathProj2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 25; // The width of Projectile hitbox
            Projectile.height = 25; // The height of Projectile hitbox

            Projectile.friendly = true; // Can the Projectile deal damage to enemies?
            Projectile.hostile = false; // Can the Projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Ranged; // Is the Projectile shoot by a ranged weapon?
            Projectile.penetrate = 1; // How many monsters the Projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 1200; // The live time for the Projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.light = 0; // How much light emit around the Projectile
            Projectile.ignoreWater = false; // Does the Projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the Projectile collide with tiles?
            Projectile.penetrate = -1;
            Projectile.hide = true;
            Projectile.timeLeft = 10000000;
        }

        public bool StuckStepBro = false;
        public NPC StepSis;
        public Vector2 StuckPosition;

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.active && !StuckStepBro)
            {
                StuckStepBro = true;
                StepSis = target;
            }
        }

        public override void AI()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Player player = Main.player[Projectile.owner];
            if (!StuckStepBro)
            {
                Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

                Projectile.rotation = Projectile.velocity.ToRotation();
                // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
                if (Projectile.spriteDirection == -1)
                {
                    Projectile.rotation += MathHelper.Pi;
                    // For vertical sprites use MathHelper.PiOver2
                }

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
            }
            if (StuckStepBro)
            {
                if (StuckStepBro)
                {
                    StuckPosition = Projectile.position;
                }

                Projectile.position = StuckPosition + StepSis.velocity;
                Projectile.velocity.Y = 0f;
                Projectile.velocity.X = 0f;
                Projectile.damage = 4;
                Projectile.knockBack = 0;
                if (!StepSis.active)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if (StepSis != null)
            {
                StepSis.StrikeNPC(20, 0f, 0, false, false, false);
            }
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
            int goreType = Mod.Find<ModGore>("BranchGore").Type;
            int goreTypeAlt = Mod.Find<ModGore>("BranchGore").Type;

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
            }
            for (int i = 0; i < 5; i++)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(null, Projectile.Center, Projectile.velocity, GoreID.TreeLeaf_Normal, 1.1f);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit3);

            for (int i = 0; i < 15; i++)
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
                    Gore.NewGore(null, Projectile.Center, Projectile.velocity, GoreID.TreeLeaf_Normal, 1.1f);
                }
            }

            for (int i = 0; i < 1; i++)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    int goreType = Mod.Find<ModGore>("BranchGore").Type;
                    int goreTypeAlt = Mod.Find<ModGore>("BranchGore").Type;
                    Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                    Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
                }
            }
            return true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

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
            float offsetX = 60f;
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