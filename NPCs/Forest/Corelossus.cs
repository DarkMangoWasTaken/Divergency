using DivergencyMod.Base;
using DivergencyMod.Bosses.Forest;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.Dusts.Particles.CorePuzzleParticles;
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
    public class Corelossus : ModNPC
    {
        public bool ShootMode;
        public float AI_Timer;
        public float Beam_Timer;
        private int timer;
        private float rand = 100000;
        private float rand2 = 0;

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Corelossus"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 4; // make sure to set this for your modNPCs.
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 120; // The width of the NPC's hitbox (in pixels)
            NPC.height = 120; // The height of the NPC's hitbox (in pixels)
            NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 30; // The amount of damage that this NPC deals
            NPC.defense = 2; // The amount of defense that this NPC has
            NPC.lifeMax = 200; // The amount of health that this NPC has
            NPC.HitSound = SoundID.NPCHit2; // The sound the NPC will make when being hit.
            NPC.value = 90f; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 0.5f;
            NPC.scale = 1f;
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
				new FlavorTextBestiaryInfoElement("A bunch of idiots. Great.")
            });
        }
        public Vector2 oldpos;
        public override void AI()
        {
            timer++;
            NPC.TargetClosest();
            if (timer == 60)
            {
                rand = Main.rand.NextFloat(100, 150);
                rand2 = Main.rand.NextFloat(2, 4);
            }
            Player player = Main.player[NPC.target];

            if (AI_Timer < 200)
            {
                AI_Timer++;
                NPC.Move(player.Center, NPC.Distance(player.Center) / rand);
            }
            else
            {
                Beam_Timer++;
                NPC.velocity *= 0.98f;

             
                if (Beam_Timer == 60)
                {
                    oldpos = player.Center;
                    ShootMode = true;
                }
                if (Beam_Timer == 150)
                {
                    ShootMode = false;
                }
                if (Beam_Timer == 180)
                {
                    AI_Timer = 0;
                    Beam_Timer = 0;
                    shootCounter = 0;
                }
                if (ShootMode)
                {
                    timer2++;


                    if (timer2 == 3)
                    {
                        for (int j = 0; j < 1; j++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(oldpos).RotatedByRandom(MathHelper.ToRadians(3)) * Main.rand.NextFloat(9, 10), ModContent.ProjectileType<CorelossusProj>(), 60 / (Main.expertMode || Main.masterMode ? 4 : 2), 0, player.whoAmI);
                            ParticleManager.NewParticle(NPC.Center, new Vector2(0, 0), ParticleManager.NewInstance<LivingCoreExplosionParticle>(), Color.Purple, 1f);

                        }
                        timer2 = 0;

                    }



                }


            }





            NPC.TargetClosest(true);





        }
        private int frame = 0;
        private int frameTimer = 0;
        public int ShootCount = 0;
        public bool alreadyRotated = false;
        private int shootCounter;
        private int timer2;

        public override void FindFrame(int frameHeight)
        {
    
            Player player = Main.player[NPC.target];
            if (ShootMode)
            {
                NPC.rotation *= 1.01f;
                alreadyRotated = true;
                NPC.rotation += 0.2f;
            }
            else
            {
                NPC.spriteDirection = -NPC.direction;
                NPC.rotation = NPC.velocity.X * 0.2f;

            }
            NPC.frame.Y = frameHeight * frame;

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
    
         
            
       



            

        public override void OnKill()
        {
            //Projectile.NewProjectile(null, NPC.Center, new Vector2(0, 0), ModContent.ProjectileType<AltarKiller>(), 0, 0);

            int goreType = Mod.Find<ModGore>("CorelingGore").Type;

            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 3; i++)
                {
                    ParticleManager.NewParticle(NPC.Center, new Vector2(0, 0), ParticleManager.NewInstance<LivingCoreExplosionParticle>(), Color.Purple, 2f);

                    NPC.NewNPC(new EntitySource_SpawnNPC("Corelossus"), (int)NPC.Top.X, (int)NPC.Top.Y, ModContent.NPCType<CoreBeamer>());
                }


            }
        }

    }
  
   public class CorelossusProj : ModProjectile
    {
        public float timer;
        public float Timer;
        private Vector2 unmodifiedVelocity;

        public override string Texture => "DivergencyMod/Items/Weapons/Magic/Invoker/InvokedProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("get phucked loser");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 15; // The width of projectile hitbox
            Projectile.height = 15; // The height of projectile hitbox

            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Generic; // Is the projectile shoot by a ranged weapon?
            Projectile.timeLeft = 80; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
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

            Projectile.velocity = unmodifiedVelocity.RotatedBy(Math.Sin((Timer - 1) * 0.1f) * 0.1f);
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



