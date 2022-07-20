using DivergencyMod.Base;
using DivergencyMod.Dusts.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace DivergencyMod.Bosses.Forest
{
    public class LivingCoreWraith : ModNPC
    {

        private enum Phase
        {
            Float,
            Dash,
            AfterDash,
            SpreadFire,
            SpreadFireCircle,
            FireBarrage,
            FlameBreathFloat,
            FlameDash,
            DetectPlayerOldPosition,
            SpreadFireCircleAfterDash,
            FlameBreathAbove
              
        }

        private int frame = 0;
        private int frameTimer = 0;
        public float State = 0;
        public bool ParticleShoot;
        public float AITimer;
        public float Timer;
        private bool SoundPlayed;
        private float frametimer;
        private int DashContinue = 3;
        public int damage;
        private Vector2 oldPlayerCenter = Vector2.Zero;
        Vector2 toPlayer = Vector2.Zero;


        public override void SetStaticDefaults()
        {
            //NPCID.Sets.TrailCacheLength[NPC.type] = 5;
          //  NPCID.Sets.TrailingMode[NPC.type] = 0;

            DisplayName.SetDefault("Living Core Wraith"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 14; // make sure to set this for your modNPCs.
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 3500;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 3, 0, 0);
            // NPC.dontTakeDamage = true;
            NPC.friendly = false;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //NPC.dontTakeDamageFromHostiles = true;
            NPC.behindTiles = false;
            
            Music = MusicLoader.GetMusicSlot("DivergencyMod/Sounds/Music/LivingCoreWraithTheme");

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
      

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Core Wraithy")
            });
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<WraithCore>(), 0, NPC.whoAmI);
            NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<WraithHand>(), 0, NPC.whoAmI);
      
            if (Main.masterMode)
            {
                NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<WraithHand2>(), 0, NPC.whoAmI);

            }

           

        }
        public override void AI()
        {
            if (Phase2)
            {
                damage = 40;
            }
            else
            {
                damage = 30;
            }
            NPC.TargetClosest();
            switch (State)
            {
                case (float)Phase.Float:
                    Float();
                    break;
                case (float)Phase.Dash:
                    Dash();
                    break;
                case (float)Phase.AfterDash:
                    Afterdash();
                    break;
                case (float)Phase.SpreadFire:
                    SpreadFire();
                    break;
                case (float)Phase.SpreadFireCircle:
                    SpreadFireCircle();
                    break;
                case (float)Phase.FireBarrage:
                    FireBarrage();
                    break;
                case (float)Phase.FlameBreathFloat:
                    FlameBreathFloat();
                    break;
                case (float)Phase.FlameDash:
                    FlameDash();
                    break;
                case (float)Phase.DetectPlayerOldPosition:
                    DetectPlayerOldPosition();
                    break;
                case (float)Phase.SpreadFireCircleAfterDash:
                    SpreadFireCircleAfterDash();
                    break;
                case (float)Phase.FlameBreathAbove:
                    FlameBreathAbove();
                    break;
            }
            if (NPC.life <= NPC.lifeMax / 2)
            {
                Phase2 = true;
            }

        }



        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.frame.Y = frameHeight * frame;

            switch (State)
            {
                case (int)Phase.Float:
                    if (State == (float)Phase.Float)
                    {


                        // Float animation
                        // Frames 0 - 11
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
                    break;
                case (int)Phase.FireBarrage:
                    if (State == (float)Phase.FireBarrage)
                    {
                        Timer++;

                        if (Timer == 1)
                        {
                            frame = 4;

                        }



                        if (frame < 13 && frame > 3)
                        {
                            frameTimer++;
                            if (frameTimer % 4 == 0)
                            {
                                frame++;
                            }
                        }
                        else
                        {
                            frameTimer++;
                            if (frameTimer % 3 == 0)
                            {
                                frame = 4;
                                frameTimer = 0;
                                Timer = 0;
                            }
                        }
                    }
                    break;
                case (int)Phase.Dash:
                    if (State == (float)Phase.Dash)
                    {
                        frame = 9;
                    }
                    break;
                case (int)Phase.SpreadFire:
                    if (State == (float)Phase.SpreadFire)
                    {
                        Timer++;

                        if (Timer == 1)
                        {
                            frame = 4;

                        }



                        if (frame < 13 && frame > 3)
                        {
                            frameTimer++;
                            if (frameTimer % 4 == 0)
                            {
                                frame++;
                            }
                        }
                        else
                        {
                            frameTimer++;
                            if (frameTimer % 4 == 0)
                            {
                                frame = 4;
                                frameTimer = 0;
                                Timer = 0;
                            }
                        }
                    }
                    break;
                case (int)Phase.SpreadFireCircle:
                    if (State == (float)Phase.SpreadFireCircle)
                    {
                        Timer++;

                        if (Timer == 1)
                        {
                            frame = 4;

                        }

                        if (frame < 13 && frame > 3)
                        {
                            frameTimer++;
                            if (frameTimer % 8 == 0)
                            {
                                frame++;
                            }
                        }
                        else
                        {
                            if (frameTimer % 8 == 0)
                            {
                                frameTimer = 0;
                                Timer = 0;
                            }
                        }
                    }
                    break;

                case (int)Phase.SpreadFireCircleAfterDash:
                    if (State == (float)Phase.SpreadFireCircleAfterDash)
                    {
                        Timer++;

                        if (Timer == 1)
                        {
                            frame = 4;

                        }



                        if (frame < 13 && frame > 3)
                        {
                            frameTimer++;
                            if (frameTimer % 2 == 0)
                            {
                                frame++; 
                            }
                        }
                        else
                        {
                            frameTimer++;
                            if (frameTimer % 2 == 0)
                            {
                                frame = 4;
                                frameTimer = 0;
                                Timer = 0;
                            }
                        }
                    }
                    break;

                case (int)Phase.FlameDash:
                    if (State == (float)Phase.FlameDash)
                    {
                        Timer++;

                        if (Timer == 1)
                        {
                            frame = 4;

                        }



                        if (frame < 9 && frame > 3)
                        {
                            frameTimer++;
                            if (frameTimer % 4 == 0)
                            {
                                frame++;
                            }
                        }
                        else
                        {
                            frameTimer++;
                            if (frameTimer % 4 == 0)
                            {
                                frame = 9;
                                frameTimer = 0;
                                Timer = 0;
                            }
                        }
                    }
                    break;
                case (int)Phase.FlameBreathAbove:
                    if (State == (float)Phase.FlameBreathAbove)
                    {
                        Timer++;
                        frame = 9;
                    }
                    break;
                case (int)Phase.FlameBreathFloat:
                    if (State == (float)Phase.FlameBreathFloat)
                    {
                        Timer++;
                        frame = 9;
                    }
                    break;

            }
        }




        private void Float()
        {
            AITimer++;
            Player player = Main.player[NPC.target];
            if (Phase2)
            {
                NPC.Move(player.Center, 4f);
            }
            else
            {
                NPC.Move(player.Center, 2f);

            }
            if (AITimer >= 300)
            {
                WeightedRandom<Phase> phase = new WeightedRandom<Phase>();
                phase.Add(Phase.FlameDash, 1f);
                phase.Add(Phase.SpreadFire, 1f);
                phase.Add(Phase.SpreadFireCircle, 1f);
                phase.Add(Phase.Dash, 1f);
                phase.Add(Phase.FireBarrage, 1f);
                phase.Add(Phase.FlameBreathFloat, 1f);
                phase.Add(Phase.FlameBreathAbove, 1f);
                

                if (Phase2)
                {
                    DashContinue = 3;
                }
                else
                {
                    DashContinue = 2;

                }

                State = (float)phase.Get();
                //State = (float)Phase.FlameDash; //<-----------------------------
                AITimer = 0;
                oldPlayerCenter = player.Center;
                Vector2 toPlayer = NPC.velocity;




            }
        }
        private void Dash()
        {
            Player player = Main.player[NPC.target];

            NPC.velocity = new Vector2(NPC.direction * 30, 0f);
            State = (float)Phase.AfterDash;



        }
        private void Afterdash()
        {
            AITimer++;
            NPC.velocity *= 0.9f;

            Player player = Main.player[NPC.target];
            if (AITimer == 30)
            {
                DashContinue--;

                NPC.velocity = NPC.DirectionTo(player.Center) * 20;

                State = (float)Phase.SpreadFire;
                AITimer = 0;
            }



        }
        private void SpreadFire()
        {
            AITimer++;
            Player player = Main.player[NPC.target];
            NPC.velocity *= 0.9f;
            if (AITimer == 20)
            {
                for (int i = 0; i < 1; i++)
                {

                    Vector2 speed = NPC.DirectionTo(player.Center);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.2f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.4f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.6f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.8f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.2f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.4f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.6f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.8f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);



                }


            }
            if (AITimer == 35)
            {
                if (DashContinue <= 0)
                {
                    State = (float)Phase.Float;
                }
                else
                {
                    State = (float)Phase.AfterDash;

                }
                AITimer = 0;
            }

        }
        private void SpreadFireCircle()
        {
            AITimer++;
            Player player = Main.player[NPC.target];
            NPC.velocity *= 0.9f;
            if (AITimer == 5)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);
                    Projectile.NewProjectile(null, NPC.Center, speed * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed * -10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

      





                }
                
            }
            if (AITimer == 38)
            {
                AITimer = 0;
                State = (float)Phase.Float;

            }   
        }
        private void SpreadFireCircleAfterDash()
        {
            AITimer++;
            Player player = Main.player[NPC.target];
            NPC.velocity *= 0.9f;
            if (AITimer == 20)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);
                    Projectile.NewProjectile(null, NPC.Center, speed * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(7f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed * -15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-7f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);


                }

            }
            if (AITimer == 30)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(10f) *13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(12f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(14f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(16f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(18f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);


                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-10f) *13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-12f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-14f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-16f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-18f) * 13,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                   

                }

            }
            if (AITimer == 40)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);
                    Projectile.NewProjectile(null, NPC.Center, speed * 20,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed * -20,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                 
                }

            }
            if (AITimer == 50)
            {
                AITimer = 0;
                State = (float)Phase.Float;

            }
        }
        private void FireBarrage()
        #region barrage
        {
            NPC.velocity *= 0.9f;


            AITimer++;
            Player player = Main.player[NPC.target];
            Vector2 speed = NPC.DirectionTo(player.Center);
            if (AITimer == 5)
            {
                NPC.velocity *= 0;
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);
                    //telegraphing ^
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.3) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.3f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                }
            }
            if (AITimer == 35)
            {
                NPC.velocity.Y -= 8;
            }
            if (AITimer == 50)
            {
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);
                    //telegraphing ^
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.3) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.3f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);


                }

                // State = (float)Phase.Float;

            }
            if (AITimer == 80)
            {
                NPC.velocity.Y += 26;

            }
            if (AITimer == 95)
            {
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);
                    //telegraphing ^
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.3) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.3f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), damage, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);


                }



            }
            if (AITimer == 110)
            {
                AITimer = 0;
                State = (float)Phase.Float;
            }



        }
        #endregion
        public int timer2;
        private bool Phase2;
        public int Timer3;

        private void FlameBreathFloat()
        {
            AITimer++;
            timer2++;

            Player player = Main.player[NPC.target];
            if (timer2 == 5 && AITimer <= 100)
            {
                Vector2 speed = NPC.DirectionTo(player.Center);
                timer2 = 0;
                //Projectile.NewProjectile(null, NPC.Center, NPC.velocity * 15, ModContent.ProjectileType<LivingFlameCage>(), damage, 0, Main.myPlayer);

            }
            if (AITimer >= 100)
            {
                if (Phase2)
                {
                    NPC.Move(player.Center, 6f);
                }
                else
                {
                    NPC.Move(player.Center, 4.5f);

                }

                Vector2 speed = NPC.DirectionTo(player.Center);
                Timer3++;

                if (Timer3 == 5)
                {
                    ParticleShoot = true;
                    if (ParticleShoot)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            ParticleManager.NewParticle(NPC.Center, speed * 15, ParticleManager.NewInstance<CoreParticle>(), Color.Purple, 0.85f);
                            Projectile.NewProjectile(null, NPC.Center, speed * 15, ModContent.ProjectileType<WraithFireBreathProj>(), damage, 0, Main.myPlayer);

                            ParticleShoot = false;
                            Timer3 = 0;
                        }
                    }
                }
            }   
            if (AITimer  == 600)
            {
                AITimer = 0;
                State = (float)Phase.Float;

            }

        }
        private void DetectPlayerOldPosition()
        {

        }
        private void FlameDash()
        {
            AITimer++;
            NPC.velocity *= 0.9f;
            Player player = Main.player[NPC.target];
            if (AITimer == 1)
            {
                toPlayer = NPC.DirectionTo(oldPlayerCenter) * 90;

                ParticleManager.NewParticle(NPC.Center, toPlayer * 0.001f, ParticleManager.NewInstance<Telegraph2>(), Color.Purple, 1f); ;
            }
         
            if (AITimer == 30)
            {
                NPC.velocity = toPlayer;
                

            }
            if(AITimer > 30 && AITimer < 40)
            {
                Projectile.NewProjectile(null, NPC.Center, NPC.velocity * 0, ModContent.ProjectileType<LivingFlameTrail>(), damage, 0, Main.myPlayer);
            }
            if (AITimer == 40)  
            {
                AITimer = 0;
                if (Phase2)
                {
                    State = (float)Phase.SpreadFireCircleAfterDash;

                }
                else
                {
                    State = (float)Phase.SpreadFireCircle;

                }



            }


        }
        private void FlameBreathAbove()
        {
            AITimer++;
            Player player = Main.player[NPC.target];

            if (AITimer < 120)
            {
                NPC.MoveAbove(player.Center, 14f);



            }
            else
            {
                Timer3++;
                NPC.MoveAbove(player.Center, 7.5f);

                if (Timer3 == 5)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);

                    ParticleShoot = true;
                    if (ParticleShoot)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            ParticleManager.NewParticle(NPC.Center, speed * 10, ParticleManager.NewInstance<CoreParticle>(), Color.Purple, 0.9f);
                            Projectile.NewProjectile(null, NPC.Center, speed * 10, ModContent.ProjectileType<WraithFireBreathProj>(), damage, 0, Main.myPlayer);

                            ParticleShoot = false;
                            Timer3 = 0;
                        }
                    }
                }
            }

            if (AITimer == 450)
            {
                AITimer = 0;
                if (Phase2)
                {
                    State = (float)Phase.SpreadFireCircleAfterDash;

                }
                else
                {
                    State = (float)Phase.SpreadFireCircle;

                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (State == (float)Phase.Dash || State == (float)Phase.AfterDash || State == (float)Phase.FlameDash || State == (float)Phase.FireBarrage)
            {

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {

                    int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
                    int startY = (int)(frameHeight * NPC.frameCounter);
                    Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
                    Vector2 origin = sourceRectangle.Size() / 2f;

                    var effects = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(drawColor) * (float)(((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
                    spriteBatch.Draw(texture, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, origin, NPC.scale, effects, 0f);
                }
            }
            return true;

        }

    }
    
}



