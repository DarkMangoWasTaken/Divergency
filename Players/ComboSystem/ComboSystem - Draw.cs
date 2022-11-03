using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security;
using System.IO.Pipelines;

namespace DivergencyMod.Players.ComboSystem
{
	public class ComboSystemDraw : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            IComboSystem iweapon = drawInfo.drawPlayer.HeldItem.ModItem as IComboSystem;

            if (drawInfo.drawPlayer.GetModPlayer<ComboSystem>().Style != 1) // this means that the player has a combo going... so show it
                return true;

            return iweapon != null;
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.BeetleBuff); // need a higher layer dont really know

		protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            // system
            ComboSystem modPlr = drawInfo.drawPlayer.GetModPlayer<ComboSystem>();

            // draw combo counter

            // item
            IComboSystem comboItem = drawInfo.drawPlayer.HeldItem.ModItem as IComboSystem;

            if (comboItem != null)
            {
                int curStyle = modPlr.CurrentStyle;
                int nextStyle = curStyle + 1;
                if (nextStyle >= comboItem.ComboProjectilesIcons.Length)
                    nextStyle = 0;

                Texture2D cStyleIcon = (Texture2D)ModContent.Request<Texture2D>(comboItem.ComboProjectilesIcons[curStyle]);
                Texture2D nStyleIcon = (Texture2D)ModContent.Request<Texture2D>(comboItem.ComboProjectilesIcons[nextStyle]);

                Rectangle rect = new Rectangle(0, 0, 32, 32);

                drawInfo.DrawDataCache.Add(new DrawData(cStyleIcon, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 50f), rect, Color.White, 0f, new Vector2(16, 16), 1f, SpriteEffects.None, 0));
                drawInfo.DrawDataCache.Add(new DrawData(nStyleIcon, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 85f), rect, Color.White, 0f, new Vector2(16f, 16f), 0.7f, SpriteEffects.None, 0));

            }
		}
	}
}