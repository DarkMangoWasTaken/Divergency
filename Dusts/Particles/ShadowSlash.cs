using DivergencyMod.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles
{
    public class ShadowSlash : Particle
    {
        private int frameCount;
        private int frameTick;

        public override void SetDefaults()
        {
            width = 1;
            height = 1;
            scale = 1f;
            timeLeft = 18;
            
        }

        public override void AI()
        {
     
                position = Main.projectile[(int)ai[1]].Center;
            
            
            if (timeLeft == 18)
            {
                velocity = DirectionTo(Main.MouseWorld);
            }
            
            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Main.projectile[(int)ai[1]].direction == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }


            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/ShadowSlash").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(1.28f, 0f, 1.28f, 0), alpha);
           // spriteBatch.Draw(tex2, position - Main.screenPosition, new Rectangle(0, 0, tex2.Width, tex2.Height), color, ai[1].InRadians().AngleLerp((ai[1] * 180f).InRadians(), (120f - timeLeft) / 120f), new Vector2(tex2.Width / 2f, tex2.Height / 2f), 0.05f * scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex3, Center - Main.screenPosition, tex3.AnimationFrame(ref frameCount, ref frameTick, 1, 18, true), color, velocity.ToRotation() + 180, new Vector2(111f, 111f) * 0.5f, 3f * scale, spriteEffects, 0f);
           // spriteBatch.Draw(tex, position - Main.screenPosition, tex.AnimationFrame(ref frameCount, ref frameTick, 4, 7, true), color, 0f, new Vector2(width / 2, height / 2), 0.5f, SpriteEffects.None, 0f);
            return false;
        }
    }
}