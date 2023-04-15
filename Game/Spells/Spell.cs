using System;
using LoLSharp.Modules;
using Encoding = System.Text.Encoding;

namespace LoLSharp.Game.Spells
{
  public class Spell
  {
    private long _address;
    private long spellBook;
    private long info;
    private long data;
    private string _speelName;
    private float _readyAt;

    public Spell(long address)
    {
      spellBook = address;
      
    
      // GetSpellBook();
      GetInfo();
      GetData();
      GetName();
    }
    public string SpeelName
    {
      get => _speelName;
      set => _speelName = value;
    }

    public void GetName()
    {
      var address = Memory.ReadByPtr(data + OffsetManager.Instances.SpellInfoDataName);
      SpeelName = Memory.ReadString(address);
    }

    public void GetData()
    {
      data = Memory.ReadByPtr(info + OffsetManager.Instances.SpellInfoSpellData);
    }

    public void GetInfo()
    {
      info = Memory.ReadByPtr(spellBook + OffsetManager.Instances.SpellInfo);
    }

    // public void GetSpellBook()
    // {
    //   spellBook = Memory.ReadByPtr(_address + OffsetManager.Instances.SpellBook+ OffsetManager.Instances.Instance);
    // }

    public float ReadyIn()
    {
      var cd = Time() - Engine.GetTime();
      return cd;
    }
    
    public bool IsReady()
    {
      var cd = ReadyIn();
      return !(cd > 0);
    }

    public float ReadyAt()
    {
      return _readyAt = Memory.Read<float>(spellBook + OffsetManager.SpellSlot.TimeCharge);
    }
    
    public float Time()
    {
      return  Memory.Read<float>(spellBook + OffsetManager.SpellSlot.Time);
    }

  }
}
