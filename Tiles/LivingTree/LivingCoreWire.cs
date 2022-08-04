using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Tiles.LivingTree
{
    public class LivingCoreWireTile : ModTile
    {
        public override void SetStaticDefaults()
        {

            Main.tileBlockLight[Type] = false;
            TileID.Sets.Platforms[Type] = true;

            Main.tileSolid[Type] = false;
            AddMapEntry(new Color(79, 55, 59));
            ModTranslation name = CreateMapEntryName();
            Main.tileMergeDirt[Type] = true;
            name.SetDefault("LivingCoreWire");
        }

        
    }
   

    public class LivingCoreWire : ModItem
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
            Item.createTile = ModContent.TileType<LivingCoreWireTile>();
        }

    }   

}