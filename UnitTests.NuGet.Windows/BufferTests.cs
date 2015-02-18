using MediaCaptureExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage.Streams;

namespace UnitTests.NuGet.Windows
{
    [TestClass]
    public class BufferTests
    {
        [TestMethod]
        unsafe public void TestNuGetBuffer()
        {
            var buffer = new Buffer(100);

            Logger.LogMessage("Filling buffer with some data");

            byte* data = buffer.GetData();
            for (int i = 0; i < buffer.Capacity; i++)
            {
                data[i] = 42;
            }
            buffer.Length = buffer.Capacity;

            Logger.LogMessage("Checking buffer content");

            var reader = DataReader.FromBuffer(buffer);
            for (int i = 0; i < buffer.Capacity; i++)
            {
                Assert.AreEqual(42, reader.ReadByte());
            }
        }
    }
}
