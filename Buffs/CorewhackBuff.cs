using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Buffs
{
	public class CorewhackBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corewhack Buff");
			Description.SetDefault(" -- || --");
			
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 10; // make it last forever
		}
	}
}