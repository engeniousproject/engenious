using System;

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
            //TODO:actual.ThumbSticks.Left = new Vector2(Math.M
            return actual;
        }
    }
}

