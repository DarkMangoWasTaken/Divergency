﻿using DivergencyMod.Buffs;
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
using static ParticleLibrary.Particle;
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
				+ "\nIncreases melee damage dealt by 10%'"
				+ "\nIncreases your melee crit chance by 8%");
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


			player.GetDamage(DamageClass.Melee) += 0.10f; // Increase dealt damage for all weapon classes by 5%
			player.GetCritChance(DamageClass.Melee) += 8;

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
                int goreType1 = Mod.Find<ModGore>("CrystalGore1").Type;
                int goreType2 = Mod.Find<ModGore>("CrystalGore2").Type;
                int goreType3 = Mod.Find<ModGore>("CrystalGore3").Type;
                int goreType4 = Mod.Find<ModGore>("CrystalGore4").Type;
                int goreType5 = Mod.Find<ModGore>("CrystalGore5").Type;
                int goreType6 = Mod.Find<ModGore>("CrystalGore6").Type;
                int goreType7 = Mod.Find<ModGore>("CrystalGore7").Type;

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType1);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType2);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType3);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType4);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType5);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType6);

                }
            }
		}
		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			if (Shieldactive)
			{
				if (ShieldReactivateCooldown == 0)
					ShieldDamage++;
                int goreType1 = Mod.Find<ModGore>("CrystalGore1").Type;
                int goreType2 = Mod.Find<ModGore>("CrystalGore2").Type;
                int goreType3 = Mod.Find<ModGore>("CrystalGore3").Type;
                int goreType4 = Mod.Find<ModGore>("CrystalGore4").Type;
                int goreType5 = Mod.Find<ModGore>("CrystalGore5").Type;
                int goreType6 = Mod.Find<ModGore>("CrystalGore6").Type;
                int goreType7 = Mod.Find<ModGore>("CrystalGore7").Type;

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType1);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType2);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType3);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType4);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType5);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType6);
                    Gore.NewGore(null, Player.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType7);

                }
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
			Projectile.alpha = 255;
			Projectile.width = Projectile.height = 1;
			Projectile.scale = 1.5f;
			Projectile.tileCollide = false;
			

		}
		public override void AI()
		{
			if (Projectile.alpha >= 100)
            {
                Projectile.alpha -= 5;
            }
            Player player = Main.player[Projectile.owner];
			Projectile.timeLeft = 10;
			Projectile.Center = player.Center + new Vector2(-29, -45);
			

				 

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
                    int goreType1 = Mod.Find<ModGore>("CrystalGore1").Type;
                    int goreType2 = Mod.Find<ModGore>("CrystalGore2").Type;
                    int goreType3 = Mod.Find<ModGore>("CrystalGore3").Type;
                    int goreType4 = Mod.Find<ModGore>("CrystalGore4").Type;
                    int goreType5 = Mod.Find<ModGore>("CrystalGore5").Type;
                    int goreType6 = Mod.Find<ModGore>("CrystalGore6").Type;
                    int goreType7 = Mod.Find<ModGore>("CrystalGore7").Type;

                    ParticleManager.NewParticle(Projectile.Center, speed * 30, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 2f, Projectile.whoAmI, Projectile.whoAmI);
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType1);
                        Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType2);
         
                        Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType5);
                        Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType6);

                    }

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

	[AutoloadEquip(EquipType.Head)]
	public class LivingCoreHelmetMage : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Living Core Mage Cowl");
			Tooltip.SetDefault("'It's a perfect fit!'");

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

		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<LivingCoreChestplate>() && legs.type == ModContent.ItemType<LivingCoreGreaves>();

		}
		public override void UpdateArmorSet(Player player)
		{

			player.setBonus = "Gain a defensive shield, if the shield breaks it lowers your defense but increases offensive stats greatly"// This is the setbonus tooltip
							+ "\nShield increases defense up to 20, if the shields breaks, the player loses defense'"
							+ "\nin exchange of bonus melee speed, movement speed and damage)";



			player.GetModPlayer<LivingCoreArmorMage>().initialize = true;

		}

	}
	public class LivingCoreArmorMage : ModPlayer
	{
		public bool initialize;
		public bool noStance;
		public bool HealerStance;
		public bool DamageStance;

		public int DoubleTapTimer = 15;
		public byte DoubleTapCounter;
		private int healingMultiplier = 10;
		private bool showHealEffect;

		public bool ActivateTimer = false;

		public override void ResetEffects()
		{
			initialize = false;
		}
		public override void PreUpdate()
		{
			if (initialize)
			{
				if (ActivateTimer)
				{
					DoubleTapTimer--;
				}
				if (DoubleTapTimer == -1)
				{
					DoubleTapTimer = 120;
					DoubleTapCounter = 0;
					ActivateTimer = false;
				}

				if (Player.controlDown)
				{
					for (int i = 0; i < 1; i++)
					{
						Vector2 speed = Main.rand.NextVector2Unit() * 0.1f;
						ParticleManager.NewParticle(Player.Center + new Vector2(Main.rand.NextFloat(-20, 20)), speed * Main.rand.NextFloat(25, 30), ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 1f, Player.whoAmI, Layer: Layer.BeforeProjectiles);
					}
					DoubleTapCounter++;
					ActivateTimer = true;
				}

				if (DoubleTapCounter >= 120 && DoubleTapTimer <= 0 && !Player.HasBuff(ModContent.BuffType<HealerStance>()) && !Player.HasBuff(ModContent.BuffType<DamageStance>()))
				{
					Player.AddBuff(ModContent.BuffType<HealerStance>(), 1);
					DoubleTapTimer = 0;
					for (int i = 0; i < 30; i++)
					{
						Vector2 speed = Main.rand.NextVector2Unit() * 0.1f;
						ParticleManager.NewParticle(Player.Center + new Vector2(Main.rand.NextFloat(-10, 10)), speed * Main.rand.NextFloat(15, 30), ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 2f, Player.whoAmI, Layer: Layer.BeforeProjectiles);
					}
				}
				else if (DoubleTapCounter >= 120 && DoubleTapTimer <= 0 && Player.HasBuff(ModContent.BuffType<HealerStance>()))
				{
					Player.ClearBuff(ModContent.BuffType<HealerStance>());
					Player.AddBuff(ModContent.BuffType<DamageStance>(), 1);
					DoubleTapTimer = 0;
					for (int i = 0; i < 3; i++)
					{
						Vector2 speed = Main.rand.NextVector2Unit() * 0.1f;
						ParticleManager.NewParticle(Player.Center + new Vector2(Main.rand.NextFloat(-10, 10)), speed * Main.rand.NextFloat(15, 30), ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 2f, Player.whoAmI, Layer: Layer.BeforeProjectiles);
					}
				}
				else if (DoubleTapCounter >= 120 && DoubleTapTimer <= 0 && Player.HasBuff(ModContent.BuffType<DamageStance>()))
				{
					Player.ClearBuff(ModContent.BuffType<DamageStance>());
					DoubleTapTimer = 0;
					for (int i = 0; i < 3; i++)
					{
						Vector2 speed = Main.rand.NextVector2Unit() * 0.1f;
						ParticleManager.NewParticle(Player.Center + new Vector2(Main.rand.NextFloat(-10, 10)), speed * Main.rand.NextFloat(15, 30), ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 2f, Player.whoAmI, Layer: Layer.BeforeProjectiles);
					}
				}


			}
		}



	}

	[AutoloadEquip(EquipType.Head)]
	public class LivingCoreHelmetSummon : ModItem
	{
		public int timer;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Living Core Summoner Headgear");
			Tooltip.SetDefault("'It's a perfect fit!'"
				+ "\nIncreases your max minions by 2'");
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
			player.maxMinions += 2;

		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<LivingCoreChestplate>() && legs.type == ModContent.ItemType<LivingCoreGreaves>();

		}
		public override void UpdateArmorSet(Player player)
		{

			player.setBonus = "Increases all summoner stats in the near of Sentries";
			player.maxTurrets += 1;

			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];

				if (proj.active && player.Distance(proj.Center) <= 300 && proj.sentry)
				{
					timer++;
					player.whipRangeMultiplier += 0.2f;
					player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1.2f;
					player.GetDamage(DamageClass.Summon) *= 1.15f;
					if (timer == 5)
					{
						for (int i2 = 0; i2 < 10; i2++)
						{
							Vector2 speed = Main.rand.NextVector2Unit() * 0.1f;
							ParticleManager.NewParticle(proj.Center + new Vector2(Main.rand.NextFloat(-10, 10)), speed * Main.rand.NextFloat(15, 30), ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 0.5f, proj.whoAmI, Layer: Layer.BeforeProjectiles);
						}
						timer = 0;
					}



				}
			}
		}
	}
   
    
}
		
 







