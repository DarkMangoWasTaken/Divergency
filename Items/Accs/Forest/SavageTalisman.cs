
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DivergencyMod.Items.Weapons.Melee.RootBreaker;
using DivergencyMod.Items.Weapons.Melee.LivingWoodGuitar;

namespace DivergencyMod.Items.Accs.Forest
{
	public class SavageTalisman : ModItem
	{
		public override string Texture => "DivergencyMod/Items/Accs/Forest/LivingCoreHeart";

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("mogus");

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
			player.GetDamage(DamageClass.Melee) *= 1.12f; // Increase ALL player damage by 100%
		

		}




	}

	public class GuardPlayer : ModPlayer
	{
	
		public override void ResetEffects()
		{
			// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
		
		}
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
			if (Main.rand.NextBool(10))
			{
				Player.statLife += 10;
				Player.HealEffect(10);

			}
		}
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {


			if (Main.rand.NextBool(10))
			{
				Player.statLife += 5;
				Player.HealEffect(5);
			}
		}



	}
}