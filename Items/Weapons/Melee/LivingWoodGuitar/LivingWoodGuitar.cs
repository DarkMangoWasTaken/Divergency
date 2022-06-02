using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Melee.LivingWoodGuitar
{
    public class LivingWoodGuitar : ModItem
    {
        public float charge = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Doorlauncher"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("'It sounds horrible'");
            DisplayName.SetDefault("Nature's Serenade");
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 1;
            Item.height = 1;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.knockBack = 1;
            Item.value = 10000;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<LivingResonance>();
            Item.UseSound = SoundID.Item133;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            //Projectile.NewProjectile(player.GetProjectileSource_Item(Item),target.position.X, target.position.Y, target.velocity.X, target.velocity.Y, ModContent.ProjectileType<MusicalCharge>(), Item.damage * 0, Item.knockBack * 0, player.whoAmI, 0, 0);
        }

        //useless for now
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1, 0);

            player.GetModPlayer<DivergencyPlayer>().itemCombo++;
            player.GetModPlayer<DivergencyPlayer>().itemComboReset = 480;
            if (player.GetModPlayer<DivergencyPlayer>().itemCombo == 1 || player.GetModPlayer<DivergencyPlayer>().itemCombo == 2 || player.GetModPlayer<DivergencyPlayer>().itemCombo == 3)
            {
                Item.shoot = ModContent.ProjectileType<LivingResonance>();
                Item.UseSound = SoundID.Item133;
            }
            if (player.GetModPlayer<DivergencyPlayer>().itemCombo >= 4)

            {
                Item.shoot = ModContent.ProjectileType<LivingResonance2>();
                player.GetModPlayer<DivergencyPlayer>().itemCombo = 0;
                Item.UseSound = SoundID.Item136;
            }

            return false; // return true to allow tmodloader to call Projectile.NewProjectile as normal
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}