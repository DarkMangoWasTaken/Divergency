using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using DivergencyMod.Items;

namespace DivergencyMod.NPCs.Critters
{
	public class Axolotl : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Axolotl");
			Main.npcFrameCount[Type] = 7;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData 
			{
				
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

		
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

			Main.npcCatchable[NPC.type] = true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Wet, 300);
		}

		public override void SetDefaults() {
			NPC.width = 56;
			NPC.height = 26;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 25;
			NPC.HitSound = SoundID.NPCHit51;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.boss = false;
			NPC.noGravity = true;
			NPC.npcSlots = 10f;

			NPC.aiStyle = 16;
			AIType = NPCID.Goldfish;
			NPC.catchItem = (short)ModContent.ItemType<Items.Consumable.AxolotlItem>();
		}


		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {

			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("A cute little amphibian living in lush enviroments *Fun fact: Axolotls never undergo full metamorphisis and stays in their larval stage unless yuo inject them with iodine (but what kind of horrible person would do that)*.")
			});
		}

		public override void FindFrame(int frameHeight) {

			int startFrame = 0;
			int finalFrame = 6;

			int frameSpeed = 5;
			NPC.frameCounter += 0.5f;
			if (NPC.frameCounter > frameSpeed) {
				NPC.frameCounter = 1;
				NPC.frame.Y += frameHeight;

				if (NPC.frame.Y > finalFrame * frameHeight)
				{
					NPC.frame.Y = startFrame * frameHeight;
				}
			}
		}

		public int Timer;

		public override void AI() 
		{
			NPC.spriteDirection = NPC.direction;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{

			return spawnInfo.SpawnTileType == TileID.JungleGrass && Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY + 1].LiquidType == LiquidID.Water ? 1f : 0f;
		}

        public override void OnCaughtBy(Player player, Item item, bool failed)
        {
			item.stack = 1;

			try
			{

			}
			catch
			{
				return;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			

		}





	}
}
