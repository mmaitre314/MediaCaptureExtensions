using MediaCaptureExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CaptureTestApp
{
    public sealed partial class MainPage : Page
    {
        DisplayRequest m_displayRequest = new DisplayRequest();
        MediaCapture m_capture;
#if !WINDOWS_PHONE_APP
        SystemMediaTransportControls m_mediaControls;
#endif

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Disable app UI rotation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;

            // Prevent screen timeout
            m_displayRequest.RequestActive();

            // Handle app going to and coming out of background
#if WINDOWS_PHONE_APP
            Application.Current.Resuming += App_Resuming;
            Application.Current.Suspending += App_Suspending;
            var ignore = InitializeCaptureAsync();
#else
            m_mediaControls = SystemMediaTransportControls.GetForCurrentView();
            m_mediaControls.PropertyChanged += m_mediaControls_PropertyChanged;

            if (!IsInBackground())
            {
                var ignore = InitializeCaptureAsync();
            }
#endif
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
#if WINDOWS_PHONE_APP
            Application.Current.Resuming -= App_Resuming;
            Application.Current.Suspending -= App_Suspending;
#else
            m_mediaControls.PropertyChanged -= m_mediaControls_PropertyChanged;
#endif

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;

            m_displayRequest.RequestRelease();
        }

#if WINDOWS_PHONE_APP
        private void App_Resuming(object sender, object e)
        {
            // Dispatch call to the UI thread since the event may get fired on some other thread
            var ignore = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await InitializeCaptureAsync();
            });
        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            DisposeCapture();
        }
#else
        void m_mediaControls_PropertyChanged(SystemMediaTransportControls sender, SystemMediaTransportControlsPropertyChangedEventArgs args)
        {
            if (args.Property != SystemMediaTransportControlsProperty.SoundLevel)
            {
                return;
            }

            if (!IsInBackground())
            {
                // Dispatch call to the UI thread since the event may get fired on some other thread
                var ignore = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await InitializeCaptureAsync();
                });
            }
            else
            {
                DisposeCapture();
            }
        }

        private bool IsInBackground()
        {
            return m_mediaControls.SoundLevel == SoundLevel.Muted;
        }
#endif

        private async Task InitializeCaptureAsync()
        {
            var settings = new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Video
            };
            await settings.SelectVideoDeviceAsync(VideoDeviceSelection.BackOrFirst);

            var capture = new MediaCapture();
            await capture.InitializeAsync(settings);

            // Start preview
            await capture.VideoDeviceController.SelectNearestPreviewResolutionAsync(this.ActualWidth, this.ActualHeight);
            Preview.Source = capture;
            await capture.StartPreviewAsync();

            // Enable continuous auto-focus (when supported)
            var control = capture.VideoDeviceController.FocusControl;
#if WINDOWS_PHONE_APP
            if (control.SupportedFocusModes.Contains(FocusMode.Continuous) &&
                control.SupportedFocusRanges.Contains(AutoFocusRange.FullRange))
            {
                control.Configure(new FocusSettings 
                { 
                    Mode = FocusMode.Continuous, 
                    AutoFocusRange = AutoFocusRange.FullRange 
                });
                await control.FocusAsync();
            }
#else
            if (control.SupportedPresets.Contains(FocusPreset.Auto))
            {
                await control.SetPresetAsync(FocusPreset.Auto);
            }
#endif

            m_capture = capture;
        }

        private void DisposeCapture()
        {
            lock (this)
            {
                if (m_capture != null)
                {
                    m_capture.Dispose();
                    m_capture = null;
                }
            }
        }

        private async void Rotation0_Click(object sender, RoutedEventArgs e)
        {
            await m_capture.SetPreviewRotationAsync(VideoRotation.None);
        }

        private async void Rotation90_Click(object sender, RoutedEventArgs e)
        {
            await m_capture.SetPreviewRotationAsync(VideoRotation.Clockwise90Degrees);
        }

        private async void Rotation180_Click(object sender, RoutedEventArgs e)
        {
            await m_capture.SetPreviewRotationAsync(VideoRotation.Clockwise180Degrees);
        }

        private async void Rotation270_Click(object sender, RoutedEventArgs e)
        {
            await m_capture.SetPreviewRotationAsync(VideoRotation.Clockwise270Degrees);
        }
    }
}
