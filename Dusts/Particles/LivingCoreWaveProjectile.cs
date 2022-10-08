using DivergencyMod.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles
{
    public class LivingCoreWaveProjectile : Particle
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BambooDoor;

        public Rectangle FrameData;
         public int FrameTick;

        public override void SetDefaults()
        {
            width = 1;
            height = 1;
            Scale = 1f;
            timeLeft = 300;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;
            if (player.dead)
            {
                active = false;
            }
            position = player.Center;

            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            int frameTime = 6;
            Player player = Main.LocalPlayer;
            SpriteEffects effect = SpriteEffects.None;

            if (player.direction == 1)
            {
                effect = SpriteEffects.None;

            }
            else
            {
                effect = SpriteEffects.None;
            }
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestAnimated").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha);
            //spriteBatch.Draw(tex2, position - Main.screenPosition, new Rectangle(0, 0, tex2.Width, tex2.Height), color, ai[1].InRadians().AngleLerp((ai[1] * 180f).InRadians(), (120f - timeLeft) / 120f), new Vector2(tex2.Width / 2f, tex2.Height / 2f), 0.05f * Scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex3, Bottom - Main.screenPosition, tex3.Animate(ref FrameData,ref FrameTick, frameTime, 2,2), color, 0, new Vector2(64f, 64f) * 0.5f, 0.7f * Scale, effect, 0f);
            //spriteBatch.Draw(tex, position - Main.screenPosition, tex.AnimationFrame(ref frameCount, ref frameTick, 4, 7, true), color, 0f, new Vector2(width / 2, height / 2), 0.5f, SpriteEffects.None, 0f);
            return false;
        }

    }

}