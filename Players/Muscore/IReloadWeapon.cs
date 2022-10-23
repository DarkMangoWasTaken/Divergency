using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivergencyMod.Players.Muscore
{
    internal interface IReloadWeapon
    { 
        void Reload();
        int GetRemainingBullets();
        string BulletTexture { get; }
    }
}
