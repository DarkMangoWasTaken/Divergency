using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using DivergencyMod.Base;

namespace DivergencyMod.Dusts.Particles
{
    public class FireParticle : Particle
    {

        private int frameCount;
        private int frameTick;

        public override void SetDefaults()
        {
            width = 1;
            height = 1;
            scale = 1f;
            timeLeft = 30;
            
        }

        public override void AI()
        {
            rotation = velocity.ToRotation();

            velocity *= 0.98f;
            scale *= 1.1f;

            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/FireParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/LivingCoreParticle").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha);
            //spriteBatch.Draw(tex2, position - Main.screenPosition, new Rectangle(0, 0, tex2.Width, tex2.Height), color, ai[1].InRadians().AngleLerp((ai[1] * 180f).InRadians(), (120f - timeLeft) / 120f), new Vector2(tex2.Width / 2f, tex2.Height / 2f), 0.05f * scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex, position - Main.screenPosition, tex.AnimationFrame(ref frameCount, ref frameTick, 4, 7, true), color, 0f, new Vector2(width / 2, height / 2), 0.9f, SpriteEffects.None, 0f);
            return false;
        }
    }
}