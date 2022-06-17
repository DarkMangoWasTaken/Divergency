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
        public  float AI_StateDiv = 0;

        public float AI_Timer;
        public float AI_Timer2;
        public float AI_Timer3;
        public float AI_Idk =>  NPC.ai[2];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flutter Slime"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 9; // make sure to set this for your modNPCs.

        }
        public override void SetDefaults()
        {
            NPC.width = 43; // The width of the NPC's hitbox (in pixels)
            NPC.height = 43; // The height of the NPC's hitbox (in pixels)
            NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 30; // The amount of damage that this NPC deals
            NPC.defense = 2; // The amount of defense that this NPC has
            NPC.lifeMax = 100; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit1; // The sound the NPC will make when being hit.
            NPC.DeathSound = SoundID.NPCDeath1; // The sound the NPC will make when it dies.
            NPC.value = 25f; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 1f;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // we would like this NPC to spawn in the overworld.
            return SpawnCondition.OverworldDaySlime.Chance * 0.45f;
        }
        public override void AI()
        {
            
            NPC.TargetClosest(true);

            Main.NewText(AI_Timer);
           
            // The NPC starts in the asleep state, waiting for a player to enter range
            switch (AI_StateDiv)
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
            NPC.aiStyle =3;
           
            if (Main.player[NPC.target].Distance(NPC.Center) < 250f)
            {

                AI_Timer++;

                if (AI_Timer >= 3)
                {
                    CombatText.NewText(NPC.getRect(), Color.White, "Notice", true, false);
                    AI_StateDiv = (float)Phase.Notice;
                    AI_Timer = 0;
                }

            }
        }
        private void Notice()
        {

            AI_Timer++;
            NPC.aiStyle = 0;
            if (AI_Timer >= 180)
            {

                CombatText.NewText(NPC.getRect(), Color.White, "Spin", true, false);

                AI_StateDiv = (float)Phase.Spin;
                AI_Timer = 0;
            }

        }
        private void Spin()
        {
            AI_Timer++;
            NPC.aiStyle = 26;


            NPC.knockBackResist = -1;

            if (AI_Timer >= 180)
            {
                CombatText.NewText(NPC.getRect(), Color.White, "Dizzy", true, false);

                AI_StateDiv = (float)Phase.Dizzy;
                AI_Timer = 0;

                NPC.knockBackResist = 1f;
            }
        }
        private void Dizzy()
        {
            AI_Timer++;
            NPC.aiStyle = 0;
            if (AI_Timer >= 120)
            {
                CombatText.NewText(NPC.getRect(), Color.White, "Walking", true, false);

                AI_StateDiv = (float)Phase.Walking;
                AI_Timer = 0;
            }
        }

    }
}



