using DivergencyMod.Base;
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
    public class CoreBeamer : ModNPC
    {

        private enum Phase
        {
            JumpTree,
            Walking,
            Scream
        }


        public float State = 0;

        public float AI_Timer;
        public float Beam_Timer;

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Woodcore Guardian"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 1; // make sure to set this for your modNPCs.
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 38; // The width of the NPC's hitbox (in pixels)
            NPC.height = 44; // The height of the NPC's hitbox (in pixels)
            NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 30; // The amount of damage that this NPC deals
            NPC.defense = 2; // The amount of defense that this NPC has
            NPC.lifeMax = 240; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit2; // The sound the NPC will make when being hit.
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
				new FlavorTextBestiaryInfoElement("A flying bundle of living core energy, be carefull or it unleases it, melting anything near it.")
            });
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            if(AI_Timer < 360)
            {
                AI_Timer++;
                NPC.Move(player.Center, 5f);
            }
            else
            {
                Beam_Timer++;
                int dustType = DustID.TerraBlade;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height ,0, 0, 0, dustType);
                NPC.velocity = new Vector2(0, 0);
                
                if(Beam_Timer == 120)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 1).RotatedByRandom(5), ProjectileID.BombSkeletronPrime, 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                }
                if(Beam_Timer > 240)
                {
                    AI_Timer = 0;
                    Beam_Timer = 0;
                }


            }
            

           


            NPC.TargetClosest(true);


           


        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
            NPC.rotation = NPC.velocity.X * 0.1f;

        }

    }
}



