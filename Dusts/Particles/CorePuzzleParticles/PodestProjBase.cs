using DivergencyMod.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System.Threading;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Dusts.Particles.CorePuzzleParticles
{
    public class PodestProjBase : Particle
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BambooDoor;

        private int frameCount;
        private bool countdown;
        private bool countdownstop;

        private int frameTick;

        public override void SetDefaults()
        {
            width = 34;
            height = 34;
            Scale = 1f;
            timeLeft = 3000;
        }

        public override void AI()
        {

            if (countdown == true && !countdownstop)
            {
                timeLeft = 20;

                countdownstop = true;
            }
            position = Main.projectile[(int)ai[1]].Center;
            if(!Main.projectile[(int)ai[1]].active)
            {

                active = false;
            }
            color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), Color.Multiply(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0f), 0.5f), (360f - timeLeft) / 360f);
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D tex = Request<Texture2D>("DivergencyMod/Dusts/Particles/InvokedParticle").Value;
            Texture2D tex3 = Request<Texture2D>("DivergencyMod/Dusts/Particles/EyeParticle").Value;
            Texture2D tex2 = Request<Texture2D>("DivergencyMod/Dusts/Particles/LivingCoreParticle2").Value;

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), alpha);
            spriteBatch.Draw(tex, position - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex3.Height), color, 0, new Vector2(tex.Width / 2f, tex.Height / 2f), 1f * Scale, SpriteEffects.None, 0f);
            // spriteBatch.Draw(tex2, position - Main.screenPosition, new Rectangle(0, 0, tex2.Width, tex2.Height), color, ai[1].InRadians().AngleLerp((ai[1] * 180f).InRadians(), (120f - timeLeft) / 120f), new Vector2(tex2.Width / 2f, tex2.Height / 2f), 0.5f * Scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, 0, new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.3f * Scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex3, position - Main.screenPosition, new Rectangle(0, 0, tex3.Width, tex3.Height), color, 0, new Vector2(tex3.Width / 2f, tex3.Height / 2f), 0.5f * Scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}