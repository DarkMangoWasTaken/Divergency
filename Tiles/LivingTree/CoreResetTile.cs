using CsvHelper.TypeConversion;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.Dusts.Particles.CorePuzzleParticles;
using DivergencyMod.Items.Ammo;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DivergencyMod.Tiles.LivingTree
{
    public class CoreResetTile : ModTile
    {
        private static bool ChangeTexture;
        private Vector2 zero = Vector2.Zero;
        private bool AlreadyDrawn;
        private bool reset;

        public override void SetStaticDefaults()
        {

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileLighted[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);

            TileObjectData.addTile(Type);
            Main.tileBouncy[Type] = false;

            AddMapEntry(new Color(120, 85, 60), Language.GetText("Resetter"));
            DustType = 7;

        }
        public override bool RightClick(int i, int j)
        {
            Vector2 pos = new Vector2(i * 16, j * 16);

            Vector2 speed = new Vector2(-10f, 0f);

            Player player = Main.LocalPlayer;
            ParticleManager.NewParticle(player.Center, player.velocity * 3, ParticleManager.NewInstance<ResetParticle>(), Color.Purple, 8f);


            Main.tileLighted[ModContent.TileType<XORCoreTile>()] = false;
            Main.tileBouncy[ModContent.TileType<XORCoreTile>()] = false;


            Main.tileLighted[ModContent.TileType<ANDCoreTile>()] = false;
            Main.tileBouncy[ModContent.TileType<ANDCoreTile>()] = false;

            Main.tileLighted[ModContent.TileType<CoreRootsTile>()] = false;
            Main.tileLighted[ModContent.TileType<CoreRootsTile1>()] = false;

            Main.tileLighted[ModContent.TileType<CoreRootsTile2>()] = false;
            Main.tileLighted[ModContent.TileType<CoreDoublerDownLeftTile>()] = false;
            Main.tileLighted[ModContent.TileType<CoreDoublerLeftUpTile>()] = false;
            Main.tileLighted[ModContent.TileType<CoreDoublerRightDownTile>()] = false;
            Main.tileLighted[ModContent.TileType<CoreDoublerUpRightTile>()] = false;

            Main.tileLighted[ModContent.TileType<LivingCorePodestTileLeft>()] = false;
            Main.tileLighted[ModContent.TileType<LivingCorePodestTileRight>()] = false;
            Main.tileLighted[ModContent.TileType<LivingCorePodestTileUp>()] = false;
            
            player.GetModPlayer<CorePuzzle>().LivingCoreAmount = 0;
   
                player.GetModPlayer<CorePuzzle>().LivingCoreAmount = 1;
                ParticleManager.NewParticle(player.Center, player.velocity * 3, ParticleManager.NewInstance<LivingCoreInsertParticle>(), Color.Purple, 1f);
                player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 20;
            for (int jo = 0; jo < 20; jo++)
            {
                Vector2 speed2 = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(pos, speed2 * 10, ParticleManager.NewInstance<CrystalParticle>(), Color.Purple, 0.9f);


            }
            for (int io = 0; io < Main.maxProjectiles; io++)
            {
                Projectile proj = Main.projectile[io];
                if (proj.type == ModContent.ProjectileType<PodestProjectile
                    >())
                {
                    proj.Kill();
                }
            }





                //if (!ChangeTexture)
                //   ChangeTexture = true;
                //else
                //   ChangeTexture = false;


                return true;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {

            if (ChangeTexture)
            {


                r = 1.45f;
                g = 2.55f;
                b = 0.94f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }

        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Placeable.Furniture.MinionBossTrophy>());
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = 20;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/CoreResetTile").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCorePodestTileLeft").Value;
            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                if (!Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, 2), Color.White);
                    AlreadyDrawn = true;
                }
                else if (Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, 2), Color.White);
                    AlreadyDrawn = true;

                }
            }

            return false;

        }
        public int FindEmptySlot()
        {
            for (int i = 0; i < Main.player[Main.myPlayer].inventory.Length; i++)
            {
                Item item = Main.player[Main.myPlayer].inventory[i];
                if (!item.active)
                {
                    return i;
                }
            }
            return -1;
        }

    }
    internal class CoreReset : ModItem
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/CoreResetTile";

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
            Item.createTile = ModContent.TileType<CoreResetTile>();
        }
    }

    public class CorePuzzle : ModPlayer
    {
        public byte LivingCoreAmount;
        public bool spawned;
        public bool particlekill;


    }
}
