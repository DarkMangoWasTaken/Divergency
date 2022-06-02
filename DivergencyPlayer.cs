
using DivergencyMod.Items.Weapons.Melee.NaturesWrath;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace DivergencyMod
{
    public class DivergencyPlayer : ModPlayer
    {
        public float ScreenShakeIntensity;
        public int itemCombo;
        public int itemComboReset;
        public int lastSelectedItem;
        public int BranchCooldown = 300;
        public int BranchReload = 0;
        public bool Slowed = false;

        public override void PostUpdateRunSpeeds()
        {
            if (Slowed)
            {
                Player.runAcceleration /= 100;
                Player.maxRunSpeed /= 100;
            }
            else
            {
                Player.runAcceleration *= 1;
                Player.maxRunSpeed *= 1;
            }
        }


        public override void ModifyScreenPosition()
        {
            if (ScreenShakeIntensity > 0.1f)
            {
                Main.screenPosition += new Vector2(Main.rand.NextFloat(ScreenShakeIntensity), Main.rand.NextFloat(ScreenShakeIntensity));

                ScreenShakeIntensity *= 0.9f;
            }
        }


        public override bool PreItemCheck()
        {
            if (Player.selectedItem != lastSelectedItem)
            {
                itemComboReset = 0;
                itemCombo = 0;
                lastSelectedItem = Player.selectedItem;
            }
            if (itemComboReset > 0)
            {
                itemComboReset--;
                if (itemComboReset == 0)
                {
                    itemCombo = 0;
                }
            }

            if (BranchReload == 0 || BranchReload == 1 || BranchReload == 2)
            {
                BranchCooldown--;
            }

            if (BranchCooldown == 0 && Player.HasItem(ModContent.ItemType<NaturesWrath>()))
            {
                if (BranchReload == 2)
                {
                    CombatText.NewText(Player.getRect(), Color.LightGreen, "Branches ready!", true, false);
                }

                BranchReload++;
                BranchCooldown = 300;

                //ParticleManager.NewParticle(Player.position, Player.velocity* 0, ParticleManager.NewInstance<TestParticle>(), Color.Green, 1f, 0);
                SoundEngine.PlaySound(SoundID.NPCHit3);
                const int numberDust = 20;

                for (int i = 0; i < numberDust; i++)
                {
                    Dust dust = Dust.NewDustDirect(Player.position - Player.velocity, Player.width, Player.height, DustID.WoodFurniture, 0, 0, 100, Color.Green, 2.2f);
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    dust = Dust.NewDustDirect(Player.position - Player.velocity, Player.width, Player.height, DustID.WoodFurniture, 0f, 0f, 1000, default, 2.2f);
                }
            }
            if (BranchCooldown == 0 && BranchReload == 2)
            {
            }

            return true;
        }

        public override void ResetEffects()
        {
            if (itemComboReset <= 0)
            {
                itemCombo = 0;
                itemComboReset = 0;
            }
            else
            {
                itemComboReset--;
            }
        }

    }
}