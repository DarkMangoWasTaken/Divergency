using DivergencyMod.NPCs.Forest;
using DivergencyMod.Tiles.LivingTree;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Events.LivingCore
{
    public static class LivingCoreEvent
    {
        // TODO: Make progress bar since BossBar may not work without an NPC
        // TODO: Play music

        public static bool Active { get; private set; }
        public static Tile Altar { get; private set; }
        public static int X { get; private set; }
        public static int Y { get; private set; }
        public static Vector2 Position { get => new(X * 16f, Y * 16f); }
        public static LivingCoreAltarRoom Room { get; private set; }

        public enum LivingCoreAltarRoom
        {
            RoomOne
        }

        public static void Update()
        {
            if (Room == LivingCoreAltarRoom.RoomOne)
                LivingCoreRoom1.Update();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Room == LivingCoreAltarRoom.RoomOne)
                LivingCoreRoom1.Draw(spriteBatch);
        }

        public static void Begin(int i, int j, LivingCoreAltarRoom room = LivingCoreAltarRoom.RoomOne)
        {
            if (Active)
                return;
            if (Main.tile[i, j].TileType != ModContent.TileType<LivingCoreAltarTile1>())
                return;

            Active = true;
            Altar = Main.tile[i, j];
            X = i;
            Y = j;
            Room = room;

            Player player = Main.LocalPlayer;
            // TODO: Add extension methods for fetching globals to allow player.Divergency().X = X;
            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 50;

            int left = i - Main.tile[i, j].TileFrameX / 18;
            int top = j - Main.tile[i, j].TileFrameY / 18;

            if (room == LivingCoreAltarRoom.RoomOne)
                LivingCoreRoom1.Begin(left, top);
        }

        public static void End()
        {
            if (Room == LivingCoreAltarRoom.RoomOne)
                LivingCoreRoom1.End();

            Active = false;
            Altar = default;
            X = 0;
            Y = 0;
            Room = default;
        }

        public static void Load()
        {
            End();
        }

        public static void Unload()
        {
            End();
        }

        public static void AddProgress(int amount)
        {
            if (Room == LivingCoreAltarRoom.RoomOne)
                LivingCoreRoom1.Kills += amount;
        }

        public static void RemoveProgress(int amount)
        {
            if (Room == LivingCoreAltarRoom.RoomOne)
                LivingCoreRoom1.Kills -= amount;
        }

        public static float GetProgress()
        {
            if (Room == LivingCoreAltarRoom.RoomOne)
                return LivingCoreRoom1.Progress;
            return 0f;
        }
    }
}
