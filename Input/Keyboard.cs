using System;

namespace engenious.Input
{
    public static class Keyboard
    {
        static Keyboard()
        {
            if (!WrappingHelper.ValidateStructs<OpenTK.Input.KeyboardState,KeyboardState>())
                throw new Exception("test");
        }
        public unsafe static KeyboardState GetState()
        {
            OpenTK.Input.KeyboardState state = OpenTK.Input.Keyboard.GetState();
            return *(KeyboardState*)(&state);


        }
        public unsafe static KeyboardState GetState(int index)
        {
            OpenTK.Input.KeyboardState state = OpenTK.Input.Keyboard.GetState(index);
            return *(KeyboardState*)(&state);


        }
    }
}

