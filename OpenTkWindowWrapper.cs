using System;
using System.Collections.Generic;
using System.ComponentModel;
using engenious.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using KeyboardState = engenious.Input.KeyboardState;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;
using MouseButton = engenious.Input.MouseButton;

namespace engenious
{
    internal class OpenTkWindowWrapper : IWindowWrapper
    {
        private readonly GameWindow _window;
        private MouseScroll _scroll;

        private KeyboardState _keyboardState;

        public OpenTkWindowWrapper(GameWindow window)
        {
            _window = window;
            _window.KeyDown += args =>
            {
                if (args.Alt && args.Key == Keys.F4)
                    Close();
            };
            // _window.RenderFrame += RenderFrame;
            // _window.UpdateFrame += UpdateFrame;
            // _window.Closing += Closing;
            // _window.FocusedChanged += FocusedChanged;
            // _window.Resize += Resize;
            // _window.TextInput += TextInput;
            // _window.MouseWheel += MouseWheel;
            // _window.Load += Load;
            _window.MouseWheel += args =>
            {
                _scroll = new MouseScroll(_scroll.X + args.OffsetX, _scroll.Y + args.OffsetY);
            };

            _window.KeyDown += args =>
            {
                _keyboardState.SetKeyState(MapOpenTKKey(args.Key), true);
            };
            _window.KeyUp += args =>
            {
                _keyboardState.SetKeyState(MapOpenTKKey(args.Key), false);
            };
        }

        private engenious.Input.Keys MapOpenTKKey(OpenTK.Windowing.GraphicsLibraryFramework.Keys key)
        {
            return key switch
            {
                Keys.Unknown => engenious.Input.Keys.Unknown,
                Keys.Space => engenious.Input.Keys.Space,
                Keys.Apostrophe => engenious.Input.Keys.Quote,
                Keys.Comma => engenious.Input.Keys.Comma,
                Keys.Minus => engenious.Input.Keys.Minus,
                Keys.Period => engenious.Input.Keys.Period,
                Keys.Slash => engenious.Input.Keys.Slash,
                Keys.D0 => engenious.Input.Keys.D0,
                Keys.D1 => engenious.Input.Keys.D1,
                Keys.D2 => engenious.Input.Keys.D2,
                Keys.D3 => engenious.Input.Keys.D3,
                Keys.D4 => engenious.Input.Keys.D4,
                Keys.D5 => engenious.Input.Keys.D5,
                Keys.D6 => engenious.Input.Keys.D6,
                Keys.D7 => engenious.Input.Keys.D7,
                Keys.D8 => engenious.Input.Keys.D8,
                Keys.D9 => engenious.Input.Keys.D9,
                Keys.Semicolon => engenious.Input.Keys.Semicolon,
                Keys.A => engenious.Input.Keys.A,
                Keys.B => engenious.Input.Keys.B,
                Keys.C => engenious.Input.Keys.C,
                Keys.D => engenious.Input.Keys.D,
                Keys.E => engenious.Input.Keys.E,
                Keys.F => engenious.Input.Keys.F,
                Keys.G => engenious.Input.Keys.G,
                Keys.H => engenious.Input.Keys.H,
                Keys.I => engenious.Input.Keys.I,
                Keys.J => engenious.Input.Keys.J,
                Keys.K => engenious.Input.Keys.K,
                Keys.L => engenious.Input.Keys.L,
                Keys.M => engenious.Input.Keys.M,
                Keys.N => engenious.Input.Keys.N,
                Keys.O => engenious.Input.Keys.O,
                Keys.P => engenious.Input.Keys.P,
                Keys.Q => engenious.Input.Keys.Q,
                Keys.R => engenious.Input.Keys.R,
                Keys.S => engenious.Input.Keys.S,
                Keys.T => engenious.Input.Keys.T,
                Keys.U => engenious.Input.Keys.U,
                Keys.V => engenious.Input.Keys.V,
                Keys.W => engenious.Input.Keys.W,
                Keys.X => engenious.Input.Keys.X,
                Keys.Y => engenious.Input.Keys.Y,
                Keys.Z => engenious.Input.Keys.Z,
                Keys.LeftBracket => engenious.Input.Keys.BracketLeft,
                Keys.Backslash => engenious.Input.Keys.BackSlash,
                Keys.RightBracket => engenious.Input.Keys.BracketRight,
                Keys.GraveAccent => engenious.Input.Keys.Grave,
                Keys.Escape => engenious.Input.Keys.Escape,
                Keys.Enter => engenious.Input.Keys.Enter,
                Keys.Tab => engenious.Input.Keys.Tab,
                Keys.Backspace => engenious.Input.Keys.BackSpace,
                Keys.Insert => engenious.Input.Keys.Insert,
                Keys.Delete => engenious.Input.Keys.Delete,
                Keys.Right => engenious.Input.Keys.Right,
                Keys.Left => engenious.Input.Keys.Left,
                Keys.Down => engenious.Input.Keys.Down,
                Keys.Up => engenious.Input.Keys.Up,
                Keys.PageUp => engenious.Input.Keys.PageUp,
                Keys.PageDown => engenious.Input.Keys.PageDown,
                Keys.Home => engenious.Input.Keys.Home,
                Keys.End => engenious.Input.Keys.End,
                Keys.CapsLock => engenious.Input.Keys.CapsLock,
                Keys.ScrollLock => engenious.Input.Keys.ScrollLock,
                Keys.NumLock => engenious.Input.Keys.NumLock,
                Keys.PrintScreen => engenious.Input.Keys.PrintScreen,
                Keys.Pause => engenious.Input.Keys.Pause,
                Keys.F1 => engenious.Input.Keys.F1,
                Keys.F2 => engenious.Input.Keys.F2,
                Keys.F3 => engenious.Input.Keys.F3,
                Keys.F4 => engenious.Input.Keys.F4,
                Keys.F5 => engenious.Input.Keys.F5,
                Keys.F6 => engenious.Input.Keys.F6,
                Keys.F7 => engenious.Input.Keys.F7,
                Keys.F8 => engenious.Input.Keys.F8,
                Keys.F9 => engenious.Input.Keys.F9,
                Keys.F10 => engenious.Input.Keys.F10,
                Keys.F11 => engenious.Input.Keys.F11,
                Keys.F12 => engenious.Input.Keys.F12,
                Keys.F13 => engenious.Input.Keys.F13,
                Keys.F14 => engenious.Input.Keys.F14,
                Keys.F15 => engenious.Input.Keys.F15,
                Keys.F16 => engenious.Input.Keys.F16,
                Keys.F17 => engenious.Input.Keys.F17,
                Keys.F18 => engenious.Input.Keys.F18,
                Keys.F19 => engenious.Input.Keys.F19,
                Keys.F20 => engenious.Input.Keys.F20,
                Keys.F21 => engenious.Input.Keys.F21,
                Keys.F22 => engenious.Input.Keys.F22,
                Keys.F23 => engenious.Input.Keys.F23,
                Keys.F24 => engenious.Input.Keys.F24,
                Keys.F25 => engenious.Input.Keys.F25,
                Keys.KeyPad0 => engenious.Input.Keys.Keypad0,
                Keys.KeyPad1 => engenious.Input.Keys.Keypad1,
                Keys.KeyPad2 => engenious.Input.Keys.Keypad2,
                Keys.KeyPad3 => engenious.Input.Keys.Keypad3,
                Keys.KeyPad4 => engenious.Input.Keys.Keypad4,
                Keys.KeyPad5 => engenious.Input.Keys.Keypad5,
                Keys.KeyPad6 => engenious.Input.Keys.Keypad6,
                Keys.KeyPad7 => engenious.Input.Keys.Keypad7,
                Keys.KeyPad8 => engenious.Input.Keys.Keypad8,
                Keys.KeyPad9 => engenious.Input.Keys.Keypad9,
                Keys.KeyPadDecimal => engenious.Input.Keys.KeypadDecimal,
                Keys.KeyPadDivide => engenious.Input.Keys.KeypadDivide,
                Keys.KeyPadMultiply => engenious.Input.Keys.KeypadMultiply,
                Keys.KeyPadSubtract => engenious.Input.Keys.KeypadSubtract,
                Keys.KeyPadAdd => engenious.Input.Keys.KeypadAdd,
                Keys.KeyPadEnter => engenious.Input.Keys.KeypadEnter,
                Keys.LeftShift => engenious.Input.Keys.ShiftLeft,
                Keys.LeftControl => engenious.Input.Keys.ControlLeft,
                Keys.LeftAlt => engenious.Input.Keys.AltLeft,
                Keys.LeftSuper => engenious.Input.Keys.WinLeft,
                Keys.RightShift => engenious.Input.Keys.ShiftRight,
                Keys.RightControl => engenious.Input.Keys.ControlRight,
                Keys.RightAlt => engenious.Input.Keys.AltRight,
                Keys.RightSuper => engenious.Input.Keys.WinRight,
                Keys.Menu => engenious.Input.Keys.Menu,
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
            };
        }

        public IGraphicsContext Context => _window.Context;

        public engenious.Input.MouseState MouseState
        {
            get
            {
                var state = _window.MouseState;
                var actual = new engenious.Input.MouseState()
                    {IsConnected = true, Position = new engenious.Vector2(state.Position.X, state.Position.Y), Scroll = _scroll};
                const int mouseButtonCount = (int)MouseButton.LastButton;
                for (var i = 0; i < mouseButtonCount; i++)
                {
                    var original = (OpenTK.Windowing.GraphicsLibraryFramework.MouseButton) i;
                    if (state[original])
                    {
                        actual.EnableBit(i);
                    }
                
                }
                return actual;
            }
        }

        public engenious.Input.KeyboardState KeyboardState
        {
            get
            {
                return _keyboardState;
            }
        }

        public IReadOnlyList<JoystickState?> JoystickStates => _window.JoystickStates;
        public string Title
        {
            get => _window.Title;
            set => _window.Title = value;
        }

        public WindowIcon? Icon
        {
            get => _window.Icon;
            set => _window.Icon = value;
        }

        public Vector2 MousePosition
        {
            get
            {
                var (x, y) = _window.MousePosition;
                return new(x, y);
            }
            set => _window.MousePosition = new OpenTK.Mathematics.Vector2(value.X, value.Y);
        }

        public bool IsVisible
        {
            get => _window.IsVisible;
            set => _window.IsVisible = value;
        }

        public bool IsFocused => _window.IsFocused;
        public bool CursorVisible
        {
            get => _window.CursorVisible;
            set => _window.CursorVisible = value;
        }

        public bool CursorGrabbed
        {
            get => _window.CursorGrabbed;
            set => _window.CursorGrabbed = value;
        }

        public Point Location
        {
            get
            {
                var (x, y) = _window.Location;
                return new Point(x, y);
            }
            set => _window.Location = new Vector2i(value.X, value.Y);
        }
        public Point Size
        {
            get
            {
                var (x, y) = _window.Size;
                return new Point(x, y);
            }
            set => _window.Size = new Vector2i(value.X, value.Y);
        }
        public Point ClientSize
        {
            get
            {
                var (x, y) = _window.ClientSize;
                return new Point(x, y);
            }
        }

        public Rectangle ClientRectangle
        {
            get
            {
                var clientRectangle = _window.ClientRectangle;
                var (x, y) = clientRectangle.Min;
                var (width, height) = clientRectangle.Size;
                return new Rectangle(x, y, width, height);
            }
            set => _window.ClientRectangle = new Box2i(value.Left, value.Top, value.Right, value.Bottom);
        }

        public WindowBorder WindowBorder
        {
            get => _window.WindowBorder;
            set => _window.WindowBorder = value;
        }

        public WindowState WindowState
        {
            get => _window.WindowState;
            set => _window.WindowState = value;
        }

        public void ProcessEvents()
        {
            _window.ProcessEvents();
        }

        public void Run()
        {
            _window.Run();
        }

        public void Close()
        {
            _window.Close();
        }

        public Point PointToScreen(Point pt)
        {
            return _window.PointToScreen(new Vector2i(pt.X, pt.Y));
        }

        public Point PointToClient(Point pt)
        {
            return _window.PointToClient(new Vector2i(pt.X, pt.Y));
        }
        public event Action<FrameEventArgs>? RenderFrame
        {
            add => _window.RenderFrame += value;
            remove => _window.RenderFrame -= value;
        }

        public event Action<FrameEventArgs>? UpdateFrame
        {
            add => _window.UpdateFrame += value;
            remove => _window.UpdateFrame -= value;
        }

        public event Action<CancelEventArgs>? Closing
        {
            add => _window.Closing += value;
            remove => _window.Closing -= value;
        }

        public event Action<FocusedChangedEventArgs>? FocusedChanged
        {
            add => _window.FocusedChanged += value;
            remove => _window.FocusedChanged -= value;
        }
        
        public event Action<ResizeEventArgs>? Resize
        {
            add => _window.Resize += value;
            remove => _window.Resize -= value;
        }
        
        public event Action? Load
        {
            add => _window.Load += value;
            remove => _window.Load -= value;
        }
        
        public event Action<TextInputEventArgs>? KeyPress
        {
            add => _window.TextInput += value;
            remove => _window.TextInput -= value;
        }
                
        public event Action<MouseWheelEventArgs>? MouseWheel
        {
            add => _window.MouseWheel += value;
            remove => _window.MouseWheel -= value;
        }

        public event Action<JoystickEventArgs> JoystickConnected
        {
            add => _window.JoystickConnected += value;
            remove=> _window.JoystickConnected -= value;
        }

        public void Dispose()
        {
            _window.Dispose();
        }
    }
}