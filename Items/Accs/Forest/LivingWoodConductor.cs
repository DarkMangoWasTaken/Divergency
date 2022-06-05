
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DivergencyMod.Items.Weapons.Melee.RootBreaker;

namespace DivergencyMod.Items.Accs.Forest
{
    public class LivingWoodConductor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("guh");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(10);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;


        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HeartDrop>().HeartHeal = true;
            player.GetDamage(DamageClass.Magic) *= 0.9f; // Increase ALL player damage by 100%

        }




    }

    public class HeartDrop : ModPlayer
    {
        public bool HeartHeal;
        public override void ResetEffects()
        {
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            HeartHeal = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (Player.HeldItem.DamageType == DamageClass.Magic && HeartHeal)
            {
                if (Player.HeldItem.DamageType == DamageClass.Magic && HeartHeal)
                {

                    for (int i = 0; i < Main.rand.Next(3, 4); i++)
                    {
                        Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-15, -10)), ModContent.ProjectileType<LivingStone>(), target.damage, 1f, Player.whoAmI);
                    }

                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {


            if (Player.HeldItem.DamageType == DamageClass.Magic && HeartHeal)
            {

                for (int i = 0; i < Main.rand.Next(3, 4); i++)
                {
                    Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, new Vector2(Main.rand.Next(-3, 3) * 1.4f, Main.rand.Next(-15, -10)) * 1.4f, ModContent.ProjectileType<LivingStone>(), target.damage, 1f, Player.whoAmI);
                }

            }
        }
    }
}