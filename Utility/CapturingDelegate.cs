using System;

namespace engenious.Utility
{
    /// <summary>
    /// A delegate call with captured data.
    /// </summary>
    public readonly partial struct CapturingDelegate
    {
        private CapturingDelegate(ICapture capture, IntPtr functionPointer)
        {
            Capture = capture;
            FunctionPointer = functionPointer;
        }
        /// <summary>
        /// The captured data to pass to the function on call.
        /// </summary>
        public ICapture Capture { get; }
        /// <summary>
        /// The function pointer to call.
        /// </summary>
        public IntPtr FunctionPointer { get; }

        /// <summary>
        /// Call the function with the captured data.
        /// </summary>
        public void Call()
        {
            Capture.Call(FunctionPointer);
        }

        /// <summary>
        /// Call the function with the captured data and return the return value.
        /// </summary>
        /// <returns>The return value of the called function.</returns>
        public T CallRet<T>()
        {
            return Capture.CallRet<T>(FunctionPointer);
        }
    }
}