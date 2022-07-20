using DivergencyMod.Base;
using DivergencyMod.Dusts.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace DivergencyMod.Bosses.Forest
{
    public class WraithFireBreathProj : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor) => new(255, 255, 255, 100);
        public override string Texture => "DivergencyMod/Items/Weapons/Magic/Invoker/InvokedProj";

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.width = Projectile.height = 40;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.alpha = 255;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 30;
            Projectile.hide = true;
            Projectile.CritChance = 0;

        }





    }
    public class LivingFlameBlast : ModProjectile
    {
        public float timer;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BewitchedSpikyBall");
            Main.projFrames[Projectile.type] = 4;

        }

        public override void SetDefaults()
        {
            Projectile.width = 15; // The width of projectile hitbox
            Projectile.height = 15; // The height of projectile hitbox

            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Generic; // Is the projectile shoot by a ranged weapon?
            Projectile.timeLeft = 360; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?

            Projectile.scale = 1.2f;    
        }

        public override void AI()
        {
            Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
            float multiplier = 0.4f;
            float max = 1f;
            float min = 1.0f;
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
            timer++;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.spriteDirection = Projectile.direction;

            if (timer == 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    ParticleManager.NewParticle(Projectile.Center, speed * 3, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 0.9f);


                }
                timer = 0;
            }





        }
       


        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {

            }
        }



        public override bool PreDraw(ref Color lightColor)
        {

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {

                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }

    public class LivingFlameTrail : ModProjectile
    {
        public float timer;
        public NPC ParentNPC;
        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BewitchedSpikyBall");
            Main.projFrames[Projectile.type] = 4;

        }

        public override void SetDefaults()
        {
            Projectile.width = 20; // The width of projectile hitbox
            Projectile.height = 20; // The height of projectile hitbox

            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Generic; // Is the projectile shoot by a ranged weapon?
            Projectile.timeLeft = 400; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?

        }

        public override void AI()
        {
            #region normal projectile code
            Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
            float multiplier = 0.4f;
            float max = 1f;
            float min = 1.0f;
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
            timer++;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180);
            Projectile.spriteDirection = Projectile.direction;

            if (timer == 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    ParticleManager.NewParticle(Projectile.Center, speed * 10, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 0.9f);


                }
                timer = 0;
            }
            #endregion
            Player player = Main.player[Projectile.owner];
       

            


        }



        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {

            }
        }



        public override bool PreDraw(ref Color lightColor)
        {

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {

                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
    public class LivingCoreSurpriseBlast : ModProjectile
    {
        public float timer;
        public float timer2;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Blast");
            Main.projFrames[Projectile.type] = 4;

        }
        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";


        public override void SetDefaults()
        {
            Projectile.width = 15; // The width of projectile hitbox
            Projectile.height = 15; // The height of projectile hitbox

            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Generic; // Is the projectile shoot by a ranged weapon?
            Projectile.timeLeft = 360; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?

            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
            float multiplier = 0.4f;
            float max = 1f;
            float min = 1.0f;
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
            timer++;
            timer2++;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.spriteDirection = Projectile.direction;

            if (timer == 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    ParticleManager.NewParticle(Projectile.Center, speed * 3, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 0.9f);


                }
                timer = 0;
            }
            for (int p = 1  ; p < Main.maxPlayers; p++)
            {
                Player target = Main.player[p];

                if (timer2 <= 30)
                {
                    Projectile.Center = target.Center;  
                }
                else
                {
                    Projectile.velocity.X = Projectile.direction * 10;
                }
            }
           




        }
     
        



        public override bool PreDraw(ref Color lightColor)
        {

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {

                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    
    }
}



