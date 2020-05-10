using System;
using System.ComponentModel;
using OpenTK;
using OpenTK.Input;

namespace engenious
{
    public interface IControlInternals
    {
        event EventHandler<FrameEventArgs> RenderFrame;
        event EventHandler<FrameEventArgs> UpdateFrame;
        event EventHandler<CancelEventArgs> Closing;
        event EventHandler<EventArgs> FocusedChanged;
        event EventHandler<KeyPressEventArgs> KeyPress;
        event EventHandler<EventArgs> Resize;
        event EventHandler<EventArgs> Load;
        event EventHandler<MouseWheelEventArgs> MouseWheel;
    }
}