﻿using DivergencyMod.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles
{
    public class Telegraph2 : Particle
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


            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);

            if (Scale <= 0f)
                active = false;
            if (timeLeft == 14)
            {

            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/Telegraph2").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha);
            spriteBatch.Draw(tex2, position - Main.screenPosition, new Rectangle(0, 0, tex2.Width, tex2.Height), color,velocity.ToRotation()+ MathHelper.PiOver2, new Vector2(tex2.Width / 2f, tex2.Height / 2f), 0.8f * Scale, SpriteEffects.None, 0f);
            //spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, ai[0].InRadians().AngleLerp((ai[0] + 90f).InRadians(), (120f - timeLeft) / 120f), new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.2f * Scale, SpriteEffects.None, 0f);
            //spriteBatch.Draw(tex, position - Main.screenPosition, tex.AnimationFrame(ref frameCount, ref frameTick, 4, 7, true), color, 0f, new Vector2(width / 2, height / 2), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}