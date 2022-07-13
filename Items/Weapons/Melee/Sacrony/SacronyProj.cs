using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using DivergencyMod.Base;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Enums;
using ParticleLibrary;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.NPCs.Forest;

namespace DivergencyMod.Items.Weapons.Melee.Sacrony

{
    public class SacronyProjStab : ModProjectile
    {
        #region stabby
        public static bool swung = false;
        public int SwingTime = 15;
        public float holdOffset = 50f;


        public override string Texture => "DivergencyMod/Items/Weapons/Melee/Sacrony/SacronyProj";

		public const int FadeInDuration = 7;
		public const int FadeOutDuration = 4;

		public const int TotalDuration = 16;

		// The "width" of the blade
		public float CollisionWidth => 10f * Projectile.scale;

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacrony");
			Main.projFrames[Projectile.type] = 2;

		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(18); // This sets width and height to the same value (important when projectiles can rotate)
			Projectile.aiStyle = -1; // Use our own AI to customize how it behaves, if you don't want that, keep this at ProjAIStyleID.ShortSword. You would still need to use the code in SetVisualOffsets() though
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true; // Prevents hits through tiles. Most melee weapons that use projectiles have this
			Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
			Projectile.hide = true; // Important when used alongside player.heldProj. "Hidden" projectiles have special draw conditions
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (Timer == 1)
            {
				Projectile.damage /= 2;
            }
			if (Projectile.frame == 1)
			{


				Vector3 RGB = new Vector3(2.51f, 1.83f, 0.65f);
				float multiplier = 1;
				float max = 2.25f;
				float min = 1.0f;
				RGB *= multiplier;
				if (RGB.X > max)
				{
					multiplier = 0.5f;
				}
				if (RGB.X < min)
				{
					multiplier = 1.5f;
				}
				Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);
				for (int j = 0; j < 3; j++)
				{
					Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
					Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, Projectile.velocity.RotatedByRandom(3), 0, default, 0.5f);
				}
			}

			if (player.GetModPlayer<FrameSwitchPlayer>().frame0)
			{

				Projectile.frame = 0;

			}
			else
			{
				Projectile.frame = 1;
			}
			Timer += 1;
			if (Timer >= TotalDuration)
			{
				// Kill the projectile if it reaches it's intented lifetime
				Projectile.Kill();
				return;
			}
			else
			{
				// Important so that the sprite draws "in" the player's hand and not fully infront or behind the player
				player.heldProj = Projectile.whoAmI;
			}

			// Fade in and out
			// GetLerpValue returns a value between 0f and 1f - if clamped is true - representing how far Timer got along the "distance" defined by the first two parameters
			// The first call handles the fade in, the second one the fade out.
			// Notice the second call's parameters are swapped, this means the result will be reverted
			Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

			// Keep locked onto the player, but extend further based on the given velocity (Requires ShouldUpdatePosition returning false to work)
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			if (player.direction == 1)
			{ 
			Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f)  * 2;
                

            }
            else
            {
				Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f) * 2;

			}



			// Set spriteDirection based on moving left or right. Left -1, right 1
			Projectile.spriteDirection = player.direction;

			// Point towards where it is moving, applied offset for top right of the sprite respecting spriteDirection
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// The code in this method is important to align the sprite with the hitbox how we want it to
			SetVisualOffsets();
		}
		

		private void SetVisualOffsets()
		{
			// 32 is the sprite size (here both width and height equal)
			const int HalfSpriteWidth = 62 / 2;
			const int HalfSpriteHeight = 90 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			Player player = Main.player[Projectile.owner];

			if (player.direction == 1)
			{
				DrawOriginOffsetX = 0;
			}
			else
            {
				DrawOriginOffsetX = 0;
				DrawOriginOffsetY = -20;


			}
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

			// Vanilla configuration for "hitbox towards the end"
			//if (Projectile.spriteDirection == 1) {
			//	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
			//	DrawOriginOffsetY = 0;
			//}
			//else {
			//	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = 0;
			//	DrawOriginOffsetY = 0;
			//}
		}

		public override bool ShouldUpdatePosition()
		{
			// Update Projectile.Center manually
			return false;
		}

		public override void CutTiles()
		{
			// "cutting tiles" refers to breaking pots, grass, queen bee larva, etc.
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
			Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity * 10f;
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];

			if (target.type == NPCID.Skeleton || target.type == NPCID.AngryBones || target.type == NPCID.BoneSerpentBody || target.type == NPCID.BoneSerpentHead
				|| target.type == NPCID.BoneSerpentTail || target.type == NPCID.CursedSkull || target.type == NPCID.DarkCaster || target.type == NPCID.Demon || target.type == NPCID.DoctorBones || target.type == NPCID.DungeonGuardian
				 || target.type == NPCID.ZombieEskimo || target.type == NPCID.Ghost || target.type == NPCID.GreekSkeleton || target.type == NPCID.MaggotZombie
				  || target.type == NPCID.SporeSkeleton || target.type == NPCID.ZombieMushroom || target.type == NPCID.Tim || target.type == NPCID.UndeadMiner
				   || target.type == NPCID.UndeadViking || target.type == NPCID.VoodooDemon || target.type == NPCID.Zombie || target.type == NPCID.BoneThrowingSkeleton
					|| target.type == NPCID.SkeletronHand || target.type == NPCID.SkeletronHead || target.type == NPCID.ZombieRaincoat || target.type == NPCID.ArmedTorchZombie
					 || target.type == NPCID.ArmedZombie || target.type == NPCID.ArmedZombieCenx || target.type == NPCID.TorchZombie || target.type == NPCID.ArmedZombieEskimo || target.type == ModContent.NPCType<Zombeye>())
			{
				player.GetModPlayer<FrameSwitchPlayer>().frame1 = true;
				player.GetModPlayer<FrameSwitchPlayer>().framereset = 400;

			}
		}

	}

    #endregion
    public class SacronyProjThrow : ModProjectile
    {
		#region mom i threw up
		public override string Texture => "DivergencyMod/Items/Weapons/Melee/Sacrony/SacronyProj";

		private int framereset;
		private int timer;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			Projectile.damage = 3;
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
		}
		public override void OnSpawn(IEntitySource source)
		{
			ParticleManager.NewParticle(Projectile.Center, Projectile.velocity * 0, ParticleManager.NewInstance<Spin>(), Color.Purple, 0.4f, Projectile.whoAmI, Projectile.whoAmI);
			

		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			float rotation = Projectile.rotation;

			timer++;
			if (timer == 1)
			{
				Projectile.velocity *= 20;
			}
			if (timer <= 15)
            {
				Projectile.velocity *= 0.87f;
            }
			if (Projectile.frame == 1)
            {
				Vector3 RGB = new Vector3(2.51f, 1.83f, 0.65f);
				float multiplier = 1;
				float max = 2.25f;
				float min = 1.0f;
				RGB *= multiplier;
				if (RGB.X > max)
				{
					multiplier = 0.5f;
				}
				if (RGB.X < min)
				{
					multiplier = 1.5f;
				}
				Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);
				for (int j = 0; j < 3; j++)
				{
					Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
					Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, speed * 10, 0, default, 0.7f);
				}
				//Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity.RotatedBy(-0.2f), ModContent.ProjectileType<RandomBullshit>(), damage, knockback, player.whoAmI);
			}

			Projectile.rotation += (Projectile.velocity.Length() * 0.04f) * Projectile.direction;
			if (timer >= 15)
            {	
				Projectile.velocity = Projectile.DirectionTo(player.Center) * 20;
				
			}
			
			if (Projectile.Hitbox.Intersects(player.Hitbox) && timer >= 15)
			{
				Projectile.Kill();

			}
			player.heldProj = Projectile.whoAmI;
			player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = rotation * player.direction;
			Projectile.netUpdate = true;
			if (framereset > 0)
			{
				framereset--;
			}
			if (player.GetModPlayer<FrameSwitchPlayer>().frame0)
			{

				Projectile.frame = 0;

			}
			else
			{
				Projectile.frame = 1;
			}

		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];


			if (player.GetModPlayer<FrameSwitchPlayer>().frame1)
			{
				target.AddBuff(BuffID.OnFire, 120);
			}
		}
		#endregion}
	}
	public class SacronyFlameProj : ModProjectile
    {
		public override string Texture => "Terraria/Images/Item_" + ItemID.LivingFireBlock;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Invoked Projectile");
		}

		public override void SetDefaults()
		{
			Projectile.damage = 3;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.alpha = 125;
		}
  
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			return false;

        }

        public float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		public override void AI()
		{
			Vector3 RGB = new Vector3(2.51f, 1.83f, 0.65f);
			float multiplier = 1;
			float max = 2.25f;
			float min = 1.0f;
			RGB *= multiplier;
			if (RGB.X > max)
			{
				multiplier = 0.5f;
			}
			if (RGB.X < min)
			{
				multiplier = 1.5f;
			}
			Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);
			if (Timer == 1)
			{
				Player player = Main.player[Projectile.owner];
				Projectile.damage /= 2;
				ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<SacronyFlameParticle>(), Color.Purple, 1f);
	
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.ai[0]++;
			Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
			Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, speed * 2, 0, default, 0.5f);

		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];

			if (player.GetModPlayer<FrameSwitchPlayer>().frame1)
			{
				target.AddBuff(BuffID.OnFire, 120);
			}
		}
    }
}