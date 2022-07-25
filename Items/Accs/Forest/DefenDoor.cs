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
using DivergencyMod.Items.Armors;
namespace DivergencyMod.Items.Accs.Forest
{

	public class DefenDoor : ModItem
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
			Item.defense = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)

		{
                player.GetModPlayer<Doorspawn>().doorspawner = true;
           
        }
	}
	public class Doorspawn : ModPlayer
	{
		public bool doorspawner;
		public int doorcooldown;
		public const int sgjsdoj = 0;
		public const int SpawnDoor = 1;
		public int DoorDir = -1;
		public int timer;
		public override void ResetEffects()
		{
			doorspawner = false;
		}
		public override void PostUpdate()
		{
			
			if (timer == 0 && doorspawner)
			{

				if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[SpawnDoor] < 15)
				{
					DoorDir = SpawnDoor;
					Projectile.NewProjectile(null, Player.Center, Player.velocity * 0, ModContent.ProjectileType<DefenDoorProj>(), 20, 5);


					timer = 600;

				}
			}
			if (timer >= 1)
			{
				timer--;
			}

		}
	}
}
