using System;

namespace engenious.Helper
{
    /// <summary>
    /// A math helper class.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// The pi constant.
        /// </summary>
        public const float Pi = (float)(Math.PI);

        /// <summary>
        /// The pi constant divided by 2.
        /// </summary>
        public const float PiOver2 = (float)(Math.PI / 2);

        /// <summary>
        /// The pi constant divided by 4.
        /// </summary>
        public const float PiOver4 = (float)(Math.PI / 4);

        /// <summary>
        /// The pi constant multiplied by 2.
        /// </summary>
        public const float TwoPi = (float)(2 * Math.PI);

        /// <summary>
        /// Wraps an angle so that it is between -<see cref="Pi"/> and +<see cref="Pi"/> radians.
        /// </summary>
        /// <param name="angle">The angle to wrap[radians].</param>
        /// <returns>The wrapped angle in radians.</returns>
        public static float WrapAngle(float angle)
        {
            angle = (float)Math.IEEERemainder(angle, 6.2831854820251465);
            if (angle <= -3.14159274f)
            {
                angle += 6.28318548f;
            }
            else
            {
                if (angle > 3.14159274f)
                {
                    angle -= 6.28318548f;
                }
            }
            return angle;
        }

        /// <summary>
        /// Converts given degrees to radian angles.
        /// </summary>
        /// <param name="degrees">The degree angle to convert.</param>
        /// <returns>The resulting angle in radians.</returns>
        public static float ToRadians(float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        /// <summary>
        /// Converts given radian to degree angles.
        /// </summary>
        /// <param name="radian">The radian angle to convert.</param>
        /// <returns>The resulting angle in degree.</returns>
        public static float ToDegree(float radian)
        {
            return (float)(radian * 180 / Math.PI);
        }

        /// <summary>
        /// Clamps a value to the given min and max values.
        /// </summary>
        /// <param name="val">The value to clamp.</param>
        /// <param name="min">The minimum to clamp to.</param>
        /// <param name="max">The maximum to clamp to.</param>
        /// <returns>The clamped result.</returns>
        public static float Clamp(float val, float min, float max)
        {
            return val > max ? max : (val < min ? min : val);
        }
    }
}

