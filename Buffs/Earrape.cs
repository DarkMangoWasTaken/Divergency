using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Buffs
{

    public class Earrape : ModBuff
    {
        public float oldDefense;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Earraped");
            Description.SetDefault("I can't hear you it's to dark in here!");

            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            oldDefense = npc.defense;

            npc.defense = npc.defDefense - 5;
        }
    } 
}