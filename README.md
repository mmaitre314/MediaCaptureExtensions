[![Build status](https://ci.appveyor.com/api/projects/status/6ino9fddjh9qxd8h?svg=true)](https://ci.appveyor.com/project/mmaitre314/mediacaptureextensions) [![NuGet package](http://mmaitre314.github.io/images/nuget.png)](https://www.nuget.org/packages/MMaitre.MediaCaptureExtensions/)

To enable the helper methods below add this [NuGet package](https://www.nuget.org/packages/MMaitre.MediaCaptureExtensions/) to your C# project and `using MediaCaptureExtensions;` to .cs files.

## Select back or front camera

```c#
var settings = new MediaCaptureInitializationSettings();
await settings.SelectVideoDeviceAsync(VideoDeviceSelection.BackOrFirst);

var capture = new MediaCapture();
await capture.InitializeAsync(settings);
```

## Rotate preview without extra black bars

```c3
var capture = new MediaCapture();
await capture.InitializeAsync();

await capture.SetPreviewRotationAsync(VideoRotation.Clockwise90Degrees);
```

## Direct access to bytes in IBuffer

This extension requires "unsafe" code to be enabled.

```c#
var buffer = new Buffer(100);
byte* data = buffer.GetData();
for (int i = 0; i < buffer.Capacity; i++)
{
    data[i] = 42;
}
buffer.Length = buffer.Capacity;
```
