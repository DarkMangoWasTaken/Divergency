using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Ranged.Doorlauncher
{
    public class DoorAmmos : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.WoodenDoor)
            {
                item.ammo = item.type;
            }

            if (item.type == ItemID.WoodenDoor || item.type == ItemID.BorealWoodDoor || item.type == ItemID.BambooDoor || item.type == ItemID.SkywareDoor || item.type == ItemID.SlimeDoor || item.type == ItemID.SolarDoor || item.type == ItemID.ShadewoodDoor
                 || item.type == ItemID.VortexDoor || item.type == ItemID.CactusDoor || item.type == ItemID.MushroomDoor || item.type == ItemID.StoneDoor)
            {
                item.ammo = ItemID.WoodenDoor;
                item.maxStack = 9999;
            }
        }

        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            {
                if (weapon.useAmmo == ItemID.WoodenDoor)
                {
                    if (ammo.type == ItemID.WoodenDoor)
                    {
                        type = ProjectileType<FlamingDoorPro>();
                    }

                    if (ammo.type == ItemID.BorealWoodDoor)
                    {
                        type = ProjectileType<BorealDoorPro>();
                    }

                    if (ammo.type == ItemID.BambooDoor)
                    {
                        type = ProjectileType<BambooDoorPro>();
                    }

                    if (ammo.type == ItemID.SkywareDoor)
                    {
                        type = ProjectileType<SkywareDoorPro>();
                    }

                    if (ammo.type == ItemID.SlimeDoor)
                    {
                        type = ProjectileType<SlimeDoorPro>();
                    }

                    if (ammo.type == ItemID.ShadewoodDoor)
                    {
                        type = ProjectileType<ShadewoodDoorPro>();
                    }

                    if (ammo.type == ItemID.CactusDoor)
                    {
                        type = ProjectileType<CactusDoorPro>();
                    }

                    if (ammo.type == ItemID.MushroomDoor)
                    {
                        type = ProjectileType<MushroomDoorPro>();
                    }
                }
            }
        }
    }

    public abstract class DoorProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("a Door");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 32;
            Projectile.scale = 1f;
            Projectile.scale = 1.1f;
            DrawOffsetX = 6;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.aiStyle = 0;

            DoorDefaults();
        }

        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.Length() * 0.04f) * Projectile.direction;

            Projectile.ai[0]++;

            if (Projectile.ai[0] >= 15)
            {
                Projectile.velocity.Y += 0.95f;
            }

            Behavior();
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 16;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<DoorExplosion>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

            return true;
        }

        public virtual void Behavior()
        { }

        public virtual void DoorDefaults()
        { }
    }

    public class FlamingDoorPro : DoorProjectile
    {
        private float Timer;
        private bool initialize = true;

        public override void Behavior()
        {
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                initialize = false;
            }
            Timer++;
            if (Timer == 5)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.VisualPosition, Projectile.velocity * 0.05f, ModContent.ProjectileType<Smoke>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                Timer = 0;
            }

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 1f);
        }

        public override void Kill(int timeLeft)
        {
            Player Player = Main.player[Projectile.owner];

            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8;

            const int numberDusts = 10;

            for (int k = 0; k < numberDusts; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WoodFurniture, 0f, 0f, 0, default, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 1f);
            }

            int goreType = Mod.Find<ModGore>("FrickinDoor_Back").Type;
            int goreTypeAlt = Mod.Find<ModGore>("FrickinDoor_Front").Type;

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
            }

            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/DoorBreak1")
            {
                Volume = 0.9f,
                PitchVariance = 0.2f,

            });
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.OnFire, 120);
            }
        }
    }

    public class BorealDoorPro : DoorProjectile
    {
        private bool initialize = true;
        private float Timer;

        public override void Behavior()
        {
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                initialize = false;
            }
            Timer++;
            if (Timer == 5)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.VisualPosition, Projectile.velocity * 0.05f, ModContent.ProjectileType<BrightSmoke>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                Timer = 0;
            }

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 0, default, 1f);
        }

        public override void Kill(int timeLeft)
        {
            Player Player = Main.player[Projectile.owner];

            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8;

            int goreType = Mod.Find<ModGore>("FrickinDoorBoreal_Back").Type;
            int goreTypeAlt = Mod.Find<ModGore>("FrickinDoorBoreal_Front").Type;

            const int numberDusts = 10;

            for (int k = 0; k < numberDusts; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BorealWood, 0f, 0f, 0, default, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 0, default, 1f);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
            }

            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/DoorBreak1")
            {
                Volume = 0.9f,
                PitchVariance = 0.2f,

            });
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(BuffID.Frostburn, 60);
            }
        }
    }

    public class BambooDoorPro : DoorProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BambooDoor;

        private bool initialize = true;

        public override void Behavior()
        {
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item63, Projectile.position);
                initialize = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player Player = Main.player[Projectile.owner];

            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8;

            int goreType = Mod.Find<ModGore>("FrickinDoorBamboo_Front").Type;
            int goreTypeAlt = Mod.Find<ModGore>("FrickinDoorBamboo_Back").Type;

            const int numberDusts = 10;

            for (int k = 0; k < numberDusts; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OasisCactus, 0f, 0f, 0, default, 1f);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
            }

            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/DoorBreak1")
            {
                Volume = 0.9f,
                PitchVariance = 0.2f,

            });
        }
    }

    public class SkywareDoorPro : DoorProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SkywareDoor;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        private bool initialize = true;

        public override void AI()
        {
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
                initialize = false;
            }

            Projectile.ai[0]++;

            if (Projectile.ai[0] >= 40)
            {
                Projectile.rotation += (Projectile.velocity.Length() * 0.04f) * Projectile.direction;
                Projectile.velocity.Y += 0.15f;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            if (Projectile.ai[0] >= 60)
            {
                Projectile.velocity.Y += 0.80f;
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player Player = Main.player[Projectile.owner];

            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8;

            int goreType = Mod.Find<ModGore>("FrickinDoorSkyware_Back").Type;
            int goreTypeAlt = Mod.Find<ModGore>("FrickinDoorSkyware_Front").Type;

            const int numberDusts = 10;

            for (int k = 0; k < numberDusts; k++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, 0f, 0f, 0, default, 1f);
                dust.noGravity = true;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
            }

            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/DoorBreak1")
            {
                Volume = 0.9f,
                PitchVariance = 0.2f,

            });
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Item[ItemID.SkywareDoor].Value;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawOrigin = new(texture.Width * 0.5f, Projectile.height * 0.5f);
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(6f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }

    public class SlimeDoorPro : DoorProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SlimeDoor;

        private bool initialize = true;

        public override void Behavior()
        {
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item81, Projectile.position);
                initialize = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            const int numberDusts = 10;

            int goreType = Mod.Find<ModGore>("FrickinDoorSlime_Front").Type;
            int goreTypeAlt = Mod.Find<ModGore>("FrickinDoorSlime_Front").Type;

            if (Main.netMode != NetmodeID.Server)
            {
                for (int k = 0; k < 3; k++)
                {
                    Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                    Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
                }
            }
            for (int k = 0; k < numberDusts; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Slime, 0f, 0f, 100, new(89, 164, 254), 1f);
                SoundEngine.PlaySound(SoundID.Item155, Projectile.position);
            }
        }

        private float remainingBounces = 2;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            remainingBounces--;

            if (remainingBounces > 0)
            {
                Projectile.velocity.Y = -Projectile.oldVelocity.Y / 2;
                SoundEngine.PlaySound(SoundID.Item154, Projectile.position);

                return false;
            }
            else
            {
                //Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<DoorExplosion>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
            }

            return true;
        }
    }

    public class ShadewoodDoorPro : DoorProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ShadewoodDoor;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        private bool initialize = true;

        public override void Behavior()
        {
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item102, Projectile.position);

                int numberProjectile = 2;

                for (int k = 0; k < numberProjectile; k++)
                {
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30f));
                    float scale = 1f - (Main.rand.NextFloat() * .3f);

                    Vector2 velocity = perturbedSpeed * scale;
                    Projectile.damage /= 2;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, velocity, ProjectileType<ShadowDoorPro>(), 15, Projectile.knockBack, Projectile.owner);
                }

                initialize = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player Player = Main.player[Projectile.owner];

            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8;

            int goreType = Mod.Find<ModGore>("ShadowDoor1").Type;
            int goreTypeAlt = Mod.Find<ModGore>("ShadowDoor2").Type;

            const int numberDusts = 10;

            for (int k = 0; k < numberDusts; k++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Shadewood, 0f, 0f, 0, default, 1f);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-2, -5)), goreType);
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-2, -5)), goreTypeAlt);
            }

            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/DoorBreak1")
            {
                Volume = 0.9f,
                PitchVariance = 0.2f,

            });
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(7))
            {
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Item[ItemID.ShadewoodDoor].Value;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawOrigin = new(texture.Width * 0.5f, Projectile.height * 0.5f);
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(6f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }

    public class ShadowDoorPro : DoorProjectile
    {
        public override void DoorDefaults()
        {
            Projectile.alpha = 255;
        }

        public override void Behavior()
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Granite, 0f, 0f, 200, default, 2f);
            dust.noGravity = true;

            if (Projectile.alpha > 100)
            {
                Projectile.alpha -= 5;
            }
        }

        public override void Kill(int timeLeft)
        {
            const int numberDusts = 20;

            for (int k = 0; k < numberDusts; k++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Granite, Projectile.velocity.X * 0.1f, -Projectile.velocity.Y * 0.1f, 200, default, 2f);
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.NPCDeath52, Projectile.position);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(7))
            {
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<DoorExplosion>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
            return true;
        }
    }

    public class CactusDoorPro : DoorProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CactusDoor;

        private bool initialize = true;

        public override void Behavior()
        {
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item17, Projectile.position);
                initialize = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player Player = Main.player[Projectile.owner];

            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 8;

            const int numberDusts = 10;
            int numberProjectiles = 3 + Main.rand.Next(3);

            for (int k = 0; k < numberProjectiles; k++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, new(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, -2f)), ProjectileID.RollingCactusSpike, 7, Projectile.knockBack,
                    Projectile.owner);
            }

            for (int k = 0; k < numberDusts; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Cactus, -Projectile.velocity.X * 0.2f, -Projectile.velocity.Y * 0.2f, 0, default, 2f);
            }

            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/DoorBreak1")
            {
                Volume = 0.9f,
                PitchVariance = 0.2f,

            });
        }
    }

    public class MushroomDoorPro : DoorProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MushroomDoor;

        private bool initialize = true;

        public override void Behavior()
        {
            Projectile.damage /= 2;
            if (initialize)
            {
                SoundEngine.PlaySound(SoundID.Item63, Projectile.position);
                initialize = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player Player = Main.player[Projectile.owner];
            int goreType = Mod.Find<ModGore>("FrickinDoorMushroom_Back").Type;
            int goreTypeAlt = Mod.Find<ModGore>("FrickinDoorMushroom_Front").Type;
            Player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 4;

            if (Main.netMode != NetmodeID.Server)
            {
                for (int k = 0; k < 3; k++)
                {
                    Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
                    Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreTypeAlt);
                }
            }

            const int numberDusts = 10;
            int numberProjectiles = 3 + Main.rand.Next(3);

            for (int k = 0; k < numberProjectiles; k++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, new(Main.rand.NextFloat(-15f, 15f), Main.rand.NextFloat(-15f, 15f)), ProjectileID.Mushroom, 6, Projectile.knockBack,
                    Projectile.owner);
            }

            for (int k = 0; k < numberDusts; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GlowingMushroom, 0f, 0f, 0, default, 1f);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Gore.NewGore(null, Projectile.position, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-1, -3)), goreType);
            }

            SoundEngine.PlaySound(new SoundStyle($"{nameof(DivergencyMod)}/Sounds/DoorBreak1")
            {
                Volume = 0.9f,
                PitchVariance = 0.2f,

            });
        }
    }
}