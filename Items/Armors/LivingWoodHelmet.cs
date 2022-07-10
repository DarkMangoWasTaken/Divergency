using DivergencyMod.Dusts.Particles;
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

		[AutoloadEquip(EquipType.Head)]
		public class LivingWoodHelmet : ModItem
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

			public override void UpdateEquip(Player player)
			{

				
				player.GetDamage(DamageClass.Generic) += 0.05f; // Increase dealt damage for all weapon classes by 5%
				player.GetCritChance(DamageClass.Generic) += 5;

			}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<LivingWoodBreastplate>() && legs.type == ModContent.ItemType<LivingWoodGreaves>();

		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Enhances every stat in the near of a tree"; // This is the setbonus tooltip
			player.GetModPlayer<TreePlayer>().treeCheck = true;
			if (player.GetModPlayer<TreePlayer>().treeNear != 0 && player.GetModPlayer<TreePlayer>().treeCheck)
            {
				player.GetDamage(DamageClass.Generic) += 0.25f;
				player.manaRegenCount += 25;



			}
		}
	}
	public class TreePlayer : ModPlayer	
    {
		public bool treeCheck;
		public int treeNear;
		public override void PreUpdate()
		{
			if (treeNear >= 1)
			{
				treeNear--;
			}
			
				// Origin position, in tile format.
				int x = (int)(Player.position.X / 16);
				int y = (int)(Player.position.Y / 16);

				// Position being checked;



				int checkX = x;
				int checkY = y;
				if (WorldGen.InWorld(checkX, y) && Main.tile[checkX, checkY].TileType == TileID.Trees)
				{
					Player.GetModPlayer<TreePlayer>().treeNear = 300;
				}
			
		}
		public override void ResetEffects()
        {
			treeCheck = false;
        }

	}






}
