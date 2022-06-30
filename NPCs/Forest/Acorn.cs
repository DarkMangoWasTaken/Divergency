using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;


namespace DivergencyMod.NPCs.Forest
{
    public class Acorn : ModNPC
    {

        private enum Phase
        {
            JumpTree,
            Walking,
            Scream
        }


        public float State = 0;

        public float AI_Timer;
        public float Timer;
        private bool SoundPlayed;

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Acorn dude idk"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 22; // make sure to set this for your modNPCs.
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Position = new Vector2(0f, 0f),
                PortraitPositionXOverride = 0f,

                PortraitPositionYOverride = 0f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);

        }
        public override void SetDefaults()
        {
            NPC.width = 20; // The width of the NPC's hitbox (in pixels)
            NPC.height = 25; // The height of the NPC's hitbox (in pixels)
            NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 30; // The amount of damage that this NPC deals
            NPC.defense = 2; // The amount of defense that this NPC has
            NPC.lifeMax = 15; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit2; // The sound the NPC will make when being hit.
            NPC.DeathSound = SoundID.LucyTheAxeTalk; // The sound the NPC will make when it dies.
            NPC.value = 90f; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 0.7f;
            NPC.scale = 0.93f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
      

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Don't let him call his cousins")
            });
        }

        public override void AI()
        {

            NPC.TargetClosest(true);

            //Main.NewText(AI_Timer);

            switch (State)
            {
                case (float)Phase.JumpTree:
                    JumpTree();
                    break;
                case (float)Phase.Walking:
                    Walking();
                    break;
                case (float)Phase.Scream:
                    Scream();
                    break;

            }


        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;

            switch (State)
            {
                case (float)Phase.Walking:
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 5)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;
                        if (NPC.frame.Y >= frameHeight * 10)
                            NPC.frame.Y = 0;
                    }
                    break;
                case (float)Phase.Scream:
       
                    
                        NPC.frameCounter++;

                    if (!SoundPlayed)
                    {
                        SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/screm")

                        {
                            Volume = 1f,
                            MaxInstances = 5,
                            
                        });
                        SoundPlayed = true;
                    }

                    if (NPC.frameCounter >= 8)
                    {
                        NPC.frameCounter = 0;
                       
                        NPC.frame.Y += frameHeight;

                            if (NPC.frame.Y >= frameHeight * 22)
                            { 
                            NPC.frame.Y = 0;
                            State = (float)Phase.Walking;
                            SoundPlayed = false;
                            }
                    }
                    break;

            }
           
        }
        private void JumpTree()
        {
            AI_Timer++;

            NPC.aiStyle = 3;
            AIType = NPCID.DesertGhoul;
            if (AI_Timer == 1)
            {
                NPC.velocity.Y -= 7;
                State = (float)Phase.Walking;
                for (int i = 0; i < 15; i++)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Vector2 perturbedSpeed = NPC.velocity.RotatedByRandom(MathHelper.ToRadians(20));

                        float scale = 1f - (Main.rand.NextFloat() * 0.75f);
                        perturbedSpeed *= scale;

                        Dust dust = Dust.NewDustDirect(NPC.position - NPC.velocity, NPC.width, NPC.height, DustID.WoodFurniture, 0, 0, 100, default, 2f);
                        dust.noGravity = true;
                        dust.velocity *= 2f;
                        dust = Dust.NewDustDirect(NPC.position - NPC.velocity, NPC.width, NPC.height, DustID.WoodFurniture, 0f, 0f, 1000, default, 2f);
                        Gore.NewGore(null, NPC.Center, NPC.velocity, GoreID.TreeLeaf_Normal, 1.1f);
                    }
                }

            }

        }
        private void Walking()
        {
            AI_Timer++;
            NPC.knockBackResist = 0.7f;
            NPC.aiStyle = 3;
            AIType = NPCID.DesertGhoul;

            if (Main.player[NPC.target].Distance(NPC.Center) < 200f)
            {



                if (AI_Timer >= 300)
                {
                    //CombatText.NewText(NPC.getRect(), Color.White, "AAAAAAAAA", true, false);
                    State = (float)Phase.Scream;
                    AI_Timer = 0;
                }

            }
        }

        private void Scream()
        {

            NPC.aiStyle = 0;
            AI_Timer++;
            if (AI_Timer == 20)
            {
                for (int i = 0; i < 2; i++)
                { 

                        Vector2 pos = NPC.position;
                        for (i = -5; i <= 5; i++)
                        {
                            bool success = TryFindTreeTop(pos + new Vector2(i * 16f, 0f), out Vector2 result);
                            NPC.NewNPC(null, (int)(result.X + Main.rand.NextFloat(-32f, 33f)), (int)(result.Y + Main.rand.NextFloat(-64f, 1f)), ModContent.NPCType<Acorn>());

                        }
                } 
            }

            NPC.knockBackResist = -1;

        }
      
        




        public override bool PreAI()
        {
          
          
            return true;
        }
        public override void OnKill()
        {
            int goreType = Mod.Find<ModGore>("JeffreyGoreBack").Type;
            int goreTypeAlt = Mod.Find<ModGore>("JeffreyGoreFront").Type;

            if (Main.netMode != NetmodeID.Server)
            {
                //Gore.NewGore(null, NPC.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                //Gore.NewGore(null, NPC.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
            }
        }
        public bool TryFindTreeTop(Vector2 position, out Vector2 result)
        {
            if (Main.tile[(int)position.X / 16, (int)position.Y / 16].TileType == TileID.Trees)
            {
                // Origin position, in tile format.
                int x = (int)(position.X / 16);
                int y = (int)(position.Y / 16);

                // Position being checked;
       
                    int checkX = x;
                    int checkY = y;

                // Checking up to a maximum of 30 tiles.
                for (int b = 0; b < 30; b++)
                {
                    // If this position is in the world, and if the tile is a Tree tile.
                    if (WorldGen.InWorld(checkX, y) && Main.tile[checkX, checkY].TileType == TileID.Trees)
                    {
                        // Checking if the tile's frames are within the range of tile frames used for the invisible tree top tiles.
                        if (Main.tile[checkX, checkY].TileFrameX == 22 && Main.tile[checkX, checkY].TileFrameY >= 198)
                        {
                            //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Yellow, null);
                            result = new Vector2(checkX * 16, checkY * 16);
                            return true;
                        }
                        // Otherwise, its a success, since it's still a tree tile. Just not the one we're looking for.
                        //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Green, null);
                        checkY--;
                    }
                    else
                    {
                        // If the tile isn't what we're looking for and since we're only iterating upwards, logically this means its useless to continue.
                        //Dust.QuickDustLine(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 5f, Color.Red);
                        //Dust.QuickDustLine(new Vector2(checkX * 16, (checkY * 16) + 16), new Vector2((checkX * 16) + 16, checkY * 16), 5f, Color.Red);
                        break;
                    }
                }
            }
            
            result = default;
            return false;
        }

    }


    
}



