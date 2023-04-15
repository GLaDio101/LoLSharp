using System.Threading;
using System.Windows.Forms;
using LoLSharp.Core;
using LoLSharp.Devices;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;
using SharpDX;

namespace LoLSharp.GameScripts
{
  class Orbwalker2
  {
    public static EntityVo _entity;
    private static float _attackSpeed;
    private static int _lastAaTick;
    private static int _moveTime;

    public static void Update()
    {
      PerformanceTimer.Start("Update2");

      Engine.LoadChampionsManager();

      _entity = Engine.NearEnemyChampInRange(3000);
      _attackSpeed = LocalPlayer.GetAttackSpeed();

      if (_entity != null)
      {
        if (Time() >= _lastAaTick)
        {
          Mouse.MouseClickRight();

          Attack();
          // LogService.Log("Attack: ");
        }
        else
        {
          Move();
        }
        // if (Time() >= _moveTime)
        // {
        //
        //   Move();
        //   LogService.Log("Move: ");
        // }
      }

      PerformanceTimer.Stop("Update2");
    }

    public static void Attack()
    {
      int attackDelay = (int)(1000.0f / _attackSpeed);
      LogService.Log("attackDelay: " + attackDelay);

      if (attackDelay > 550)
      {
        _lastAaTick = (int)(Time() + attackDelay - 300);
      }
      else
      {
        _lastAaTick = (int)(Time());
      }
      // _moveTime = (int)(Time() + 500);

      var screenPosition = _entity.GetScreenPosition();

      var position = Cursor.Position;

      Mouse.MouseMove(screenPosition.X, screenPosition.Y - 35);
      Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_A);
      Thread.Sleep(50);
      Mouse.MouseMove(position.X, position.Y);
    }

    public static void Move()
    {
      Mouse.MouseClickRight();
    }

    public static float Time()
    {
      return Engine.GetGameTime() * 1000;
    }
  }
}
