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

namespace DivergencyMod.Items.Armors.Vanity
{
    [AutoloadEquip(EquipType.Body)]

    public class MonsoonBreastplate : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Living Wood Breastplate");
            Tooltip.SetDefault("'It is really uncomfortable to wear, do I really have to do this?'"
                + "\nIncreases damage dealt by 10%'"
                + "\n Unlocks one Warden slot and increases your max number of Wardens");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.defense = 60;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {

            //will unlock one guardian slot

            player.GetDamage(DamageClass.Generic) += 0.1f; // Increase dealt damage for all weapon classes by 10%

        }
    }
}

