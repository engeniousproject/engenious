﻿using System;

namespace engenious.Input
{
    public class GamePad
    {
        public GamePad()
        {
            
        }

        public unsafe static GamePadState GetState(int index = 0)
        {
            OpenTK.Input.GamePadState state = OpenTK.Input.GamePad.GetState(index);

            GamePadState actual = *(GamePadState*)&state;

            return actual;
        }
    }
}

