using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DivergencyMod.Base
{
	
	public static class BaseExtension
    {
        public static float InRadians(this float degrees) => MathHelper.ToRadians(degrees);

        /// <summary>Shorthand for converting radians of rotation into a degrees equivalent.</summary>
        public static float InDegrees(this float radians) => MathHelper.ToDegrees(radians);

        public static Rectangle AnimationFrame(this Texture2D texture, ref int frame, ref int frameTick, int frameTime, int frameCount, bool frameTickIncrease, int overrideHeight = 0)
        {
            if (frameTick >= frameTime)
            {
                frameTick = -1;
                frame = frame == frameCount - 1 ? 0 : frame + 1;
            }
            if (frameTickIncrease)
                frameTick++;
            return new Rectangle(0, overrideHeight != 0 ? overrideHeight * frame : (texture.Height / frameCount) * frame, texture.Width, texture.Height / frameCount);
        }


        public static float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }
        public static void Move(this NPC npc, Vector2 pos, float speed, float divider)
    {
        Vector2 vel = npc.DirectionTo(pos) * speed;
        npc.velocity = (npc.velocity * divider + vel) / (divider + 1);
    }

    public static void Move(this NPC npc, Vector2 vector, float speed, float turnResistance = 10f,
        bool toPlayer = false)
    {
        Player player = Main.player[npc.target];
        Vector2 moveTo = toPlayer ? player.Center + vector : vector;
        Vector2 move = moveTo - npc.Center;
        float magnitude = Magnitude(move);
        if (magnitude > speed)
        {
            move *= speed / magnitude;
        }

        move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
        magnitude = Magnitude(move);
        if (magnitude > speed)
        {
            move *= speed / magnitude;
        }

        npc.velocity = move;
    }

    public static void Move(this Projectile projectile, Vector2 vector, float speed, float turnResistance = 10f, bool toPlayer = false)
    {
        Player player = Main.player[projectile.owner];
        Vector2 moveTo = toPlayer ? player.Center + vector : vector;
        Vector2 move = moveTo - projectile.Center;
        float magnitude = Magnitude(move);
        if (magnitude > speed)
        {
            move *= speed / magnitude;
        }

        move = (projectile.velocity * turnResistance + move) / (turnResistance + 1f);
        magnitude = Magnitude(move);
        if (magnitude > speed)
        {
            move *= speed / magnitude;
        }

        projectile.velocity = move;
    }

}
	
}
