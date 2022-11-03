using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using DivergencyMod.Items.Weapons.Melee.LivingCoreSpear;
using DivergencyMod.Players.ComboSystem;

namespace DivergencyMod.Items.Weapons.Melee.LivingCoreSpear
{
    public class LivingCoreSpear : ModItem, IComboSystem
    {
        public int[] ComboProjectiles => new int[] {
            ModContent.ProjectileType<LivingCoreSpearStab>(),
            ModContent.ProjectileType<LivingCoreSpearStab>(),
            ModContent.ProjectileType<LivingCoreSpearStab>(),
            ModContent.ProjectileType<LivingCoreSpearStab>(),
            ModContent.ProjectileType<LivingCoreSpearStab>(),
            ModContent.ProjectileType<LivingCoreSpearStab>(),
            ModContent.ProjectileType<LivingCoreSpearStab>(),
        };

        public override string Texture => "DivergencyMod/Items/Weapons/Melee/LivingCoreSpear/LivingCoreSpear";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The ultimate clusterfuck");

            ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Common Properties
            Item.rare = ItemRarityID.Pink; // Assign this item a rarity level of Pink
            Item.value = Item.sellPrice(silver: 10); // The number and type of coins item can be sold for to an NPC

            // Use Properties
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.useAnimation = 12; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useTime = 30; // The length of the item's use time in ticks (60 ticks == 1 second.)
            Item.UseSound = SoundID.Item71; // The sound that this item plays when used.
            Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            // Weapon Properties
            Item.damage = 19;
            Item.knockBack = 6.5f;
            Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

            // Projectile Properties
            Item.shootSpeed = 3.7f; // The speed of the projectile measured in pixels per frame.
            Item.shoot = ModContent.ProjectileType<LivingCoreSpearStab>(); // The projectile that is fired from this weapon
            Item.channel = true;
            Item.DefaultToSpear(ModContent.ProjectileType<LivingCoreSpearStab>(), 1f, 24);

        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool? UseItem(Player player)
        {
            // Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
               //SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }

            return null;
        }

       
    }

    public class ComboSpearPlayer : ModPlayer
    {
        public int StyleResetTimer; //reset timer for the Style level
        public float Style = 1; //Style level, decreases when hitting enemies with the same attackType but increases 
        public byte AttackType; //1 = Stab, 2 = Swing, 3 = Spin, 4 = Dash, 5 = Jump
        public bool Charged = false; //determines if the player is currently charged or not, to decrease his movement speed while charging 
        public override void PreUpdate()
        {
            //resetting the style level

            //ability to switch between attacks
        }
        public override void PreUpdateMovement()
        {
            if (Charged)
            {
                Player.moveSpeed /= 3;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            //dynamic style system
        }
    }
}   