﻿using DivergencyMod.Dusts.Particles;
using DivergencyMod.Helpers;
using DivergencyMod.Items.Weapons.Melee.LivingCoreSword;
using DivergencyMod.NPCs.Forest;
using DivergencyMod.Tiles.LivingTree;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ModLoader;

namespace DivergencyMod.Events.LivingCore
{
    public class Wave
    {
        public string name;
        public Instance[] enemies;

        public Wave(string name, Instance[] enemies)
        {
            this.name = name;
            this.enemies = enemies;
        }
    }

    public class Instance
    {
        public int NPCID;
        public Vector2 SpawnOffset;

        public Instance(int NPCID, Vector2 SpawnOffset)
        {
            this.NPCID = NPCID;
            this.SpawnOffset = SpawnOffset;
        }
    }
}
