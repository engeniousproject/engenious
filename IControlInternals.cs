using System;
using System.ComponentModel;
using OpenToolkit.Windowing.Common;

namespace engenious
{
    public interface IControlInternals
    {
        event Action<FrameEventArgs> RenderFrame;
        event Action<FrameEventArgs> UpdateFrame;
        event Action<CancelEventArgs> Closing;
        event Action<FocusedChangedEventArgs> FocusedChanged;
        event Action<TextInputEventArgs> KeyPress;
        event Action<ResizeEventArgs> Resize;
        event Action Load;
        event Action<MouseWheelEventArgs> MouseWheel;
    }
}