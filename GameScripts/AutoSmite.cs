using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
  public class AutoSmite
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

    private static Dictionary<string, int> smites = new Dictionary<string, int>()
    {
      { "SummonerSmite", 450 },
      { "S5_SummonerSmiteDuel", 450 },
      { "S5_SummonerSmitePlayerGanker", 450 }
    };

    private static EntityVo[] _targetEntities = new EntityVo[] { };
    private static EntityVo _targetEntity = null;

    private static int _damage = 0;
    private static bool _focus;
    private static Point _screenPosition;
    private static float _distance;
    private static float _hp;
    private static bool _smited;
    private static EntityVo _smitedTarget;

    public static void Update()
    {
      if (_smited)
      {
        if (_smitedTarget.GetHp() > 1)
        {
          _distance = Vector3.Distance(LocalPlayer.Vo.GetPosition(), _smitedTarget.GetPosition());

          if (_distance > 1000)
            return;

          _screenPosition = _smitedTarget.GetScreenPosition();

          Mouse.MouseMove(_screenPosition.X, _screenPosition.Y - 25);

          Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_2);

          LogService.Log(" _smited Smite ");
          return;
        }
        
        _smitedTarget = null;
        _smited = false;
      }

      PerformanceTimer.Start("Update");
      try
      {
        FindTarget();

        if (_targetEntity != null)
        {
          var targetEntity = _targetEntity;

          _hp = targetEntity.GetHp();

          LogService.Log("Target Hp: " + _hp);

          if (_damage == 0)
          {
            GetDamage();
          }

          if (_hp > 0 && _hp < _damage + 1000)
          {
            _distance = Vector3.Distance(LocalPlayer.Vo.GetPosition(), targetEntity.GetPosition());
            if (_distance < 1000)
            {
              _focus = true;
            }
            _screenPosition = targetEntity.GetScreenPosition();
            Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_Y);

            while (true)
            {
              _hp = targetEntity.GetHp();

              if (_hp <= _damage)
                break;

              if (LocalPlayer.SettingsVo.SmiteAll)
              {
                _distance = Vector3.Distance(LocalPlayer.Vo.GetPosition(), targetEntity.GetPosition());
                if (_distance > 1000)
                {
                  _focus = false;
                  return;
                }
              }
              LogService.Log(_targetEntity.name + " Target Hp: " + _hp);
            }

            PerformanceTimer.Start("AutoSmite");

            NativeImport.BlockInput(true);

            Mouse.MouseMove(_screenPosition.X, _screenPosition.Y - 55);

            if (_distance < 690)
            {
              Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_2);

              LogService.Log(" Auto Smite " + _hp);

              _smited = true;
              _smitedTarget = _targetEntity;
            }

            _targetEntity = null;

            NativeImport.BlockInput(false);

            PerformanceTimer.Stop("AutoSmite", true);

            Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_Y);

            _focus = false;
            Thread.Sleep(500);
          }
        }
        _focus = false;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }

      PerformanceTimer.Stop("Update");
    }

    public static void Reset()
    {
      _focus = false;

      _targetEntity = null;
      
      _smitedTarget = null;
      _smited = false;
    }

    public static void LateUpdate()
    {
      PerformanceTimer.Start("LateUpdate");

      try
      {
        if (_focus)
        {
          return;
        }

        var targetEntity = _targetEntity;

        if (targetEntity != null)
        {
          GetDamage();

          _distance = Vector3.Distance(LocalPlayer.Vo.GetPosition(), targetEntity.GetPosition());

          // LogService.Log("target hp: " + _targetEntity.Hp + " distance = " + _distance);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }

      PerformanceTimer.Stop("LateUpdate");
    }
    private static void GetDamage()
    {
      LocalPlayer.ReadSpells();
      var containsKey = LocalPlayer.SpellMap.ContainsKey("SummonerSmite");

      // S5_SummonerSmitePlayerGanker
      if (LocalPlayer.SpellMap.ContainsKey("SummonerSmite"))
      {
        _damage = 600;
      }
      else if (LocalPlayer.SpellMap.ContainsKey("S5_SummonerSmitePlayerGanker"))
      {
        _damage = 900;
      }
      else
      {
        _damage = 1200;
      }
    }
    private static void FindTarget()
    {
      if (!LocalPlayer.SettingsVo.SmiteAll)
      {
        _targetEntities = Engine.Monsters.Where(e => targets.Contains(e.name)).ToArray();


        var visibleTargets = _targetEntities.Where(vo => vo.GetIsVisible()).ToArray();

        List<EntityVo> activeList = new List<EntityVo>();

        var mePosition = LocalPlayer.GetPosition();
        foreach (var target in visibleTargets)
        {
          target.GetPosition();
          var distance = Vector3.Distance(mePosition, target.Position);
          if (distance < 1000)
          {
            target.GetHp();
            if (target.Hp > 0)
              activeList.Add(target);
          }
        }

        if (activeList.Count > 0)
        {
          _targetEntity = activeList.OrderBy(o => o.Hp).ToList()[0];
        }
        else
        {
          _targetEntity = null;
        }
      }

      else
      {
        var mPos = LocalPlayer.GetPosition();
        _targetEntities = Engine.Monsters.Where(vo => !vo.name.Contains("Mini") && !vo.name.Contains("Respawn") && !vo.name.Contains("Test") && !vo.name.Contains("Jungle") && !vo.name.Contains("Plant") && !vo.name.Contains("Ward")).ToArray();

        if (_targetEntities == null || _targetEntities.Length <1)
          return;
        
        var entityVo = _targetEntities.OrderBy(e =>
        {
          var position = e.GetPosition();
          e.distance = Vector3.Distance(mPos, position);

          if (e.distance < 1000)
          {
            if (e.GetHp() < 1)
              return int.MaxValue;
          }
          else
          {
            return int.MaxValue;
          }
          return e.distance;
        }).First();

        if (entityVo.distance > 1000)
        {
          _targetEntity = null;
        }
        else
        {
          _targetEntity = entityVo;
          LogService.Log("TargetName: " + _targetEntity.name);
        }
      }
    }

    public static void Draw()
    {
      // if (_targetEntity != null)
      //   _targetEntity.DrawCustomRange(Color.Aqua, 2.5f, 5);
    }
  }

}
