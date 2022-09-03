using DivergencyMod.NPCs.Forest;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DivergencyMod.Helpers;
using Humanizer;
using ReLogic.Content;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameContent;
using IL.Terraria.DataStructures;
using System;
using System.Collections.Generic;

namespace DivergencyMod.Tiles.LivingTree
{
    public class LivingCoreAltarTile1 : ModTile
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCoreAltar";

        private Vector2 zero = Vector2.Zero;
        

        public override void SetStaticDefaults()
        {

            Main.tileFrameImportant[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileBouncy[Type] = true;

            Main.tileLighted[Type] = false;
            Main.tileAxe[Type] = false;
            Main.tileBrick[Type] = false;
            Main.tileHammer[Type] = false;
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(120, 85, 60), Language.GetText("Living Core Altar"));
            DustType = 7;

        }
        public override bool RightClick(int i, int j)
        {
            Vector2 pos = new Vector2(i * 16, j * 16);

            Vector2 speed = new Vector2(-10f, 0f);

            Player player = Main.LocalPlayer;
            int left = i - Main.tile[i, j].TileFrameX / 18;
            int top = j - Main.tile[i, j].TileFrameY / 18;

            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 50;

            NPC.NewNPC(null, left * 16 + 30, top * 16, ModContent.NPCType<AltarHandler1>());
            WorldGen.PlaceTile(left  - 10, top , ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left  + 10, top , ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left - 10, top -1, ModContent.TileType<LivingCoreWoodTile>());
            WorldGen.PlaceTile(left + 10, top-1, ModContent.TileType<LivingCoreWoodTile>());


            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Placeable.Furniture.MinionBossTrophy>());
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            //offsetY = 20;
        }
        public float y = -50;
        private bool rewardactive = false;

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D itemglow = ModContent.Request<Texture2D>("DivergencyMod/Effects/LivingCoreGlow").Value;
            Texture2D tiletex = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCoreAltar").Value;
            Texture2D tiletex1 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCoreAltar4").Value;
            Texture2D tiletex2 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCoreAltar3").Value;
            Texture2D tiletex3 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCoreAltar2").Value;
            Texture2D tiletex4 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCoreAltar1").Value;



            int left = i - Main.tile[i, j].TileFrameX / 18;
            int top = j - Main.tile[i, j].TileFrameY / 18;
            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                if (Main.tileBouncy[Type]) //&& !rewardactive)
                {
                   
                    Projectile.NewProjectile(null, left * 16 + 33, top * 16 + -50, 0, 0.0001f, ModContent.ProjectileType<AltarReward>(), 0, 0);
                    Main.tileBouncy[Type] = false;
                }

                if (!Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tiletex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, -0), Color.White);

                }
                else if (Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tiletex1, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, -0), Color.White);
                    Main.tileHammer[Type] = false;
                }
                if (Main.tileAxe[Type])
                {
                    spriteBatch.Draw(tiletex2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, -0), Color.White);
                    Main.tileLighted[Type] = false;
                }
                if (Main.tileBrick[Type])
                {
                    spriteBatch.Draw(tiletex3, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, -0), Color.White);
                    Main.tileLighted[Type] = false;
                    Main.tileAxe[Type] = false;


                }
                if (Main.tileHammer[Type])
                {
                    spriteBatch.Draw(tiletex4, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, -0), Color.White);
                    Main.tileLighted[Type] = false;
                    Main.tileAxe[Type] = false;
                    Main.tileBrick[Type] = false;
                }



            }

            return false;

        }


    }
    internal class LivingCoreAltar : ModItem
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCore";

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.value = 1000;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<LivingCoreAltarTile1>();
        }
    }
    public class AltarReward : ModProjectile
    {
        public TrailRenderer prim;
        public TrailRenderer prim2;
        public int timer;
        public string path;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override string Texture => "DivergencyMod/Effects/LivingCoreSwordGlow";

        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.height = 20;
            Projectile.width = 10;
            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.timeLeft = 3000;
            

        }
     
        public override void AI()
        {
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

            Projectile.gfxOffY = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 10;
            if (DownedHelper.ClearedAltar)
            {
                Projectile.Kill();

            }
            else
            {
                Projectile.active = true;
            }
            timer++;
            Projectile.timeLeft = 10;
            if (timer == 30)
            { timer = 0;
                Projectile.rotation = 5.48f;
            }
          

        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
           // behindNPCsAndTiles.Add(index);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, 0.8f, SpriteEffects.None, 0);

            return false;
        }

    }

    public class AltarHandler1 : ModNPC
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCore";

        public override void SetDefaults()
        {
            NPC.width = NPC.height = 1;
            NPC.alpha = 0;
            NPC.immortal = true;
            NPC.lifeMax = 16;
            NPC.friendly = false;
            NPC.dontTakeDamage = true;
            Music = MusicLoader.GetMusicSlot("DivergencyMod/Sounds/Music/CoreBattle");
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.boss = true;
         //   NPC.BossBar = ModContent.GetInstance<AltarProgressBar>();
            NPC.ShowNameOnHover = false;
            NPC.alpha = 255;






                

        }

        public int timer = 0;
        public int deathtimer = 0;
        public int spawntimer = 0;
        public bool stoptimerreset = false;
        public byte clearedWaveCount = 0;
        public bool stoppls = false;
        public bool stoppls2 = false;
        public bool stoppls3 = false;
        private bool wave3;
        private bool wave4;

        public override void AI()
        {
            timer++;
            NPC.active = true;
            if (!stoptimerreset)
            {
                spawntimer++;
            }
            if (clearedWaveCount == 1)
            {
                spawntimer = 1;
                clearedWaveCount = 0;
                if (!stoppls)
                {
                    stoppls = true;
                }
                if (!stoppls2 && stoppls && wave3)
                {
                    stoppls2 = true;
                }
                if (!stoppls3 && stoppls2 && wave4)
                {
                    stoppls3 = true;
                }
            }
            Vector2 pos = NPC.Center;

            if (NPC.life == 16)
            {

                Main.tileLighted[ModContent.TileType<LivingCoreAltarTile1>()] = true;
                if (spawntimer == 1)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "WAVE 1!", true, false);
                    stoptimerreset = false;


                }
                if (spawntimer == 100)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "3!", false, false);
                }
                if (spawntimer == 160)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "2!", false, false);
                }
                if (spawntimer == 220)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "1!", false, false);
                }
                if (spawntimer == 280)
                {
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    stoptimerreset = true;
                    spawntimer = 0;
                }


            }
            if (NPC.life == 12)
            {
                if (!stoppls)
                {
                    clearedWaveCount = 1;
                    stoppls = true;
                   
                }


                Main.tileAxe[ModContent.TileType<LivingCoreAltarTile1>()] = true;
                if (spawntimer == 1)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "WAVE 2!", true, false);
                    stoptimerreset = false;

                }
                if (spawntimer == 100)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "3!", false, false);
                }
                if (spawntimer == 160)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "2!", false, false);
                }
                if (spawntimer == 220)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "1!", false, false);
                }
                if (spawntimer == 280)
                {
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    stoptimerreset = true;
                    spawntimer = 0;
                }


            }
            if (NPC.life == 8)
            {
                wave3 = true;
                if (!stoppls2)
                {
                    clearedWaveCount = 1;
                    stoppls2 = true;

                }



                Main.tileBrick[ModContent.TileType<LivingCoreAltarTile1>()] = true;
                if (spawntimer == 1)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "WAVE 3!", true, false);
                    stoptimerreset = false;
                }
                if (spawntimer == 100)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "3!", false, false);
                }
                if (spawntimer == 160)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "2!", false, false);
                }
                if (spawntimer == 220)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "1!", false, false);
                }
                if (spawntimer == 280)
                {
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    stoptimerreset = true;
                    spawntimer = 0;
                }


            }
            if (NPC.life == 4)
            {
                wave4 = true;
                if (!stoppls3)
                {
                    clearedWaveCount = 1;
                    stoppls3 = true;

                }

                Main.tileHammer[ModContent.TileType<LivingCoreAltarTile1>()] = true;
                if (spawntimer == 1)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "WAVE 4!", true, false);
                    stoptimerreset = false;
                }
                if (spawntimer == 100)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "3!", false, false);
                }
                if (spawntimer == 160)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "2!", false, false);
                }
                if (spawntimer == 220)
                {
                    CombatText.NewText(NPC.getRect(), Color.LightGreen, "1!", false, false);
                }
                if (spawntimer == 280)
                {
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                    stoptimerreset = true;
                    spawntimer = 0;
                }


            }
           


            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (timer == 300)
                {
                    if (player.dead)
                    {
                        NPC.active = false;
                    }
                }
            }
           

         
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedHelper.ClearedAltar, -1);
            Main.NewText("Cleared!");
            Main.tileLighted[ModContent.TileType<LivingCoreAltarTile1>()] = false;
            Main.tileAxe[ModContent.TileType<LivingCoreAltarTile1>()] = false;
            Main.tileBrick[ModContent.TileType<LivingCoreAltarTile1>()] = false;
            Main.tileHammer[ModContent.TileType<LivingCoreAltarTile1>()] = false;

        }

    } 


        public class AltarProgressBar : ModBossBar
        {
            private int bossHeadIndex = -1;

            public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
            {
                // Display the previously assigned head index
                if (bossHeadIndex != -1)
                {
                    return TextureAssets.NpcHeadBoss[bossHeadIndex];
                }
                return null;
            }

            public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float lifePercent, ref float shieldPercent)
            {
                // Here the game wants to know if to draw the boss bar or not. Return false whenever the conditions don't apply.
                // If there is no possibility of returning false (or null) the bar will get drawn at times when it shouldn't, so write defensive code!

                NPC npc = Main.npc[info.npcIndexToAimAt];
                if (!npc.active)
                    return false;

                // We assign bossHeadIndex here because we need to use it in GetIconTexture
                bossHeadIndex = npc.GetBossHeadTextureIndex();

                lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);


                return true;
            }
        }
    

}
