using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Ammo
{
    public class Pebble : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Effective against glass'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.maxStack = 999; // The maximum number of items that can be contained within a single stack
            Item.consumable = true;
            Item.ammo = Item.type; // Important. The first item in an ammo class sets the AmmoID to its type
            Item.damage = 5;
            Item.knockBack = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}