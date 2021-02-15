using System;
using System.Collections.Generic;
using System.ComponentModel;
using engenious.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using KeyboardState = engenious.Input.KeyboardState;
using MouseState = engenious.Input.MouseState;

namespace engenious
{
    /// <summary>
    /// A wrapper for wrapping opentk windows.
    /// </summary>
    public interface IWindowWrapper : IDisposable
    {
        /// <summary>
        /// Gets the underlying <see cref="IGraphicsContext"/>.
        /// </summary>
        IGraphicsContext Context { get; }
        /// <summary>
        /// Gets the current mouse state on the wrapped window.
        /// </summary>
        MouseState MouseState { get; }
        /// <summary>
        /// Gets the current keyboard state on the wrapped window.
        /// </summary>
        KeyboardState KeyboardState { get; }
        /// <summary>
        /// Gets the current joystick states on the wrapped window.
        /// </summary>
        IReadOnlyList<JoystickState?> JoystickStates { get; }
        
        /// <summary>
        /// Gets the title on the wrapped window.
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// Gets the current icon of the wrapped window.
        /// </summary>
        WindowIcon? Icon { get; set; }
                
        /// <summary>
        /// Gets the current mouse position on the wrapped window.
        /// </summary>
        Vector2 MousePosition { get; set; }
                        
        /// <summary>
        /// Gets whether the current wrapped window is visible.
        /// </summary>
        bool IsVisible { get; set; }
                                
        /// <summary>
        /// Gets whether the current wrapped window is focused.
        /// </summary>
        bool IsFocused { get; }
                                
        /// <summary>
        /// Gets whether the mouse cursor on the current wrapped window is visible.
        /// </summary>
        bool CursorVisible { get; set; }
 
        /// <summary>
        /// Gets whether the mouse cursor on the current wrapped window is grabbed.
        /// </summary>
        bool CursorGrabbed { get; set; }

        /// <summary>
        /// Gets the current position of the wrapped window.
        /// </summary>
        Point Location { get; set; }
        
        /// <summary>
        /// Gets the current client size of the wrapped window.
        /// </summary>
        Point ClientSize { get; }
        
        /// <summary>
        /// Gets the current size of the wrapped window.
        /// </summary>
        Point Size { get; set; }

        /// <summary>
        /// Gets the current client rectangle of the wrapped window.
        /// </summary>
        Rectangle ClientRectangle { get; set; }
        
        /// <summary>
        /// Gets the current window border of the wrapped window.
        /// </summary>
        WindowBorder WindowBorder { get; set; }
        
        /// <summary>
        /// Gets the current window state of the wrapped window.
        /// </summary>
        WindowState WindowState { get; set; }

        /// <summary>
        /// Runs the wrapped windows messaging loop.
        /// </summary>
        void Run();

        /// <summary>
        /// Closes the window.
        /// </summary>
        void Close();

        /// <summary>
        /// Converts a point in client coordinates to screen coordinates.
        /// </summary>
        /// <param name="pt">The point in client coordinates.</param>
        /// <returns>Given input in screen coordinates.</returns>
        Point PointToScreen(Point pt);
        
        /// <summary>
        /// Converts a point in screen coordinates to client coordinates.
        /// </summary>
        /// <param name="pt">The point in screen coordinates.</param>
        /// <returns>Given input in client coordinates.</returns>
        Point PointToClient(Point pt);

        /// <summary>
        /// Occurs when the wrapped window is loaded.
        /// </summary>
        event Action? Load;

        /// <summary>
        /// Occurs when a frame is to be rendered on the wrapped window.
        /// </summary>
        event Action<FrameEventArgs>? RenderFrame;
        /// <summary>
        /// Occurs when a frame is to be updated on the wrapped window.
        /// </summary>
        event Action<FrameEventArgs>? UpdateFrame;
        /// <summary>
        /// Occurs when the wrapped window is closing.
        /// </summary>
        event Action<CancelEventArgs>? Closing;
        /// <summary>
        /// Occurs when the wrapped windows focus changed.
        /// </summary>
        event Action<FocusedChangedEventArgs>? FocusedChanged;
        /// <summary>
        /// Occurs when the wrapped window resized.
        /// </summary>
        event Action<ResizeEventArgs>? Resize;
        /// <summary>
        /// Occurs when text was typed on the wrapped window.
        /// </summary>
        event Action<TextInputEventArgs>? KeyPress;
        /// <summary>
        /// Occurs when mouse wheel was scrolled on the wrapped window.
        /// </summary>
        event Action<MouseWheelEventArgs>? MouseWheel;
    }
}