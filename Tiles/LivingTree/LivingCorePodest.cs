using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DivergencyMod.Tiles.LivingTree
{
	// Simple 3x3 tile that can be placed on a wall
	public class LivingCorePodestTile : ModTile
	{
        private static bool ChangeTexture;
        private Vector2 zero= Vector2.Zero;
        private bool AlreadyDrawn;

        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileLighted[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			DustType = 7;

		}
        public override bool RightClick(int i, int j)
        {
            if (!ChangeTexture)
                ChangeTexture = true;
            else
                ChangeTexture = false;

            return true;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (ChangeTexture)
            {


                r = 1.45f;
                g = 2.55f;
                b = 0.94f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }

        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Placeable.Furniture.MinionBossTrophy>());
		}
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = 2;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCorePodestTile").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCorePodestCharged").Value;
            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                if (!ChangeTexture)
                {
                    spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                    AlreadyDrawn = true;
                }
                else
                {
                    spriteBatch.Draw(tex2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                    AlreadyDrawn = true;

                }
            }
     
            return false;

        }

    }
    internal class LivingCorePodest : ModItem
    {

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
            Item.createTile = ModContent.TileType<LivingCorePodestTile>();
        }
    }

    public class PodestProjectile : ModProjectile
    {
        private const float PiBeamDivisor = MathHelper.Pi / 3;

        // How much more damage the beams do when the Prism is fully charged. Damage smoothly scales up to this multiplier.
        private const float MaxDamageMultiplier = 1.5f;

        // Beams increase their scale from 0 to this value as the Prism charges up.
        private const float MaxBeamScale = 1.8f;

        // Beams reduce their spread to zero as the Prism charges up. This controls the maximum spread.
        private const float MaxBeamSpread = 2f;

        // The maximum possible range of the beam. Don't set this too high or it will cause significant lag.
        private const float MaxBeamLength = 2400f;

        // The width of the beam in pixels for the purposes of tile collision.
        // This should generally be left at 1, otherwise the beam tends to stop early when touching tiles.
        private const float BeamTileCollisionWidth = 1f;

        // The width of the beam in pixels for the purposes of entity hitbox collision.
        // This gets scaled with the beam's scale value, so as the beam visually grows its hitbox gets wider as well.
        private const float BeamHitboxCollisionWidth = 22f;

        // The number of sample points to use when performing a collision hitscan for the beam.
        // More points theoretically leads to a higher quality result, but can cause more lag. 3 tends to be enough.
        private const int NumSamplePoints = 3;

        // How quickly the beam adjusts to sudden changes in length.
        // Every frame, the beam replaces this ratio of its current length with its intended length.
        // Generally you shouldn't need to change this.
        // Setting it too low will make the beam lazily pass through walls before being blocked by them.
        private const float BeamLengthChangeFactor = 0.75f;

        // The charge percentage required on the host prism for the beam to begin visual effects (e.g. impact dust).
        private const float VisualEffectThreshold = 0.1f;

        // Each Last Prism beam draws two lasers separately: an inner beam and an outer beam. This controls their opacity.
        private const float OuterBeamOpacityMultiplier = 0.75f;
        private const float InnerBeamOpacityMultiplier = 0.1f;

        // The maximum brightness of the light emitted by the beams. Brightness scales from 0 to this value as the Prism's charge increases.
        private const float BeamLightBrightness = 0.75f;

        // These variables control the beam's potential coloration.
        // As a value, hue ranges from 0f to 1f, both of which are pure red. The laser beams vary from 0.57 to 0.75, which winds up being a blue-to-purple gradient.
        // Saturation ranges from 0f to 1f and controls how greyed out the color is. 0 is fully grayscale, 1 is vibrant, intense color.
        // Lightness ranges from 0f to 1f and controls how dark or light the color is. 0 is pitch black. 1 is pure white.
        private const float BeamColorHue = 0.57f;
        private const float BeamHueVariance = 0.18f;
        private const float BeamColorSaturation = 0.66f;
        private const float BeamColorLightness = 0.53f;
        private float BeamID
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // This property encloses the internal AI variable projectile.ai[1].
        private float HostPrismIndex
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        // This property encloses the internal AI variable projectile.localAI[1].
        // Normally, localAI is not synced over the network. This beam manually syncs this variable using SendExtraAI and ReceiveExtraAI.
        private float BeamLength
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // If the target is touching the beam's hitbox (which is a small rectangle vaguely overlapping the host Prism), that's good enough.
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            // Otherwise, perform an AABB line collision check to check the whole beam.
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + Projectile.velocity * BeamLength;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, BeamHitboxCollisionWidth * Projectile.scale, ref _);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // If the beam doesn't have a defined direction, don't draw anything.
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 centerFloored = Projectile.Center.Floor() + Projectile.velocity * Projectile.scale * 10.5f;
            Vector2 drawScale = new Vector2(Projectile.scale);

            // Reduce the beam length proportional to its square area to reduce block penetration.
            float visualBeamLength = BeamLength - 14.5f * Projectile.scale * Projectile.scale;

            DelegateMethods.f_1 = 1f; // f_1 is an unnamed decompiled variable whose function is unknown. Leave it at 1.
            Vector2 startPosition = centerFloored - Main.screenPosition;
            Vector2 endPosition = startPosition + Projectile.velocity * visualBeamLength;

            // Draw the outer beam.
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, GetOuterBeamColor() * OuterBeamOpacityMultiplier * Projectile.Opacity);

            // Draw the inner beam, which is half size.
            drawScale *= 0.5f;
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, GetInnerBeamColor() * InnerBeamOpacityMultiplier * Projectile.Opacity);

            // Returning false prevents Terraria from trying to draw the Projectile itself.
            return false;
        }

        private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
        {
            Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);

            // c_1 is an unnamed decompiled variable which is the render color of the beam drawn by DelegateMethods.RainbowLaserDraw.
            DelegateMethods.c_1 = beamColor;
            Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
        }

        private Color GetOuterBeamColor()
        {
            // This hue calculation produces a unique color for each beam based on its Beam ID.
            float hue = (2) % BeamHueVariance + BeamColorHue;

            // Main.hslToRgb converts Hue, Saturation, Lightness into a Color for general purpose use.
            Color c = Main.hslToRgb(hue, BeamColorSaturation, BeamColorLightness);

            // Manually reduce the opacity of the color so beams can overlap without completely overwriting each other.
            c.A = 64;
            return c;
        }

        // Inner beams are always pure white so that they act as a "blindingly bright" center to each laser.
        private Color GetInnerBeamColor() => Color.White;
    }
}
