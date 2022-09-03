using DivergencyMod.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Tiles.LivingTree
{
    public class LivingCoreSmolCrystalTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            
            Main.tileMerge[Type][ModContent.TileType<LivingCoreWoodTile>()] = true;
            Main.tileMerge[ModContent.TileType<LivingCoreWoodTile>()][Type] = true;
            Main.tileFrameImportant[Type] = true;


                
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(13, 255, 13));
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Living Core Crystal");
            DustType = 7;

        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFramingHelper.GemFrame(i, j);
            return false;
        }
        public override bool CanPlace(int i, int j)
        {
            Tile top = Framing.GetTileSafely(i, j - 1);
            if (top.HasTile && !top.BottomSlope && top.TileType >= 0 && Main.tileSolid[top.TileType] && !Main.tileSolidTop[top.TileType])
            {
                return true;
            }
            Tile bottom = Framing.GetTileSafely(i, j + 1);
            if (bottom.HasTile && !bottom.IsHalfBlock && !bottom.TopSlope && bottom.TileType >= 0 && (Main.tileSolid[bottom.TileType] || Main.tileSolidTop[bottom.TileType]))
            {
                return true;
            }
            Tile left = Framing.GetTileSafely(i - 1, j);
            if (left.HasTile && left.TileType >= 0 && Main.tileSolid[left.TileType] && !Main.tileSolidTop[left.TileType])
            {
                return true;
            }
            Tile right = Framing.GetTileSafely(i + 1, j);
            if (right.HasTile && right.TileType >= 0 && Main.tileSolid[right.TileType] && !Main.tileSolidTop[right.TileType])
            {
                return true;
            }
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        


                r = 0.08f;
                g = 1.99f;
                b = 0f; 
        }
     


    }




    public class LivingCoreCrystalShard : ModItem
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
            Item.createTile = ModContent.TileType<LivingCoreSmolCrystalTile>();
        }
    }
}