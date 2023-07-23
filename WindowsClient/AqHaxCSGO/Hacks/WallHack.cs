using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AqHaxCSGO.Objects;
using AqHaxCSGO.Objects.Structs;
using static AqHaxCSGO.Objects.GlobalLists;
using System.Drawing;

namespace AqHaxCSGO.Hacks
{
    static class WallHack
    {
        public static void WallHackThread()
        {
            while (true)
            {
                if (!Globals.WallHackEnabled)
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
                for (int i = 0; i < mp; i++)
                {
                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);

                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Health <= 0) continue;

                    if (entity.Team != CBasePlayer.Team)
                    {

                        string text1 = "Enemy";
                        using (Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            RectangleF rectF1 = new RectangleF(30, 10, 100, 122);
                            Graphics a = Graphics.FromImage(new Bitmap(100, 100));
                            a.DrawString(text1, font1, Brushes.Blue, rectF1);
                        }

                        GlowObject glowObject = entityList[i].GlowObject;
                        glowObject.r = Globals.WallHackEnemy.R / 255;
                        glowObject.g = Globals.WallHackEnemy.G / 255;
                        glowObject.b = Globals.WallHackEnemy.B / 255;
                        glowObject.a = 5f;
                        glowObject.m_bFullBloom = Globals.WallHackFullEnabled;
                        glowObject.BloomAmount = Globals.FullBloomAmount;
                        glowObject.m_nGlowStyle = Globals.WallHackGlowOnly ? 1 : 0;
                        glowObject.m_bRenderWhenOccluded = true;
                        glowObject.m_bRenderWhenUnoccluded = false;

                        entityList[i].GlowObject = glowObject;
                    }
                    else
                    {
                        string text1 = "Teamate";
                        using (Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            RectangleF rectF1 = new RectangleF(30, 10, 100, 122);
                            Graphics a = Graphics.FromImage(new Bitmap(100, 100));
                            a.DrawString(text1, font1, Brushes.Blue, rectF1);
                        }
                        GlowObject glowObject = entityList[i].GlowObject;
                        glowObject.r = Globals.WallHackTeammate.R / 255;
                        glowObject.g = Globals.WallHackTeammate.G / 255;
                        glowObject.b = Globals.WallHackTeammate.B / 255;
                        glowObject.a = 5f;
                        glowObject.m_bFullBloom = Globals.WallHackFullEnabled;
                        glowObject.BloomAmount = Globals.FullBloomAmount;
                        glowObject.m_nGlowStyle = Globals.WallHackGlowOnly ? 1 : 0;
                        glowObject.m_bRenderWhenOccluded = true;
                        glowObject.m_bRenderWhenUnoccluded = false;

                        entityList[i].GlowObject = glowObject;
                    }
                }

                Thread.Sleep(50);
            }
        }

        public static void RenderColorThread()
        {
            while (true)
            {
                if (!Globals.RenderEnabled)
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
                for (int i = 0; i < mp; i++)
                {
                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);
                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Health <= 0) continue;

                    if (entity.Team != CBasePlayer.Team)
                    {
                        RenderColor rco = new RenderColor();
                        rco.r = Globals.RenderColor.R;
                        rco.g = Globals.RenderColor.G;
                        rco.b = Globals.RenderColor.B;
                        rco.a = 255;
                        entity.RenderColor = rco;
                    }

                    if (!Globals.RenderEnemyOnly) 
                    {
                        RenderColor rc = new RenderColor();
                        rc.r = Globals.RenderColor.R;
                        rc.g = Globals.RenderColor.G;
                        rc.b = Globals.RenderColor.B;
                        rc.a = 255;
                        entity.RenderColor = rc;
                    }

                    EngineDLL.ModelAmbientIntensity = Globals.RenderBrightness;
                }

                Thread.Sleep(100);
            }
        }

        public static void RadarThread() 
        {
            while (true) 
            {
                if (!Globals.RadarEnabled)
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
                for (int i = 0; i < mp; i++)
                {

                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);
                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Team == CBasePlayer.Team) continue;

                    if (!entity.Spotted) entity.Spotted = true;

                }

                Thread.Sleep(100);
            }
        }
    }
}
