using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivergencyMod.Players.ComboSystem
{
    internal interface IComboSystem
    {
        int[] ComboProjectiles { get; }
        string[] ComboProjectilesIcons { get; }
        float[] ComboProjectilesDamageMultiplers { get; }
        string FullCharge { get; }
        string EmptyCharge { get; }
    }
}
