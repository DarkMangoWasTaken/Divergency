﻿using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using System.Collections.Generic;
using System.Threading;
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

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Saw");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 50; // in SetStaticDefaults()
            NPCID.Sets.TrailingMode[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 30;
            NPC.defense = 2;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit2;
           // NPC.DeathSound = SoundID.LucyTheAxeTalk;

            NPC.aiStyle = -1;

            NPC.maxAI = 5;

            NPC.noGravity = true;

            NPC.knockBackResist = 0f;

            NPC.behindTiles = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f; //SpawnCondition.OverworldDay.Chance * 0.3f;
        }

        public override void AI()
        {
           
            if (NPC.ai[2] == 0)
                gameAI();
            else if (NPC.ai[2] == -1)
                jumpAI();
            else
            {
                for (int j = 0; j < 1; j++)

                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                    Vector2 ParticleLoc = NPC.Center + Main.rand.NextVector2Circular(8, 8f) * 10;
                    ParticleManager.NewParticle(ParticleLoc, ParticleLoc.DirectionTo(NPC.Center) * 1f, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 1f);


                }
                NPC.ai[2]++;
                NPC.velocity = new Vector2(0, 0);
            }

            return;
        }



        private void jumpAI()
        {
            // here it should rotate... maby...

            Vector2 move = new Vector2(NPC.ai[3], NPC.ai[4]);
            move.Normalize();
            move *= 10f;
            Vector2 moveWithCollision = Collision.TileCollision(NPC.position, move, 40, 40); // might also want to add fx new vector2(10, 10) to position and set 40 -> 20

            if (moveWithCollision.Length() < 0.001f)
            {;
                NPC.ai[2] = 0;
            }
            else
                NPC.position += moveWithCollision;
        }

        public override bool PreAI()
        {
        
            NPC.type = NPCID.BlazingWheel;
            return true;
        }
        public TrailRenderer prim;
        public TrailRenderer prim2;
        public int timer;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.NewText(timer);
            timer++;
            if (timer == 2)
            {
                Projectile.NewProjectileDirect(null, NPC.Center, NPC.velocity, ModContent.ProjectileType<SawVisuals>(), 0, 0);    
            }
            Main.spriteBatch.End();

            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/Trail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(60f) * (1f - p), (p) => NPC.GetAlpha(Color.LimeGreen) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = NPC.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(30f) * (1f - p), (p) => NPC.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = NPC.Size / 2f;
            }
            Main.spriteBatch.Begin();
            prim.Draw(NPC.oldPos);
            prim2.Draw(NPC.oldPos);
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, new Rectangle(0, 0, NPC.width, NPC.height), Color.White, NPC.rotation, new Vector2(NPC.width / 2, NPC.height / 2), NPC.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();

            Main.spriteBatch.Begin();
            return false;
        }

        private void gameAI()
        {
            
            if (NPC.ai[0] == 0f)
            {
                NPC.TargetClosest();
                NPC.directionY = 1;
                NPC.ai[0] = 1f;
            }
            int num810 = 6;
            if (NPC.ai[1] == 0f)
            {
                NPC.rotation += (float)(NPC.direction * NPC.directionY) * 0.13f;
                if (NPC.collideY)
                {
                    NPC.ai[0] = 2f;
                }
                if (!NPC.collideY && NPC.ai[0] == 2f)
                {
                    NPC.direction = -NPC.direction;
                    NPC.ai[1] = 1f;
                    NPC.ai[0] = 1f;
                }
                if (NPC.collideX)
                {
                    NPC.directionY = -NPC.directionY;
                    NPC.ai[1] = 1f;
                }
            }
            else
            {
                NPC.rotation -= (float)(NPC.direction * NPC.directionY) * 0.13f;
                if (NPC.collideX)
                {
                    NPC.ai[0] = 2f;
                }
                if (!NPC.collideX && NPC.ai[0] == 2f)
                {
                    NPC.directionY = -NPC.directionY;
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 1f;
                }
                if (NPC.collideY)
                {
                    NPC.direction = -NPC.direction;
                    NPC.ai[1] = 0f;
                }
            }
            NPC.velocity.X = num810 * NPC.direction;
            NPC.velocity.Y = num810 * NPC.directionY;

            Player owner = Main.player[NPC.target];

            Vector2 diff = NPC.Center - owner.Center;

            if (Collision.CanHit(NPC, owner))
            {
                if (NPC.ai[1] == 0)
                {
                    if (MathF.Abs(diff.X) < 6 && diff.Length() > 20f)
                    {
                        NPC.ai[2] = -21;
                        NPC.directionY *= -1;
                    }
                }
                else if (NPC.ai[1] == 1)
                {
                    if (MathF.Abs(diff.Y) < 6 && diff.Length() > 20f)
                    {
                        NPC.ai[2] = -21;
                        NPC.direction *= -1;
                    }
                }
            }

            if (NPC.ai[2] == -21)
            {
                diff.Normalize();

                NPC.ai[3] = 0;
                NPC.ai[4] = 0;

                if (MathF.Abs(diff.X) > MathF.Abs(diff.Y))
                    NPC.ai[3] = -diff.X;
                else
                    NPC.ai[4] = -diff.Y;
            }
        }
    }
    public class SawVisuals : ModProjectile
    {
        
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DD2BetsyFireball;

        public TrailRenderer prim;
        public TrailRenderer prim2;
        private int timer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("get phucked loser2");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25; // in SetStaticDefaults()
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 15; // The width of projectile hitbox
            Projectile.height = 15; // The height of projectile hitbox

            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Generic; // Is the projectile shoot by a ranged weapon?
            Projectile.timeLeft = 1000; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?

            Projectile.scale = 1f;
        }
        public override void AI()
        {

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC proj = Main.npc[i];

                if (proj.active && Projectile.Hitbox.Intersects(proj.Hitbox))
                {
                    Projectile.Center = proj.Center;
                    
                    Projectile.timeLeft = 10;
                }

            }
        }
        public override bool PreDraw(ref Color lightColor)
        {



            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/Trail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(30f) * (1f - p), (p) => Projectile.GetAlpha(Color.LimeGreen) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = Projectile.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(20f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
            prim2.Draw(Projectile.oldPos);
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

         
            return false;
        }
    } 
}



