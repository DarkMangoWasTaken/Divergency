using DivergencyMod.Base;
using DivergencyMod.Bosses.Forest;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
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

        public float AI_Timer;
        public float Beam_Timer;
        private int timer;
        private float rand = 100000;

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
            NPC.lifeMax = 200; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit2; // The sound the NPC will make when being hit.
            NPC.value = 90f; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 0.1f;
            NPC.scale = 0.93f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
      

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("They HATE normal attacks, will only use powerful and flashy attacks because the 'funny'. What an idiot.")
            });
        }

        public override void AI()
        {
            timer++;
            NPC.TargetClosest();
            if (timer == 60)
            {
                rand = Main.rand.NextFloat(40, 120);
            }
            Player player = Main.player[NPC.target];

            if(AI_Timer < 360)
            {
                AI_Timer++;
                NPC.Move(player.Center, player.Distance(NPC.Center) / rand);
            }
            else
            {
                Beam_Timer++;
                int dustType = DustID.TerraBlade;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height ,0, 0, 0, dustType);
                NPC.Move(player.Center, player.Distance(NPC.Center) / 300);

                if (Beam_Timer == 120)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * Main.rand.NextFloat(5, 10) * NPC.velocity.RotatedByRandom(MathHelper.ToRadians(5)), ModContent.ProjectileType<LivingFlameBlast>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                    }
                }
                if (Beam_Timer == 140)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * Main.rand.NextFloat(5, 10) * NPC.velocity.RotatedByRandom(MathHelper.ToRadians(5)), ModContent.ProjectileType<LivingFlameBlast>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                    }
                }
                if (Beam_Timer == 160)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * Main.rand.NextFloat(5, 10) * NPC.velocity.RotatedByRandom(MathHelper.ToRadians(5)), ModContent.ProjectileType<LivingFlameBlast>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                    }
                }
                if (Beam_Timer > 180)
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
            NPC.rotation = NPC.velocity.X * 0.2f;

        }

    }
}



