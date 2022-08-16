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

	public class SamLegs : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Meme Greaves");
			Tooltip.SetDefault("'We're all pawns controlled by something greater...'");

		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
		}

	}
}
