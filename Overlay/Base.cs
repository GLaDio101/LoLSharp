using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LoLSharp.Modules;
using LoLSharp.Overlay.Drawing;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using Matrix = SharpDX.Matrix;
using Timer = System.Windows.Forms.Timer;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LoLSharp.Overlay
{
  public partial class Base : Form
  {
    public Base()
    {
      InitializeComponent();
    }

    private void Base_Load(object sender, EventArgs e)
    {
      OnLoad();
    }

    private void Base_Paint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      Rectangle rect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
      LinearGradientBrush brush = new LinearGradientBrush(rect, Color.LightBlue, Color.LightGreen, 90);
      g.FillRectangle(brush, rect);
      OnPaint();
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        // turn on WS_EX_TOOLWINDOW style bit
        cp.ExStyle |= 0x80;
        return cp;
      }
    }

    // internal void OnDraw()
    // {
    //
    //
    // }


    private static bool IsInitialised = false;
    internal void OnLoad()
    {
      Timer tmr = new Timer();
      tmr.Interval = 1000; // milliseconds
      tmr.Tick += (sender, args) => { Tmr_Tick(); }; // set handler
      tmr.Start();

      NativeImport.SetWindowLong(this.Handle, DrawFactory.GWL_EXSTYLE,
        (IntPtr)(NativeImport.GetWindowLong(this.Handle, DrawFactory.GWL_EXSTYLE) ^ DrawFactory.WS_EX_LAYERED ^ DrawFactory.WS_EX_TRANSPARENT));

      NativeImport.SetLayeredWindowAttributes(this.Handle, 0, 255, DrawFactory.LWA_ALPHA);

      if (IsInitialised == false)
      {
        PresentParameters presentParameters = new PresentParameters();
        presentParameters.Windowed = true;
        presentParameters.SwapEffect = SwapEffect.Discard;
        presentParameters.BackBufferFormat = Format.A8R8G8B8;

        DrawFactory.device = new Device(DrawFactory.D3D, 0, DeviceType.Hardware, this.Handle, CreateFlags.HardwareVertexProcessing, presentParameters);

        DrawFactory.drawLine = new Line(DrawFactory.device);
        DrawFactory.drawBoxLine = new Line(DrawFactory.device);
        DrawFactory.drawCircleLine = new Line(DrawFactory.device);
        DrawFactory.drawFilledBoxLine = new Line(DrawFactory.device);
        DrawFactory.drawTriLine = new Line(DrawFactory.device);

        FontDescription fontDescription = new FontDescription()
        {
          FaceName = "Fixedsys Regular",
          CharacterSet = FontCharacterSet.Default,
          Height = 20,
          Weight = FontWeight.Bold,
          MipLevels = 0,
          OutputPrecision = FontPrecision.Default,
          PitchAndFamily = FontPitchAndFamily.Default,
          Quality = FontQuality.ClearType
        };

        DrawFactory.font = new SharpDX.Direct3D9.Font(DrawFactory.device, fontDescription);
        DrawFactory.InitialiseCircleDrawing(DrawFactory.device);

        IsInitialised = true;
        RunLoop();
      }
    }
    private void RunLoop()
    {
      RenderLoop.Run(this, () =>
      {
        if (!LoLSharp.Events.Drawing.IsMenuBeingDrawn) NativeImport.BringWindowToTop(this.Handle);

        DrawFactory.device.Clear(ClearFlags.Target, new SharpDX.Mathematics.Interop.RawColorBGRA(0, 0, 0, 0), 1.0f, 0);
        DrawFactory.device.SetRenderState(RenderState.ZEnable, false);
        DrawFactory.device.SetRenderState(RenderState.Lighting, false);
        DrawFactory.device.SetRenderState(RenderState.CullMode, Cull.None);
        DrawFactory.device.SetTransform(TransformState.Projection, Matrix.OrthoOffCenterLH(0, this.Width, this.Height, 0, 0, 1));

        DrawFactory.device.BeginScene();

        LoLSharp.Events.Drawing.OnDeviceDraw();

        DrawFactory.device.EndScene();
        DrawFactory.device.Present();
      });
    }
    private void Tmr_Tick()
    {
      // Engine.LoadMinionManager();
      // LogService.Log(JsonConvert.SerializeObject(Engine.Monsters));
    }

    public void OnPaint()
    {
      DrawFactory.Marg.Left = 0;
      DrawFactory.Marg.Top = 0;
      DrawFactory.Marg.Right = this.Width;
      DrawFactory.Marg.Bottom = this.Height;

      NativeImport.DwmExtendFrameIntoClientArea(this.Handle, ref DrawFactory.Marg);
    }
  }
}
