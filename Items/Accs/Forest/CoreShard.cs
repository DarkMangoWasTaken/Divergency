
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DivergencyMod.Items.Weapons.Melee.RootBreaker;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DivergencyMod.Items.Accs.Forest
{
    public class CoreShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("guh2");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(10);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;


        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
                player.GetModPlayer<ShardDrop>().ShardSpawn = true;
                player.GetModPlayer<ShardDrop>().ShardCooldown--;
            


            player.statDefense += 1;
        }




    }
    public class ShardProj : ModProjectile
    {
        public float Timer = 0;
        public bool Collided = false;
        public float OffsetY = 80;
        public bool OffsetYRaise;
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 360;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
            Timer++;
            if (OffsetYRaise && OffsetY >= 50)
            {
                for (int i = 0; i < 5; i++)
                {
                    OffsetY--;
                }
            }
            if (!Collided)
            {
                Projectile.damage = 0;
                if (Timer >= 30)
                {
                    
                    Projectile.velocity.Y += 1.2f;
                   
                }
                else
                {
                    

                    Projectile.velocity.Y += 0.3f;
                    Projectile.rotation += (Projectile.velocity.Length() * 0.04f) * Projectile.direction;

                }
            }
            else
            {
                Projectile.damage = 30;
                Projectile.velocity.Y = 0;
                Projectile.velocity.X = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Collided = true;
            Projectile.rotation = 0;
            return false;

      
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            OffsetYRaise = true;
           
          
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.LocalPlayer;

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 2;

            for (int j = 0; j < 10; j++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                Dust.NewDustPerfect(Projectile.Center, DustID.GemEmerald, speed * 2, 0, Color.Green, 1f);
            }
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
            float offsetX = 20f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

            // If sprite is vertical
             float offsetY = OffsetY;
             origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);

            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }
       
    }



    public class ShardDrop : ModPlayer
    {
        
        public bool ShardSpawn;
        public int ShardCooldown = 600;
        public override void ResetEffects()
        {
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            
            ShardSpawn = false;
        }
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (ShardCooldown <= 0 && ShardSpawn)
            {
                for (int i = 0; i < Main.rand.Next(3, 4); i++)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(Main.rand.Next(-3, 3) * 1.1f, Main.rand.Next(-15, -10) * 1.1f), ModContent.ProjectileType<ShardProj>(), 20, 1f, Player.whoAmI);
                    ShardCooldown = 600;
                }
            }
        }


    }
   

}