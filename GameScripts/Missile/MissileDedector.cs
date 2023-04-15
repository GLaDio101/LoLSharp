using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Differ.Data;
using LoLSharp.Core;
using LoLSharp.Devices;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;
using LoLSharp.Overlay.Drawing;
using SharpDX;
using Collision = Differ.Collision;

namespace LoLSharp.GameScripts.Missile
{
  public class MissileDetection
  {
    public static Dictionary<int, MissileVo> MissileVos = new Dictionary<int, MissileVo>();
    public static List<Vector2> moveTargets = new List<Vector2>();
    public static MissileVo LastMissile { get; set; }

    public static bool CheckCollision(EntityVo entity, MissileVo missile, out ShapeCollision collision)
    {
      var collideInfo = Collision.shapeWithShape(entity.HitBox(), missile.HitBox());
      collision = collideInfo;
      return collideInfo != null;
    }

    public static void Draw()
    {
      if (LastMissile != null)
      {
        LastMissile.Draw();
        LastMissile = null;
      }
      // foreach (var target in moveTargets)
      // {
      //   DrawFactory.DrawCircle((int)target.X, (int)target.Y, 50, 6, Color.Aqua);
      // }
    }

    public static void Update()
    {
      var missiles = ReadMissiles();
      MissileVos.Clear();
      // moveTargets.Clear();
      foreach (MissileVo missile in missiles)
      {
        if (!string.IsNullOrEmpty(missile.SpellName) && !missile.SpellName.Contains("Minion") && !missile.SpellName.Contains("BasicAttack"))
        {
          var team = missile.GetTeam();
          if (team == LocalPlayer.Vo.Team)
            continue;

          if (MissileVos.ContainsKey(missile.Address))
            continue;

          LastMissile = missile;
          MissileVos.Add(missile.Address, missile);

          if (CheckCollision(LocalPlayer.Vo, missile, out var response))
          {
            var screenPos = LocalPlayer.Vo.GetScreenPositionVector2();
            LogService.Log(response.separationX + " - " + response.separationY);
            var vec = new Vector2(screenPos.X - response.separationX * 4, screenPos.Y - response.separationY * 4);

            // vec += missile.ScreenEndPos();
            NativeImport.BlockInput(true);


            var position = Cursor.Position;
            Mouse.MouseMove((int)vec.X, (int)vec.Y);

            Mouse.MouseClickRight();

            // Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_U);
            Thread.Sleep(50);

            Mouse.MouseMove(position.X, position.Y);
            Thread.Sleep(150);

            NativeImport.BlockInput(false);

            // moveTargets.Add(vec);
            LogService.Log("Collision Detected> " + missile.SpellName);
            Thread.Sleep(300);
            break;
          }
        }
      }
    }

    public static MissileVo[] ReadMissiles()
    {
      var missileManager = Memory.Read<int>(OffsetManager.Missile.MissileManager);
      var rootNode = Memory.Read<int>(missileManager + 0x4);
      var missilesSize = Memory.Read<int>(missileManager + 0x8);
      if (missilesSize == 0)
        return new MissileVo[] { };

      var addresses = ReadMap(rootNode, missilesSize + 1);

      var missileVos = addresses.Select(i => new MissileVo(i));
      return missileVos.ToArray();
    }

    public static int[] ReadMap(int rootNode, int size)
    {
      List<int> checkedd = new List<int>();
      List<int> toCheck = new List<int>();

      toCheck.Add(rootNode);

      while (toCheck.Count > 0)
      {
        var target = toCheck.ToArray()[0];
        checkedd.Add(target);
        toCheck.Remove(target);
        var nextObject1 = Memory.Read<int>(target + 0x0);
        if (!checkedd.Contains(nextObject1)) toCheck.Add(nextObject1);
        if (size > 0 && checkedd.Count >= size) break;
        var nextObject2 = Memory.Read<int>(target + 0x4);
        if (!checkedd.Contains(nextObject2)) toCheck.Add(nextObject2);
        if (size > 0 && checkedd.Count >= size) break;
        var nextObject3 = Memory.Read<int>(target + 0x8);
        if (!checkedd.Contains(nextObject3)) toCheck.Add(nextObject3);
        if (size > 0 && checkedd.Count >= size) break;
      }

      checkedd.RemoveAt(0);
      return checkedd.ToArray();
    }

  }
}
