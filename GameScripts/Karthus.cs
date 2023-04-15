using System;
using System.Threading;
using System.Windows.Forms;
using LoLSharp.Devices;
using LoLSharp.Events;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;
using LoLSharp.Overlay.Drawing;
using SharpDX;

namespace LoLSharp.GameScripts
{

  public class Karthus
  {
    public const string Q = "KarthusLayWasteA1";

    private static DateTime _qUseTime;
    private static Vector3 _lastTargetPos;
    private static Vector3 _target;
    private static Vector2 _worldToScreen;
    private static Vector2 _worldToScreen2;
    private static EntityVo _entity;

    public static void Draw()
    {
      try
      {
        if (LocalPlayer.Vo == null)
          return;


        if (_entity == null)
          return;
        var e = _entity;
        var position = LocalPlayer.Vo.GetPosition();
        DrawFactory.DrawCircleRange(position, 880, Color.Aqua, 2.7f);

        // var endPath = _entity.GetAiManager().GetEndPath();
        // var targetPos = _entity.GetPosition();
        // var dQ = new Vector3(endPath.X - targetPos.X, endPath.Y - targetPos.Y, endPath.Z - targetPos.Z);
        // Vector3 direction = Vector3.Normalize(dQ);
        // Vector3 target = targetPos + direction * 110;
        //
        // var worldToScreen3 = Renderer.WorldToScreen(new Vector3(target.X, target.Y, target.Z));

        DrawFactory.DrawLine((int)_worldToScreen.X, (int)_worldToScreen.Y, (int)_worldToScreen2.X, (int)_worldToScreen2.Y, 10, Color.Aqua);
        // DrawFactory.DrawLine((int)_worldToScreen.X, (int)_worldToScreen.Y, (int)worldToScreen3.X, (int)worldToScreen3.Y, 10, Color.Red);

        DrawFactory.DrawCircleRange(_lastTargetPos, e.GetAttackRange(), Color.Aqua, 2.7f);
      }
      catch (Exception exception)
      {
        Console.WriteLine("Draw:" + exception);
        throw;
      }
    }
    public static void Update()
    {
      try
      {
        if (LocalPlayer.Vo == null)
          return;

        // LogService.Log(_qUseTime.Ticks - DateTime.UtcNow.Ticks + " ms");

        if (DateTime.UtcNow.Ticks < _qUseTime.Ticks)
        {
          // LogService.Log("Waiting Q");
          return;
        }

        LocalPlayer.Vo.ReadSpells();

        var entity2 = Engine.LowestHealthEnemyChampInRange(1000);
        if (entity2 != null)
        {
          var voSpellMap = LocalPlayer.Vo.SpellMap;
          if (voSpellMap.ContainsKey("KarthusWallOfPain"))
          {
            var voSpell = voSpellMap["KarthusWallOfPain"];
            if (voSpell.IsReady())
            {
              // NativeImport.BlockInput(true);
              var position = Cursor.Position;

              Mouse.MouseMove((int)_worldToScreen2.X, (int)_worldToScreen2.Y);
              Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_W);

              Thread.Sleep(50);

              Mouse.MouseMove(position.X, position.Y);
              // NativeImport.BlockInput(false);
            }
          }
        }

        var entity3 = Engine.LowestHealthEnemyChampInRange(550);
        if (entity3 != null)
        {
          {
            var voSpellMap = LocalPlayer.Vo.SpellMap;
            if (voSpellMap.ContainsKey("KarthusDefile"))
            {
              var voSpell = voSpellMap["KarthusDefile"];
              if (voSpell.IsReady())
              {
                NativeImport.BlockInput(true);
                Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_E);
                NativeImport.BlockInput(false);
              }
            }
          }
        }

        var entity = Engine.LowestHealthEnemyChampInRange(880);
        if (entity != null)
        {
          var mePos = LocalPlayer.Vo.GetPosition();



          var distance = Engine.Distance(mePos, _target);

          if (distance > 880)
          {
            return;
          }

          NativeImport.BlockInput(true);
          var position = Cursor.Position;
          var cooldownReduction = LocalPlayer.GetCooldownReduction();
          _qUseTime = DateTime.UtcNow.AddMilliseconds(cooldownReduction / 2 * 1000);

          Mouse.MouseMove((int)_worldToScreen2.X, (int)_worldToScreen2.Y);
          Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_Q);

          LogService.Log("_qUseTime");
          Thread.Sleep(50);

          Mouse.MouseMove(position.X, position.Y);
          NativeImport.BlockInput(false);
        }
      }
      catch (Exception exception)
      {
        Console.WriteLine("Update:" + exception);
        throw;
      }
    }
    public static void Update2()
    {
      try
      {
        if (LocalPlayer.Vo == null)
          return;

        _entity = Engine.LowestHealthEnemyChampInRange(1200);
        if (_entity != null)
        {
          var targetWorldPos = _entity.GetPosition();

          bool isMoving = _lastTargetPos != targetWorldPos;


          _lastTargetPos = targetWorldPos;


          var endPath = _entity.GetAiManager().GetEndPath();
          var targetPos = _entity.GetPosition();
          var dQ = new Vector3(endPath.X - targetPos.X, endPath.Y - targetPos.Y, endPath.Z - targetPos.Z);
          Vector3 direction = Vector3.Normalize(dQ);
          Vector3 target = targetPos + direction * 250;
          _target = target;

          if (!isMoving)
          {
            _target = targetWorldPos;
          }

          _worldToScreen = Renderer.WorldToScreen(_lastTargetPos);
          _worldToScreen2 = Renderer.WorldToScreen(_target);
        }
      }
      catch (Exception exception)
      {
        Console.WriteLine("Update2:" + exception);
        throw;
      }
    }
  }
}
