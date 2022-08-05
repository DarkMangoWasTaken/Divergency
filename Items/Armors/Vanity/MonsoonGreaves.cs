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

namespace DivergencyMod.Items.Armors.Vanity
{
	[AutoloadEquip(EquipType.Legs)]

	public class MonsoonGreaves : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Living Wood Greaves");
			Tooltip.SetDefault("'It's very itchy in places that shouldn't be itchy...'"	
				+ "\nIncreases movement speed by 10%'"
				+ "\nGives you a leafy double jump");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}

	}
}
