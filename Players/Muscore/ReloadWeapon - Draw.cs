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

namespace DivergencyMod.Players.Muscore
{
	public class ItemSwapKeybindDraw : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            IReloadWeapon iweapon = drawInfo.drawPlayer.HeldItem.ModItem as IReloadWeapon;

			return iweapon != null;
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.BeetleBuff); // need a higher layer dont really know

		protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            ReloadWeapon modPlr = drawInfo.drawPlayer.GetModPlayer<ReloadWeapon>();
			IReloadWeapon iweapon = drawInfo.drawPlayer.HeldItem.ModItem as IReloadWeapon;

            // draw bullet ui

            Texture2D Bullet = (Texture2D)ModContent.Request<Texture2D>(iweapon.BulletTexture);

            int curBullets = iweapon.GetRemainingBullets();

            for (int i = 0; i < curBullets; i++)
            {
                Rectangle bulletRect = new Rectangle(0, 0, 14, 22);

                int spaceBetween = 14; // gets div by 2

                int pos = -(((14 + spaceBetween) * curBullets) / 2);
                pos += (14 + spaceBetween / 2) * i+spaceBetween;

                drawInfo.DrawDataCache.Add(new DrawData(Bullet, new Vector2(Main.screenWidth / 2+ pos, Main.screenHeight / 2 - 60f), bulletRect, Color.White, 0f, new Vector2(7, 11), 1f, SpriteEffects.None, 0));
            }
            
			// draw reload ui

			if (modPlr.itemReloading != null)
            {
                //Texture2D LoadingBorder = (Texture2D)ModContent.Request<Texture2D>(iweapon.);
                Texture2D Pixel = (Texture2D)ModContent.Request<Texture2D>("DivergencyMod/Placeholder/WhitePixel");

                int width = (int)MathF.Floor((float)100 * ((float)modPlr.timeTillReload / ((float)modPlr.secondsToReload * 60)));

                //Rectangle back = new Rectangle(0, 0, 112, 32);
                Rectangle fill = new Rectangle(0, 0, width, 12);

                Vector3 colorStart = new Vector3(0.15f, 0.59f, 0.31f);
                Vector3 colorEnd = new Vector3(0.47f, 0.93f, 0.64f);

                Vector3 resColor = colorStart + ((colorEnd - colorStart) / 100f * (float)width);

                //drawInfo.DrawDataCache.Add(new DrawData(LoadingBorder, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 60f), back, Color.White, 0f, new Vector2(56f, 16f), 1f, SpriteEffects.None, 0));
                drawInfo.DrawDataCache.Add(new DrawData(Pixel, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 60f), fill, new Color(resColor), 0f, new Vector2(50f, 6f), 1f, SpriteEffects.None, 0));
            }
		}
	}
}