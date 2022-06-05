
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DivergencyMod.Items.Weapons.Melee.RootBreaker;
using DivergencyMod.Items.Weapons.Melee.LivingWoodGuitar;

namespace DivergencyMod.Items.Accs.Forest
{
	public class Attackspeeder : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("dont do speed kids");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;


		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{

			player.GetDamage(DamageClass.Ranged) *= 0.5f; // Increase ALL player damage by 100%
             player.GetAttackSpeed(DamageClass.Ranged) *= 2f;


		}




	}

}