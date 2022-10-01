using DivergencyMod.Base;
using DivergencyMod.Bosses.Forest;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.Dusts.Particles.CorePuzzleParticles;
using DivergencyMod.Events.LivingCore;
using DivergencyMod.Helpers;
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

            DisplayName.SetDefault("Coreling");
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
        public override void OnSpawn(IEntitySource source) // move this to ai later
        {
            if (source is EntitySource_SpawnNPC spawnNPC && spawnNPC.Context == "Corelossus")
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                NPC.velocity += speed * 15;
            }
        }
        public override void AI()
        {
            timer++;
            AI_Timer++; 

            NPC.TargetClosest();
            if (timer == 60)
            {
                rand = Main.rand.NextFloat(40, 120);
                rand2 = Main.rand.NextFloat(2, 4);
            }
            Player player = Main.player[NPC.target];

            if(AI_Timer < 360 && AI_Timer > 30)
            {
                NPC.Move(player.Center, NPC.Distance(player.Center) / rand);
            }
            else if (AI_Timer > 360)
            {
                NPC.velocity *= 0.98f;

                if (Main.rand.NextBool(3))
                {
                    Beam_Timer++;

                }
                if (Beam_Timer >= 0 && Beam_Timer <= 80)
                {
                    for (int j = 0; j < 1; j++)

                    {
                        Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                        ParticleManager.NewParticle(NPC.Center, speed * 20, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 1f);


                    }
                    //ParticleManager.NewParticle(NPC.Center -  Main.rand.NextVector2Circular(-10f, 10f), new Vector2(0, 0), ParticleManager.NewInstance<CoreLoadParticle>(), Color.Purple, 0.3f, NPC.whoAmI, NPC.whoAmI);
                    //ParticleManager.NewParticle(NPC.Center + Main.rand.NextVector2Circular(-10f, 10f), new Vector2(0, 0), ParticleManager.NewInstance<CoreLoadParticle>(), Color.Purple, 0.3f, NPC.whoAmI,NPC.whoAmI); }
                }

                if (Beam_Timer == 20)
                {
                    playAnim = true;
                    frame = 4;
                }
               
                if (Beam_Timer == 40)
                {
                    playAnim = true;
                    frame = 4;
                }
             
                if (Beam_Timer == 60)
                {
                    playAnim = true;
                    frame = 4;

                }
                
                if (Beam_Timer > 80)
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
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center).RotatedByRandom(MathHelper.ToRadians(5)) * Main.rand.NextFloat(9, 10), ModContent.ProjectileType<LivingCoreBeamerProj>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
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
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center).RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(5, 8), ModContent.ProjectileType<LivingCoreBeamerProj>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
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
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center).RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(4, 6), ModContent.ProjectileType<LivingCoreBeamerProj>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
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
            int goreType = Mod.Find<ModGore>("CorelingGore").Type;

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, NPC.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType, NPC.scale);
            }
        }

    }

   public class LivingCoreBeamerProj : ModProjectile
    {
        public float timer;
        public float Timer;
        private Vector2 unmodifiedVelocity;

        public override string Texture => "DivergencyMod/Items/Weapons/Magic/Invoker/InvokedProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("get phucked loser");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25; // in SetStaticDefaults()
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 15; // The width of projectile hitbox
            Projectile.height = 15; // The height of projectile hitbox

            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Generic; // Is the projectile shoot by a ranged weapon?
            Projectile.timeLeft = 180; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?

            Projectile.scale = 1f;
        }
        public override void AI()
        {
            Timer++;
            if (Timer == 1)
            {
                unmodifiedVelocity = Projectile.velocity;
            }

            Projectile.velocity = unmodifiedVelocity.RotatedBy(Math.Sin((Timer - 40) * 0.3f) * 0.3f);
            Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
            float multiplier = 0.4f;
            float max = 1f;
            float min = 1.0f;
            RGB *= multiplier;
            if (RGB.X > max)
            {
                multiplier = 0.5f;
            }
            if (RGB.X < min)
            {
                multiplier = 1.5f;
            }
            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);
            timer++;
       
            if (Timer == 1)
            {
                ParticleManager.NewParticle(Projectile.Center, Projectile.velocity * 3, ParticleManager.NewInstance<PodestProjBase>(), Color.Purple, 0.3f, Projectile.whoAmI, Projectile.whoAmI);

            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.spriteDirection = Projectile.direction;

            if (timer == 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    ParticleManager.NewParticle(Projectile.Center, speed * 3, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 0.9f, Projectile.whoAmI, Projectile.whoAmI);


                }
                timer = 0;
            }





        }
        public TrailRenderer prim;
        public TrailRenderer prim2;
        public override bool PreDraw(ref Color lightColor)
        {
            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/Trail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(30f) * (1f - p), (p) => Projectile.GetAlpha(Color.LimeGreen) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = Projectile.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(15f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
            prim2.Draw(Projectile.oldPos);


            return false;
        }
    }
}