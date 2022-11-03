using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace DivergencyMod.Players.ComboSystem
{
	// See Common/Systems/KeybindSystem for keybind registration.
	public class ComboSystem : ModPlayer
    {
        public static int StyleResetTimerMax = 200;
        public int StyleResetTimer = StyleResetTimerMax;
        public float Style = 1; // the style score
        public int CurrentStyle = 0; // ranges from 0 to inf
        public int inactiveCharge = 0; // the charge for the next weapon

        public int thisProjStyle = -1;
        public int lastProjStyle = -1;

        public bool didHitThisProj = false;

        public int currentProjectile = -1;

        public void NewAttack()
        {
            lastProjStyle = thisProjStyle;
            thisProjStyle = CurrentStyle;

            StyleResetTimer = StyleResetTimerMax;
            didHitThisProj = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (Player.HeldItem.ModItem as IComboSystem == null)
                return;

            if (lastProjStyle == thisProjStyle && didHitThisProj == false)
                Style *= 0.9f;
            else
                Style *= (1f/0.9f);

            didHitThisProj = true;
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.ModItem as IComboSystem == null)
                return true;

            IComboSystem comboItem = item.ModItem as IComboSystem;

            type = comboItem.ComboProjectiles[CurrentStyle];
            
            NewAttack();
            Style = 1;

            currentProjectile = Projectile.NewProjectile(source, Player.Center, new Vector2(0, 0), type, (int)(Style * item.damage), item.knockBack, Player.whoAmI);
            Player.heldProj = currentProjectile;
            Player.channel = true;

            return false;
        }

        /*(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.ModItem as IComboSystem == null) {
                base.ModifyShootStats(item, ref position, ref velocity, ref type, ref damage, ref knockback);
                return;
            }

            IComboSystem comboItem = item.ModItem as IComboSystem;

            type = comboItem.ComboProjectiles[CurrentStyle];
        }
        */

        private bool didCountMouseDown = false;


        public override void PostUpdate()
        {
            if (currentProjectile != -1)
                Mod.Logger.Info(Main.projectile[currentProjectile].direction);

            if (currentProjectile != -1)
                Main.projectile[currentProjectile].direction = Player.direction;

            StyleResetTimer--;
            if (StyleResetTimer <= 0)
                Style = 1;


            Item item = Player.HeldItem;
            if (item.ModItem as IComboSystem == null)
                return;
            IComboSystem comboItem = item.ModItem as IComboSystem;


            if (Main.mouseRight)
            {
                if (!didCountMouseDown)
                {
                    CurrentStyle++;
                    if (CurrentStyle >= comboItem.ComboProjectiles.Length)
                        CurrentStyle = 0;
                }
            }
            didCountMouseDown = Main.mouseRight;


            if (currentProjectile != -1)
            {
                if (Main.mouseLeft)
                    inactiveCharge++;
                else
                    inactiveCharge = 0;

                if (Main.projectile[currentProjectile].active == false)
                {
                    if (inactiveCharge != 0)
                    { // spawn next projectile at charge

                        int type = comboItem.ComboProjectiles[CurrentStyle];

                        currentProjectile = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0, 0), type, (int)(Style * item.damage), item.knockBack, Player.whoAmI);
                        Player.direction = (Main.mouseX > Main.screenWidth / 2) ? 1 : -1;
                        Main.projectile[currentProjectile].direction = Player.direction;
                        Player.heldProj = currentProjectile;


                        NewAttack();

                        Main.projectile[currentProjectile].ai[0] = inactiveCharge;

                        Player.channel = true;
                    }
                    else
                    {
                        currentProjectile = -1;
                        Player.channel = false;
                    }
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {

        }
	}
}