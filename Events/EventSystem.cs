using DivergencyMod.Events.LivingCore;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Events
{
    public class EventSystem : ModSystem
	{
		public override void PreUpdateInvasions()
		{
			if (LivingCoreEvent.Active)
				LivingCoreEvent.Update();
		}

		public override void PostDrawTiles()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			if (LivingCoreEvent.Active)
				LivingCoreEvent.Draw(Main.spriteBatch);

			Main.spriteBatch.End();
		}

		public override void OnWorldLoad()
		{
			LivingCoreEvent.Load();
		}

		public override void OnWorldUnload()
		{
			LivingCoreEvent.Unload();
		}
	}
}
