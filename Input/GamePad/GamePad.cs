using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace engenious.Input
{
    /// <summary>
    /// Static class to get gamepad input.
    /// </summary>
    public class GamePad
    {
        private static IRenderingSurface _window = null!;

        internal static void UpdateWindow<TControl>(TControl window)
            where TControl : class, IRenderingSurface
        {
            _window = window;
        }
        /// <summary>
        /// Get the current gamepad state by a given gamepad index.
        /// </summary>
        /// <param name="index">The index of the gamepad to get the state of.</param>
        /// <returns>The current gamepad state of the gamepad specified by <paramref name="index"/>.</returns>
        public static unsafe GamePadState GetState(int index = 0)
        {
            GLFW.PollEvents();

            var isGamePad = GLFW.JoystickIsGamepad(index);

            var actual = new GamePadState();
            actual.SetName(isGamePad ? GLFW.GetGamepadName(index) : GLFW.GetJoystickName(index));
            actual.SetIsGamePad(isGamePad);

            var state = _window.WindowInfo!.JoystickStates.ElementAtOrDefault(index);

            if (state is null)
                return actual;

            var snapshot = state.GetSnapshot();

            actual.SetConnected(GLFW.JoystickPresent(index));

            // Buttons
            for (int i = 0; i < snapshot.ButtonCount; i++)
            {
                actual.SetButton((Buttons)(1 << i), snapshot.IsButtonDown(i));
            }

            // Axis
            for (int i = 0; i < snapshot.AxisCount; i++)
            {
                actual.SetAxis((GamePadAxes)(1 << i), snapshot.GetAxis(i));
            }

            // Hat
            var hats = new ArraySegment<GamePadHat>(ArrayPool<GamePadHat>.Shared.Rent(snapshot.HatCount), 0, snapshot.HatCount);
            for (int i = 0; i < snapshot.HatCount; i++)
            {
                hats[i] = translateHat(snapshot.GetHat(i));

                static GamePadHat translateHat(OpenTK.Windowing.Common.Input.Hat h)
                {
                    return h switch
                    {
                        OpenTK.Windowing.Common.Input.Hat.Centered => GamePadHat.Centered,
                        OpenTK.Windowing.Common.Input.Hat.Up => GamePadHat.Up,
                        OpenTK.Windowing.Common.Input.Hat.Right => GamePadHat.Right,
                        OpenTK.Windowing.Common.Input.Hat.Down => GamePadHat.Down,
                        OpenTK.Windowing.Common.Input.Hat.Left => GamePadHat.Left,
                        OpenTK.Windowing.Common.Input.Hat.RightUp => GamePadHat.RightUp,
                        OpenTK.Windowing.Common.Input.Hat.RightDown => GamePadHat.RightDown,
                        OpenTK.Windowing.Common.Input.Hat.LeftUp => GamePadHat.LeftUp,
                        OpenTK.Windowing.Common.Input.Hat.LeftDown => GamePadHat.LeftDown,
                    };
                };
                actual.SetHats(hats);
            }

            return actual;
        }
    }
}

