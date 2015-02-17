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
    /// Extension methods on MediaCapture
    /// </summary>
    static public class MediaCaptureExtensions
    {
        ///<summary>MF_MT_VIDEO_ROTATION</summary>
        static public Guid RotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");

        /// <summary>
        /// A version of MediaCapture.SetPreviewRotation() which does not add
        /// unnecessary black bars.
        /// </summary>
        public static async Task SetPreviewRotationAsync(this MediaCapture capture, VideoRotation rotation)
        {
            // Convert VideoRotation to MFVideoRotationFormat
            uint rotationValue;
            switch (rotation)
            {
                case VideoRotation.None: rotationValue = 0; break;
                case VideoRotation.Clockwise90Degrees: rotationValue = 90; break;
                case VideoRotation.Clockwise180Degrees: rotationValue = 180; break;
                case VideoRotation.Clockwise270Degrees: rotationValue = 270; break;
                default: throw new ArgumentException("rotation");
            }
  
            // Rotate preview
            var props = capture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
            props.Properties.Add(RotationKey, rotationValue);
            await capture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, null); 
        }
    }
}
