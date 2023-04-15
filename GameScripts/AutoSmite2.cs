using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LoLSharp.Core;
using LoLSharp.Devices;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;
using SharpDX;
using Point = System.Drawing.Point;

namespace LoLSharp.GameScripts
{
  public enum AutoSmite2State
  {
    FindTarget,
    WaitTargetHp,
    Smite
  }

  [CustomScriptAttribute("Auto Smite",true)]
  public class AutoSmite2 : CustomScriptBase
  {
    private static string[] targets =
    {
      "SRU_Baron",
      "SRU_Dragon_Air",
      "SRU_Dragon_Fire",
      "SRU_Dragon_Water",
      "SRU_Dragon_Earth",
      "SRU_Dragon_Elder",
      "SRU_RiftHerald",
      "SRU_Dragon_Chemtech",
      "SRU_Dragon_Hextech"
    };

    private static string[] targets2 =
    {
      "SRU_Blue",
      "SRU_Red",
      "SRU_Gromp",
      "SRU_Crab",
    };


    private static Dictionary<string, int> smites = new Dictionary<string, int>()
    {
      { "SummonerSmite", 450 },
      { "S5_SummonerSmiteDuel", 450 },
      { "S5_SummonerSmitePlayerGanker", 450 }
    };

    public static AutoSmite2State state = AutoSmite2State.FindTarget;
    public static EntityVo TargetEntityVo;
    public static int Damage;

    public static int LateUpdateCounter;
    private static Point _screenPosition;
    public static AutoSmite2State State
    {
      get => state;
      set
      {
        state = value;

        if (state == AutoSmite2State.FindTarget)
          TargetEntityVo = null;
        LogService.Log("AutoSmite> State: " + state);
      }
    }

    public override void Update()
    {
      PerformanceTimer.Start("Update");


      if (State == AutoSmite2State.Smite)
      {
        CheckSmiteSuccess();
      }

      if (State == AutoSmite2State.WaitTargetHp)
      {
        OnWaitTargetHp();
      }

      if (LateUpdateCounter == 40)
      {
        LateUpdate();
        LateUpdateCounter = 0;
      }

      LateUpdateCounter++;
      PerformanceTimer.Stop("Update");
    }
    private static void CheckSmiteSuccess()
    {
      if (TargetEntityVo == null)
      {
        state = AutoSmite2State.FindTarget;
        return;
      }

      var hp = TargetEntityVo.GetHp();

      if (hp < 1)
      {
        State = AutoSmite2State.FindTarget;
        return;
      }

      if (CheckTargetDistance())
      {
        Smite();
      }
    }
    private static void OnWaitTargetHp()
    {
      if (TargetEntityVo == null)
      {
        State = AutoSmite2State.FindTarget;
        return;
      }

      var hp = TargetEntityVo.GetHp();

      if (hp < 1)
      {
        State = AutoSmite2State.FindTarget;
        return;
      }

      if (hp <= Damage)
      {
        Smite();
      }

      // LogService.Log(TargetEntityVo.name + " Target Hp: " + hp);
    }

    private static void Smite()
    {
      NativeImport.BlockInput(true);

      var currentPos = Cursor.Position;
      _screenPosition = TargetEntityVo.GetScreenPosition();

      Mouse.MouseMove(1038, 1007);
      Mouse.MouseClickLeft();

      Mouse.MouseMove(_screenPosition.X, _screenPosition.Y - 55);

      Mouse.MouseClickLeft();

      Mouse.MouseMove(currentPos.X, currentPos.Y);


      // Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_2);

      LogService.Log("------ Auto Smite ------");

      NativeImport.BlockInput(false);
      // Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_Y);

      State = AutoSmite2State.Smite;
    }

    public override void LateUpdate()
    {
      // LogService.Log("------ Auto Smite LateUpdate ------");

      if (State == AutoSmite2State.FindTarget || LocalPlayer.SettingsVo.SmiteAll)
      {
        FindTarget();
      }

      if (State == AutoSmite2State.WaitTargetHp)
      {
        CheckTargetDistance();
      }
    }

    private static bool CheckTargetDistance()
    {
      var distance = Vector3.Distance(LocalPlayer.GetPosition(), TargetEntityVo.GetPosition());

      if (distance > 1200)
      {
        State = AutoSmite2State.FindTarget;
        return false;
      }

      return true;
    }

    private static void FindTarget()
    {
      EntityVo[] list = null;
      if (LocalPlayer.SettingsVo.SmiteAll)
      {
        list = Engine.Monsters.Where(e => targets.Contains(e.name) || targets2.Contains(e.name)).ToArray();
      }
      else
      {
        list = Engine.Monsters.Where(e => targets.Contains(e.name)).ToArray();
      }

      var mePosition = LocalPlayer.GetPosition();

      var minDistance = float.MaxValue;
      EntityVo t = null;
      foreach (EntityVo target in list)
      {
        target.GetPosition();
        var distance = Vector3.Distance(mePosition, target.Position);
        if (distance > 1000)
        {
          continue;
        }

        if (distance < minDistance)
        {
          target.GetHp();
          if (target.Hp > 0)
          {
            minDistance = distance;
            t = target;
          }
        }
      }

      if (t != null)
      {
        if (TargetEntityVo != null)
        {
          if (TargetEntityVo.name.Equals(t.name))
          {
            return;
          }
        }

        TargetEntityVo = t;

        GetDamage();

        State = AutoSmite2State.WaitTargetHp;
        LogService.Log("AutoSmite> New Target: " + TargetEntityVo.name);
      }
    }


    private static void GetDamage()
    {
      LocalPlayer.ReadSpells();

      // S5_SummonerSmitePlayerGanker
      if (LocalPlayer.SpellMap.ContainsKey("SummonerSmite"))
      {
        Damage = 600;
      }
      else if (LocalPlayer.SpellMap.ContainsKey("S5_SummonerSmitePlayerGanker"))
      {
        Damage = 900;
      }
      else
      {
        Damage = 1200;
      }
    }

    public static void Rengar()
    {
      var entityVo = LocalPlayer.Vo;
      var voGrayHp = entityVo.GetGrayHp();
      if (voGrayHp > 320)
      {
        var position = Cursor.Position;
        Mouse.MouseMove(873, 1007);
        Mouse.MouseClickLeft();
        Mouse.MouseMove(position.X, position.Y);
      }
    }
  }

}
