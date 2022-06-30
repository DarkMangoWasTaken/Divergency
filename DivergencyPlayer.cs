
using DivergencyMod.Items.Weapons.Melee.NaturesWrath;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using DivergencyMod.NPCs.Forest;
using System;

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
        public int Spawned = 600;



        public bool TryFindTreeTop(Vector2 position, out Vector2 result)
        {

            if (Main.tile[(int)position.X / 16, (int)position.Y / 16].TileType == TileID.Trees)
            {
                // Origin position, in tile format.
                int x = (int)(position.X / 16);
                int y = (int)(position.Y / 16);

                // Position being checked;



                int checkX = x;
                int checkY = y;

                // Checking up to a maximum of 30 tiles.
                for (int b = 0; b < 30; b++)
                {
                    // If this position is in the world, and if the tile is a Tree tile.
                    if (WorldGen.InWorld(checkX, y) && Main.tile[checkX, checkY].TileType == TileID.Trees)
                    {
                        // Checking if the tile's frames are within the range of tile frames used for the invisible tree top tiles.
                        if (Main.tile[checkX, checkY].TileFrameX == 22 && Main.tile[checkX, checkY].TileFrameY >= 198)
                        {
                            //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Yellow, null);
                            result = new Vector2(checkX * 16, checkY * 16);
                            return true;
                        }
                        // Otherwise, its a success, since it's still a tree tile. Just not the one we're looking for.
                        //Dust.QuickBox(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 10, Color.Green, null);
                        checkY--;
                    }
                    else
                    {
                        // If the tile isn't what we're looking for and since we're only iterating upwards, logically this means its useless to continue.
                        //Dust.QuickDustLine(new Vector2(checkX * 16, checkY * 16), new Vector2((checkX * 16) + 16, (checkY * 16) + 16), 5f, Color.Red);
                        //Dust.QuickDustLine(new Vector2(checkX * 16, (checkY * 16) + 16), new Vector2((checkX * 16) + 16, checkY * 16), 5f, Color.Red);
                        break;
                    }
                }
            }

            result = default;
            return false;
        }
     

        public override void PreUpdate()
        {
            if (Spawned > 0)
            {
                Spawned--;
            }
            if (Main.rand.NextBool(200) && Main.dayTime && !Main.IsItAHappyWindyDay && Spawned == 0)
            {
                if (TryFindTreeTop(Player.Center, out Vector2 result))
                {
                    NPC.NewNPC(null, (int)(result.X + Main.rand.NextFloat(-32f, 33f)), (int)(result.Y + Main.rand.NextFloat(-64f, 1f)), ModContent.NPCType<Acorn>());
                    Spawned = 600;
                }
            }
            else if (Main.rand.NextBool(100) && Main.dayTime && Main.IsItAHappyWindyDay && Spawned == 0)
            {
                if (TryFindTreeTop(Player.Center, out Vector2 result))
                {
                    NPC.NewNPC(null, (int)(result.X + Main.rand.NextFloat(-32f, 33f)), (int)(result.Y + Main.rand.NextFloat(-64f, 1f)), ModContent.NPCType<Acorn>());
                    Spawned = 450;
                }
            }

        }

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