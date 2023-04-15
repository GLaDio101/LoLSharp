using System;
using System.Collections.Generic;
using System.Linq;
using Differ.Math;
using Differ.Shapes;
using LoLSharp.Game.Objects;
using LoLSharp.Game.Spells;
using LoLSharp.Modules;
using LoLSharp.Overlay.Drawing;
using SharpDX;
using Encoding = System.Text.Encoding;
using Point = System.Drawing.Point;

namespace LoLSharp.Game
{
  public class EntityVo
  {
    private long _address;
    private int _team;
    public string champName;
    public string name;
    private Vector3 _position;
    private float _hp;

    public EntityVo(long address)
    {
      _address = address;
      UpdateVariables();
    }
    public void UpdateVariables()
    {
      champName = GetChampName();
      name = GetName();
      GetTeam();
      GetIsVisible();
      GetPosition();
    }
    public Vector3 Position
    {
      get => _position;
      set => _position = value;
    }
    public float Hp
    {
      get => _hp;
      set => _hp = value;
    }
    public int Team
    {
      get => _team;
      set => _team = value;
    }
    public bool IsVisible
    {
      get => _isVisible;
      set => _isVisible = value;
    }

    public Dictionary<string, Spell> SpellMap = new Dictionary<string, Spell>();
    private bool _isVisible;
    public float distance;
    private float MaxHP;

    public string GetChampName()
    {
      var readString = Memory.ReadString(_address + OffsetManager.Object.ChampionName);
      
      return readString;
    }

    public string GetName()
    {
      var readString = Memory.ReadStringName(_address);
      return readString;
    }


    public AiManager GetAiManager()
    {
      var v1 = Memory.Read<byte>(_address + OffsetManager.Ai.AiManager);
      var v2 = _address + OffsetManager.Ai.AiManager - 8;
      var v3 = Memory.Read<int>(v2 + 4);
      var v4 = Memory.Read<int>(v2 + (4 * v1 + 12));
      v4 ^= ~v3;
      var aiManagerAddress = Memory.Read<int>(v4 + 0x8);
      return new AiManager(aiManagerAddress);
    }

    public float Distance()
    {
      var entityVo = LocalPlayer.Vo;
      distance = Distance2(entityVo.GetPosition(), GetPosition());
      return distance;
    }
    float Distance2(Vector3 a, Vector3 b)
    {
      float num1 = a.X - b.X;
      float num2 = a.Y - b.Y;
      float num3 = a.Z - b.Z;
      return (float)Math.Sqrt((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3);
    }
    public void ReadSpells()
    {
      SpellMap.Clear();
      for (var i = 0; i < 6; i++)
      {
        var spell = new Spell(_address + (i * 4));
        SpellMap[spell.SpeelName] = spell;
      }
    }

    public Vector3 GetPosition()
    {
      var vector3 = Memory.Read<Vector3>(_address + OffsetManager.Object.Pos);
      Position =vector3;
      return Position;
    }

    public bool GetIsVisible()
    {
     return Memory.Pubmemory.ReadBoolean(new IntPtr(_address + OffsetManager.Object.Visibility));
      // IsVisible = Memory.rea<bool>(_address + OffsetManager.Object.Visibility);
      // return IsVisible;
    }

    public float GetHp()
    {
      Hp = Memory.Read<float>(_address + OffsetManager.Object.HP);
      return Hp;
    }
    
    public float GetMaxHP()
    {
      MaxHP = Memory.Read<float>(_address + OffsetManager.Object.HP);
      return MaxHP;
    }
    
    public float GetGrayHp()
    {
      GrayHp = Memory.Read<float>(_address + OffsetManager.Object.GreyHealth);
      return GrayHp;
    }
    public float GrayHp { get; set; } 


    public Vector3 GetDir()
    {
      float posX = Memory.Read<float>(_address + OffsetManager.Object.Direction);
      float posY = Memory.Read<float>(_address + OffsetManager.Object.Direction + 0x4);
      float posZ = Memory.Read<float>(_address + OffsetManager.Object.Direction + 0x8);

      var position = new Vector3() { X = posX, Y = posY, Z = posZ };
      return position;
    }

    public int GetTeam()
    {
      Team = Memory.Read<int>(_address + OffsetManager.Object.Team);
      return Team;
    }

    public Point GetScreenPosition()
    {
      var position = GetPosition();
      var worldToScreen = Renderer.WorldToScreen(position);
      return new Point((int)worldToScreen.X, (int)worldToScreen.Y);
    }

    public Vector2 GetScreenPositionVector2()
    {
      var worldToScreen = Renderer.WorldToScreen(GetPosition());
      return new Vector2((int)worldToScreen.X, (int)worldToScreen.Y);
    }

    public float GetAttackRange()
    {
      return Memory.Read<float>(_address + OffsetManager.Object.AttackRange);
    }

    public float GetMoveSpeed()
    {
      return Memory.Read<float>(_address + OffsetManager.Object.MoveSpeed);
    }

    public int GetBoundingRadius()
    {
      return int.Parse(LocalPlayer.UnitRadiusData[champName]["Gameplay radius"].ToString());
    }

    public void DrawAttackRange(Color Colour, float Thickness)
    {
      DrawFactory.DrawCircleRange(GetPosition(), GetBoundingRadius() + GetAttackRange(), Colour, Thickness);
    }

    public void DrawCustomRange(Color Colour, float Thickness, float range)
    {
      DrawFactory.DrawCircleRange(GetPosition(), range, Colour, Thickness);
    }

    public Circle HitBox()
    {
      var screenPositionVector2 = GetScreenPositionVector2();
      return new Circle(screenPositionVector2.X, screenPositionVector2.Y, 60);
    }

    public void DrawName()
    {
      DrawFactory.DrawFont(champName, 24, GetScreenPositionVector2(), Color.Red);
    }
  }
}
