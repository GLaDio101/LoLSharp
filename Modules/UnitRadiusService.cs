using System;
using System.IO;
using Newtonsoft.Json.Linq;
namespace LoLSharp.Modules
{
    class UnitRadiusService
    {
        public static void ParseUnitRadiusData()
        {
            try
            {
                string UnitRadiusDataString = File.ReadAllText(Directory.GetCurrentDirectory() + @"\UnitRadius.json");
                Game.Objects.LocalPlayer.UnitRadiusData = JObject.Parse(UnitRadiusDataString);

                LogService.Log("Successfully Parsed Unit Radius Data.");
            }
            catch (Exception Ex)
            {
                LogService.Log(Ex.ToString(), Enums.LogLevel.Error);
                throw new Exception("UnitRadiusParseExecption");
            }
        }
    }
}
