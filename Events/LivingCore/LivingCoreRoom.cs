using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using DivergencyMod.Items.Weapons.Melee.LivingCoreSword;
using DivergencyMod.Tiles.LivingTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace DivergencyMod.Events.LivingCore
{
    public abstract class LivingCoreRoom
    {
        List<NPC> currentNPCs = new List<NPC>();

        public int Kills = 0;
        public float Progress => (float)TotalKills / (TotalEnemies);
        private int KillsRemaining { get => CurWaveObject != null ? CurWaveObject.enemies.Length - Kills : 0; }

        private int Timer = 0;
        private int SpawnTimer = 0;
        private int CurWave = 0;

        private int TotalEnemies = 0;
        private int TotalKills = 0;

        private bool Intermission = false;

        private Wave CurWaveObject;

        private string[] Textures = new string[] {
            "DivergencyMod/Tiles/LivingTree/LivingCoreAltar1",
            "DivergencyMod/Tiles/LivingTree/LivingCoreAltar2",
            "DivergencyMod/Tiles/LivingTree/LivingCoreAltar3",
            "DivergencyMod/Tiles/LivingTree/LivingCoreAltar4"
        };

        public LivingCoreRoom()
        {
            TotalEnemies = 0;

            int curWaveTest = 1;
            Wave wave = getWave(curWaveTest);

            while (wave != null)
            {
                TotalEnemies += wave.enemies.Length;
                curWaveTest++;
                wave = getWave(curWaveTest);
            }
        }

        public virtual int Music { get { return 0; } }

        public virtual Wave? getWave(int wave)
        {
            return new Wave("Wave #&¤%!", new Instance[] {});
        }

        public virtual int getWaves()
        {
            return 0;
        }

        public void Update()
        {
            Timer++;

            Rectangle textPosition = new(LivingCoreEvent.X * 16 + 24, LivingCoreEvent.Y * 16 + 24, 0, 0);

            Console.WriteLine(Timer + " | " + SpawnTimer + " | " + CurWave + " | " + KillsRemaining + " | " + TotalEnemies);

            if (SpawnTimer == 0 && KillsRemaining == 0)
            {
                CurWave++;

                if (CurWave == getWaves() + 1)
                {
                    LivingCoreEvent.End();
                    return;
                }

                Kills = 0;
                CurWaveObject = getWave(CurWave);

                if (CurWaveObject == null)
                {
                    LivingCoreEvent.End();
                    return;
                }

                CombatText.NewText(textPosition, Color.LightGreen, CurWaveObject.name, true, false);
                Intermission = true;
            }

            if (SpawnTimer == 100)
            {
                CombatText.NewText(textPosition, Color.LightGreen, "3!", false, false);
            }
            if (SpawnTimer == 160)
            {
                CombatText.NewText(textPosition, Color.LightGreen, "2!", false, false);
            }
            if (SpawnTimer == 220)
            {
                CombatText.NewText(textPosition, Color.LightGreen, "1!", false, false);
            }
            if (SpawnTimer == 280)
            {
                foreach (Instance instance in CurWaveObject.enemies)
                {
                    Vector2 spawnPosition = LivingCoreEvent.Position + instance.SpawnOffset;

                    currentNPCs.Add(NPC.NewNPCDirect(null, spawnPosition, instance.NPCID));

                    ParticleManager.NewParticle<ResetParticle>(spawnPosition, Vector2.Zero, Color.White, 1f, 1.05f);
                }

                SpawnTimer = 0;
                Intermission = false;
            }

            if (SpawnTimer > 100)
            {
                foreach (Instance instance in CurWaveObject.enemies)
                {
                    Vector2 spawnPosition = LivingCoreEvent.Position + instance.SpawnOffset;

                    float rotation = Main.rand.NextFloat(MathHelper.TwoPi);

                    for (int i = 0; i < 2; i++)
                    {
                        ParticleManager.NewParticle<FlareLineParticle>(spawnPosition + new Vector2(128f, 0f).RotatedBy(rotation), new Vector2(4f, 0f).RotatedBy(rotation + MathHelper.Pi), new Color(0.50f, 2.05f, 0.5f, 0), 1f, Main.rand.NextFloat(0.8f, 1.1f));
                    }

                    ParticleManager.NewParticle<CoreLoadParticle>(spawnPosition, Main.rand.NextVector2Circular(3f, 3f), Color.Purple, Main.rand.NextFloat(0.5f, 0.75f), 1f);
                }
            }

            if (Intermission)
            {
                SpawnTimer++;
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active)
                    continue;

                if (player.active && player.dead)
                {
                    LivingCoreEvent.End();
                    return;
                }
            }

            clearList();
        }

        private void clearList()
        {
            foreach (NPC npc in currentNPCs)
            {
                if (npc.active == false)
                {
                    currentNPCs.Remove(npc);
                    clearList();
                    Kills++;
                    TotalKills++;
                    return;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D altarWave = default;
            if (CurWave != 0)
                altarWave = ModContent.Request<Texture2D>(Textures[CurWave-1]).Value;

            Texture2D altar = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCore").Value;

            Vector2 offscreen = new(Main.offScreenRange);
            Vector2 position = new Vector2(LivingCoreEvent.X * 16f, LivingCoreEvent.Y * 16f) - Main.screenPosition;

            if (CurWave != 0)
                spriteBatch.Draw(altarWave, position, Color.White);

            if (SpawnTimer >= 100)
            {
                Texture2D glow = ModContent.Request<Texture2D>("DIvergencyMod/Dusts/Particles/InvokedParticle").Value;
                Texture2D star = ModContent.Request<Texture2D>("DIvergencyMod/Dusts/Particles/InvokedParticle2").Value;

                float alpha = (SpawnTimer - 100) / 180f;

                foreach (Instance instance in CurWaveObject.enemies)
                {
                    Vector2 spawnPosition = LivingCoreEvent.Position + instance.SpawnOffset;

                    spriteBatch.Draw(glow, spawnPosition - Main.screenPosition, glow.Bounds, new Color(0.50f, 2.05f, 0.5f, 0) * alpha, 0f, glow.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

                    spriteBatch.Draw(star, spawnPosition - Main.screenPosition, star.Bounds, new Color(0.50f, 2.05f, 0.5f, 0) * alpha, 0f, star.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.Draw(altar, LivingCoreEvent.Position, Color.White);
        }

        public void Begin(int i, int j)
        {
            NPC.NewNPCDirect(null, LivingCoreEvent.Position + new Vector2(24f), ModContent.NPCType<LivingCoreEventHandler>());

            int left = i - Main.tile[i, j].TileFrameX / 18;
            int top = j - Main.tile[i, j].TileFrameY / 18;

            WorldGen.PlaceTile(left - 74, top - 10, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 11, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 12, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 13, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 14, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 15, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 16, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 17, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 18, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 19, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 74, top - 20, ModContent.TileType<LivingCoreWoodTile>());

            WorldGen.PlaceTile(left + 74, top, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 1, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 2, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 3, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 4, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 5, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 6, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 7, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top - 8, ModContent.TileType<LivingCoreWoodTile>());

            WorldGen.PlaceTile(left + 74, top + 1, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top + 2, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top + 3, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top + 4, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top + 5, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top + 6, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top + 7, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 74, top + 8, ModContent.TileType<LivingCoreWoodTile>());

            Kills = 0;
            Timer = 0;
            CurWave = 0;
            SpawnTimer = 0;
        }

        public void End()
        {
            Kills = 0;
            Timer = 0;
            CurWave = 0;
            SpawnTimer = 0;

            if (CurWave == getWaves() + 1)
            {
                if (!DownedHelper.ClearedAltar)
                {
                    Item.NewItem(null, LivingCoreEvent.Position, ModContent.ItemType<LivingCoreSword>());
                }

                NPC.SetEventFlagCleared(ref DownedHelper.ClearedAltar, -1);
                Main.NewText("Cleared!");
            }

            int left = LivingCoreEvent.X - LivingCoreEvent.Altar.TileFrameX / 18;
            int top = LivingCoreEvent.Y - LivingCoreEvent.Altar.TileFrameY / 18;

            WorldGen.KillTile(left - 74, top - 10);
            WorldGen.KillTile(left - 74, top - 11);
            WorldGen.KillTile(left - 74, top - 12);
            WorldGen.KillTile(left - 74, top - 13);
            WorldGen.KillTile(left - 74, top - 14);
            WorldGen.KillTile(left - 74, top - 15);
            WorldGen.KillTile(left - 74, top - 16);
            WorldGen.KillTile(left - 74, top - 17);
            WorldGen.KillTile(left - 74, top - 18);
            WorldGen.KillTile(left - 74, top - 19);
            WorldGen.KillTile(left - 74, top - 20);

            WorldGen.KillTile(left + 74, top);
            WorldGen.KillTile(left + 74, top - 1);
            WorldGen.KillTile(left + 74, top - 2);
            WorldGen.KillTile(left + 74, top - 3);
            WorldGen.KillTile(left + 74, top - 4);
            WorldGen.KillTile(left + 74, top - 5);
            WorldGen.KillTile(left + 74, top - 6);
            WorldGen.KillTile(left + 74, top - 7);

            WorldGen.KillTile(left + 74, top + 1);
            WorldGen.KillTile(left + 74, top + 2);
            WorldGen.KillTile(left + 74, top + 3);
            WorldGen.KillTile(left + 74, top + 4);
            WorldGen.KillTile(left + 74, top + 5);
            WorldGen.KillTile(left + 74, top + 6);
            WorldGen.KillTile(left + 74, top + 7);
            WorldGen.KillTile(left + 74, top + 8);
        }
    }
}
