using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;


namespace DivergencyMod.Items.Accs.Forest
{
	public class AcornRepellentSpray : ModItem
	{

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Being annoyed by noisy Acorns? Then this is what you were searching for! Favorite to activate its effects.'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Green;
	


		}

        public override void UpdateInventory(Player player)
        {
			if (Item.favorited)
			{
				player.GetModPlayer<DivergencyPlayer>().checkTree = false;
			}
            else
            {
				player.GetModPlayer<DivergencyPlayer>().checkTree = true;

			}
		}
		

		




	}

	



	
}