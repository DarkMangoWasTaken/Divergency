using DivergencyMod.Base;
using DivergencyMod.Bosses.Forest;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.Tiles.LivingTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
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
        private float rand2 = 0;

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Coreling"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 10; // make sure to set this for your modNPCs.
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
            NPC.knockBackResist = 1f;
            NPC.scale = Main.rand.NextFloat(0.75f,1.05f);
            NPC.noGravity = true;
            NPC.noTileCollide = false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
      

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("They HATE normal attacks, will only use fancy and flashy attacks because of the 'funny'. What an idiot.")
            });
        }

        public override void AI()
        {
            timer++;

            NPC.TargetClosest();
            if (timer == 60)
            {
                rand = Main.rand.NextFloat(40, 120);
                rand2 = Main.rand.NextFloat(2, 4);
            }
            Player player = Main.player[NPC.target];

            if(AI_Timer < 360)
            {
                AI_Timer++;
                NPC.Move(player.Center, NPC.Distance(player.Center) / rand);
            }
            else
            {
                NPC.velocity *= 0.98f;

                if (Main.rand.NextBool(3))
                {
                    Beam_Timer++;

                }
                for (int j = 0; j < 1; j++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    ParticleManager.NewParticle(NPC.Center, speed * 20, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 1f);


                }
                //ParticleManager.NewParticle(NPC.Center -  Main.rand.NextVector2Circular(-10f, 10f), new Vector2(0, 0), ParticleManager.NewInstance<CoreLoadParticle>(), Color.Purple, 0.3f, NPC.whoAmI, NPC.whoAmI);
                //ParticleManager.NewParticle(NPC.Center + Main.rand.NextVector2Circular(-10f, 10f), new Vector2(0, 0), ParticleManager.NewInstance<CoreLoadParticle>(), Color.Purple, 0.3f, NPC.whoAmI,NPC.whoAmI);

                if (Beam_Timer == 10)
                {
                    playAnim = true;
                    frame = 4;
                }
               
                if (Beam_Timer == 100)
                {
                    playAnim = true;
                    frame = 4;
                }
             
                if (Beam_Timer == 190)
                {
                    playAnim = true;
                    frame = 4;

                }
                
                if (Beam_Timer > 220)
                {
                    AI_Timer = 0;
                    Beam_Timer = 0;
                    playAnim = false;
                    frame = 0;

                }


            }
            

           


            NPC.TargetClosest(true);


           


        }
        private int frame = 0;
        private int frameTimer = 0;
        private bool Charge;
        private bool playAnim;
        private int timer2;
        public int ShootCount = 0;

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[NPC.target];

            NPC.spriteDirection = -NPC.direction;
            NPC.rotation = NPC.velocity.X * 0.2f;
            NPC.frame.Y = frameHeight * frame;
            if (!playAnim)
            {
                if (frame < 3)
                {
                    frameTimer++;
                    if (frameTimer % 8 == 0)
                    {
                        frame++;
                    }
                }
                else
                {
                    frameTimer++;
                    if (frameTimer % 8 == 0)
                    {
                        frame = 0;
                        frameTimer = 0;
                    }
                }
            }
            if (playAnim)
            {

              
                if (frame < 10 && frame > 3)
                {
                    frameTimer++;
                    if (frameTimer % 8 == 0)
                    {
                        frame++;
                    }
                }
            
                if (frame == 7)
                {
                    if (ShootCount == 0)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center).RotatedByRandom(MathHelper.ToRadians(5)) * Main.rand.NextFloat(9, 10), ModContent.ProjectileType<LivingFlameBlast>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                            ParticleManager.NewParticle(NPC.Center, new Vector2(0, 0), ParticleManager.NewInstance<LivingCoreExplosionParticle>(), Color.Purple, 1f);
                            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 2;
                            Beam_Timer++;
                            playAnim = false;
                            frame = 0;


                        }
                        ShootCount++;
                        NPC.velocity = NPC.Center.DirectionFrom(player.Center) * 1;

                    }
                    else if (ShootCount == 1)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center).RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(5, 8), ModContent.ProjectileType<LivingFlameBlast>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                            ParticleManager.NewParticle(NPC.Center, new Vector2(0, 0), ParticleManager.NewInstance<LivingCoreExplosionParticle>(), Color.Purple, 1f);
                            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 2;
                            Beam_Timer++;
                            playAnim = false;
                            frame = 0;

                        }
                        NPC.velocity = NPC.Center.DirectionFrom(player.Center) * 2;



                    }
                    else if (ShootCount == 2)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center).RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(4, 6), ModContent.ProjectileType<LivingFlameBlast>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                            ParticleManager.NewParticle(NPC.Center, new Vector2(0, 0), ParticleManager.NewInstance<LivingCoreExplosionParticle>(), Color.Purple, 0.8f);
                            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 4;
                            Beam_Timer++;
                            playAnim = false;
                            frame = 0;
                        }
                    }
                    NPC.velocity = NPC.Center.DirectionFrom(player.Center) * 4;


                }

            }
       



            

        }
        public override void OnKill()
        {
            Projectile.NewProjectile(null, NPC.Center, new Vector2(0, 0), ModContent.ProjectileType<AltarKiller>(), 0, 0);
        }

    }
    public class AltarKiller : ModProjectile
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCore";

        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 10;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC altar = Main.npc[i];
                if (altar.type == ModContent.NPCType<AltarHandler1>())
                {
                    Projectile.Center = altar.Center;

                    if (Projectile.Hitbox.Intersects(altar.Hitbox))
                    {
                        if (altar.life == 1)
                        {
                            altar.dontTakeDamage = false;
                            altar.immortal = false;
                            altar.StrikeNPC(1, 0, 0);
                          
                        }
                        else
                        {
                            altar.life--;

                        }
                        Projectile.Kill();
                    }
                }
            }
            
        }
    }
}



