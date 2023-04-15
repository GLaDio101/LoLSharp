using System;
using System.Threading;
using System.Threading.Tasks;
using LoLSharp.Modules;
using Timer = LoLSharp.Modules.Timer;

namespace LoLSharp.Events
{
  class Base
  {
    private static Base Instance { get; set; } = new Base();
    private static Timer Ticker;
    private static Timer LateTicker;

    private Base()
    {
      var thread = new Thread(StartLoop);
      thread.Priority = ThreadPriority.Highest;
      thread.Start();
      // Ticker = new Timer(0.0001f);
      // Ticker.Elapsed += OnTickElapsed;
      // Ticker.Start();

      LateTicker = new Timer(1000);
      LateTicker.Elapsed += OnLateTickElapsed;
      LateTicker.Start();
      Instance = this;
    }
    private async void StartLoop()
    {
      // Set previous game time
      // DateTime previousGameTime = DateTime.Now;

      while (true)
      {
        // // Calculate the time elapsed since the last game loop cycle
        // TimeSpan gameTime = DateTime.Now - previousGameTime;
        // // Update the current previous game time
        // previousGameTime += gameTime;
        // Update the game
        OnTick?.Invoke();
        // Update Game at 60fps
        await Task.Delay(1);
      }
    }

    public static ToggleResult Toggle(TickElapsed Value)
    {
      if (OnTick != null)
      {
        foreach (TickElapsed s in OnTick.GetInvocationList())
        {
          if (s == Value)
          {
            OnTick -= Value;
            return ToggleResult.Disabled;
          }
        }
      }
      OnTick += Value;
      return ToggleResult.Enabled;
    }

    internal static event TickElapsed OnTick;
    internal static event TickElapsed OnLateTick;

    private static void OnTickElapsed(object Sender, TimerElapsedEventArgs e)
    {
      OnTick?.Invoke();
    }

    private static void OnLateTickElapsed(object Sender, TimerElapsedEventArgs e)
    {
      OnLateTick?.Invoke();
    }
  }

  public enum ToggleResult
  {
    Disabled,
    Enabled,
  }

  public delegate void TickElapsed();
}
