using System;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;

namespace engenious.Input
{
    public static class Mouse
    {

        private static OpenTK.GameWindow window;
        static float deltaPrecise=0;
        static Mouse()
        {
            if (!WrappingHelper.ValidateStructs<OpenTK.Input.MouseState,MouseState>())
                throw new Exception("test");
        }

        internal static void UpdateWindow(OpenTK.GameWindow window)
        {
            Mouse.window = window;
            window.MouseWheel += delegate(object sender, OpenTK.Input.MouseWheelEventArgs e)
            {
                deltaPrecise+=(e.DeltaPrecise);
            };
        }
        public static unsafe MouseState GetState(int index)
        {
            OpenTK.Input.MouseState state = OpenTK.Input.Mouse.GetState(index);
            MouseState actual = *(MouseState*)(&state);
            
            actual.X = window.Mouse.X;
            actual.Y = window.Mouse.Y;
            return actual;

        }
        public static unsafe MouseState GetState()
        {
            OpenTK.Input.MouseState state = OpenTK.Input.Mouse.GetState();
            MouseState actual = *(MouseState*)(&state);
            actual.X = window.Mouse.X;
            actual.Y = window.Mouse.Y;
            //actual.Scroll = new MouseScroll(actual.Scroll.X,deltaPrecise);//.Y = ;
            deltaPrecise = 0;
            return actual;

        }

        public static void SetPosition(double x, double y)
        {
            
            var pt = window.PointToScreen(new System.Drawing.Point((int)x, (int)y));
            x -= (int)x;
            y -= (int)y;
            OpenTK.Input.Mouse.SetPosition(pt.X + x, pt.Y + y);
        }
    }
}

