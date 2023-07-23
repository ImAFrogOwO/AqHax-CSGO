using AqHaxCSGO.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AqHaxCSGO.Hacks
{
    static class FovChanger 
    {

        public static void ChangeFoVThread()
        {
            while (true)
            {

                if (!Globals.FoV)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                if (!EngineDLL.InGame)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                for (var i = 0; i < 10; i++)
                {
                    CBaseCombatWeapon currentWeapon = GlobalLists.weaponList[i];

                    currentWeapon.Recoil = 0;
                    currentWeapon.Ammo = 999;

                }

            }
        }

    }
}
