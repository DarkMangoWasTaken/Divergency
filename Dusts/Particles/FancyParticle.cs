﻿using DivergencyMod.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles
{
    public class FancyParticle : Particle
    {
        private int frameCount;
        private int frameTick;

        public override void SetDefaults()
        {
            width = 34;
            height = 34;
            Scale = 1f;
            timeLeft = 30;
        }

        public override void AI()
        {
            Scale /= 1.1f;
            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);
        }
      
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/FancyParticle").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha);
                spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, 0, new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.2f * scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, 0, new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.2f * scale, SpriteEffects.None, 0f);

            return false;
        }
    }
    public class LivingCoreEyeParticle : Particle
    {
        private int frameCount;
        private int frameTick;

        public override void SetDefaults()
        {
            width = 34;
            height = 34;
            Scale = 2.4f;
            timeLeft = 300;
            layer = Layer.BeforeProjectiles;
        }
      
        public override void AI()
        {
            position = Main.projectile[(int)ai[1]].Center;
            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);
        }
     
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {

            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/FancyParticle").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha);
            spriteBatch.Draw(tex3, layer == Layer.BeforeProjectiles ? position : screenPos, new Rectangle(0, 0, tex3.Width, tex3.Height), color, 0, new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.3f * scale, SpriteEffects.None, 0f);


            return false;
        }
    }
}