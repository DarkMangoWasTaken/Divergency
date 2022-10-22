using DivergencyMod.Dusts.Particles;
using DivergencyMod.Dusts.Particles.CorePuzzleParticles;
using DivergencyMod.Helpers;
using DivergencyMod.Items.Weapons.Melee.NaturesWrath;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using System.IO;
using System.Threading;
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
    public class ANDCoreTile : ModTile
    {

        private Vector2 zero = Vector2.Zero;
        private bool Shoot;

        public override void SetStaticDefaults()
        {

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            Main.tileLighted[Type] = false;
            Main.tileBouncy[Type] = false;


            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(120, 85, 60), Language.GetText("AND Gate"));
            DustType = 7;

        }
   

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {

            


                r = 1.45f;
                g = 2.55f;
                b = 0.94f;
            
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Placeable.Furniture.MinionBossTrophy>());
        }
       
        public int timer2 = 0;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/ANDCoreTile").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/ANDCoreTileCharged1").Value;
            Texture2D tex3 = ModContent.Request<Texture2D>("DivergencyMod/Tiles/LivingTree/ANDCoreTileCharged2").Value;
            int left = i - Main.tile[i, j].TileFrameX / 18;
            int top = j - Main.tile[i, j].TileFrameY / 18;  
            Vector2 pos = new Vector2(left * 16 + 16f, top * 16 + 16f);

            Tile tile = Framing.GetTileSafely(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {

                if (!Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                }
                else if (Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);

                }
                if (Main.tileBouncy[Type] && Main.tileLighted[Type])
                {
                    spriteBatch.Draw(tex3, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, Color.White);
                    if (!Shoot)
                    {
                        Vector2 speed = new Vector2(3, 0);

                        timer2++;
                        for (int jo = 0; jo < 10; jo++)
                        {
                            Vector2 vel = Main.rand.NextVector2Circular(1f, 1f);

                            if (timer2 == 120)
                            {
                                ParticleManager.NewParticle(pos, vel * 10, ParticleManager.NewInstance<GateParticle>(), Color.Purple, 0.9f);
                                timer2 = 0;
                            }


                        }
                        Projectile.NewProjectile(null, pos, speed, ModContent.ProjectileType<GateProjectile>(), 0, 0);
                        SoundEngine.PlaySound(SoundID.NPCDeath44 with { Volume = 1.1f, Pitch = Main.rand.NextFloat(1f), MaxInstances = 400 });
                        SoundEngine.PlaySound(SoundID.Item34 with { Volume = 1f, Pitch = Main.rand.NextFloat(0.5f, 2f), MaxInstances = 400 });


                        Shoot = true;
                    }
                }
                if (!Main.tileBouncy[Type] && !Main.tileLighted[Type])
                {
                    Shoot = false;
                }


            }

            return false;

        }

    }
    internal class ANDCore : ModItem
    {
        public override string Texture => "DivergencyMod/Tiles/LivingTree/ANDCoreTile";

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
            Item.createTile = ModContent.TileType<ANDCoreTile>();
        }
    }
    internal class GateProjectile : ModProjectile ///////////// HERE YOU BLIND FUCK 
    {
        public override string Texture => "DivergencyMod/Bosses/Forest/LivingFlameBlast";
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 20;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;

        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Blast");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public int Timer = 0;
        public override void AI()
        {
            Player player = Main.LocalPlayer;

            Timer++;
            if (Timer < 180)
            {
                player.GetModPlayer<DivergencyPlayer>().ScreenPosOnProjectile = true;
            }
            else
            {
                player.GetModPlayer<DivergencyPlayer>().ScreenPosOnProjectile = false;

            }
            for (int j = 0; j < 4; j++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                ParticleManager.NewParticle(Projectile.Center, speed * 60, ParticleManager.NewInstance<FancyParticle>(), Color.Purple, 0.9f);


            }
            if (Timer == 1)
            {
                ParticleManager.NewParticle(Projectile.Center, Projectile.velocity, ParticleManager.NewInstance<PodestProjBase>(), Color.Purple, 1.3f, Projectile.whoAmI, Projectile.whoAmI);

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
           // Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
 
            if (Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].TileType == ModContent.TileType<LivingCoreCrystalTile>())
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

                    if (Main.tile[checkX, checkY].TileType == ModContent.TileType<LivingCoreCrystalTile>())

                    {
                        Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                        WorldGen.KillTile(checkX, checkY);
                        WorldGen.KillTile(checkX, checkY + 1);
                        WorldGen.KillTile(checkX, checkY - 1);


                        player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 20;
                        for (int j = 0; j < 10; j++)
                        {
                            ParticleManager.NewParticle(Projectile.Center, speed * 10, ParticleManager.NewInstance<CrystalParticle>(), Color.Purple, 0.9f);
                        }

                    }
                }
            }
                    Vector2 pos = Projectile.position;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Projectile.spriteDirection = Projectile.direction;
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
                prim2 = new TrailRenderer(TrailTex, TrailRenderer.DefaultPass, (p) => new Vector2(20f) * (1f - p), (p) => Projectile.GetAlpha(Color.White) * 0.9f * (float)Math.Pow(1f - p, 2f));
                prim2.drawOffset = Projectile.Size / 2f;
            }
            prim.Draw(Projectile.oldPos);
            prim2.Draw(Projectile.oldPos);


            return false;
        }
    }
   
    
}

