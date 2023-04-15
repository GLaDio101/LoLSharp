using System.Text;
using LoLSharp.Modules;
namespace LoLSharp.Game.Spells
{
    class SpellClass
    {
        public int CurrentSpell;

        public bool IsSpellReady()
        {
            return GetLevel() >= 1 && API.GameStats.GetGameTime() >= GetCooldownExpire();
        }

        public string GetSpellName()
        {
            return Memory.ReadString(GetSpellData() + 0x6C);
        }

        public int GetLevel()
        {
            return Memory.Read<int>(CurrentSpell + 0x6C);
        }

        public float GetCooldown()
        {
            return Memory.Read<float>(CurrentSpell + 0x78);
        }

        public float GetCooldownExpire()
        {
            return Memory.Read<float>(CurrentSpell + 0x28);
        }

        public float GetFinalCooldownExpire()
        {
            return Memory.Read<float>(CurrentSpell + 0x64);
        }

        public int GetCharges()
        {
            return Memory.Read<int>(CurrentSpell + 0x58);

            /*
             if is smite
                    {
                        if (spell->GetCharges() >= 1)
                            cooldownRemaining = spell->GetCDExpires() - LTimerModule::Get().GetGameTime();
                        else
                            cooldownRemaining = spell->GetFinalCDExpires() - LTimerModule::Get().GetGameTime();
                    }
             */
        }

        public int GetSpellInfo()
        {
            return Memory.Read<int>(CurrentSpell + 0x8);
        }

        public int GetSpellData()
        {
            return Memory.Read<int>(GetSpellInfo() + 0x44);
        }
    }
}
