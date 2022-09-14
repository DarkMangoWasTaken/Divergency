using DivergencyMod.Dusts.Particles;
using DivergencyMod.Dusts.Particles.CorePuzzleParticles;
using DivergencyMod.Helpers;
using DivergencyMod.Items.Ammo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ParticleLibrary;
using System;
using System.Drawing.Drawing2D;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DivergencyMod.Tiles.LivingTree
{
    // Simple 3x3 tile that can be placed on a wall
    public class LivingCorePodestTileUp : ModTile
    {
        private Vector2 zero = Vector2.Zero;

        public override void SetStaticDefaults()
        {

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileLighted[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.Width = 2; // unless it's already 2 tiles wide
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 2);

            TileObjectData.addTile(Type);
            Main.tileBouncy[Type] = true;

            AddMapEntry(new Color(120, 85, 60), Language.GetText("Podest"));
            DustType = 7;

        }
        public override bool RightClick(int i, int j)
        {
            Vector2 speed = new Vector2(0f, -10f);

            int left = i - Main.tile[i, j].TileFrameX / 18;
            int top = j - Main.tile[i, j].TileFrameY / 18;


            Vector2 pos = new Vector2(left * 16f + 8f, top * 16f + 8f);
            Player player = Main.LocalPlayer;
            if (!Main.tileLighted[Type])
            {
                if (player.GetModPlayer<CorePuzzle>().LivingCoreAmount != 0)
                {
                    Projectile.NewProjectile(null, pos, speed, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                    player.GetModPlayer<CorePuzzle>().LivingCoreAmount--;
                    Main.tileLighted[Type] = true;
                    SoundEngine.PlaySound(SoundID.NPCDeath44 with { Volume = 1f, Pitch = Main.rand.NextFloat(0.5f, 2f), MaxInstances = 400 });



                }
                else
                {

                }
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.Unlock, -1, -1, null, player.whoAmI, 1f, left, top);
            }
            return true;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {

            if (Main.tileLighted[Type])
            {


                r = 1.45f;
                g = 2.55f;
                b = 0.94f;
            }
            else if(!Main.tileLighted[Type])
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
            Texture2D tex = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCorePodestTileUp").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/LivingCorePodestTileUpCharged").Value;
            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                if (!Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, 9), Color.White);
                }
                else if (Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero + new Vector2(0, 7), Color.White);

                }
            }

            return false;

        }

    }
    internal class LivingCorePodestUp : ModItem
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/LivingCorePodestTileUp";


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
            Item.createTile = ModContent.TileType<LivingCorePodestTileUp>();

        }
    }

    public class PodestProjectile : ModProjectile
    {
        private Vector2 tilePos = Vector2.Zero;
        public static int DoubleCooldown;

        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Blast");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

        }
        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.height = 20;
            Projectile.width = 10;
            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.timeLeft = 3000;

        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void OnSpawn(IEntitySource source)
        {

        }

        public override void AI()
        {

            Timer++;
            for (int j = 0; j < 2; j++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 50, ParticleManager.NewInstance<WraithFireParticle>(), Color.Purple, 0.9f);


            }
            if (Timer == 1)
            {
                ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<PodestProjBase>(), Color.Purple, 1f, Projectile.whoAmI, Projectile.whoAmI);

            }
            if (DoubleCooldown >= 1)
            {
                DoubleCooldown--;
            }
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
        
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Vector2 pos = Projectile.position;
            for (int i = -5; i <= 5; i++)
            {
                bool success = TryFindNearPodest(pos + new Vector2(i * 16f, 0f), out Vector2 result);
                result = tilePos;
               
           
           }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.spriteDirection = Projectile.direction;
            if (
                    
                     Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreMirrorTileDown>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreMirrorTileUp>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreMirrorTileRight>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreMirrorTileLeft>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<XORCoreTile>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<ANDCoreTile>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreDoublerLeftUpTile>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreDoublerDownLeftTile>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreDoublerRightDownTile>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreDoublerUpRightTile>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreRootsTile>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreRootsTile1>()
                    || Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<CoreRootsTile2>())
            {
                // Origin position, in tile format.
                int x = (int)(Projectile.position.X / 16);
                int y = (int)(Projectile.position.Y / 16);

                // Position being checked;



                int checkX = x;
                int checkY = y;
                Tile tile = Framing.GetTileSafely(checkX, checkY);

   
                
                // Checking up to a maximum of 30 tiles.
                for (int b = 0; b < 1; b++)
                {

                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreMirrorTileDown>())

                    {
                        Projectile.velocity = new Vector2(0, 10);
                    }
                    if (tile.TileType == (ushort)ModContent.TileType<CoreMirrorTileUp>())

                    {
                        Projectile.velocity = new Vector2(0, -10);
                    }
                    if (tile.TileType == ModContent.TileType<CoreMirrorTileRight>())

                    {
                        Projectile.velocity = new Vector2(10, 0);
                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreMirrorTileLeft>())

                    {
                        Projectile.velocity = new Vector2(-10, 0);
                    }
                 
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreRootsTile>() && !Main.tileLighted[ModContent.TileType<CoreRootsTile>()])
                    {
                        Projectile.Kill();

                        Main.tileLighted[ModContent.TileType<CoreRootsTile>()] = true;
                       
                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreRootsTile1>() && !Main.tileLighted[ModContent.TileType<CoreRootsTile1>()])
                    {
                        Projectile.Kill();

                        Main.tileLighted[ModContent.TileType<CoreRootsTile1>()] = true;

                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreRootsTile2>() && !Main.tileLighted[ModContent.TileType<CoreRootsTile2>()])
                    {
                        Projectile.Kill();

                        Main.tileLighted[ModContent.TileType<CoreRootsTile2>()] = true;

                    }

                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<XORCoreTile>() && Main.tileLighted[ModContent.TileType<XORCoreTile>()])
                    {
                        Main.tileBouncy[ModContent.TileType<XORCoreTile>()] = true;

                        Projectile.Kill();
                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<XORCoreTile>() && !Main.tileLighted[ModContent.TileType<XORCoreTile>()])
                    {
                        Main.tileLighted[ModContent.TileType<XORCoreTile>()] = true;

                        Projectile.Kill();
                    }

                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<ANDCoreTile>() && Main.tileLighted[ModContent.TileType<ANDCoreTile>()])
                    {
                        Main.tileBouncy[ModContent.TileType<ANDCoreTile>()] = true;

                        Projectile.Kill();
                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<ANDCoreTile>() && !Main.tileLighted[ModContent.TileType<ANDCoreTile>()])
                    {
                        Main.tileLighted[ModContent.TileType<ANDCoreTile>()] = true;

                        Projectile.Kill();
                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreDoublerUpRightTile>())
                    {
                        Vector2 up = new Vector2(0f, -10f);
                        Vector2 right = new Vector2(10f, 0f);

                        if (!Main.tileLighted[ModContent.TileType<CoreDoublerUpRightTile>()])
                        {
                            Projectile.NewProjectile(null, Projectile.Center, up, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                            Projectile.NewProjectile(null, Projectile.Center, right, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                            Main.tileLighted[ModContent.TileType<CoreDoublerUpRightTile>()] = true;
                            Projectile.Kill();


                        }


                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreDoublerLeftUpTile>())
                    {
                        Vector2 up = new Vector2(0f, -10f);
                        Vector2 left = new Vector2(-10f, 0f);

                        if (!Main.tileLighted[ModContent.TileType<CoreDoublerLeftUpTile>()])
                        {
                            Projectile.NewProjectile(null, Projectile.Center, up, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                            Projectile.NewProjectile(null, Projectile.Center, left, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                            Main.tileLighted[ModContent.TileType<CoreDoublerLeftUpTile>()] = true;
                            Projectile.Kill();

                        }




                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreDoublerDownLeftTile>())
                    {
                        Vector2 down = new Vector2(0f, 10f);
                        Vector2 left = new Vector2(-10f, 0f);

                        if (!Main.tileLighted[ModContent.TileType<CoreDoublerDownLeftTile>()])
                        {
                            Projectile.NewProjectile(null, Projectile.Center, down, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                            Projectile.NewProjectile(null, Projectile.Center, left, ModContent.ProjectileType<PodestProjectile>(), 0, 0); 
                            Main.tileLighted[ModContent.TileType<CoreDoublerDownLeftTile>()] = true;
                            Projectile.Kill();

                        }




                    }
                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<CoreDoublerRightDownTile>())
                    {
                        Vector2 down = new Vector2(0f, 10f);
                        Vector2 right = new Vector2(10f, 0f);


                        if (!Main.tileLighted[ModContent.TileType<CoreDoublerRightDownTile>()])
                        {
                            Projectile.NewProjectile(null, Projectile.Center, down, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                            Projectile.NewProjectile(null, Projectile.Center, right, ModContent.ProjectileType<PodestProjectile>(), 0, 0);
                            Main.tileLighted[ModContent.TileType<CoreDoublerRightDownTile>()] = true;
                            Projectile.Kill();


                        }




                    }
                }
            }

        }
      

        public bool TryFindNearPodest(Vector2 position, out Vector2 result)
        {
            if (Main.tile[(int)position.X / 16, (int)position.Y / 16].TileType == ModContent.TileType<LivingCorePodestTileUp>())
            {
                // Origin position, in tile format.
                int x = (int)(position.X / 16);
                int y = (int)(position.Y / 16);

                // Position being checked;

                int checkX = x;
                int checkY = y;

                // Checking up to a maximum of 30 tiles.
                for (int b = 0; b < 30; b++)
                {
                    // If this position is in the world, and if the tile is a Tree tile.
                    if (WorldGen.InWorld(checkX, y) && Main.tile[checkX, checkY].TileType == ModContent.TileType<LivingCorePodestTileUp>())
                    {
                        // Checking if the tile's frames are within the range of tile frames used for the invisible tree top tiles.
                 
                            //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Yellow, null);
                            result = new Vector2(checkX * 16, checkY * 16);
                        
                        // Otherwise, its a success, since it's still a tree tile. Just not the one we're looking for.
                        //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Green, null);
                        return true;

                    }
                    else
                    {
                        // If the tile isn't what we're looking for and since we're only iterating upwards, logically this means its useless to continue.
                        //Dust.QuickDustLine(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 5f, Color.Red);
                        //Dust.QuickDustLine(new Vector2(checkX * 16, (checkY * 16) + 16), new Vector2((checkX * 16) + 16, checkY * 16), 5f, Color.Red);
                        break;
                    }
                }
            }


            result = default;
            return false;
        }
        public TrailRenderer prim;

        public TrailRenderer prim2;
        public override bool PreDraw(ref Color lightColor)
        {
            var TrailTex = ModContent.Request<Texture2D>("DivergencyMod/Trails/Trail").Value;
            Color color = Color.Multiply(new(0.50f, 2.05f, 0.5f, 0), 80);
            if (prim == null)
            {
                prim = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(20f) * (1f - p), (p) => Projectile.GetAlpha(Color.LimeGreen) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim.drawOffset = Projectile.Size / 2f;
            }
            if (prim2 == null)
            {
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(10f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
            prim2.Draw(Projectile.oldPos);


            return false;
        }

    



    }
}
