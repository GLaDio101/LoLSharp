using System;
using System.Windows.Forms;
using LoLSharp.Core;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.GameScripts;
using LoLSharp.Modules;

namespace LoLSharp
{
  class Main
  {
    public static void OnMain()
    {
      try
      {
        if (LocalPlayer.Vo == null)
          return;

        var customScriptBases = CustomScriptController.GetActiveScripts();

        foreach (var scriptBase in customScriptBases)
        {
          scriptBase.Update();
        }

        if (LocalPlayer.Vo.champName == "Karthus")
        {
          Karthus.Update2();
        }

        if (LocalPlayer.Vo.champName == "Rengar")
        {
          AutoSmite2.Rengar();
        }


        if (Utils.IsKeyPressed(Keys.Space) && Utils.IsGameOnDisplay())
        {
          // Engine.LoadChampionsManager();
          //
          // var enemies = Engine.TeamDistinct.GetEnemies();
          // if (enemies.Length > 0)
          // {
          //   var enemy = enemies[0];
          //   var isVisible = enemy.GetIsVisible();
          //   var hp = enemy.GetHp();
          //   LogService.Log(" name: " + enemy.name  + " hp: " + hp + " isVisible: " + isVisible);
          // }


          if (LocalPlayer.Vo.champName == "Karthus")
          {
            Karthus.Update();
          }

          if (LocalPlayer.SettingsVo.Orbwalker)
          {
            // Orbwalker2.Update();
          }

          // Orbwalker.Orbwalk();

          /*Point EnemyPosition = ObjectManager.GetEnemyPosition();
  
          if (EnemyPosition != Point.Empty)
          {
              SpellBook.CastSpell(SpellBook.SpellSlot.Q, EnemyPosition);
  
              SpellBook.SpellSlot[] SpellArray = new SpellBook.SpellSlot[]
              {
                  SpellBook.SpellSlot.Q,
                  SpellBook.SpellSlot.W,
                  SpellBook.SpellSlot.E
              };
  
              SpellBook.CastMultiSpells(SpellArray, EnemyPosition);
          }
          else
          {
              Engine.IssueOrder(Enums.GameObjectOrder.MoveTo, Cursor.Position);
          }*/
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
    public static void OnLate()
    {
      if (Utils.IsKeyPressed(Keys.Delete) && Utils.IsGameOnDisplay())
      {
        LocalPlayer.SettingsVo.SmiteAll = !LocalPlayer.SettingsVo.SmiteAll;
        LogService.Log("SmiteAll: " + LocalPlayer.SettingsVo.SmiteAll);

        AutoSmite2.State = AutoSmite2State.FindTarget;
      }

      if (Utils.IsKeyPressed(Keys.End) && Utils.IsGameOnDisplay())
      {
        LocalPlayer.SettingsVo.Orbwalker = !LocalPlayer.SettingsVo.Orbwalker;
        LogService.Log("Orbwalker: " + LocalPlayer.SettingsVo.Orbwalker);
      }


      Engine.LoadMinionManager();

      //
      // ScriptManager.ExecuteLateUpdates();
      // AutoSmite.LateUpdate();
    }
  }
}
