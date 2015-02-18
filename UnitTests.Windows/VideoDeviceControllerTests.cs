using MediaCaptureExtensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;

namespace UnitTests.Windows
{
    [TestClass]
    public class VideoDeviceControllerTests
    {
        [TestMethod]
        public async Task TestSelectNearestPreviewResolution()
        {
            var capture = NullMediaCapture.Create();

            Logger.LogMessage("Selecting preview resolution");
            VideoEncodingProperties props = await capture.VideoDeviceController.SelectNearestPreviewResolutionAsync(640, 480);

            Assert.AreEqual("NV12", props.Subtype);
            Assert.AreEqual(320, (int)props.Width);
            Assert.AreEqual(240, (int)props.Height);
        }
    }
}
