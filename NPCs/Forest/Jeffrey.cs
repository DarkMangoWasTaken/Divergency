using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;


namespace DivergencyMod.NPCs.Forest
{
    public class Jeffrey : ModNPC
    {

        private enum Phase
        {
            Walking,
            Notice,
            Spin,
            Dizzy
        }
        public ref float AI_State => ref NPC.ai[0];
        public ref float AI_Timer => ref NPC.ai[1];
        public ref float AI_S => ref NPC.ai[2];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flutter Slime"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 9; // make sure to set this for your modNPCs.

        }
        public override void SetDefaults()
        {
            NPC.width = 40; // The width of the NPC's hitbox (in pixels)
            NPC.height = 40; // The height of the NPC's hitbox (in pixels)
            NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 30; // The amount of damage that this NPC deals
            NPC.defense = 2; // The amount of defense that this NPC has
            NPC.lifeMax = 100; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit1; // The sound the NPC will make when being hit.
            NPC.DeathSound = SoundID.NPCDeath1; // The sound the NPC will make when it dies.
            NPC.value = 25f; // How many copper coins the NPC will drop when killed.
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // we would like this NPC to spawn in the overworld.
            return SpawnCondition.OverworldDaySlime.Chance * 0.45f;
        }
        public override void AI()
        {

            
           
            // The NPC starts in the asleep state, waiting for a player to enter range
            switch (AI_State)
            {
                case (float)Phase.Walking:
                    Walking();
                    break;
                case (float)Phase.Notice:
                    Notice();
                    break;
                case (float)Phase.Spin:
                    Spin();
                    break;
                case (float)Phase.Dizzy:
                    Dizzy();
                    break;
            }
     

        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;

            NPC.frameCounter++;
            if (NPC.frameCounter >= 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * 9)
                    NPC.frame.Y = 0;
            }
        }
        private void Walking()
        {
            NPC.aiStyle = NPCID.Skeleton;
        }
        private void Notice()
        {
            
        }
        private void Spin()
        {
        }
        private void Dizzy()
        {
        }
    }
}



