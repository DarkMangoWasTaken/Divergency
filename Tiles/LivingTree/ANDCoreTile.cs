using DivergencyMod.Items.Weapons.Melee.NaturesWrath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DivergencyMod.Tiles.LivingTree
{
    public class ANDCoreTile : ModTile
    {

        private static bool ChangeTexture;
        private Vector2 zero = Vector2.Zero;
        private bool AlreadyDrawn;
        private int timer;
        private bool Shoot;

        public override void SetStaticDefaults()
        {

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            Main.tileLighted[Type] = false;
            Main.tileBouncy[Type] = false;


            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(120, 85, 60), Language.GetText("AND Gate"));
            DustType = 7;

        }
        public override void RandomUpdate(int i, int j)
        {
            base.RandomUpdate(i, j);
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
            offsetY = 2;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/ANDCoreTile").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/ANDCoreTileCharged1").Value;
            Texture2D tex3 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/ANDCoreTileCharged2").Value;

            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {

                if (!Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                }
                else if (Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);

                }
                if (Main.tileBouncy[Type] && Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex3, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                    if (!Shoot)
                    {
                        Vector2 pos = new Vector2(i * 16, j * 16);
                        Vector2 speed = new Vector2(3, 0);



                        Projectile.NewProjectile(null, pos, speed, ModContent.ProjectileType<GateProjectile>(), 0, 0);
                        Shoot = true;
                    }
                }
                if (!Main.tileBouncy[Type] && !Main.tileLighted[Type])
                {
                    Shoot = false;
                }


            }

            return false;

        }

    }
    internal class ANDCore : ModItem
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/ANDCoreTile";

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
            Item.createTile = ModContent.TileType<ANDCoreTile>();
        }
    }
    internal class GateProjectile : ModProjectile ///////////// HERE YOU BLIND FUCK 
    {
        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 20;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;

        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Blast");
            Main.projFrames[Projectile.type] = 4;
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
           // Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Player player = Main.LocalPlayer;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.type == ModContent.ProjectileType<Barriere>() && proj.active && Projectile.Hitbox.Intersects(proj.Hitbox))
                {
                    Projectile.velocity.DirectionTo(proj.Center);

                    proj.Kill();
                    Main.tileLighted[ModContent.TileType<BarrierSpawnerTile>()] = true;
                    player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 30;
                }
            }
            if (Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<LivingCoreCrystalTile>())
            {
                // Origin position, in tile format.
                int x = (int)(Projectile.position.X / 16);
                int y = (int)(Projectile.position.Y / 16);

                // Position being checked;



                int checkX = x;
                int checkY = y;
                Tile tile = Framing.GetTileSafely(checkX, checkY);



                // Checking up to a maximum of 30 tiles.
                for (int b = 0; b < 1; b++)
                {

                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<LivingCoreCrystalTile>())

                    {
                        WorldGen.KillTile(checkX, checkY);
                    }
                }
            }
                    Vector2 pos = Projectile.position;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.spriteDirection = Projectile.direction;
        }
    }
    internal class Barriere : ModProjectile
    {
      
        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Blast");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 30;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 2;
        }
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            if (player.Hitbox.Intersects(Projectile.Hitbox))
            {
            }
            
              
            

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
            //Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Vector2 pos = Projectile.position;

            Projectile.spriteDirection = Projectile.direction;
        }

    }
    internal class BarrierSpawnerTile : ModTile
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCoreCrystalTile";

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<LivingCoreWoodTile>()] = true;
            Main.tileMerge[ModContent.TileType<LivingCoreWoodTile>()][Type] = true;
            Main.tileMerge[Type][TileID.LeafBlock] = true;
            Main.tileMerge[TileID.LeafBlock][Type] = true;

            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            AddMapEntry(new Color(13, 255, 13));
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Living Core Crystal");
            DustType = 10;

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {



            r = 0.08f;
            g = 1.99f;
            b = 0f;
        }
        private int timer = 0;
        private protected bool spawned;

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Vector2 pos = new Vector2(i * 16 + 8f, j * 16);
            Vector2 speed = new Vector2(0, 0);
            base.NearbyEffects(i, j, closer);
            if (!spawned)
            {
                Projectile.NewProjectile(null, pos, speed, ModContent.ProjectileType<Barriere>(), 0, 0);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
        
            Vector2 pos = new Vector2(i * 16 + 8f, j * 16);
            Vector2 speed = new Vector2(0,0);
   



            if (Main.tileLighted[Type])
            {
                spawned = true;
                WorldGen.KillTile(i, j);
                Main.tileLighted[Type] = false;

            }

             

            return true;
        }
    }
    internal class BarrierSpawner : ModItem
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCoreCrystal";

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.value = 1000;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<BarrierSpawnerTile>();
        }
    }



}   
