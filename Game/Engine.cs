using System;
using System.Linq;
using System.Windows.Forms;
using LoLSharp.Devices;
using LoLSharp.Enums;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;
using SharpDX;
using Encoding = System.Text.Encoding;
using Point = System.Drawing.Point;

namespace LoLSharp.Game
{
  public class Engine
  {
    private static EntityVo[] _minions = new EntityVo[] { };
    private static EntityVo[] _monsters = new EntityVo[] { };
    private static TeamDistinct _teamDistinct;
    public static EntityVo[] Wards = new EntityVo[] { };
    public static long GetLocalPlayer { get; } = Memory.ReadByPtr(OffsetManager.Instances.LocalPlayer2);
    public static EntityVo[] Minions
    {
      get => _minions;
      set => _minions = value;
    }
    public static EntityVo[] Monsters
    {
      get => _monsters;
      set => _monsters = value;
    }
    public static TeamDistinct TeamDistinct
    {
      get => _teamDistinct;
      set => _teamDistinct = value;
    }

    public static float GetTime()
    {
      return Memory.Read<float>(OffsetManager.Instances.GameTime);
    }
    public static float GetGameTime()
    {
      return API.GameStats.GetGameTime();
    }

    public static string GameVersion()
    {
      var read = Memory.Read<int>(OffsetManager.Instances.GameVersion);
      var readString = Memory.ReadString(OffsetManager.Instances.GameVersion);
      return readString;
    }
    public const Int32 zoommin = 0x24;
    public const Int32 zoommax = 0x28;
    public static void Zoom()
    {
      var instance = Memory.ReadByPtr(OffsetManager.Instances.ZoomHackInstance);
      var zoomMin = Memory.Read<float>(instance + zoommin);
      var zoomMax = Memory.Read<float>(instance + zoommax);
      
      float setmaxzoom= 2650;
      Memory.WriteFloat((IntPtr)(instance + zoommax),setmaxzoom);
      LogService.Log("ZoomMin: " + zoomMin);
      LogService.Log("ZoomMax: " + zoomMax);

    }

    public static int GetObjectUnderMouse()
    {
      return Memory.Read<int>(OffsetManager.Instances.UnderMouseObject);
    }

    public static void LoadMinionManager()
    {
      var minionManager = Memory.ReadByPtr(OffsetManager.Instances.MinionManager);
      var addresses = Memory.ReadVTable(minionManager);
      var entitiesWithShits = addresses.Select(i => new EntityVo(i)).ToArray();
      var entities = entitiesWithShits.Where(e => !e.champName.Contains("PreSeason")).ToArray();
      Wards = entities.Where(e => e.champName.Equals("BlueTrinket") || e.champName.Equals("JammerDevice") || e.champName.Equals("YellowTrinket")).ToArray();

      Minions = entities.Where(e => !Wards.Contains(e) && e.champName.Contains("Minion")).ToArray();
      Monsters = entities.Where(e => !Wards.Contains(e) && !Minions.Contains(e)).ToArray();
      // var entityVos = entities.Where(vo => vo.GetHp() == 5730).ToArray()[0];
      // LogService.Log("name:: " + entityVos.GetName() + "name2:: " + entityVos.GetChampName());
    }

    public static void LoadChampionsManager()
    {
      var heroManager = Memory.ReadByPtr(OffsetManager.Instances.HeroList);
      var addresses = Memory.ReadVTable(heroManager);
      var champions = addresses.Select(i => new EntityVo(i)).ToArray();

      TeamDistinct = new TeamDistinct(champions);
    }

    public static void IssueOrder(GameObjectOrder Order, Point Vector2D = new Point())
    {
      if (Utils.IsGameOnDisplay())
      {
        switch ( Order )
        {
          case GameObjectOrder.HoldPosition:
            Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_S);
            break;
          case GameObjectOrder.MoveTo:
            if (Vector2D.X == 0 && Vector2D.Y == 0)
            {
              Mouse.MouseClickRight();
              break;
            }
            if (Vector2D == new Point(Cursor.Position.X, Cursor.Position.Y))
            {
              Mouse.MouseClickRight();
              break;
            }
            Mouse.MouseMove(Vector2D.X, Vector2D.Y);
            Mouse.MouseClickRight();
            break;
          case GameObjectOrder.AttackUnit:
            if (Vector2D.X == 0 && Vector2D.Y == 0)
            {
              Mouse.MouseMove(Cursor.Position.X, Cursor.Position.Y);
              Mouse.MouseClickRight();
              break;
            }
            Mouse.MouseMove(Vector2D.X, Vector2D.Y);
            Mouse.MouseClickRight();
            break;
          case GameObjectOrder.AutoAttack:
            Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_OPENING_BRACKETS);
            break;
          case GameObjectOrder.Stop:
            Keyboard.SendKey((short)Keyboard.KeyBoardScanCodes.KEY_S);
            break;
        }
      }
    }

    public static Vector3 PredictPosition(EntityVo target, Vector3 mePos, float castTime)
    {
      var aiManager = target.GetAiManager();
      var startPath = aiManager.GetStartPath();
      var endPath = aiManager.GetEndPath();
      var dQ = new Vector3(endPath.X - mePos.X, endPath.Y - mePos.Y, endPath.Z - mePos.Z);

      if ((int)startPath.X == (int)endPath.X && (int)startPath.Y == (int)endPath.Y)
      {
        return dQ;
      }



      dQ.Normalize();

      var moveSpeed = target.GetMoveSpeed();
      var dQTravel = moveSpeed * castTime;
      var mul = new Vector3(dQ.X * dQTravel, dQ.Y * dQTravel, dQ.Z * dQTravel);
      return new Vector3(startPath.X + mul.X, startPath.Y + mul.Y, startPath.Z + mul.Z);
    }
    // export function lowestHealthEnemyChampInRange(range: number) {
    //   return enemyChampsInRange(range).sort((a, b) => a.hp - b.hp)[0];
    // }

    public static EntityVo LowestHealthEnemyChampInRange(int range)
    {
      var me = LocalPlayer.Vo;
      var mePos = me.GetPosition();
      var list = TeamDistinct.GetEnemies().Where(vo => vo.GetHp() > 0);

      var entityVos = list.Where(e => e.GetIsVisible() && Distance(e.GetPosition(), mePos) < range).ToList();
      if (entityVos.Count < 1)
      {
        return null;
      }

      entityVos.Sort((a, b) => (int)a.Hp - (int)b.Hp);
      var entityVo = entityVos[0];
      return entityVo;
    }
    
    public static EntityVo NearEnemyChampInRange(int range)
    {
      var me = LocalPlayer.Vo;
      var mePos = me.GetPosition();
      var list = TeamDistinct.GetEnemies().Where(vo => vo.GetHp() > 0);

      var entityVos = list.Where(e => e.GetIsVisible() && Distance(e.GetPosition(), mePos) < range).ToList();
      if (entityVos.Count < 1)
      {
        return null;
      }
      var min = entityVos.OrderBy(vo => Distance(mePos,vo.Position)).FirstOrDefault();
      return min;
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
      float num1 = a.X - b.X;
      float num2 = a.Y - b.Y;
      float num3 = a.Z - b.Z;
      return (float)Math.Sqrt((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3);
    }

  }
}
