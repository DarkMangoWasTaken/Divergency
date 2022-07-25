
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.IO;
using DivergencyMod.Helpers;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public class DivergencyWorld : ModSystem
{
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
	{
		
		int TreeTask = tasks.FindIndex(genpass => genpass.Name.Equals("World Gen Tree Build"));
		if (TreeTask != -1)
		{

			tasks.Insert(TreeTask + 1, new PassLegacy("World Gen Tree Build", WorldGenTreeTask));

		}








	}

	private void WorldGenTreeTask(GenerationProgress progress, GameConfiguration configuration)
	{
		// 7. Setting a progress message is always a good idea. This is the message the user sees during world generation and can be useful for identifying infinite loops.      
		progress.Message = "Growing a big ass tree";
		int xz = WorldGen.genRand.Next(0, Main.maxTilesX);
		int yz = WorldGen.genRand.Next((int)WorldGen.worldSurface, Main.maxTilesY);
		for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 1); k++)
		{
			// 10. We randomly choose an x and y coordinate. The x coordinate is choosen from the far left to the far right coordinates. The y coordinate, however, is choosen from between WorldGen.worldSurfaceLow and the bottom of the map. We can use this technique to determine the depth that our ore should spawn at.
			Tile tile = Main.tile[xz, yz];	
			Point Loc = new Point(xz + Main.rand.Next(0, 900), yz + Main.rand.Next(0, 900));

			if (tile.HasTile && tile.TileType == TileID.Dirt)
			{
				StructureLoader.ReadStruct(Loc, "DivergencyMod/Structures/GiantTree");
			}

			// 11. Finally, we do the actual world generation code. In this example, we use the WorldGen.TileRunner method. This method spawns splotches of the Tile type we provide to the method. The behavior of TileRunner is detailed in the Useful Methods section below.

			WorldGen.digTunnel(xz, yz, 1, 0.01f, 100, 5, false);
		}

	}
}

