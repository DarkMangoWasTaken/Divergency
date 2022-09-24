using DivergencyMod.NPCs.Forest;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DivergencyMod.Helpers;
using System;
using System.Collections.Generic;
using DivergencyMod.Events.LivingCore;

namespace DivergencyMod.Tiles.LivingTree
{
	public class LivingCoreAltarTile1 : ModTile
	{
		public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCoreAltar";

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			Main.tileBouncy[Type] = true;

			Main.tileLighted[Type] = false;
			Main.tileAxe[Type] = false;
			Main.tileBrick[Type] = false;
			Main.tileHammer[Type] = false;
			Main.tileAlch[Type] = false;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(120, 85, 60), Language.GetText("Living Core Altar"));
			DustType = 7;
		}

		public override bool RightClick(int i, int j)
		{
			int left = i - Main.tile[i, j].TileFrameX / 18;
			int top = j - Main.tile[i, j].TileFrameY / 18;

			LivingCoreEvent.Begin(left, top, new Events.LivingCore.Rooms.FirstRoom());
			// TODO: Netsync begin signal

            return true;
        }

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCoreAltar").Value;

			Tile tile = Framing.GetTileSafely(i, j);
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

			if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
			{
				if ((LivingCoreEvent.X != i && LivingCoreEvent.Y != j))
				{
					spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, -0), Color.White);
					Main.tileHammer[Type] = false;
				}
			}

			return false;
		}
	}

	public class LivingCoreAltar : ModItem
	{
		public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCore";

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.value = 1000;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.White;
			Item.createTile = ModContent.TileType<LivingCoreAltarTile1>();
		}
	}

	public class AltarReward : ModProjectile
	{
		public TrailRenderer prim;
		public TrailRenderer prim2;
		public int timer;
		public string path;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override string Texture => "DivergencyMod/Effects/LivingCoreSwordGlow";

		public override void SetDefaults()
		{
			Projectile.damage = 100;
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.height = 20;
			Projectile.width = 10;
			Projectile.friendly = true;
			Projectile.scale = 1f;
			Projectile.timeLeft = 3000;


		}

		public override void AI()
		{
			Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
			float multiplier = 0.4f;
			float max = 1f;
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

			Projectile.gfxOffY = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 10;
			if (DownedHelper.ClearedAltar)
			{
				Projectile.Kill();

			}
			else
			{
				Projectile.active = true;
			}
			timer++;
			Projectile.timeLeft = 10;
			if (timer == 30)
			{
				timer = 0;
				Projectile.rotation = 5.48f;
			}


		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			// behindNPCsAndTiles.Add(index);

		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;

			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			Color drawColor = Projectile.GetAlpha(lightColor);

			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, Projectile.rotation, origin, 0.8f, SpriteEffects.None, 0);

			return false;
		}

	}
}
