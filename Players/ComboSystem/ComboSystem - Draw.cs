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
                // draw the items set of styles, in some way...
                // should be a thing in IComboSystem
            }
		}
	}
}