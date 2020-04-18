namespace engenious.Input
{
    /// <summary>
    /// Static class to get gamepad input.
    /// </summary>
    public class GamePad
    {
        /// <summary>
        /// Get the current gamepad state by a given gamepad index.
        /// </summary>
        /// <param name="index">The index of the gamepad to get the state of.</param>
        /// <returns>The current gamepad state of the gamepad specified by <paramref name="index"/>.</returns>
        public static unsafe GamePadState GetState(int index = 0)
        {
            var state = OpenTK.Input.GamePad.GetState(index);

            var actual = *(GamePadState*)&state;
            //TODO:actual.ThumbSticks.Left = new Vector2(Math.M
            return actual;
        }
    }
}

