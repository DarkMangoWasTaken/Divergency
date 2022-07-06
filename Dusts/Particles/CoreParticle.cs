using DivergencyMod.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles
{
    public class CoreParticle : Particle
    {
        private int frameCount;
        private int frameTick;
        public override string Texture => "Terraria/Images/Item_" + ItemID.BambooDoor;
        public override void SetDefaults()
        {
            width = 34;
            height = 34;
            scale = 1f;
            timeLeft = 30;
        }
       

        public override void AI()
        {
            rotation += Utils.Clamp(velocity.X * 1.025f, -ai[1], ai[1]);

            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);
            velocity *= 0.98f;
            scale *= 1.08f;
            if (scale <= 0f)
                active = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha);
            spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, ai[3].InRadians().AngleLerp((ai[3] + 90f).InRadians(), (120f - timeLeft) / 120f), new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.07f * scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}