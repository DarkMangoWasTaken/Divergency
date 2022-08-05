using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    // Simple 3x3 tile that can be placed on a wall
    public class LivingCorePodestTile : ModTile
    {
        private static bool ChangeTexture;
        private Vector2 zero = Vector2.Zero;
        private bool AlreadyDrawn;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileLighted[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
            DustType = 7;

        }
        public override bool RightClick(int i, int j)
        {
            Vector2 pos = new Vector2(i, j);
            if (Main.tileLighted[Type])
            {

                Projectile.NewProjectile(null, pos, )
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
            offsetY = 2;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCorePodestTile").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCorePodestCharged").Value;
            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                if (!Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                    AlreadyDrawn = true;
                }
                else if (Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                    AlreadyDrawn = true;

                }
            }

            return false;

        }

    }
    internal class LivingCorePodest : ModItem
    {

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
            Item.createTile = ModContent.TileType<LivingCorePodestTile>();
        }
    }

    public class PodestProjectile : ModProjectile
    {
        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";

        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 110;
            Projectile.width = 110;
            Projectile.friendly = true;
            Projectile.scale = 1f;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {

            if (Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<LivingCorePodestTile>())
            {
                // Origin position, in tile format.
                int x = (int)(Projectile.position.X / 16);
                int y = (int)(Projectile.position.Y / 16);

                // Position being checked;



                int checkX = x;
                int checkY = y;
                Tile tile = Framing.GetTileSafely(checkX, checkY);

                if (WorldGen.InWorld(checkX, y) && Main.tile[checkX, checkY].TileType == ModContent.TileType<LivingCorePodestTile>())
                {
                    Main.tileLighted[ModContent.TileType<LivingCorePodestTile>()] = true;
                    Main.NewText("found");

                }

                // Checking up to a maximum of 30 tiles.
                for (int b = 0; b < 2; b++)
                {
                    // If this position is in the world, and if the tile is a Tree tile.
                    if (WorldGen.InWorld(checkX, y) && Main.tile[checkX, checkY].TileType == ModContent.TileType<LivingCorePodestTile>())
                    {
                        // Checking if the tile's frames are within the range of tile frames used for the invisible tree top tiles.
                        if (Main.tile[checkX, checkY].TileType == ModContent.TileType<LivingCorePodestTile>())
                        {
                            Main.tileLighted[ModContent.TileType<LivingCorePodestTile>()] = true;
                            Main.NewText("found22222");

                        }
                    }

                }
            }

        }

        public bool TryFindNearPodest(Vector2 position, out Vector2 result)
        {
            if (Main.tile[(int)position.X / 16, (int)position.Y / 16].TileType ==== ModContent.TileType<LivingCorePodestTile>())
            {
                // Origin position, in tile format.
                int x = (int)(position.X / 16);
                int y = (int)(position.Y / 16);

                // Position being checked;

                int checkX = x;
                int checkY = y;

                // Checking up to a maximum of 30 tiles.
                for (int b = 0; b < 30; b++)
                {
                    // If this position is in the world, and if the tile is a Tree tile.
                    if (WorldGen.InWorld(checkX, y) && Main.tile[checkX, checkY].TileType == ModContent.TileType<LivingCorePodestTile>())
                    {
                        // Checking if the tile's frames are within the range of tile frames used for the invisible tree top tiles.
                        if (Main.tile[checkX, checkY].TileFrameX == 22 && Main.tile[checkX, checkY].TileFrameY >= 198)
                        {
                            //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Yellow, null);
                            result = new Vector2(checkX * 16, checkY * 16);
                            return true;
                        }
                        // Otherwise, its a success, since it's still a tree tile. Just not the one we're looking for.
                        //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Green, null);
                        checkY--;
                    }
                    else
                    {
                        // If the tile isn't what we're looking for and since we're only iterating upwards, logically this means its useless to continue.
                        //Dust.QuickDustLine(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 5f, Color.Red);
                        //Dust.QuickDustLine(new Vector2(checkX * 16, (checkY * 16) + 16), new Vector2((checkX * 16) + 16, checkY * 16), 5f, Color.Red);
                        break;
                    }
                }
            }

            result = default;
            return false;
        }

    }
}
