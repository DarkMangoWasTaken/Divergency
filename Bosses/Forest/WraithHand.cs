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
    public class WraithHand : ModNPC
    {

        private enum Phase
        {
            JumpTree,
            Walking,
            Scream
        }

        public static float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }
        public float State = 0;

        public float AI_Timer;
        public float Timer;
        private bool SoundPlayed;

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("WraithHand"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 1; // make sure to set this for your modNPCs.
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1000;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
            NPC.width = 10;
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
				new FlavorTextBestiaryInfoElement("Don't let him call his cousins")
            });
        }
        public NPC Parentnpc;
        public override void OnSpawn(IEntitySource source)
        {

            Parentnpc = Main.npc[(int)NPC.ai[0]];


        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];

    


            Vector2 vector; float speed; float turnResistance = 10f; bool toNPC = false;
            Vector2 WraithPos = Parentnpc.BottomLeft;

            speed = NPC.Distance(WraithPos);

            Vector2 moveTo = Parentnpc.BottomLeft;
            Vector2 move = moveTo - NPC.Top;
            float magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            move = (NPC.velocity * turnResistance + move) / (turnResistance + 1.1f);
            magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            NPC.velocity = move;
            NPC.TargetClosest();





        }
    }
        
      

    
}



