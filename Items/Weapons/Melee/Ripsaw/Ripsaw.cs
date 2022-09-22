using DivergencyMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Melee.Ripsaw
{
    public class Ripsaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ripsaw");
            Tooltip.SetDefault("'These filthy trees will shat themselves!'"
            +"\nPenetrates 3 enemy defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 9;
            Item.axe = 5;
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.ArmorPenetration = 3;
            Item.shoot = ProjectileType<RipsawPro>();
            Item.shootSpeed = 30f;

            Item.width = Item.height = 16;
            Item.scale = 1.3f;

            Item.useTime = Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noUseGraphic = true;
            Item.channel = true;

            Item.value = Item.sellPrice(0, 1, 40, 0);
            Item.rare = ItemRarityID.Blue;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item23 with { Volume = 1f, MaxInstances = 1 }); 

            return true;
        }
    }

    public class RipsawPro : ModProjectile
    {
        public float rot ;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ripsaw");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 22;
            Projectile.scale = 1.3f;
            Projectile.hide = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.ownerHitCheck = true;

            Projectile.aiStyle = 20;
        }

        public override void AI()
        {
            rot += 0.2f;
    
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 0, default, 1.2f);
            dust.noGravity = true;
            Player player = Main.player[Projectile.owner];
            //  Projectile.rotation = Projectile.rotation + MathHelper.ToRadians(90);   
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<DivergencyPlayer>().ScreenShakeIntensity = 2;

            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Projectile.velocity * 0.1f, ProjectileType<RipsawBackshot>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Projectile.owner);

            for (int k = 0; k < Main.rand.Next(3, 10); k++)
            {
                Dust.NewDustPerfect(target.Center, DustID.Blood, (Projectile.velocity * Main.rand.NextFloat(0.1f, 0.3f)).RotatedByRandom(0.3f), 0, default, 2f);
                Dust dust = Dust.NewDustPerfect(target.Center, DustID.Torch, (Projectile.velocity * Main.rand.NextFloat(0.1f, 0.3f)).RotatedByRandom(0.3f), 0, default, 2f);
                dust.noGravity = true;
                dust = Dust.NewDustPerfect(target.Center, DustID.Smoke, (Projectile.velocity * Main.rand.NextFloat(0.1f, 0.3f)).RotatedByRandom(0.3f), 0, default, 1f);
                dust.noGravity = true;
            }

           // SoundEngine.PlaySound(new SoundStyle("DivergenceMod/Assets/Sounds/Items/RipsawCut")
           // {
               // Volume = 0.8f,
                //PitchVariance = 0.1f,
                //MaxInstances = 3,
            //});
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Texture2D texture = ModContent.Request<Texture2D>("DivergencyMod/Items/Weapons/Melee/Ripsaw/Saw").Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(20f * player.direction, Projectile.gfxOffY),
                sourceRectangle, drawColor, rot, origin, Projectile.scale, SpriteEffects.None, 0);

            return true;
        }
    }
}