using DivergencyMod.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles
{
    public class LivingCoreExplosionParticle : Particle
    {
        private int frameCount;
        private int frameTick;
        public override string Texture => "Terraria/Images/Item_" + ItemID.BambooDoor;
        public override void SetDefaults()
        {
            width = 34;
            height = 34;
            Scale = 1f;
            timeLeft = 10;
        }
       

        public override void AI()
        {
            rotation = velocity.ToRotation();

            Scale *= 1.2f;
            if (Scale <= 0f)
                active = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
        {
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/FireParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/LivingCoreParticle2").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha );
            Color color2 = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha );

            spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, rotation,  new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.1f * Scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}