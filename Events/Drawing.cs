using System;
using System.Collections.Generic;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.GameScripts;
using LoLSharp.GameScripts.Missile;
using LoLSharp.Managers;
using LoLSharp.Modules;
using LoLSharp.Overlay.Drawing;
using SharpDX;

namespace LoLSharp.Events
{
  class Drawing
  {
    public static bool IsMenuBeingDrawn = false;

    public static void OnDeviceDraw()
    {
      // var x = 1920 / 2;
      // var y = 1080 / 2;
      // DrawFactory.DrawFilledBox(x,y,100,100,Color.Red);            

      // DrawFactory.DrawPoint(x, y, Color.Aqua);
      // DrawFactory.DrawCircle(x,y,200,6,Color.Aqua);

      if (Utils.IsGameOnDisplay())
      {
        try
        {

          if (LocalPlayer.SettingsVo.Orbwalker)
          {
            var entityVo = Orbwalker2._entity;
            if (entityVo != null)
            {
              entityVo.DrawCustomRange(Color.Pink, 2.5f, 100);
            }
            else
            {
              LogService.Log("entityVo null : " );

            }
            // var enemyPosition = Orbwalker.EnemyPosition;
            // LogService.Log("enemyPosition: " + enemyPosition);
            //
            // DrawFactory.DrawCircle(enemyPosition.X,enemyPosition.Y, 10.5f,5 ,Color.Pink);
          }
          
          // ScriptManager.ExecuteDraws();

          LocalPlayer.Vo.DrawAttackRange(Color.Aqua, 2.5f);

          // var entityVos = Engine.TeamDistinct.GetEnemies();

          // foreach (var entity in entityVos)
          // {
          //   entity.DrawAttackRange(Color.Red, 2.5f);
          //   entity.DrawName();
          // }

          var targetEntityVo = AutoSmite2.TargetEntityVo;
          if (targetEntityVo != null)
          {
            targetEntityVo.DrawCustomRange(Color.Red, 2.5f, 100);
          }
          
          foreach (var ward in Engine.Wards)
          {
            ward.DrawCustomRange(Color.Pink, 2.5f, 100);
          }
        }
        catch (Exception e)
        {
          LogService.Log(e.Message);
          throw;
        }


        //When ~ key is pressed...
        DrawMenu();
      }
    }

    private static void DrawMenu()
    {
      var menuBasePlate = Program.MenuBasePlate;
      if (Utils.IsKeyPressed(System.Windows.Forms.Keys.Oemtilde))
      {
        menuBasePlate.Show();
        IsMenuBeingDrawn = true;
      }
      else
      {
        menuBasePlate.Hide();
      }
    }
  }
}
