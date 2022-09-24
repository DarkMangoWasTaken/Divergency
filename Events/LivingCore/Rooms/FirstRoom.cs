using DivergencyMod.NPCs.Forest;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DivergencyMod.Events.LivingCore.Rooms
{
    internal class FirstRoom : LivingCoreRoom
    {
        public override int Music => MusicLoader.GetMusicSlot("DivergencyMod/Sounds/Music/CoreBattle");

        public override Wave? getWave(int wave)
        {
            switch (wave)
            {
                case 1:
                    return new Wave("WAVE 1!",
                        new Instance[]
                        {
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, 300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, 300)),
                        });
                case 2:
                    return new Wave("WAVE 2!",
                        new Instance[]
                        {
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, 300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, 300)),
                        });
                case 3:
                    return new Wave("WAVE 3!",
                        new Instance[]
                        {
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, 300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, 300)),
                        });
                case 4:
                    return new Wave("WAVE 4!",
                        new Instance[]
                        {
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, 300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, -300)),
                            new Instance(ModContent.NPCType<CoreBeamer>(), new Vector2(-300, 300)),
                        });
            }
            return null;
        }
        public override int getWaves()
        {
            return 4;
        }
    }
}
