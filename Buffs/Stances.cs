using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Buffs
{

    public class HealerStance : ModBuff
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Stance");
            Description.SetDefault("Boring healing buffs and decreases stats (boo)");
            Main.buffNoTimeDisplay[Type] = true;

            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        }

        
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 10; // make it last forever

            player.GetDamage(DamageClass.Magic) -= 0.15f;
                player.statDefense += 8;
           
            
    
            
        }
    }
    public class DamageStance : ModBuff
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Damage Stance");
            Description.SetDefault("Awesome magic damage buffs and armor pen, but increased mana usage (epic)");
            Main.buffNoTimeDisplay[Type] = true;
            
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 10; // make it last forever
            player.GetDamage(DamageClass.Magic) += 0.3f;
            player.GetArmorPenetration(DamageClass.Magic) += 5;
            player.GetCritChance(DamageClass.Magic) += 20;
            player.statDefense -= 8;


        }
    }
}