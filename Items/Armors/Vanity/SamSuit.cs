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

    public class SamSuit : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Meme Breastplate");
            Tooltip.SetDefault("'Religion is a joke...'");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player)
        {

          


        }
    }
}

