using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;


namespace DivergencyMod.NPCs.Forest
{
    public class LivingCoreSaw : ModNPC
    {
        private Dictionary<int, Vector2> rotations = new Dictionary<int, Vector2>()
        {
            {0, new Vector2(1f, 0f)}, // right
            {1, new Vector2(0f, 1f)}, // down
            {2, new Vector2(-1f, 0f)}, // left
            {3, new Vector2(0f, -1f)}, // up
        };

        private Vector2 collisionOffset = new Vector2(12, 12);
        private int collisionWidth = 22;
        private int collisionHeight = 22;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Saw");
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.aiStyle = -1;
            NPC.damage = 30;
            NPC.defense = 2;
            NPC.lifeMax = 1;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.LucyTheAxeTalk;

            NPC.aiStyle = -1; //Terraria.ID.NPCAIStyleID.BlazingWheel; this bs makes a little light :/

            NPC.noGravity = true;

            NPC.velocity = new Vector2(0f, 0f);
            NPC.ai[0] = 0; // rotation
            NPC.ai[1] = 0; // did hit ground 0 = false

            NPC.direction = (Main.rand.Next(3)*2)-1; // left(-1)/right(1)

            NPC.behindTiles = true;

            NPC.dontTakeDamage = true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f; //SpawnCondition.OverworldDay.Chance * 0.3f;
        }

        public override void AI()
        {
            NPC.rotation += 0.15f * NPC.direction;

            // Mod.Logger.Info(NPC.ai[2]);
            if (NPC.ai[2] == 0)
            {
                baseAI();
            }
            else if (NPC.ai[2] != -1)
            {
                // some rotation maby, also spawn particles NPC.rotation += 0.35f * NPC.direction;
                NPC.ai[2]++;
            }
            else if (NPC.ai[2] == -1)
            {
                jumpAI();
            }
        }
        private void jumpAI()
        {
            Vector2 move = rotations[((int)NPC.ai[0] - 1 + 4) % 4] * 6f;

            Vector2 moveWithCollision = Collision.TileCollision(NPC.position + collisionOffset, move, collisionWidth, collisionHeight);

            if (moveWithCollision.Length() < 0.001f)
            {
                NPC.ai[2] = 0;

                NPC.ai[0] = (NPC.ai[0] + 2) % 4; // flip direction
            }
            else
                NPC.position += moveWithCollision;
        }

        private void baseAI()
        {
            Vector2 orgVel = rotations[(int)NPC.ai[0]] * 6f;
            Vector2 orgVel2 = rotations[((int)NPC.ai[0] + 1 + 4) % 4] * 6f;

            orgVel *= NPC.direction;

            Vector2 vel = Collision.TileCollision(NPC.position + collisionOffset, orgVel, collisionWidth, collisionHeight);
            Vector2 vel2 = Collision.TileCollision(NPC.position + collisionOffset, orgVel2, collisionWidth, collisionHeight);

            NPC.position += vel + vel2;
            // Mod.Logger.Info(vel2.Length());

            if (vel2.Length() < 0.001f)
                NPC.ai[1] = 1;
            else
            {
                if (NPC.ai[1] == 1)
                    NPC.ai[0] += NPC.direction;

                NPC.ai[1] = 0;
            }

            if (vel.Length() < 0.001f)
                NPC.ai[0]++;

            Player plr = Main.player[NPC.target];

            Vector2 diff = plr.Center - NPC.Center;

            orgVel2 *= -1; // only for check

            Mod.Logger.Info(diff + " | " + orgVel2);

            if (Collision.CanHit(NPC, plr))
            {
                if (orgVel2.X == 0)
                {
                    if (MathF.Abs(diff.X) < 10 && (diff.Y < -16 && orgVel2.Y < 0 || diff.Y > 16 && orgVel2.Y > 0)) // 40 is the width of 'i will attack'
                        NPC.ai[2] = -21;
                }
                else if (orgVel2.Y == 0)
                {
                    if (MathF.Abs(diff.Y) < 10 && (diff.X < -16 && orgVel2.X < 0 || diff.X > 16 && orgVel2.X > 0)) // 40 is the width of 'i will attack'
                        NPC.ai[2] = -21;
                }
            }

            NPC.ai[0] = (NPC.ai[0] + 4) % 4;
        }
    }
}



