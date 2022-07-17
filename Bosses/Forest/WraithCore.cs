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
using DivergencyMod.Base;



namespace DivergencyMod.Bosses.Forest
{
    public class WraithCore : ModNPC
    {
        public static float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }

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
        private int frametimer;

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Living Core"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 7; // make sure to set this for your modNPCs.
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

              public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 6000  ;
            NPC.damage = 30;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.width = 50;
            NPC.height = 50;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
           // NPC.dontTakeDamage = true;
            NPC.friendly = false;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //NPC.dontTakeDamageFromHostiles = true;
            NPC.behindTiles = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
      

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("its orbin time")
            });
        }
        public NPC Parentnpc;
        public override void OnSpawn(IEntitySource source)
        {
           
            Parentnpc = Main.npc[(int)NPC.ai[0]];


        }
        public override void AI()

        {
            Vector2 vector; float speed; float turnResistance = 10f; bool toNPC = false;
            Vector2 WraithPos = Parentnpc.Center;

            speed = NPC.Distance(WraithPos) / 4;

            Vector2 moveTo = Parentnpc.Center;
            Vector2 move = moveTo - NPC.Center;
            float magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            move = (NPC.velocity * turnResistance + move) / (turnResistance + 0.5f);
            magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            NPC.velocity = move;
            NPC.TargetClosest(true);


          


        }
        public override void FindFrame(int frameHeight)
        {
            frametimer++;
            if (frametimer == 5)
            {
                NPC.frameCounter++;
                frametimer = 0;
                NPC.frame.Y += frameHeight;

            }
            NPC.frameCounter = 0;
            if (NPC.frame.Y >= frameHeight * 7)
                NPC.frame.Y = 0;

        }
        #region Movement towards main body
  


        #endregion

    }





}



