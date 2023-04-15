using System;
using System.Management;
using LoLSharp.Enums;
using LoLSharp.Modules;
namespace LoLSharp.Events
{
    class EventsManager
    {
        private static ManagementEventWatcher ProcessStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
        private static ManagementEventWatcher ProcessStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");

        public static void SubscribeToEvents()
        {
            try
            {
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnProcessException);

                ProcessStartEvent.EventArrived += new EventArrivedEventHandler(Game.OnGameLoad);
                ProcessStartEvent.Start();
                ProcessStopEvent.EventArrived += new EventArrivedEventHandler(Game.OnGameExit);
                ProcessStopEvent.Start();

                Base.OnTick += Main.OnMain;
                Base.OnLateTick += Main.OnLate;

                LogService.Log("Successfully Subscribed To Events");
            }
            catch (Exception Ex)
            {
                LogService.Log(Ex.ToString(), LogLevel.Error);
                throw new Exception("EventSubscriptionException");
            }
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            // Overlay.Drawing.DrawFactory.DisposeGraphicsFactory();
            // Program.DrawBase.Close();
        }

        private static void OnProcessException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"Error: {(e.ExceptionObject as Exception).Message}");

            Console.ReadKey();
        }
    }
}
