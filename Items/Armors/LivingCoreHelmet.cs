using DivergencyMod.Buffs;
using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using DivergencyMod.Items.Armors.Vanity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using System.Threading;
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
	public class LivingCoreHelmetMelee : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Living Core War Helmet");
			Tooltip.SetDefault("'It's a perfect fit!'"
				+ "\nIncreases damage dealt by 10%'"
				+ "\nIncreases your crit chance by 8%");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 0;
		}

		public override void UpdateEquip(Player player)
		{


			player.GetDamage(DamageClass.Generic) += 0.10f; // Increase dealt damage for all weapon classes by 5%
			player.GetCritChance(DamageClass.Generic) += 8;
			
			//player.GetAttackSpeed(DamageClass.Melee) += 0.7f;

		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<LivingCoreChestplate>() && legs.type == ModContent.ItemType<LivingCoreGreaves>();

		}
		public override void UpdateArmorSet(Player player)
		{

			player.setBonus = "Gain a defensive shield, if the shield breaks it lowers your defense but increases offensive stats greatly"// This is the setbonus tooltip
							+ "\nShield increases defense up to 20, if the shields breaks, the player loses 20 defense'"
							+ "\nin exchange of bonus melee speed, movement speed and damage)";



			player.GetModPlayer<LivingCoreArmorMelee>().Shieldactive = true;

		}
	}
	public class LivingCoreArmorMelee : ModPlayer
	{
		public bool ProjectileSpawned;
		public bool Shieldactive;
		public byte ShieldDamage;
		public int ShieldReactivateCooldown = 0;
		public bool ShieldReactivate = true;
		public bool RageActive;

		public override void ResetEffects()
		{
			Shieldactive = false;
		}
		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			if (Shieldactive)
			{
				if (ShieldReactivateCooldown == 0)
					ShieldDamage++;
			}
		}
		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			if (Shieldactive)
			{
				if (ShieldReactivateCooldown == 0)
					ShieldDamage++;
			}
		}
		public override void PreUpdate()
		{
			if (Shieldactive)
			{
				if (RageActive)
				{
					Vector2 speed = Main.rand.NextVector2Unit() * 0.02f;
					ParticleManager.NewParticle(Player.Top + new Vector2(0, 10), speed * 30, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 0.4f, Player.whoAmI);

                }
                if (ShieldReactivateCooldown > 0)
				{

					ShieldReactivateCooldown--;
				}
				if (ShieldReactivateCooldown == 0 && ShieldReactivate)
				{
					RageActive = false;
					Projectile.NewProjectile(null, Player.Center + new Vector2(-20, -30), new Vector2(0), ModContent.ProjectileType<LivingCoreShield>(), 0, 0, Player.whoAmI);
					ShieldReactivate = false;
				}
			}

		}

	}
	public class LivingCoreShield : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Living Core Shield");
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.alpha = 100;
			Projectile.width = Projectile.height = 1;
			Projectile.scale = 2f;
			Projectile.tileCollide = false;

		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.timeLeft = 10;
			Projectile.Center = player.Center + new Vector2(-30, -35);
			//Projectile.rotation += 0.02f;
			if (player.GetModPlayer<LivingCoreArmorMelee>().ShieldDamage == 0)
			{
				player.statDefense = player.statDefense + 20;
				Projectile.frame = 0;

			}
			if (player.GetModPlayer<LivingCoreArmorMelee>().ShieldDamage == 1)
			{
				player.statDefense = player.statDefense + 15;
				Projectile.frame = 1;
			}
			if (player.GetModPlayer<LivingCoreArmorMelee>().ShieldDamage == 2)
			{
				player.statDefense = player.statDefense + 10;
				Projectile.frame = 2;

			}
			if (player.GetModPlayer<LivingCoreArmorMelee>().ShieldDamage == 3)
			{
				player.statDefense = player.statDefense + 5;
				Projectile.frame = 3;

			}
			if (player.GetModPlayer<LivingCoreArmorMelee>().ShieldDamage >= 4)
			{
				player.statDefense = player.statDefense - 5;

				Projectile.Kill();
				//ParticleManager.NewParticle(player.Center, Projectile.velocity * 0, ParticleManager.NewInstance<LivingAura>(), Color.Purple, 1f);
				//  ParticleManager.NewParticle(player.Center, Projectile.velocity * 0, ParticleManager.NewInstance<LivingRageParticle>(), Color.Purple, 1f);
				for (int j = 0; j < 60; j++)
				{
					Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

					ParticleManager.NewParticle(Projectile.Center, speed * 30, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 2f, Projectile.whoAmI, Projectile.whoAmI);


				}
				//ParticleManager.NewParticle(player.Center, Projectile.velocity * 0, ParticleManager.NewInstance<LivingAura>(), Color.Purple, 1f);

				player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 30;

				player.GetModPlayer<LivingCoreArmorMelee>().RageActive = true;
                Projectile.NewProjectile(null, player.Center + new Vector2(-20, -30), new Vector2(0), ModContent.ProjectileType<LivingCoreEye>(), 0, 0, player.whoAmI);
				player.AddBuff(ModContent.BuffType<LivingCoreRageBuff>(), 300);
                player.GetModPlayer<LivingCoreArmorMelee>().ShieldReactivate = true;
				player.GetModPlayer<LivingCoreArmorMelee>().ShieldReactivateCooldown = 300;
				player.GetModPlayer<LivingCoreArmorMelee>().ShieldDamage = 0;

			}
		}

	}
	public class LivingCoreEye : ModProjectile
	{
		int timer;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Living Core Shield"); 
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25; // in SetStaticDefaults()
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
		public override void SetDefaults()
		{
			Projectile.alpha = 100;
			Projectile.width = Projectile.height = 1;
			Projectile.scale = 2f;
			Projectile.timeLeft = 300;

		}
		public override void AI()
		{
			timer++;
			Player player = Main.player[Projectile.owner];
			Projectile.Center = player.Top + new Vector2(0, 10);
			if (timer == 1)
			{
                ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<LivingCoreEyeParticle>(), Color.Purple, 1f, Projectile.whoAmI, Projectile.whoAmI);

            }
        }

        public TrailRenderer prim;
        public TrailRenderer prim2;
        public override bool PreDraw(ref Color lightColor)
        {
            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/Trail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(30f) * (1f - p), (p) => Projectile.GetAlpha(Color.LimeGreen) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = Projectile.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(15f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
            prim2.Draw(Projectile.oldPos);


            return false;
        }
    }
}
		
 







