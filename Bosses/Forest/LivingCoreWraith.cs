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
using DivergencyMod.Base;


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
            FireBarrage    
        }


        public float State = 0;

        public float AITimer;
        public float Timer;
        private bool SoundPlayed;
        private float frametimer;
        private int DashContinue = 3;

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
            NPC.defense = 10;
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
            NPC.behindTiles = true;
            Music = MusicLoader.GetMusicSlot("DivergencyMod/Sounds/Music/RedSus");

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

                State = (float)Phase.Dash;
                AITimer = 0;

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
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.2f) *3 , ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.4f) * 3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.6f)* 3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(0.8f)* 3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(1f)*3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.2f)*3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.4f)*3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.6f)*3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-0.8f)*3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);
                    Projectile.NewProjectile(null, NPC.Center, speed.RotatedBy(-1f)*3, ProjectileID.CursedFlameHostile, 28, 0, Main.myPlayer);



                }


            }
            if (AITimer == 30)
            {
                if (DashContinue <= 0)
                {
                    State = (float)Phase.Float;
                    DashContinue = 3;
                }
                else
                {
                    State = (float)Phase.AfterDash;

                }
                AITimer = 0;
            }

        }


    }


    
}



