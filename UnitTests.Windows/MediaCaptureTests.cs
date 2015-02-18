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
    public class MediaCaptureTests
    {
        [TestMethod]
        public async Task TestSetPreviewRotation()
        {
            var capture = NullMediaCapture.Create();

            Logger.LogMessage("Applying rotation");
            await capture.SetPreviewRotationAsync(VideoRotation.Clockwise90Degrees);
        }
    }
}
