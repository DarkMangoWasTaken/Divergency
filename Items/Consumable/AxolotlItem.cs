using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DivergencyMod.NPCs.Critters;

namespace DivergencyMod.Items.Consumable
{
	public class AxolotlItem : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Axolotl");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 56;
			Item.height = 24;
			Item.maxStack = 999;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.makeNPC = (short)ModContent.NPCType<Axolotl>();
		}

		

	}
}
