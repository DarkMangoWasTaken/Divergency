using DivergencyMod.Dusts.Particles;
using DivergencyMod.Items.Accs.Forest;
using Microsoft.Xna.Framework;
using ParticleLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Localization.GameCulture;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Armors
{
	[AutoloadEquip(EquipType.Legs)]

	public class LivingCoreGreaves: ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Living Core Greaves");
			Tooltip.SetDefault("'Feels oddly comfortable in places that should feel comfortable :)'"
				+ "\nIncreases movement speed by 12%'");
				
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{

		
			player.moveSpeed += 0.12f;
			//will allow a double jump

		}
	}
	
}
