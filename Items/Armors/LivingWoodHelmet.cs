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
            if (!player.controlDown)
            {
				player.setBonus = "Enhances various stats in the near of a tree, press down for more info"; // This is the setbonus tooltip

			}
			else
            {
				player.setBonus = "Increased life regen, mana regen and damage while in the near of trees"
				+"\nIncreases damage dealt by 15%'"
				+"\nIncreases life regen by 3'"
				+"\nIncreases mana regen'"
				+"\nIncreases defense by 2'"
				+"\nIncreases damage reduction by 5%'"
				+"\nIncreases movement speed by 30%'";


			}
			player.GetModPlayer<TreePlayer>().treeCheck = true;
			if (player.GetModPlayer<TreePlayer>().treeNear != 0 && player.GetModPlayer<TreePlayer>().treeCheck)
            {
				player.GetDamage(DamageClass.Generic) += 0.15f;
				player.manaRegenCount += 8;
				player.statDefense += 2;
				player.lifeRegenCount += 3;
				player.endurance += 0.05f;
				player.moveSpeed += 0.3f;



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
