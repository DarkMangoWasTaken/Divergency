using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.LivingCoreBow
{
    public class LivingCoreBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Bow"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault(" -- | O_O | -- ");
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 0;
            Item.height = 0;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.crit = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Ranged.LivingCoreArrow>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            // Item.scale = 1.4f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Weapons.Ranged.LivingCoreArrow>(), damage, knockback, player.whoAmI);

            return false; // base.Shoot(player, source, position, velocity, ModContent.ProjectileType<Projectiles.Weapons.Ranged.LivingCoreArrow>(), damage, knockback);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
    }
}