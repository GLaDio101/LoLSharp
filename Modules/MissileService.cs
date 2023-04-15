using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LoLSharp.Modules
{
  public class MissileDataVo
  {
    public string Name;
    public string ChampionName;
    public string ChampionNameLowerCase;
    public float Delay;
    public int CastRange;
    public float CastRadius;
    public float Width;
    public float Height;
    public float Speed;
    public float Flags;

    public MissileDataVo(string name)
    {
      Name = name;
      ChampionName = "Default";
    }

    public MissileDataVo()
    {
    }

  }

  public class MissileService
  {
    static string Missiles = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Missiles.json");
    public static MissileDataVo[] List;
    public static Dictionary<string, MissileDataVo> Map = new Dictionary<string, MissileDataVo>();

    public static void ParseMissileDBData()
    {
      try
      {
        List = JsonConvert.DeserializeObject<MissileDataVo[]>(Missiles);
        foreach (var vo in List)
        {
          Map[vo.Name] = vo;
        }
      }
      catch (Exception Ex)
      {
        LogService.Log(Ex.ToString(), Enums.LogLevel.Error);
        throw new Exception("MissileServiceParseExecption");
      }
    }
  }
}
