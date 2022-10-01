using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Buffs
{

    public class LivingCoreRageBuff : ModBuff
    {
        public bool buffed;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Core Rage");
            Description.SetDefault("RIDE THE FIRE, BORN TO END EVERY LIFE...");

            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        }


        public override void Update(Player player, ref int buffIndex)
        {
         
                player.GetAttackSpeed(DamageClass.Melee) += 0.5f;
                player.moveSpeed += 1f;
                player.statDefense -= 15;
    
            
        }
    } 
}