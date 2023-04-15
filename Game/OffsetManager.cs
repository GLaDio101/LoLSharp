using System;
using System.Diagnostics;
using LoLSharp.Modules;

namespace LoLSharp.Game
{
  internal class OffsetManager
  {
    public static long BaseAddress2 = Utils.GetLeagueProcess().MainModule.BaseAddress.ToInt64();
    public static IntPtr Handle = Utils.GetLeagueProcess().Handle;
    public static Process Process = Utils.GetLeagueProcess();

    public class Instances
    {
      public static long GameVersion = BaseAddress2 + 0x324FA0; //8B 44 24 04 BA ? ? ? ? 2B D0

      //Version 10.20.337.6669 [PUBLIC] // 8B 44 24 04 BA ? ? ? ? 2B D0 
      public static long LocalPlayer = BaseAddress2 + 0x518EB00; // 10.25.348.1797// string xref blueHero -> Above "hero" subrtn  // 51 8B 0D ? ? ? ? 85 C9 74 21 
      public static long LocalPlayer2 = BaseAddress2 + 0x518EB00; // 10.25.348.1797// string xref blueHero -> Above "hero" subrtn  // 51 8B 0D ? ? ? ? 85 C9 74 21 
      public static long Renderer = BaseAddress2 + 0x51d4028; // A1 ?? ?? ?? ?? 56 57 BF ?? ?? ?? ?? 8B 
      public static long ViewMatrix = BaseAddress2 + 0x51CBCA0; // 10.25.348.1797// B9 ? ? ? ? E8 ? ? ? ? B9 ? ? ? ? E9 ? ? ? ? // First result: unk_0x...

      public static int SpellBook = 0x3118;
      public static int Instance = 0x4C8;
      public static int SpellBookActiveSpellEntry = 0x90;
      public static int SpellBookSpellSlots = 0x750;
      public static int SpellInfoDataName = 0x80;
      public static int SpellSlotSpellInfo = 0x2E8;
      public static long SpellInfo = 0x130;
      public static long SpellInfoSpellData = 0x60;

      public static long GameTime = BaseAddress2 + 0x316B268; // F3 0F 11 05 ? ? ? ? 8B 49

      public static long ZoomHackInstance = BaseAddress2 + 0x517E720;

      public static int UnderMouseObject = 0x252230C; // no find// 8B 0D ? ? ? ? 89 0D

      public static long MinionManager = BaseAddress2 + 0x3930220; // 8B 0D ? ? ? ? E8 ? ? ? ? EB 09 56 E8 ? ? ? ? 83 C4 04 8B CB C7 44 24 ? ? ? ? ?  First

      public static long HeroList = BaseAddress2 + 0x20D5AA0; // 83 EC 64 A1 ? ? ? ? 33 C4 89 44 24 60 8B 44 24 68 83 CA FF 
    }

    public class Ai
    {
      public static int AiManager = 0x2E7C; //0F B6 83 ?? ?? ?? ?? 33 C9
      public static int AiManagerTargetPos = 0x2EC;
      public static int AiManagerStartPath = 0x1cc;
      public static int AiManagerEndPath = 0x1D8;
      public static int AiManagerIsMoving = 0x1C0;
      public static int AiManagerIsDashing = 0x214;
      public static int AiManagerCurrentSegment = 0x1c4;
      public static int AiManagerDashSpeed = 0x1f8;
    }


    public class SpellSlot
    {
      public static int Time = 0x24;
      public static int TimeCharge = 0x74;
    }
    public class Missile
    {
      public static long MissileManager = BaseAddress2 + 0x312D904; // 8D 4E 04 C7 05 ? ? ? ? ? ? ? ? 
      public static int DestCheck = 0x31C;
      public static int ObjectEntry = 0x14;
      public static int SpellInfo = 0x0260;
      public static int SrcIdx = 0x2C4; //0x2DC
      public static int DestIdx = 0x318; // 0x330;
      public static int StartPos = 0x2DC;
      public static int EndPos = 0x02E8;
    }


    public class Object
    {
      public static long ChampionName = 0x38A0;
      public static int Name = 0x38A0;
      // public static int Name2 = 0x2B04;
      public static int Pos = 0x220;
      public static int Damage = 0x189FF88;
      public static int GreyHealth = 0x1100;
      public static long Visibility = 0x310;
      public static int AttackRange = 0x169C;
      public static int HP = 0x1058;
      public static int MaxHP = 0x1070;
      public static int Team = 0x3c;
      public static int Direction = 0x1BF4;
      public static int IsMoving = 0x34E9;
      public static int MoveSpeed = 0x139C;
      public static long Level = 0x4030;
    }
  }
}
