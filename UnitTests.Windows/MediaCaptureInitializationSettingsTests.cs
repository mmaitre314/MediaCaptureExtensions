using MediaCaptureExtensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace UnitTests.Windows
{
    [TestClass]
    public class MediaCaptureInitializationSettingsTests
    {
        [TestMethod]
        public async Task TestSelectVideoDevice()
        {
            var settings = new MediaCaptureInitializationSettings();

            Logger.LogMessage("Selecting camera");
            await settings.SelectVideoDeviceAsync(VideoDeviceSelection.BackOrFirst);
        }
    }
}
