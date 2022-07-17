using DivergencyMod.Base;
using DivergencyMod.Dusts.Particles;
using Microsoft.Xna.Framework;
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


        public float State = 0;
        public bool ParticleShoot;
        public float AITimer;
        public float Timer;
        private bool SoundPlayed;
        private float frametimer;
        private int DashContinue = 3;
        private Vector2 oldPlayerCenter = Vector2.Zero;
        Vector2 toPlayer = Vector2.Zero;


        public override void SetStaticDefaults()
        {

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
            NPC.lifeMax = 6000;
            NPC.damage = 30;
            NPC.defense = 15;
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
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

        }
        public override void AI()
        {
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

        }



        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            frametimer++;
            if (frametimer == 6)
            {
                NPC.frameCounter++;
                frametimer = 0;
                NPC.frame.Y += frameHeight;

            }
            NPC.frameCounter = 0;
            if (NPC.frame.Y >= frameHeight * 14)
                NPC.frame.Y = 0;

        }



        private void Float()
        {
            AITimer++;
            Player player = Main.player[NPC.target];

            NPC.Move(player.Center, 2f);
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


                DashContinue = 3;

                State = (float)phase.Get();
                //State = (float)Phase.FlameBreathAbove;
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
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.2f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.4f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.6f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.8f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.2f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.4f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.6f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.8f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 3,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);



                }


            }
            if (AITimer == 30)
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
            if (AITimer == 20)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);
                    Projectile.NewProjectile(null, NPC.Center, speed * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed * -10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    AITimer = 0;
                    State = (float)Phase.Float;





                }
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
                    Projectile.NewProjectile(null, NPC.Center, speed * 6,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed * -6,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);


                }

            }
            if (AITimer == 30)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(10f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(12f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(14f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(16f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(18f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);


                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-10f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-12f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-14f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-16f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-18f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                   

                }

            }
            if (AITimer == 40)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);
                    Projectile.NewProjectile(null, NPC.Center, speed * 6,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed * -6,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-2f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-3f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-4f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-5f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-6f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-7f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-8f) * 10,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    AITimer = 0;
                    State = (float)Phase.Float;

                }

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
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);
                    //telegraphing ^
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.3) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.3f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
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
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);
                    //telegraphing ^
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.3) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.3f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);


                }

                // State = (float)Phase.Float;

            }
            if (AITimer == 80)
            {
                NPC.velocity.Y += 32;

            }
            if (AITimer == 95)
            {
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);
                    //telegraphing ^
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(-0.3) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.3f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.3f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.5f) * 15,ModContent.ProjectileType<LivingFlameBlast>(), 28, 0, Main.myPlayer);
                    ParticleManager.NewParticle(NPC.Center, speed.RotatedBy(0.5f) * 0.01f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);


                }
                AITimer = 0;
                State = (float)Phase.Float;


            }



        }
        #endregion
        private void FlameBreathFloat()
        {
            AITimer++;
         
            Player player = Main.player[NPC.target];

            if (AITimer >= 60)
            {
                NPC.Move(player.Center, 3.5f);

                Vector2 speed = NPC.DirectionTo(player.Center);
                Timer++;

                if (Timer == 5)
                {
                    ParticleShoot = true;
                    if (ParticleShoot)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            ParticleManager.NewParticle(NPC.Center, speed * 12, ParticleManager.NewInstance<CoreParticle>(), Color.Purple, 0.85f);
                            Projectile.NewProjectile(null, NPC.Center, speed * 12, ModContent.ProjectileType<WraithFireBreathProj>(), 28, 0, Main.myPlayer);

                            ParticleShoot = false;
                            Timer = 0;
                        }
                    }
                }
            }
            if (AITimer == 400)
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

            if (AITimer == 5)
            {
                toPlayer = NPC.DirectionTo(oldPlayerCenter) * 70;
                ParticleManager.NewParticle(NPC.Center, toPlayer * 0.001f, ParticleManager.NewInstance<Telegraph>(), Color.Purple, 1.5f);

            }
            if (AITimer == 15)
            {
                DashContinue--;
                NPC.velocity = toPlayer;

        



            }
            if (AITimer == 40)
            {
                AITimer = 0;
                State = (float)Phase.SpreadFireCircleAfterDash;



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
                Timer++;
                NPC.MoveAbove(player.Center, 7.5f);

                if (Timer == 5)
                {
                    Vector2 speed = NPC.DirectionTo(player.Center);

                    ParticleShoot = true;
                    if (ParticleShoot)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            ParticleManager.NewParticle(NPC.Center, speed * 10, ParticleManager.NewInstance<CoreParticle>(), Color.Purple, 0.85f);
                            Projectile.NewProjectile(null, NPC.Center, speed * 12, ModContent.ProjectileType<WraithFireBreathProj>(), 28, 0, Main.myPlayer);

                            ParticleShoot = false;
                            Timer = 0;
                        }
                    }
                }
            }

            if (AITimer == 450)
            {
                AITimer = 0;
                State = (float)Phase.SpreadFireCircle;
            }
        }


    }
    public class WraithFireBreathProj : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor) => new(255, 255, 255, 100);
        public override string Texture => "DivergencyMod/Items/Weapons/Magic/Invoker/InvokedProj";

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.width = Projectile.height = 40;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.alpha = 255;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 30;
            Projectile.hide = true;
            Projectile.CritChance = 0;

        }



    }
}



