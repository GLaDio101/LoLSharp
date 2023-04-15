using System;
using LoLSharp.Enums;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.Overlay.Drawing;
using SharpDX;
using Point = System.Drawing.Point;
namespace LoLSharp.Core
{
    public class Context
    {
        public static void Log(string message)
        {
            System.Console.WriteLine("log: " + message);
        }
        
        public static void Log(float message)
        {
            System.Console.WriteLine("log: " + message);
        }
        
        public static void Log(int message)
        {
            System.Console.WriteLine("log: " + message);
        }
        
        public static void Log(Exception message)
        {
            System.Console.WriteLine("log error " + message);
        }

        public static void DrawCircle(Vector3 vec, int range, Color color, float thickness)
        {
            DrawFactory.DrawCircleRange(vec, range, color, thickness);
        }

        public static void MouseMove(int x, int y)
        {
            Point vec = new Point(x, y);
            Engine.IssueOrder(GameObjectOrder.MoveTo, vec);
        }

        public static void UpdateMonstersAndMinions()
        {
            Engine.LoadMinionManager();
        }
        
        public static void MouseMove(EntityVo target)
        {
            Engine.IssueOrder(GameObjectOrder.MoveTo, target.GetScreenPosition());
        }
        
        public static EntityVo[] Monsters()
        {
           return Engine.Monsters;
        }
        
        public static EntityVo GetMe()
        {
           return  LocalPlayer.Vo;
        }
        
    }
}
