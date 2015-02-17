using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;

namespace MediaCaptureExtensions
{
    /// <summary>
    /// Extension methods on VideoDeviceController
    /// </summary>
    static public class VideoDeviceControllerExtensionsExtensions
    {
        /// <summary>
        /// Enumerate video preview formats and select the one
        /// whose dimensions are nearest to the input width/height.
        /// </summary>
        /// <returns>Selected format</returns>
        public static async Task<VideoEncodingProperties> SelectNearestPreviewResolutionAsync(this VideoDeviceController controller, double width, double height)
        {
            var formats = controller.GetAvailableMediaStreamProperties(MediaStreamType.VideoPreview);

            var format = (VideoEncodingProperties)formats.OrderBy((item) =>
            {
                var props = (VideoEncodingProperties)item;
                return Math.Abs(props.Width - width) + Math.Abs(props.Height - height);
            }).First();

            await controller.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, format);

            return format;
        }
    }
}
