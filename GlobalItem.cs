
using Terraria.ID;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Utilities;


using DivergencyMod.Items;



namespace Terraria.ModLoader
{
    public class ModGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }


        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.SpikyBall)
            {
                item.ammo = ItemID.SpikyBall;

            }
        }

        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            {
                if (weapon.useAmmo == ItemID.SpikyBall)
                {
                    if (ammo.type == ItemID.SpikyBall)
                    {
                        type = NPCID.SpikeBall;
                    }
                }
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            return true;
        }
    }
}

              