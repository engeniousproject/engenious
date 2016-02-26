using System;

namespace engenious
{
    public class MouseDevice
    {
        OpenTK.Input.MouseDevice dev;

        public MouseDevice(OpenTK.Input.MouseDevice dev)
        {
            this.dev = dev;
        }

        public int X{ get { return dev.X; } }

        public int Y{ get { return dev.Y; } }
    }
}

