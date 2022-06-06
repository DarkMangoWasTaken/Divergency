
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using DivergencyMod.Items.Weapons.Melee.RootBreaker;

namespace DivergencyMod.Items.Accs.Forest
{
    public class LivingCoreConductor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("guh2");

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
            player.GetDamage(DamageClass.Magic) *= 0.8f; // Increase ALL player damage by 100%
            player.GetModPlayer<HeartDrop>().HeartHeal = true;
            player.GetModPlayer<Aura>().AuraCooldown--;
            player.GetModPlayer<Aura>().AuraSpawn = true;


            //player.GetDamage(DamageClass.Magic) *= 0.9f; // Increase ALL player damage by 100%

        }




    }

   
    
}