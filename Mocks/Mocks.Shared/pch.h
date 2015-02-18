#pragma once

#include <collection.h>
#include <ppltasks.h>
#include <wrl.h>

#include <mfapi.h>
#include <mfidl.h>
#include <Mferror.h>

#include <windows.media.capture.h>

//
// Error handling
//

// Exception-based error handling
#define CHK(statement)  {HRESULT _hr = (statement); if (FAILED(_hr)) { throw ref new Platform::COMException(_hr); };}
#define CHKNULL(p)  {if ((p) == nullptr) { throw ref new Platform::NullReferenceException(L#p); };}
#define CHKOOM(p)  {if ((p) == nullptr) { throw ref new Platform::OutOfMemoryException(L#p); };}

// Exception-free error handling
#define CHK_RETURN(statement) {hr = (statement); if (FAILED(hr)) { return hr; };}

//
// Error origin
//

// A method to track error origin
template <size_t N>
HRESULT OriginateError(_In_ HRESULT hr, _In_ wchar_t const (&str)[N])
{
    if (FAILED(hr))
    {
        ::RoOriginateErrorW(hr, N - 1, str);
    }
    return hr;
}

// A method to track error origin
inline HRESULT OriginateError(_In_ HRESULT hr)
{
    if (FAILED(hr))
    {
        ::RoOriginateErrorW(hr, 0, nullptr);
    }
    return hr;
}

//
// Exception boundary (converts exceptions into HRESULTs)
//

inline HRESULT ExceptionBoundary(std::function<void()>&& lambda)
{
    HRESULT hr = S_OK;
    try
    {
        lambda();
    }
#ifdef _INC_COMDEF // include comdef.h to enable
    catch (const _com_error& e)
    {
        hr = e.Error();
    }
#endif
#ifdef __cplusplus_winrt // enable C++/CX to use (/ZW)
    catch (Platform::Exception^ e)
    {
        hr = e->HResult;
    }
#endif
    catch (const std::bad_alloc&)
    {
        hr = E_OUTOFMEMORY;
    }
    catch (const std::out_of_range&)
    {
        hr = E_BOUNDS;
    }
    catch (const std::exception&)
    {
        hr = E_FAIL;
    }
    catch (...)
    {
        hr = E_FAIL;
    }

    return hr;
}

//
// Casting
//

template<typename T, typename U>
Microsoft::WRL::ComPtr<T> As(const Microsoft::WRL::ComPtr<U>& in)
{
    Microsoft::WRL::ComPtr<T> out;
    CHK(in.As(&out));
    return out;
}

template<typename T, typename U>
Microsoft::WRL::ComPtr<T> As(U* in)
{
    Microsoft::WRL::ComPtr<T> out;
    CHK(in->QueryInterface(IID_PPV_ARGS(&out)));
    return out;
}

template<typename T, typename U>
Microsoft::WRL::ComPtr<T> As(U^ in)
{
    Microsoft::WRL::ComPtr<T> out;
    CHK(reinterpret_cast<IInspectable*>(in)->QueryInterface(IID_PPV_ARGS(&out)));
    return out;
}

//
// mfmediacapture.h is missing in Phone SDK 8.1
//

MIDL_INTERFACE("24E0485F-A33E-4aa1-B564-6019B1D14F65")
IAdvancedMediaCaptureSettings : public IUnknown
{
public:
    virtual HRESULT STDMETHODCALLTYPE GetDirectxDeviceManager(
        /* [out] */ IMFDXGIDeviceManager **value) = 0;

};

MIDL_INTERFACE("D0751585-D216-4344-B5BF-463B68F977BB")
IAdvancedMediaCapture : public IUnknown
{
public:
    virtual HRESULT STDMETHODCALLTYPE GetAdvancedMediaCaptureSettings(
        /* [out] */ __RPC__deref_out_opt IAdvancedMediaCaptureSettings **value) = 0;

};

//
// Namespace shortcuts
//

namespace AWF = ::ABI::Windows::Foundation;
namespace AWFC = ::ABI::Windows::Foundation::Collections;
namespace AWM = ::ABI::Windows::Media;
namespace AWMC = ::ABI::Windows::Media::Capture;
namespace AWMD = ::ABI::Windows::Media::Devices;
namespace AWMMp = ::ABI::Windows::Media::MediaProperties;
namespace AWS = ::ABI::Windows::Storage;
namespace AWSS = ::ABI::Windows::Storage::Streams;
namespace MW = ::Microsoft::WRL;
namespace MWD = ::Microsoft::WRL::Details;
namespace MWW = ::Microsoft::WRL::Wrappers;
namespace WF = ::Windows::Foundation;
namespace WFC = ::Windows::Foundation::Collections;
namespace WFM = ::Windows::Foundation::Metadata;
namespace WGI = ::Windows::Graphics::Imaging;
namespace WM = ::Windows::Media;
namespace WMC = ::Windows::Media::Capture;
namespace WMCo = ::Windows::Media::Core;
namespace WMMp = ::Windows::Media::MediaProperties;
namespace WS = ::Windows::Storage;
namespace WSS = ::Windows::Storage::Streams;
namespace WUXMI = ::Windows::UI::Xaml::Media::Imaging;
namespace WUXC = ::Windows::UI::Xaml::Controls;
