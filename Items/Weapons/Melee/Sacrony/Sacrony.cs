using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using DivergencyMod.Base;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.DataStructures;
using DivergencyMod.NPCs.Forest;
using ParticleLibrary;
using DivergencyMod.Dusts.Particles;

namespace DivergencyMod.Items.Weapons.Melee.Sacrony

{
    public class SacronyProjSwing : ModProjectile
    {
        #region First two swings
        public static bool swung = false;
        public int SwingTime;
        public float holdOffset = 75f;
        public bool frame1 = true;
        public bool frame2 = false;
        private bool _initialized;
        private int timer;

        public override string Texture => "DivergencyMod/Items/Weapons/Melee/Sacrony/SacronyProj";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 70;
            Projectile.width = 70;
            Projectile.friendly = true;
            Projectile.scale = 0.85f;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public virtual float Lerp(float val)
        {
            return val == 1f ? 1f : (val == 1f ? 1f : (float)Math.Pow(2, val * 10f - 10f) / 2f);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!_initialized && Main.myPlayer == Projectile.owner)
            {
                timer++;

                SwingTime = (int)(16 / player.GetAttackSpeed(DamageClass.Melee));
                Projectile.alpha = 255;
                Projectile.timeLeft = SwingTime;
                _initialized = true;
                Projectile.damage -= 9999;
                Projectile.netUpdate = true;

            }
            else if (_initialized)
            {
                Projectile.alpha = 0;
                if (timer == 1)
                {
                    Projectile.damage += 9999;
                    timer++;
                }
                if (!player.active || player.dead || player.CCed || player.noItems)
                {
                    return;
                }

                if (player.GetModPlayer<FrameSwitchPlayer>().frame0)
                {

                    Projectile.frame = 0;

                }
                if (player.statLife < player.statLifeMax / 3 || (player.GetModPlayer<FrameSwitchPlayer>().frame1))
                {
                    Projectile.frame = 1;
                }
                if (Projectile.frame == 1)
                {
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
                    }
                }
                int dir = (int)Projectile.ai[1];
                float swingProgress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
                // the actual rotation it should have
                float defRot = Projectile.velocity.ToRotation();
                // starting rotation
                float endSet = ((MathHelper.PiOver2) / 0.2f);
                float start = defRot - endSet;

                // ending rotation
                float end = defRot + endSet;
                // current rotation obv
                float rotation = dir == 1 ? start.AngleLerp(end, swingProgress) : start.AngleLerp(end, 1f - swingProgress);
                // offsetted cuz sword sprite
                Vector2 position = player.RotatedRelativePoint(player.MountedCenter);
                position += rotation.ToRotationVector2() * holdOffset;
                Projectile.Center = position;
                Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver2;

                player.heldProj = Projectile.whoAmI;
                player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                player.itemRotation = rotation * player.direction;
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.netUpdate = true;
            
            }



        }

        public override bool ShouldUpdatePosition() => false;

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
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            if ((target.type == NPCID.Skeleton || target.type == NPCID.AngryBones || target.type == NPCID.BoneSerpentBody || target.type == NPCID.BoneSerpentHead
                || target.type == NPCID.BoneSerpentTail || target.type == NPCID.CursedSkull || target.type == NPCID.DarkCaster || target.type == NPCID.Demon || target.type == NPCID.DoctorBones || target.type == NPCID.DungeonGuardian
                 || target.type == NPCID.ZombieEskimo || target.type == NPCID.Ghost || target.type == NPCID.GreekSkeleton || target.type == NPCID.MaggotZombie
                  || target.type == NPCID.SporeSkeleton || target.type == NPCID.ZombieMushroom || target.type == NPCID.Tim || target.type == NPCID.UndeadMiner
                   || target.type == NPCID.UndeadViking || target.type == NPCID.VoodooDemon || target.type == NPCID.Zombie || target.type == NPCID.BoneThrowingSkeleton
                    || target.type == NPCID.SkeletronHand || target.type == NPCID.SkeletronHead || target.type == NPCID.ZombieRaincoat || target.type == NPCID.ArmedTorchZombie
                     || target.type == NPCID.ArmedZombie || target.type == NPCID.ArmedZombieCenx || target.type == NPCID.TorchZombie || target.type == NPCID.ArmedZombieEskimo || target.type == ModContent.NPCType<Zombeye>()))
            {
                player.GetModPlayer<FrameSwitchPlayer>().frame1 = true;
                player.GetModPlayer<FrameSwitchPlayer>().framereset = 400;

            }
            if (player.GetModPlayer<FrameSwitchPlayer>().frame1)
            {
                target.AddBuff(BuffID.OnFire, 60);
                target.AddBuff(BuffID.ShadowFlame, 60);

            }


        }
    
    }
    #endregion
    public class Sacrony : ModItem
    {
        public int AttackCounter = 1;
        public int combowombo = 0;
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.width = 0;
            Item.height = 0;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 4;
            Item.value = 10000;
            Item.noMelee = true;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SacronyProjSwing>();
            Item.shootSpeed = 2.1f;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Master;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int dir = AttackCounter;
            AttackCounter = -AttackCounter;

            player.GetModPlayer<DivergencyPlayer>().itemCombo++;
            player.GetModPlayer<DivergencyPlayer>().itemComboReset = 600;
            if (player.GetModPlayer<DivergencyPlayer>().itemCombo <= 2 || player.GetModPlayer<DivergencyPlayer>().itemCombo == 9)
            {
                Item.UseSound = SoundID.Item1;
                Projectile.NewProjectile(null, position, velocity * 10, ModContent.ProjectileType<SacronyProjSwing>(), damage, knockback, player.whoAmI, 1, dir);
                ParticleManager.NewParticle(position, velocity * 2f, ParticleManager.NewInstance<SlashParticle>(), Color.Purple, 1.5f);
                if (player.GetModPlayer<FrameSwitchPlayer>().frame1 || player.statLife < player.statLifeMax / 3)
                {
                    Projectile.NewProjectile(null, position, velocity.RotatedBy(-0.4f) * 8, ModContent.ProjectileType<SacronyFlameProj>(), damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(null, position, velocity.RotatedBy(0.4f) * 8, ModContent.ProjectileType<SacronyFlameProj>(), damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(null, position, velocity * 8, ModContent.ProjectileType<SacronyFlameProj>(), damage, knockback, player.whoAmI);

                }


            }
            if (player.GetModPlayer<DivergencyPlayer>().itemCombo > 2 && player.GetModPlayer<DivergencyPlayer>().itemCombo != 8 &&
                player.GetModPlayer<DivergencyPlayer>().itemCombo != 0)
            {
                Projectile.NewProjectile(null, position, velocity, ModContent.ProjectileType<SacronyProjStab>(), damage, knockback, player.whoAmI);
                Item.UseSound = SoundID.Item1;
            }
            if (player.GetModPlayer<DivergencyPlayer>().itemCombo == 8)
            {
                Projectile.NewProjectile(null, position, velocity, ModContent.ProjectileType<SacronyProjThrow>(), damage, knockback, player.whoAmI);
                Item.UseSound = SoundID.Item1;
                player.GetModPlayer<DivergencyPlayer>().itemCombo = 0;

            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            


            return false;
        }
    }
    public class FrameSwitchPlayer : ModPlayer
    {
        public bool frame0 = true;
        public bool frame1;
        public int framereset;
        public override void PreUpdate()
        {
            if (frame1 && framereset >= 1)
            {
                framereset--;
            }
            if (framereset == 0)
            {
                frame0 = true;
                frame1 = false;
            }
            if (framereset >= 1)
            {
                frame0 = false;
            }

        }
    }
   
}
