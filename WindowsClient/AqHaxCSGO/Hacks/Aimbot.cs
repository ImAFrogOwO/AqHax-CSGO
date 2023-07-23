using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using AqHaxCSGO.Objects;
using AqHaxCSGO.Objects.Structs;
using static System.Math;
using static AqHaxCSGO.Objects.GlobalLists;

namespace AqHaxCSGO.Hacks
{
    static class Aimbot
    {
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetAsyncKeyState(
            int vKey
        );

        public static void TriggerThread()
        {
            while (true)
            {
                if (!Globals.TriggerEnabled && !Globals.AimShootOnCollide)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                if (!EngineDLL.InGame)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }

                if (Globals.TriggerPressOnlyEnabled && !Globals.AimShootOnCollide)
                {
                    if ((GetAsyncKeyState(Globals.TriggerKey) & 0x8000) > 0)
                    {
                        if (CBasePlayer.CrosshairID > 0 && CBasePlayer.CrosshairID < EngineDLL.MaxPlayer + 2)
                        {
                            CBaseEntity baseEntity = entityList[CBasePlayer.CrosshairID - 1];
                            if (baseEntity == null) continue;
                            CCSPlayer crossEntity = new CCSPlayer(baseEntity);
                            if (crossEntity == null) continue; // TRIGGER BOT CRASH FIX
                            if (crossEntity != null && crossEntity.Team != CBasePlayer.Team)
                            {
                                Thread.Sleep(1);
                                ClientDLL.ForceAttack(true);
                                Thread.Sleep(5);
                                ClientDLL.ForceAttack(false);
                            }
                        }
                    }
                }
                else
                {
                    if (CBasePlayer.CrosshairID > 0 && CBasePlayer.CrosshairID < EngineDLL.MaxPlayer + 2)
                    {
                        CBaseEntity baseEntity = entityList[CBasePlayer.CrosshairID - 1];
                        if (baseEntity == null) continue;
                        CCSPlayer crossEntity = new CCSPlayer(baseEntity);
                        if (crossEntity == null) continue;
                        if (crossEntity != null && crossEntity.Team != CBasePlayer.Team)
                        {
                            Thread.Sleep(1);
                            ClientDLL.ForceAttack(true);
                            Thread.Sleep(5);
                            ClientDLL.ForceAttack(false);
                        }
                    }
                }

                Thread.Sleep(2);
            }
        }

        public static void AimbotThread()
        {
            while (true)
            {
                if (!Globals.AimEnabled)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                if (!EngineDLL.InGame)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }

                int mp = EngineDLL.MaxPlayer;
                Rectangle screen = Objects.Structs.Misc.GetWindowRect();
                Vector2 screenOrigin = new Vector2(screen.Width / 2, screen.Height / 2);
                double latestDistance = screen.Width;
                Vector3 closestEntityPos = new Vector3(99999f, 0f, 0f);
                for (int i = 0; i < 100; i++)
                {
                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);
                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Health <= 0) continue;
                    if (entity.Team == CBasePlayer.Team && Globals.FreeForAll != true) continue;

                    Vector3 entSelectedPos = entity.GetBonePosition((int)Globals.AimPosition);
                    Vector2 entPosOnScreen;
                    if (entSelectedPos.PointOnScreen(out entPosOnScreen))
                    {
                        if (entPosOnScreen.x > screen.Width || entPosOnScreen.x < 0 || entPosOnScreen.y > screen.Height || entPosOnScreen.y < 0)
                        {
                            continue;
                        }
                    }
                    else continue;

                    double dist = Sqrt(Pow(screenOrigin.x - entPosOnScreen.x, 2) + Pow(screenOrigin.y - entPosOnScreen.y, 2));
                    if (dist < latestDistance)
                    {
                        latestDistance = dist;
                        closestEntityPos = entSelectedPos;
                    }
                }

                if (closestEntityPos.x != 99999f && (GetAsyncKeyState(Globals.TriggerKey) & 0x8000) > 0)
                {
 
                    Angle AimAt = CalculateAngle(CBasePlayer.VectorEyeLevel, closestEntityPos);

                    if (Globals.AimRecoil)
                    {
                        Angle Punch = CBasePlayer.ViewPunchAngle * 2.0f;
                        AimAt.x -= Punch.x;
                        AimAt.y -= Punch.y;
                    }

                    CBasePlayer.ViewAngle = AimAt;

                    if (!Globals.AimShootOnCollide)
                    {
                        if (weaponList.ActiveWeapon.IsSniper())
                        {
                            ClientDLL.ForceRightAttack(true);
                            Thread.Sleep(5);
                            ClientDLL.ForceAttack(true);
                            Thread.Sleep(5);
                            ClientDLL.ForceRightAttack(false);
                            ClientDLL.ForceAttack(false);
                        }
                        else
                        {
                            if (Globals.AutoShoot)
                            {
                                ClientDLL.ForceAttack(true);
                                Thread.Sleep(5);
                                ClientDLL.ForceAttack(false);
                            }
                        }
                    }
                }

                Thread.Sleep(0);
            }
        }

        static Angle CalculateAngle(Vector3 source, Vector3 destination)
        {
            Angle angles;
            Vector3 delta = new Vector3(destination.x - source.x, destination.y - source.y, destination.z - source.z);
            double hypotenuse = Math.Sqrt(delta.x * delta.x + delta.y * delta.y);

            angles.x = (float)(Math.Atan2(-delta.z, hypotenuse) * 180.0 / Math.PI);
            angles.y = (float)(Math.Atan2(delta.y, delta.x) * 180.0 / Math.PI);

            return angles;
        }

    }
}
