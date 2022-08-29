using CsvHelper.TypeConversion;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.Dusts.Particles.CorePuzzleParticles;
using DivergencyMod.Items.Ammo;
using DivergencyMod.Items.Weapons.Melee.ShadowflameSword;
using DivergencyMod.NPCs.Forest;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DivergencyMod.Tiles.LivingTree
{
    public class LivingCoreAltarTile1 : ModTile
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCoreAltar";

        private static bool ChangeTexture;
        private Vector2 zero = Vector2.Zero;
        private bool AlreadyDrawn;
        private bool reset;

        public override void SetStaticDefaults()
        {

            Main.tileFrameImportant[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileBouncy[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
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

            NPC.NewNPC(null, left * 16, top * 16, ModContent.NPCType<AltarHandler1>());
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
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D item = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCore").Value;

            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                if (Main.tileBouncy[Type])
                {
                    spriteBatch.Draw(item, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, -40), Color.White);

                }


            }

            return true;

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

    public class AltarPlayer : ModPlayer
    {
        public byte LivingCoreAmount;
        public bool spawned;
        public bool particlekill;


    }
    public class AltarHandler1 : ModNPC
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCore";

        public override void SetDefaults()
        {
            NPC.width = NPC.height = 1;
            NPC.alpha = 0;
            NPC.immortal = true;
            NPC.lifeMax = 12;
            NPC.friendly = true;
            Music = MusicLoader.GetMusicSlot("DivergencyMod/Sounds/Music/LivingCoreWraithTheme");
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.boss = true;
            
        }
        public int timer = 0;
        public int deathtimer = 0;
        public int spawntimer = 0;
        public override void AI()
        {
            timer++;
            spawntimer++;

          

            
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
            Vector2 pos = NPC.Center;
            
            if (spawntimer == 30)
            {
                NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());


            }
            if (spawntimer == 1000)
            {
                NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());


            }
            if (spawntimer == 1500)
            {
                NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y - 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X - 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());
                NPC.NewNPC(null, (int)pos.X + 300, (int)pos.Y + 300, ModContent.NPCType<CoreBeamer>());


            }


        }
    } 
}
