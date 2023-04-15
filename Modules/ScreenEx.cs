using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace LoLSharp.Modules
{
    class ScreenEx
    {
        public static Bitmap GetScreenCapture(Rectangle fov)
        {
            var screenCopy = new Bitmap(fov.Width, fov.Height, PixelFormat.Format24bppRgb);

            using (var gdest = Graphics.FromImage(screenCopy))

            using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hSrcDc = gsrc.GetHdc();
                var hDc = gdest.GetHdc();
                NativeImport.BitBlt(hDc, 0, 0, fov.Width, fov.Height, hSrcDc, fov.X, fov.Y, (int)CopyPixelOperation.SourceCopy);

                gdest.ReleaseHdc();
                gsrc.ReleaseHdc();
            }

            return screenCopy;
        }
    }
}
