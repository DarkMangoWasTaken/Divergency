
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DivergencyMod.Items.Weapons.Melee.RootBreaker;
using DivergencyMod.Items.Weapons.Melee.LivingWoodGuitar;

namespace DivergencyMod.Items.Accs.Forest
{
	public class LivingCoreHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Heartwarming");

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
			player.GetModPlayer<Aura>().AuraSpawn = true;
			player.GetDamage(DamageClass.Magic) *= 0.8f; // Increase ALL player damage by 100%
			player.GetModPlayer<Aura>().AuraCooldown--;


		}




	}

	public class Aura : ModPlayer
	{
		public bool AuraSpawn;
		public int increasedLifeRegen;
		public int AuraCooldown = 7200;
		public override void ResetEffects()
		{
			// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
			AuraSpawn = false;
			Player.lifeRegen += increasedLifeRegen;
			increasedLifeRegen = 0;
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (Player.HeldItem.DamageType == DamageClass.Magic && AuraSpawn && AuraCooldown <= 0)
			{


				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Player.velocity, ModContent.ProjectileType<AuraProj>(), 0, 1f, Player.whoAmI);
				AuraCooldown = 7200;
				
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{


			if (Player.HeldItem.DamageType == DamageClass.Magic && AuraSpawn && AuraCooldown <= 0)
			{

				
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Player.velocity, ModContent.ProjectileType<AuraProj>(), 0, 1f, Player.whoAmI);
				    AuraCooldown = 7200;

			}
		}
	}
}