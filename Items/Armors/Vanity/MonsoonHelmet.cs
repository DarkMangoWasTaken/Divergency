using DivergencyMod.Dusts.Particles;
using DivergencyMod.Items.Armors.Vanity;
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

	[AutoloadEquip(EquipType.Head)]
	public class MonsoonHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Living Wood Helmet");
			Tooltip.SetDefault("'It barely fits'"
				+ "\nIncreases damage dealt by 5%'"
				+ "\nIncreases your crit chance by 5%");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 60;
			Item.defense = 3;
		}









	}
}
