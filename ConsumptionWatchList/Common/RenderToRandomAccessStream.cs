using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;

namespace ConsumptionWatchList.Common
{
    static class RenderToRandomAccessStream
    {
        public static async Task<IRandomAccessStream> RenderToRandomAccessStreamAsync(this UIElement element)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(element);

            var pixelBuffer = await rtb.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();

            var dpInfo = DisplayInformation.GetForCurrentView();
            var stream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied,
                (uint)rtb.PixelWidth,
                (uint)rtb.PixelHeight,
                dpInfo.RawDpiX,
                dpInfo.RawDpiY,
                pixels);

            await encoder.FlushAsync();

            stream.Seek(0);

            return stream;
        }
    }
}
