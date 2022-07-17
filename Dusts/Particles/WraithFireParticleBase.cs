using DivergencyMod.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles
{
    public class WraithFireParticleBase : Particle
    {
        private int frameCount;
        private int frameTick;
        private bool ProjDed;

        public override string Texture => "Terraria/Images/Item_" + ItemID.BambooDoor;
        public override void SetDefaults()
        {
            width = 34;
            height = 34;
            scale = 1f;
            timeLeft = 1800;
        }
       

        public override void AI()
        {
            rotation = velocity.ToRotation();
            if (!Main.projectile[(int)ai[1]].active)
            {
                if (!ProjDed)
                {
                    timeLeft = 3;
                }
                ProjDed = true;
            }
                if (scale <= 0f)
                active = false;
            opacity = 125f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/SacronyFlameParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/TestParticle3").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/WraithFireParticleBase").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha / 2);


            //            spriteBatch.Draw(tex, position - Main.screenPosition, tex.AnimationFrame(ref frameCount, ref frameTick, 7, 7, true), color2, 0f, new Vector2(tex.Width / 2f, tex.Height / 2f / 7f), scale / 3.2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, rotation,  new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.17f * scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}