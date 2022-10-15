using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Tiles.LivingTree
{
    public class LivingCoreCrystalTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<LivingCoreWoodTile>()] = true;
            Main.tileMerge[ModContent.TileType<LivingCoreWoodTile>()][Type] = true;
            Main.tileMerge[Type][TileID.LeafBlock] = true;
            Main.tileMerge[TileID.LeafBlock][Type] = true;

            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(13, 255, 13));
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("LivingCoreCrystal");
            DustType = 10;

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        


                r = 0.2f * 0.2f;
                g = 2.55f * 0.2f;
                b = 1.33f * 0.2f; 
        }
     


    }




    public class LivingCoreCrystal : ModItem
    {
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
            Item.createTile = ModContent.TileType<LivingCoreCrystalTile>();
        }
    }
}