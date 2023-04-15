using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using LoLSharp.Modules;

namespace LoLSharp.Core
{
  public class PerformanceTimer
  {
    private static Dictionary<string, DateTime> _startTimeMap = new Dictionary<string, DateTime>();

    private static DateTime _startTime;
    public static void Start(string key)
    {
      _startTime = DateTime.Now;

      _startTimeMap[key] = _startTime;
    }

    public static void Stop(string key, bool forceWrite = false)
    {
      if (!_startTimeMap.ContainsKey(key))
        return;
      var startTime = (DateTime.Now - _startTimeMap[key]);
      if (startTime.TotalMilliseconds > 300 || forceWrite)
      {
        LogService.Log("PerformanceTimer: " + key + " > " + startTime.TotalMilliseconds);
      }
    }

  }
}
