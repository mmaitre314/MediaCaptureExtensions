#pragma once

namespace Mocks
{
    ///<summary>A mock MediaCapture used for testing</summary>
    public ref class NullMediaCapture sealed
    {
    public:

        static Windows::Media::Capture::MediaCapture^ Create();

    private:

        NullMediaCapture()
        {
        }
    };

}
