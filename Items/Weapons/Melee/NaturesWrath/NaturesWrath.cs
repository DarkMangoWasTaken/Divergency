using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Melee.NaturesWrath
{
    public class NaturesWrath : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Doorlauncher"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Rams branches into enemies which can be destroyed by a smash attack"
                + "\nPerform a smash attack with Right click");
            DisplayName.SetDefault("Nature's Wrath");
        }

        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Melee;
            Item.width = 0;
            Item.height = 0;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.noMelee = true;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            //Item.shoot = ModContent.ProjectileType<NaturesWrathProj2>();
            Item.shootSpeed = 20f;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                Item.noMelee = true;

                Item.shoot = ModContent.ProjectileType<NaturesWrathProj>();
            }
            else
            {
                Item.noUseGraphic = false;
                Item.noMelee = false;
                if (player.GetModPlayer<DivergencyPlayer>().BranchReload == 1 || player.GetModPlayer<DivergencyPlayer>().BranchReload == 2 || player.GetModPlayer<DivergencyPlayer>().BranchReload == 3)
                {
                    Item.shoot = ModContent.ProjectileType<NaturesWrathProj2>();
                    player.GetModPlayer<DivergencyPlayer>().BranchReload--;
                }
                else
                {
                    Item.shoot = ProjectileID.None;
                }
            }
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //Vector2 Offset = Vector2.Normalize(velocity) * 30f;

            //if (Collision.CanHit(position, 100, -300, position + Offset, 100, -300))
            {
                //position += Offset;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
    }
}