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
    public class Core_MiniBoss : ModNPC
    {

        public float State = 0;

        public float Phase;

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
            NPC.width = 168; // The width of the NPC's hitbox (in pixels)
            NPC.height = 140; // The height of the NPC's hitbox (in pixels)
            NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 0; // The amount of damage that this NPC deals
            NPC.defense = 2; // The amount of defense that this NPC has
            NPC.lifeMax = 2500; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit55; // The sound the NPC will make when being hit.
            NPC.value = 90f; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 0f;
            NPC.scale = 1;
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
				new FlavorTextBestiaryInfoElement("The guardian of the living core")
            });
        }

        private int teleport;

        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

         switch (Phase)
         {
                case 0:

                    NPC.velocity = new Vector2(0, -0.2f);

                    teleport ++;

                    if(teleport >= 300)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 87, NPC.velocity.X * 0.4f, NPC.velocity.Y * 0.4f, DustID.TerraBlade, default, 2f);
                            dust.noGravity = true;
                            dust.velocity /= 1f;
                        }

                        NPC.alpha = 255;

                        NPC.Center = player.Center + new Vector2(Main.rand.Next(-300,300), Main.rand.Next(-80, 120));

                        Phase = Main.rand.Next(3);
                    }

                    break;

                case 1:

                    if(teleport < 1)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 87, NPC.velocity.X * 0.4f, NPC.velocity.Y * 0.4f, DustID.TerraBlade, default, 2f);
                            dust.noGravity = true;
                            dust.velocity /= 1f;
                        }

                        CombatText.NewText(NPC.getRect(), Color.LightGreen, "Insert attack 1 here", true, false);
                        Phase = 0;
                        NPC.alpha = 0;
                        NPC.dontTakeDamage = false;
                    }
                    else
                    {
                        NPC.velocity = new Vector2(0, 0);
                        NPC.alpha = 255;
                        teleport -= 10;
                        NPC.dontTakeDamage = true;
                    }


                    break;


                case 2:

                    if (teleport < 1)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 87, NPC.velocity.X * 0.4f, NPC.velocity.Y * 0.4f, DustID.TerraBlade, default, 2f);
                            dust.noGravity = true;
                            dust.velocity /= 1f;
                        }

                        CombatText.NewText(NPC.getRect(), Color.LightGreen, "Insert attack 2 here", true, false);
                        Phase = 0;
                        NPC.alpha = 0;
                        NPC.dontTakeDamage = false;
                    }
                    else
                    {
                        NPC.velocity = new Vector2(0, 0);
                        NPC.alpha = 255;
                        teleport -= 10;
                        NPC.dontTakeDamage = true;
                    }

                    break;
         }




            NPC.TargetClosest(true);





        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction *-1;
        }

    }
}



