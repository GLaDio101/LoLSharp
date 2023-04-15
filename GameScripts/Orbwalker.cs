using System.Drawing;
using System.Windows.Forms;
using LoLSharp.Devices;
using LoLSharp.Enums;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;

namespace LoLSharp.GameScripts
{
    class Orbwalker
    {
        private static bool IsOrbAttackable = true;

        private static int LastAATick = 0;
        private static Point LastMovePoint;
        public static Point EnemyPosition;

        public static void Update()
        {
            EnemyPosition = ObjectManager.GetEnemyPosition();
            var attackSpeed = LocalPlayer.GetAttackSpeed();
            LogService.Log("attackSpeed: " + attackSpeed);

            int AttackDelay = (int)(1000.0f / attackSpeed);

            if (IsOrbAttackable && EnemyPosition != Point.Empty)
            {
                LastMovePoint = Cursor.Position;

                Engine.IssueOrder(GameObjectOrder.AttackUnit, EnemyPosition);
                Engine.IssueOrder(GameObjectOrder.AttackUnit, EnemyPosition);

                Engine.IssueOrder(GameObjectOrder.MoveTo, LastMovePoint);

                LastAATick = (int)((Engine.GetGameTime() * 1000) +  AttackDelay);

                IsOrbAttackable = false;
            }
            else
            {
                if ((Engine.GetGameTime() * 1000) >= LastAATick)
                {
                    Mouse.MouseClickRight();

                    IsOrbAttackable = true;
                }
                else
                {
                    LastMovePoint = Cursor.Position;
                    Engine.IssueOrder(GameObjectOrder.MoveTo, LastMovePoint);
                    LastMovePoint = Cursor.Position;
                }
            }
        }
    }
}
