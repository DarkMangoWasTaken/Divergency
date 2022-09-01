using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using DivergencyMod.Helpers;    
using static Terraria.ModLoader.ModContent;
using Aequus.Tiles;

namespace DivergencyMod.Tiles.LivingTree
{
    public class LivingMudTile : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][TileID.LeafBlock] = true;
            Main.tileMerge[TileID.LeafBlock][Type] = true;
            Main.tileMerge[Type][ModContent.TileType<LivingCoreWoodTile>()] = true;
            Main.tileMerge[ModContent.TileType<LivingCoreWoodTile>()][Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(79, 55, 59));
            ModTranslation name = CreateMapEntryName();
            Main.tileMergeDirt[Type] = true;
            name.SetDefault("Living Core Wood");
            DustType = 7;

        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFramingHelper.MergeWithFrame(i, j, Type, TileID.LeafBlock);
            TileFramingHelper.MergeWithFrame(i, j, Type, ModContent.TileType<LivingCoreWoodTile>());

            return false;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Texture2D tex = Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCoreWoodGlow").Value;
            int height = tile.TileFrameY == 36 ? 18 : 16;
            if (tile.Slope == 0 && !tile.IsHalfBlock)
            {
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }




    public class LivingMud: ModItem
    {
        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";

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
            Item.createTile = ModContent.TileType<LivingMudTile>();
        }
    }
}