using System;
using OpenTK.Platform;

namespace engenious
{
    public interface IControl : IDisposable
    {
        /// <summary>
        /// Calculates a <see cref="Point"/> in client coordinates to screen coordinates.
        /// </summary>
        /// <param name="pt">The <see cref="Point"/> in client coordinates.</param>
        /// <returns>The <see cref="Point"/> translated into screen coordinates.</returns>
        Point PointToScreen(Point pt);

        /// <summary>
        /// Calculates a <see cref="Point"/> in screen coordinates to client coordinates.
        /// </summary>
        /// <param name="pt">The <see cref="Point"/> in screen coordinates.</param>
        /// <returns>The <see cref="Point"/> translated into client coordinates.</returns>
        Point PointToClient(Point pt);
        
        /// <summary>
        /// Gets or sets a <see cref="Rectangle"/> for this control client area.
        /// </summary>
        Rectangle ClientRectangle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Size"/> of this controls client area.
        /// </summary>
        Size ClientSize { get; set; }

        /// <summary>
        /// Gets or sets whether the <see cref="IRenderingSurface"/> is in focus.
        /// </summary>
        bool Focused { get; }

        /// <summary>
        /// Gets or sets whether the mouse cursor is visible on this <see cref="IRenderingSurface"/>.
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets or sets whether the <see cref="IRenderingSurface"/> is visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets a native handle for this <see cref="IRenderingSurface"/>.
        /// </summary>
        IntPtr Handle { get; }
        
        /// <summary>
        /// Gets a representation of native surface handle for this <see cref="IRenderingSurface"/>.
        /// </summary>
        IWindowInfo WindowInfo { get; }
    }
}