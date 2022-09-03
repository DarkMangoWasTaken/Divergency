using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using DivergencyMod.Helpers;    
using static Terraria.ModLoader.ModContent;

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


       
        
    }




    public class LivingMud: ModItem
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
            Item.createTile = ModContent.TileType<LivingMudTile>();
        }
    }
}