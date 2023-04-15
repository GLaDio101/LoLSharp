namespace LoLSharp.API
{
    class GameStats
    {
        internal static string GetGameMode()
        {
            return Service.GetGameStatsData()["gameMode"].ToString();
        }

        internal static float GetGameTime()
        {
            return Service.GetGameStatsData()["gameTime"].ToObject<float>();
        }

        internal static string GetMapName()
        {
            return Service.GetGameStatsData()["mapName"].ToString();
        }

        internal static int GetMapNumber()
        {
            return Service.GetGameStatsData()["mapNumber"].ToObject<int>();
        }

        internal static string GetMapTerrain()
        {
            return Service.GetGameStatsData()["mapTerrain"].ToString();
        }
    }
}
