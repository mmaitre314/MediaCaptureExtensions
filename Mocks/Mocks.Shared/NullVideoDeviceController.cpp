#include "pch.h"
#include "NullVideoDeviceController.h"

using namespace concurrency;
using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Media::Capture;
using namespace Windows::Media::MediaProperties;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

NullVideoDeviceController::NullVideoDeviceController()
{
    auto videoRecordProperties = VideoEncodingProperties::CreateUncompressed(
        MediaEncodingSubtypes::Nv12,
        320,
        240
        );
    videoRecordProperties->FrameRate->Numerator = 30;
    videoRecordProperties->FrameRate->Denominator = 1;
    _videoRecordProperties = videoRecordProperties;

    auto videoPreviewProperties = VideoEncodingProperties::CreateUncompressed(
        MediaEncodingSubtypes::Nv12,
        320,
        240
        );
    videoPreviewProperties->FrameRate->Numerator = 30;
    videoPreviewProperties->FrameRate->Denominator = 1;
    _videoPreviewProperties = videoPreviewProperties;

    _audioRecordProperties = AudioEncodingProperties::CreatePcm(44100, 1, 16);
}

NullVideoDeviceController::~NullVideoDeviceController()
{
}

STDMETHODIMP NullVideoDeviceController::GetMediaStreamProperties(
    _In_ AWMC::MediaStreamType mediaStreamType,
    _COM_Outptr_ AWMMp::IMediaEncodingProperties **value
    )
{
    *value = nullptr;

    return ExceptionBoundary([=]()
    {
        switch (mediaStreamType)
        {
        case AWMC::MediaStreamType_VideoPreview:
            *value = As<AWMMp::IMediaEncodingProperties>(_videoPreviewProperties).Detach();
            break;

        case AWMC::MediaStreamType_VideoRecord:
            *value = As<AWMMp::IMediaEncodingProperties>(_videoRecordProperties).Detach();
            break;

        case AWMC::MediaStreamType_Audio:
            *value = As<AWMMp::IMediaEncodingProperties>(_audioRecordProperties).Detach();
            break;

        default:
            throw ref new InvalidArgumentException(L"mediaStreamType");
        }
    });
}

STDMETHODIMP NullVideoDeviceController::SetMediaStreamPropertiesAsync(
    _In_ AWMC::MediaStreamType mediaStreamType,
    _In_ AWMMp::IMediaEncodingProperties *mediaEncodingProperties,
    _COM_Outptr_ AWF::IAsyncAction **asyncInfo
    )
{
    *asyncInfo = nullptr;

    return ExceptionBoundary([=]()
    {
        CHKNULL(mediaEncodingProperties);

        switch (mediaStreamType)
        {
        case AWMC::MediaStreamType_VideoPreview:
            _videoPreviewProperties = reinterpret_cast<IMediaEncodingProperties^>(mediaEncodingProperties);
            break;

        case AWMC::MediaStreamType_VideoRecord:
            _videoRecordProperties = reinterpret_cast<IMediaEncodingProperties^>(mediaEncodingProperties);
            break;

        case AWMC::MediaStreamType_Audio:
            _audioRecordProperties = reinterpret_cast<IMediaEncodingProperties^>(mediaEncodingProperties);
            break;

        default:
            throw ref new InvalidArgumentException(L"mediaStreamType");
        }

        IAsyncAction^ action = create_async([]()
        {
            return task_from_result();
        });

        *asyncInfo = As<AWF::IAsyncAction>(action).Detach();
    });
}

STDMETHODIMP NullVideoDeviceController::GetAvailableMediaStreamProperties(
    _In_ AWMC::MediaStreamType mediaStreamType,
    _COM_Outptr_ AWFC::IVectorView<AWMMp::IMediaEncodingProperties*> **value
    )
{
    *value = nullptr;

    return ExceptionBoundary([=]()
    {
        auto list = ref new Vector<IMediaEncodingProperties^>();

        switch (mediaStreamType)
        {
        case AWMC::MediaStreamType_VideoPreview:
        case AWMC::MediaStreamType_VideoRecord:
        {
            auto props = VideoEncodingProperties::CreateUncompressed(
                MediaEncodingSubtypes::Nv12,
                320,
                240
                );
            props->FrameRate->Numerator = 30;
            props->FrameRate->Denominator = 1;
            list->Append(props);

            props = VideoEncodingProperties::CreateUncompressed(
                MediaEncodingSubtypes::Yuy2,
                320,
                240
                );
            props->FrameRate->Numerator = 30;
            props->FrameRate->Denominator = 1;
            list->Append(props);
        }
        break;

        case AWMC::MediaStreamType_Audio:
        {
            list->Append(AudioEncodingProperties::CreatePcm(44100, 1, 16));
            list->Append(AudioEncodingProperties::CreatePcm(44100, 2, 16));
        }
        break;

        default:
            throw ref new InvalidArgumentException(L"mediaStreamType");
        }

        *value = As<AWFC::IVectorView<AWMMp::IMediaEncodingProperties*>>(list->GetView()).Detach();
    });
}
