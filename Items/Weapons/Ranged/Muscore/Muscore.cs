using DivergencyMod.Helpers;
using DivergencyMod.Items.Ammo;
using DivergencyMod.Players.Muscore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DivergencyMod.Items.Weapons.Ranged.Muscore
{
    public class Muscore : ModItem, IReloadWeapon
    {
        int shotsLeft = 3;

        public string BulletTexture { get { return "DivergencyMod/Items/Weapons/Ranged/Muscore/Bullet"; } }
        public int GetRemainingBullets() { return shotsLeft; }
        public void Reload()
        {
            shotsLeft = 3;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Muscore");

            Tooltip.SetDefault("some description'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 4;
            Item.crit = 20;
            Item.knockBack = 2f;
            Item.noMelee = true;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.None;

            Item.width = 78;
            Item.height = 32;
            Item.scale = 1f;

            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item167;
            Item.autoReuse = false;
            Item.useTurn = false;

            Item.value = Item.sellPrice(999, 0, 0, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

        }

        public override bool CanUseItem(Player player)
        {
            if (shotsLeft > 0)
                return base.CanUseItem(player);
            else
                player.GetModPlayer<ReloadWeapon>().TryReload(player);

            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shotsLeft--;
            Mod.Logger.Info("Remaining shots: " + shotsLeft);

            return true;
        }

        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }
    }
}