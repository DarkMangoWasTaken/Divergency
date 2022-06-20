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
        public float Timer;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            DisplayName.SetDefault("Bushling"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 9; // make sure to set this for your modNPCs.
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
            NPC.width = 46; // The width of the NPC's hitbox (in pixels)
            NPC.height = 50; // The height of the NPC's hitbox (in pixels)
            NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 30; // The amount of damage that this NPC deals
            NPC.defense = 2; // The amount of defense that this NPC has
            NPC.lifeMax = 70; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit2; // The sound the NPC will make when being hit.
            NPC.DeathSound = SoundID.LucyTheAxeTalk; // The sound the NPC will make when it dies.
            NPC.value = 90f; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 1f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
      

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Call me Jeffrey!!!")
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
   
            NPC.aiStyle = 3;
            AIType = NPCID.LarvaeAntlion;

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
            NPC.aiStyle = -1;

            if (AI_Timer == 1)
            {
                NPC.velocity = new Vector2(NPC.direction * 10, 0f);
            }
            if (NPC.velocity.X == 0)
            {
                 AI_Timer = 180;
            }
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
        public override bool PreAI()
        {
            if (NPC.aiStyle == -1 && AI_Timer >= 1)
            {
                Timer++;


                
                for (int i = 0; i < 1; i++)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (Timer == 3)

                        {
                            Gore.NewGore(null, NPC.Bottom, NPC.velocity, GoreID.TreeLeaf_Normal, 1f);
                            Timer = 0;
                        }
                    }
                }
                if (NPC.collideX && NPC.velocity.X != NPC.oldVelocity.X && Math.Abs(NPC.velocity.X) > 0.1f)
                {
                    NPC.velocity.X = -NPC.oldVelocity.X * 0.5f; // change 0.8 to a number you see fit.
                    NPC.velocity.Y += -5f;
                }
            }
            return true;
        }
        public override void OnKill()
        {
            int goreType = Mod.Find<ModGore>("JeffreyGoreBack").Type;
            int goreTypeAlt = Mod.Find<ModGore>("JeffreyGoreFront").Type;

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, NPC.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                Gore.NewGore(null, NPC.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.aiStyle == -1 && AI_Timer >= 1)
            {

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {

                    int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
                    int startY = (int)(frameHeight * NPC.frameCounter);
                    Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
                    Vector2 origin = sourceRectangle.Size() / 2f;

                    var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(drawColor) * (float)(((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
                    spriteBatch.Draw(texture, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, origin, NPC.scale, effects, 0f);
                }
            }
            return true;
        
        }

    }
}



