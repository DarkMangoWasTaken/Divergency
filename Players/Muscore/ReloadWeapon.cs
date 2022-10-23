using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace DivergencyMod.Players.Muscore
{
	// See Common/Systems/KeybindSystem for keybind registration.
	public class ReloadWeapon : ModPlayer
	{
		public Item itemReloading = null;
        public int secondsToReload = 2;
        public int timeTillReload = 2;

        public int bulletsOnReload = 0;

        public bool canShoot()
		{
			return (itemReloading == null);
		}

        public override void PreUpdate()
        {
			if (itemReloading == null)
				return;

			if (Player.HeldItem != itemReloading)
				return;

            if (bulletsOnReload != (Player.HeldItem.ModItem as IReloadWeapon).GetRemainingBullets())
			{
				itemReloading = null;
				return;
			}

			timeTillReload--;

			if (timeTillReload == 0)
			{
                IReloadWeapon iweapon = itemReloading.ModItem as IReloadWeapon;

				iweapon.Reload();
			}
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Item weapon = Player.HeldItem;
            IReloadWeapon iweapon = weapon as IReloadWeapon;

            if (iweapon == null)
                return;

            if (KeybindSystem.Reload.JustPressed)
            {
                TryReload(Player);
            }
        }

		public void TryReload(Player plr)
        {
            Item weapon = plr.HeldItem;

            itemReloading = weapon;
            timeTillReload = secondsToReload * 60;
            bulletsOnReload = (weapon.ModItem as IReloadWeapon).GetRemainingBullets();
        }
	}
}