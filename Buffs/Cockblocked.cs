using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Buffs
{

   

	public class Cockblocked : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cockblocked");
			Description.SetDefault("An mysterious power is preventing you from placing more shields");
			
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		}

	
	}
}