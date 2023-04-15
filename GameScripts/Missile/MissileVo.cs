using System;
using System.Collections.Generic;
using System.Linq;
using Differ.Math;
using Differ.Shapes;
using LoLSharp.Game;
using LoLSharp.Modules;
using LoLSharp.Overlay.Drawing;
using SharpDX;
using SharpDX.Text;

namespace LoLSharp.GameScripts.Missile
{
  public class MissileVo
  {
    public int Address;
    private MissileDataVo _missileDataVo;
    public string SpellName;

    public MissileVo(int address)
    {
      Address = address;
      SpellName = GetSpellName();

      _missileDataVo = GetMissileInfo();
    }

    public int GetEntry()
    {
      return Memory.Read<int>(Address + OffsetManager.Missile.ObjectEntry);
    }

    public int GetTeam()
    {
      return Memory.Read<int>(GetEntry() + OffsetManager.Object.Team);
    }

    public Vector3 GetStartPos()
    {
      return GetPosition(OffsetManager.Missile.StartPos);
    }

    public Vector3 GetPosition(long address)
    {
      float posX = Memory.Read<float>(GetEntry() + address);
      float posY = Memory.Read<float>(GetEntry() + address + 0x4);
      float posZ = Memory.Read<float>(GetEntry() + address + 0x8);

      var position = new Vector3() { X = posX, Y = posY, Z = posZ };
      return position;
    }

    public Vector3 GetEndPos()
    {
      return GetPosition(OffsetManager.Missile.EndPos);
    }

    public Vector2 ScreenStartPos()
    {
      return GetScreenPositionVector2(GetStartPos());
    }

    public Vector2 ScreenEndPos()
    {
      return GetScreenPositionVector2(GetEndPos());
    }

    public Vector2 GetScreenPositionVector2(Vector3 vec)
    {
      var worldToScreen = Renderer.WorldToScreen(vec);
      return new Vector2((int)worldToScreen.X, (int)worldToScreen.Y);
    }


    public MissileDataVo GetMissileInfo()
    {
      return MissileService.Map.ContainsKey(SpellName) ? MissileService.Map[SpellName] : new MissileDataVo(SpellName);
    }

    public string ChampionName()
    {
      return _missileDataVo.ChampionName;
    }

    public int CastRange()
    {
      return _missileDataVo.CastRange;
    }

    public float Width()
    {
      return _missileDataVo.Width;
    }
    public float Height()
    {
      return _missileDataVo.Height;
    }

    public string GetSpellName()
    {
      var sInfo = Memory.Read<int>(GetEntry() + OffsetManager.Missile.SpellInfo);
      var sData = Memory.Read<int>(sInfo + OffsetManager.Instances.SpellInfoSpellData);
      var spellNameAddress = Memory.Read<int>(sData + OffsetManager.Instances.SpellInfoDataName);
      var spellName = Memory.ReadString(spellNameAddress);
      return spellName;
    }

    // public Sat.Polygon SatHitBox()
    // {
    //   var screenStartPos = ScreenStartPos();
    //   var screenEndPos = ScreenEndPos();
    //   var length = CastRange() != 0 ? CastRange() : Hypot(screenEndPos.X - screenStartPos.X, screenEndPos.Y - screenStartPos.Y);
    //   var width = Width() != 0 ? Width() : 80;
    //   var angle = (float)Math.Atan2(screenEndPos.Y - screenStartPos.Y, screenEndPos.X - screenStartPos.X);
    //   var hitBox = new Sat.Box(new System.Numerics.Vector2(screenStartPos.X, screenStartPos.Y), (int)length, (int)width).ToPolygon();
    //   hitBox.SetAngle(angle);
    //   hitBox.SetOffset(new System.Numerics.Vector2(0, -width / 2));
    //   return hitBox;
    // }

    public Polygon HitBox()
    {
      var screenStartPos = ScreenStartPos();
      var screenEndPos = ScreenEndPos();
      var length = CastRange() != 0 ? CastRange() : Hypot(screenEndPos.X - screenStartPos.X, screenEndPos.Y - screenStartPos.Y);
      var width = Width() != 0 ? Width() : 80;
      var angle = (float)Math.Atan2(screenEndPos.Y - screenStartPos.Y, screenEndPos.X - screenStartPos.X);
      var rectangle = Polygon.rectangle(screenStartPos.X, screenStartPos.Y, length/2, width, System.Numerics.Vector2.Zero, false);
      rectangle.rotation = angle;
      return rectangle;
    }

    public void Draw()
    {
      var hitBox = HitBox();
      var vector2S = hitBox.transformedVertices.Select(vector => new Vector2(vector.x, vector.y)).ToArray();
      DrawFactory.DrawPolygon(vector2S, Color.Pink);
    }

    public float Hypot(float side1, float side2)
    {
      return (float)Math.Sqrt(Math.Pow(side1, 2) + Math.Pow(side2, 2));
    }

  }
}
