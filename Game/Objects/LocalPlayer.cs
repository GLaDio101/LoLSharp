using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoLSharp.Game.Spells;
using LoLSharp.Modules;
using Newtonsoft.Json.Linq;
using SharpDX;
namespace LoLSharp.Game.Objects
{
    public class SettingsVo
    {
        public bool SmiteAll;
        public bool Orbwalker { get; set; } = false;
    }
    class LocalPlayer
    {
        public static JObject UnitRadiusData;
        private static int _team;
        public static EntityVo Vo;
        public static SettingsVo SettingsVo = new SettingsVo();
        public static int Team
        {
            get => _team;
            set => _team = value;
        }
        public static string GetSummonerName()
        {
            return API.ActivePlayerData.GetSummonerName();
        }
        
        public static int GetTeam()
        {
            Team = Memory.Read<int>(OffsetManager.Instances.LocalPlayer + OffsetManager.Object.Team);
            return Team;
        }

        public static int GetLevel()
        {
            return API.ActivePlayerData.GetLevel();
        }

        public static float GetCurrentGold()
        {
            return API.ActivePlayerData.GetCurrentGold();
        }

        public static Dictionary<string, Spell> SpellMap = new Dictionary<string, Spell>();

        public static void ReadSpells()
        {
            SpellMap.Clear();   
            var address = Engine.GetLocalPlayer;
            
            byte[] dataBuffer = new byte[0x4100];
            NativeImport.ReadProcessMemory(Memory.HProcess.Handle, (IntPtr)address, dataBuffer, dataBuffer.Length, out IntPtr bytesRead);
         
            long[] spellSlotPointerBuffer = new long[7];
            Buffer.BlockCopy(dataBuffer, OffsetManager.Instances.SpellBook, spellSlotPointerBuffer, 0, sizeof(UInt64) * 6);

            foreach (var s in spellSlotPointerBuffer)
            {
                var spell = new Spell(s);
                SpellMap[spell.SpeelName] = spell;
            }
            
            // var d = spellSlotPointerBuffer[4];
            // var f = spellSlotPointerBuffer[5];
            // var info = Memory.ReadByPtr(f + OffsetManager.Instances.SpellInfo);
            // var data = Memory.ReadByPtr(info + OffsetManager.Instances.SpellInfoSpellData);
            // var address2 = Memory.ReadByPtr(data + OffsetManager.Instances.SpellInfoDataName);
            // var SpeelName = Memory.ReadString(address2);
            // byte[] buffer = new byte[0x150];
            // //
            // NativeImport.ReadProcessMemory(Memory.HProcess.Handle, (IntPtr)f, buffer, buffer.Length, out IntPtr bytesRead2);
            
            // byte[] buff = new byte[50];
            // UInt64 spellInfoPtr = 0;
            // long spellInfoPtr = BitConverter.ToInt64(buffer, OffsetManager.Instances.SpellSlotSpellInfo);
            // var bytes = BitConverter.GetBytes(spellInfoPtr);
            // Buffer.BlockCopy(buffer, OffsetManager.Instances.SpellSlotSpellInfo, dst: bytes, 0, sizeof(UInt64));
            // UInt64 spellInfoPtr = BitConverter.ToUInt64(buffer,OffsetManager.Instances.SpellSlotSpellInfo);
            // var spellDataPtr = Memory.ReadByPtr(spellInfoPtr + OffsetManager.Instances.SpellInfoSpellData);    
            // var spellNamePtr = Memory.ReadByPtr(spellDataPtr + OffsetManager.Instances.SpellInfoDataName);    
            // NativeImport.ReadProcessMemory(Memory.HProcess.Handle, (IntPtr)spellNamePtr, buff, buff.Length, out IntPtr bytesRead3);
            // var name = Encoding.ASCII.GetString(buff);

            
            // for (var i = 0; i < 6; i++)
            // {
            //     var spell = new Spell(address + (i * 4));
            //     SpellMap[spell.SpeelName] = spell;
            // }
        }

        public static void DrawAttackRange(Color Colour, float Thickness)
        {
            if (IsVisible() && GetCurrentHealth() > 1.0f)
            {
                Overlay.Drawing.DrawFactory.DrawCircleRange(GetPosition(), GetBoundingRadius() + GetAttackRange(), Colour, Thickness);
            }
        }

        public static void DrawSpellRange(Spells.SpellBook.SpellSlot Slot, Color Colour, float Thickness)
        {
            if (IsVisible() && GetCurrentHealth() > 1.0f)
            {
                Overlay.Drawing.DrawFactory.DrawCircleRange(GetPosition(), SpellBook.GetSpellRadius(Slot), Colour, Thickness);
            }
        }

        private static List<string> RangeSlotList = new List<string>() { "Q", "W", "E", "R" };
        private static List<float> UsedRangeSlotsList = new List<float>();
        public static void DrawAllSpellRange(Color RGB)
        {
            foreach (string RangeSlot in RangeSlotList)
            {
                float SpellRange = SpellBook.SpellDB[RangeSlot].ToObject<JObject>()["Range"][0].ToObject<float>();

                if (UsedRangeSlotsList.Count != 0)
                {
                    if (!UsedRangeSlotsList.Contains(SpellRange))
                    {
                        UsedRangeSlotsList.Add(SpellRange);
                    }
                }
                else
                {
                    UsedRangeSlotsList.Add(SpellRange);
                }
            }

            foreach (float Range in UsedRangeSlotsList)
            {
                Overlay.Drawing.DrawFactory.DrawCircleRange(GetPosition(), Range, RGB, 2.5f);
            }
        }

        public static bool IsVisible()
        {
            return Memory.Read<bool>(Engine.GetLocalPlayer + OffsetManager.Object.Visibility);
        }

        public static Vector3 GetPosition()
        {
            float posX = Memory.Read<float>(Engine.GetLocalPlayer + OffsetManager.Object.Pos);
            float posY = Memory.Read<float>(Engine.GetLocalPlayer + OffsetManager.Object.Pos + 0x4);
            
            float posZ = Memory.Read<float>(Engine.GetLocalPlayer + OffsetManager.Object.Pos + 0x8);

            return new Vector3() { X = posX, Y = posY, Z = posZ };
        }

        public static string GetChampionName()
        {
            return API.AllPlayerData.AllPlayers.Where(x => x.SummonerName == GetSummonerName()).First().ChampionName;
        }

        public static string GetName()
        {
            var read = Memory.Read<int>(Engine.GetLocalPlayer + OffsetManager.Object.ChampionName);
            var readString = Memory.ReadString(read);
            return readString;
        }

        public static float GetAttackRange()
        {
            return Memory.Read<float>(Engine.GetLocalPlayer + OffsetManager.Object.AttackRange);
        }

        public static int GetBoundingRadius()
        {
            return int.Parse(UnitRadiusData[GetChampionName()]["Gameplay radius"].ToString());
        }

        public static float GetCurrentHealth()
        {
            return API.ActivePlayerData.ChampionStats.GetCurrentHealth();
        }

        public static float GetMaxHealth()
        {
            return API.ActivePlayerData.ChampionStats.GetMaxHealth();
        }

        public static float GetHealthRegenRate()
        {
            return API.ActivePlayerData.ChampionStats.GetHealthRegenRate();
        }

        public string GetResourceType()
        {
            return API.ActivePlayerData.ChampionStats.GetResourceType();
        }

        public static float GetCurrentMana()
        {
            return API.ActivePlayerData.ChampionStats.GetResourceValue();
        }

        public static float GetCurrentManaMax()
        {
            return API.ActivePlayerData.ChampionStats.GetResourceMax();
        }

        public static float GetAbilityPower()
        {
            return API.ActivePlayerData.ChampionStats.GetAbilityPower();
        }

        public static float GetArmor()
        {
            return API.ActivePlayerData.ChampionStats.GetArmor();
        }

        public static float GetArmorPenetrationFlat()
        {
            return API.ActivePlayerData.ChampionStats.GetArmorPenetrationFlat();
        }

        public static float GetArmorPenetrationPercent()
        {
            return API.ActivePlayerData.ChampionStats.GetArmorPenetrationPercent();
        }

        public static float GetAttackSpeed()
        {
            return API.ActivePlayerData.ChampionStats.GetAttackSpeed();
        }

        public static float GetBonusArmorPenetrationPercent()
        {
            return API.ActivePlayerData.ChampionStats.GetBonusArmorPenetrationPercent();
        }

        public static float GetBonusMagicPenetrationPercent()
        {
            return API.ActivePlayerData.ChampionStats.GetBonusMagicPenetrationPercent();
        }

        public static float GetCooldownReduction()
        {
            return API.ActivePlayerData.ChampionStats.GetCooldownReduction();
        }

        public static float GetCritChance()
        {
            return API.ActivePlayerData.ChampionStats.GetCritChance();
        }

        public static float GetCritDamage()
        {
            return API.ActivePlayerData.ChampionStats.GetCritDamage();
        }

        public static float GetLifeSteal()
        {
            return API.ActivePlayerData.ChampionStats.GetLifeSteal();
        }

        public static float GetMagicLethality()
        {
            return API.ActivePlayerData.ChampionStats.GetMagicLethality();
        }

        public static float GetMagicPenetrationFlat()
        {
            return API.ActivePlayerData.ChampionStats.GetMagicPenetrationFlat();
        }

        public static float GetMagicPenetrationPercent()
        {
            return API.ActivePlayerData.ChampionStats.GetMagicPenetrationPercent();
        }

        public static float GetMagicResist()
        {
            return API.ActivePlayerData.ChampionStats.GetMagicResist();
        }

        public static float GetMoveSpeed()
        {
            return API.ActivePlayerData.ChampionStats.GetMoveSpeed();
        }

        public static float GetPhysicalLethality()
        {
            return API.ActivePlayerData.ChampionStats.GetPhysicalLethality();
        }

        public static float GetSpellVamp()
        {
            return API.ActivePlayerData.ChampionStats.GetSpellVamp();
        }

        public static float GetTenacity()
        {
            return API.ActivePlayerData.ChampionStats.GetTenacity();
        }
    }
}
