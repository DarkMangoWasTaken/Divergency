using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
            Walking,
            Scream
        }


        public float State = 0;

        public float AI_Timer;
        public float Timer;

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
            NPC.lifeMax = 30; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit2; // The sound the NPC will make when being hit.
            NPC.DeathSound = SoundID.LucyTheAxeTalk; // The sound the NPC will make when it dies.
            NPC.value = 90f; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 1f;
            NPC.scale = 0.9f;
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
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // we would like this NPC to spawn in the overworld.
            return SpawnCondition.OverworldDay.Chance * 0.3f;
        }
        public override void AI()
        {

            NPC.TargetClosest(true);

            //Main.NewText(AI_Timer);

            switch (State)
            {
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
                    if (NPC.frameCounter >= 6)
                    {
                        NPC.frameCounter = 0;
                       
                        NPC.frame.Y += frameHeight;

                            if (NPC.frame.Y >= frameHeight * 22)
                        {
                            NPC.frame.Y = 0;
                            State = (float)Phase.Walking;
                        }
                    }
                    break;

            }
           
        }
        private void Walking()
        {

            NPC.aiStyle = 3;
            AIType = NPCID.LarvaeAntlion;

            if (Main.player[NPC.target].Distance(NPC.Center) < 200f)
            {

                AI_Timer++;

                if (AI_Timer >= 300)
                {
                    CombatText.NewText(NPC.getRect(), Color.White, "AAAAAAAAA", true, false);
                    State = (float)Phase.Scream;
                    AI_Timer = 0;
                }

            }
        }

        private void Scream()
        {
       
            NPC.aiStyle = 0;

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

        
        }

    
}



