using DivergencyMod.Dusts.Particles;
using Microsoft.Xna.Framework;
using ParticleLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Localization.GameCulture;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod.Items.Weapons.Magic.Corewhack
{
    public class Corewhack : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corewhack");

            Tooltip.SetDefault("Stuff");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 12;
            Item.crit = 0;
            Item.shootSpeed = 12f;
            Item.mana = 3;
            Item.width = 52;
            Item.height = 84;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Magic.Minions.Corewhack_Summon>();

            Item.rare = ItemRarityID.Green;

            Item.autoReuse = true;
            Item.useTurn = false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<Buffs.CorewhackBuff>(), 100);

            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);

            return false;
        }

    }


}