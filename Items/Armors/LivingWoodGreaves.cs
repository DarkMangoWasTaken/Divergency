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

	public class LivingWoodGreaves : ModItem
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

		public override void UpdateEquip(Player player)
		{

		
				player.GetModPlayer<LeafJump>().JumpLeaf = true;
			player.moveSpeed += 0.10f;
			//will allow a double jump

		}
	}
	public class LeafJump : ModPlayer
	{
		public bool JumpLeaf;
		public const int JumpDown = 0;
		public const int JumpUp = 1;
		public int JumpDir = -1;
		public int JumpVelocity;
        private Vector2 newVelocity;
		public int timer;
        public override void ResetEffects()
		{
			// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
			JumpLeaf = false;


			// ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
			// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
			// If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
		
		}
		public override void PostUpdate()	
		{
			if (timer >= 1)
			{
				timer--;
			}

				if (timer == 0 && JumpLeaf)
			    {
		
			    	if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[JumpUp] < 15)
				    {
					JumpDir = JumpUp;

					Player.velocity.Y -= 20f;
					JumpLeaf = false;
					Player.justJumped = true;
					timer = 600;

				    }
		     	}

		}
	}
}
