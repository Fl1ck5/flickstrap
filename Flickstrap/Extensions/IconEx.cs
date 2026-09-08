using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Flickstrap.Extensions
{
    public static class IconEx
    {
        public static Icon GetSized(this Icon icon, int width, int height) => new(icon, new Size(width, height));

        public static ImageSource GetImageSource(this Icon icon, bool handleException = true)
        {
            // Force extraction of the largest available frame (up to 256x256) before converting.
            // Without this, the Icon object may already be holding onto a small default frame
            // (commonly ~32x32), and converting that directly produces a blurry result anywhere
            // it's displayed larger than that - which several places in this app do.
            using Icon sizedIcon = icon.GetSized(256, 256);

            using MemoryStream stream = new();
            sizedIcon.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);

            if (handleException)
            {
                try
                {
                    return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
                catch (Exception ex)
                {
                    App.Logger.WriteException("IconEx::GetImageSource", ex);
                    Frontend.ShowMessageBox(string.Format(Strings.Dialog_IconLoadFailed, ex.Message));
                    return BootstrapperIcon.IconFlickstrap.GetIcon().GetImageSource(false);
                }
            }
            else
            {
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }
    }
}
