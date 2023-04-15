using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlatUI;
using LoLSharp.Core;
using LoLSharp.Game;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;

namespace LoLSharp
{
  class Program
  {
    public static Overlay.Base DrawBase;
    public static Menu.BasePlate MenuBasePlate;

    static void Main(string[] args)
    {
      CustomScriptController.Init();

      Task.Run(async () =>
      {
        try
        {
          bool run = false;

          while (!run)
          {
            run = await Task.Run(() => API.Service.IsLiveGameRunning());
          }

          await Task.Run(() => LogService.Log("Found Live Instance of The Game."));
          await Task.Run(Memory.Init);

          await Task.Run(() => Events.EventsManager.SubscribeToEvents());

          await Task.Run(UnitRadiusService.ParseUnitRadiusData);
          // await Task.Run(MissileService.ParseMissileDBData);

          // await Task.Run(() => SpellDBService.ParseSpellDBData());

          await Task.Run(() => LogService.Log("Initialising Overlay Rendering..."));
          await Task.Run(() => LogService.Log("Game Version: " + Engine.GameVersion()));
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          throw;
        }
        await Task.Run(() =>
        {
          MenuBasePlate = new Menu.BasePlate();
          DrawBase = new Overlay.Base();

          AddScriptToggles();

          var localPlayer = Engine.GetLocalPlayer;
          LocalPlayer.Vo = new EntityVo(localPlayer);


          Engine.LoadChampionsManager();
          Engine.Zoom();

          LogService.Log(LocalPlayer.Vo.champName);

          DrawBase.Show();
        });
      }).GetAwaiter().GetResult();
    }
    private static void AddScriptToggles()
    {
      var customScriptBases = CustomScriptController.GetScripts();
      for (var index = 0; index < customScriptBases.Length; index++)
      {
        var scriptBase = customScriptBases[index];
        var panel = new TableLayoutPanel();
        var label = new FlatLabel();
        var checkBox = new FlatToggle();

        label.Text = scriptBase.Title;
        label.AutoSize = true;

        checkBox.Text = "Toggle";
        checkBox.Checked = scriptBase.Active;
        checkBox.Dock = DockStyle.Right;
        checkBox.CheckedChanged +=
          sender =>
          {
            scriptBase.Active = checkBox.Checked;
          };

        panel.ColumnCount = 2;
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.RowCount = 1;
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        panel.Controls.Add(label, 0, 0);
        panel.Controls.Add(checkBox, 1, 0);

        panel.Dock = DockStyle.Top;
        panel.Height = 50;

        MenuBasePlate.Controls.Add(panel);
      }
      
      var topPanel = new Panel();
      topPanel.BackColor = Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(45)))), ((int)(((byte)(66)))));
      topPanel.Dock = DockStyle.Top;
      topPanel.Location = new Point(0, 0);
      topPanel.Name = "TopPanel";
      topPanel.Size = new Size(299, 30);
      topPanel.TabIndex = 0;
      topPanel.MouseDown += MenuBasePlate.TopPanel_MouseDown;
      
      var title = new FlatLabel();

      title.Text = "LoLSharp";
      title.AutoSize = true;
      title.Location = new Point(topPanel.Width / 2 - title.Width / 2, topPanel.Height / 2 - title.Height / 2);
      
      topPanel.Controls.Add(title);
      
      MenuBasePlate.Controls.Add(topPanel);

    }
  }
}
