using System;
using System.Threading;
using System.Windows.Forms;
using LoLSharp.Events;
using LoLSharp.Game;
using LoLSharp.Modules;
using SharpDX;

namespace LoLSharp.Menu
{
  public partial class BasePlate : Form
  {
    public const string flash = "SummonerFlash";

    public BasePlate()
    {
      InitializeComponent();
    }

    private void BasePlate_Load(object sender, EventArgs e)
    {
    }

    public void TopPanel_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        NativeImport.ReleaseCapture();
        NativeImport.SendMessage(Handle, NativeImport.WM_NCLBUTTONDOWN, NativeImport.HTCAPTION, 0);
      }
    }

  }
}
