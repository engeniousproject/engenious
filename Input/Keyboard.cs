using System;
using engenious.Helper;

namespace engenious.Input
{
    /// <summary>
    /// Static class to get keyboard input.
    /// </summary>
    public static class Keyboard
    {
        static Keyboard()
        {
            if (!WrappingHelper.ValidateStructs<OpenTK.Input.KeyboardState,KeyboardState>())
                throw new TypeLoadException("Can't wrap OpenTK Keyboard two own internal struct");
        }

        /// <summary>
        /// Get the current keyboard state.
        /// </summary>
        /// <returns>The current keyboard state.</returns>
        public static unsafe KeyboardState GetState()
        {
            var state = OpenTK.Input.Keyboard.GetState();
            return *(KeyboardState*)(&state);


        }

        /// <summary>
        /// Get the current keyboard state by a given keyboard index.
        /// </summary>
        /// <param name="index">The index of the keyboard to get the state of.</param>
        /// <returns>The current keyboard state of the keyboard specified by <paramref name="index"/>.</returns>
        public static unsafe KeyboardState GetState(int index)
        {
            var state = OpenTK.Input.Keyboard.GetState(index);
            return *(KeyboardState*)(&state);


        }
    }
}

