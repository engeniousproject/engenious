using System;
using System.ComponentModel;
using OpenTK.Windowing.Common;

namespace engenious
{
    /// <summary>
    /// Base interface for internal events of controls.
    /// </summary>
    public interface IControlInternals
    {
        /// <summary>
        /// Event that gets called to render a frame.
        /// </summary>
        event Action<FrameEventArgs>? RenderFrame;
        /// <summary>
        /// Event that gets called to update a frame.
        /// </summary>
        event Action<FrameEventArgs>? UpdateFrame;
        /// <summary>
        /// Event that gets called when the control is closing.
        /// </summary>
        event Action<CancelEventArgs>? Closing;
        /// <summary>
        /// Event that gets called when the controls' focus changed.
        /// </summary>
        event Action<FocusedChangedEventArgs>? FocusedChanged;
        /// <summary>
        /// Event that gets called when a key is pressed on the control.
        /// </summary>
        event Action<TextInputEventArgs>? KeyPress;
        /// <summary>
        /// Event that gets called when the control is resized.
        /// </summary>
        event Action<ResizeEventArgs>? Resize;
        
        /// <summary>
        /// Event that gets called when the control loading.
        /// </summary>
        event Action? Load;
        /// <summary>
        /// Event that gets called when a mouse wheel event occured on the control.
        /// </summary>
        event Action<MouseWheelEventArgs>? MouseWheel;
    }
}