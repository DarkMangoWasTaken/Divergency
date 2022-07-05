using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Doorlauncher
{
    public class Doorlauncher : ModItem
    {
        public bool Dusty = false;
        public bool Cockblock = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doorlauncher");

            Tooltip.SetDefault("Uses various doors as ammo"
                + "\n'Revolutionary technology'"
                + "\n'So that's the reason they are stealing those doors!'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 35f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 55;
            Item.crit = 3;
            Item.knockBack = 8f;
            Item.noMelee = true;

            Item.useAmmo = ItemID.WoodenDoor;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 17f;

            Item.width = Item.height = 16;
            Item.scale = 1f;

            Item.useTime = Item.useAnimation = 85;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.value = Item.sellPrice(999, 0, 0, 0);
            Item.rare = ItemRarityID.White;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, -10);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int numberDust = 10;

            for (int i = 0; i < numberDust; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(30));

                float scale = 1f - (Main.rand.NextFloat() * 0.75f);
                perturbedSpeed *= scale;

                Dust dust = Dust.NewDustPerfect(position, DustID.Torch, perturbedSpeed, 0, default, 3f);
                dust.noGravity = true;

                if (Main.rand.NextBool(1))
                {
                    perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(30));

                    scale = 1f - (Main.rand.NextFloat() * 0.75f);
                    perturbedSpeed *= scale;

                    dust = Dust.NewDustPerfect(position, DustID.Smoke, perturbedSpeed, 0, default, 2f);
                    dust.noGravity = true;
                }
            }

            return true;
        }

        //useless for now

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}