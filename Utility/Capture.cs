using System;
namespace engenious.Utility
{
    /// <summary>
    /// Interface for capture classes to be able to call a generic function pointer with its data.
    /// </summary>
    public interface ICapture : IDisposable
    {
        /// <summary>
        /// Calls a function pointer using the capture data.
        /// </summary>
        /// <param name="funcPtr">The function pointer to call.</param>
        void Call(IntPtr funcPtr);
        /// <summary>
        /// Calls a function pointer using the capture data and returns its return value.
        /// </summary>
        /// <param name="funcPtr">The function pointer to call.</param>
        /// <typeparam name="T">The type of the delegates return value.</typeparam>
        /// <returns>The return value returned by the function call.</returns>
        T CallRet<T>(IntPtr funcPtr);
    }
    /// <summary>
    /// A memory poolable capture class without parameters as a placeholder for delegates without parameters.
    /// </summary>
    public class Capture : ICapture
    {
        /// <summary>
        /// A Capture instance for delegates without parameters.
        /// </summary>
        public static readonly Capture Instance = new Capture();
        unsafe void ICapture.Call(IntPtr funcPtr)
        {
            Call((delegate* <void>)funcPtr);
        }
        unsafe T ICapture.CallRet<T>(IntPtr funcPtr)
        {
            return CallRet<T>((delegate* <T>)funcPtr);
        }
        internal unsafe void Call(delegate* <void> funcPtr)
        {
            funcPtr();
        }
        internal unsafe T CallRet<T>(delegate* <T> funcPtr)
        {
            return funcPtr();
        }
        /// <inheritdoc />
        public void Dispose() { }
    }
    /// <summary>
    /// A memory poolable capture class.
    /// </summary>
    /// <typeparam name="T1">The type of the 2nd element to capture.</typeparam>
    public class Capture<T1> : ICapture
    {
        /// <inheritdoc />
        public void Dispose()
        {
            MemoryPool<Capture<T1>>.Release(this);
        }
        /// <summary>
        /// Gets the 1st captured element.
        /// </summary>
        public T1 Item1 { get; internal set; }
        unsafe void ICapture.Call(IntPtr funcPtr)
        {
            Call((delegate* <T1, void>)funcPtr);
        }
        unsafe T ICapture.CallRet<T>(IntPtr funcPtr)
        {
            return CallRet<T>((delegate* <T1, T>)funcPtr);
        }
        internal unsafe void Call(delegate* <T1, void> funcPtr)
        {
            funcPtr(Item1);
        }
        internal unsafe T CallRet<T>(delegate* <T1, T> funcPtr)
        {
            return funcPtr(Item1);
        }
    }
    /// <summary>
    /// A memory poolable capture class.
    /// </summary>
    /// <typeparam name="T1">The type of the 2nd element to capture.</typeparam>
    /// <typeparam name="T2">The type of the 3rd element to capture.</typeparam>
    public class Capture<T1, T2> : ICapture
    {
        /// <inheritdoc />
        public void Dispose()
        {
            MemoryPool<Capture<T1, T2>>.Release(this);
        }
        /// <summary>
        /// Gets the 1st captured element.
        /// </summary>
        public T1 Item1 { get; internal set; }
        /// <summary>
        /// Gets the 2nd captured element.
        /// </summary>
        public T2 Item2 { get; internal set; }
        unsafe void ICapture.Call(IntPtr funcPtr)
        {
            Call((delegate* <T1, T2, void>)funcPtr);
        }
        unsafe T ICapture.CallRet<T>(IntPtr funcPtr)
        {
            return CallRet<T>((delegate* <T1, T2, T>)funcPtr);
        }
        internal unsafe void Call(delegate* <T1, T2, void> funcPtr)
        {
            funcPtr(Item1, Item2);
        }
        internal unsafe T CallRet<T>(delegate* <T1, T2, T> funcPtr)
        {
            return funcPtr(Item1, Item2);
        }
    }
    /// <summary>
    /// A memory poolable capture class.
    /// </summary>
    /// <typeparam name="T1">The type of the 2nd element to capture.</typeparam>
    /// <typeparam name="T2">The type of the 3rd element to capture.</typeparam>
    /// <typeparam name="T3">The type of the 4th element to capture.</typeparam>
    public class Capture<T1, T2, T3> : ICapture
    {
        /// <inheritdoc />
        public void Dispose()
        {
            MemoryPool<Capture<T1, T2, T3>>.Release(this);
        }
        /// <summary>
        /// Gets the 1st captured element.
        /// </summary>
        public T1 Item1 { get; internal set; }
        /// <summary>
        /// Gets the 2nd captured element.
        /// </summary>
        public T2 Item2 { get; internal set; }
        /// <summary>
        /// Gets the 3rd captured element.
        /// </summary>
        public T3 Item3 { get; internal set; }
        unsafe void ICapture.Call(IntPtr funcPtr)
        {
            Call((delegate* <T1, T2, T3, void>)funcPtr);
        }
        unsafe T ICapture.CallRet<T>(IntPtr funcPtr)
        {
            return CallRet<T>((delegate* <T1, T2, T3, T>)funcPtr);
        }
        internal unsafe void Call(delegate* <T1, T2, T3, void> funcPtr)
        {
            funcPtr(Item1, Item2, Item3);
        }
        internal unsafe T CallRet<T>(delegate* <T1, T2, T3, T> funcPtr)
        {
            return funcPtr(Item1, Item2, Item3);
        }
    }
    /// <summary>
    /// A memory poolable capture class.
    /// </summary>
    /// <typeparam name="T1">The type of the 2nd element to capture.</typeparam>
    /// <typeparam name="T2">The type of the 3rd element to capture.</typeparam>
    /// <typeparam name="T3">The type of the 4th element to capture.</typeparam>
    /// <typeparam name="T4">The type of the 5th element to capture.</typeparam>
    public class Capture<T1, T2, T3, T4> : ICapture
    {
        /// <inheritdoc />
        public void Dispose()
        {
            MemoryPool<Capture<T1, T2, T3, T4>>.Release(this);
        }
        /// <summary>
        /// Gets the 1st captured element.
        /// </summary>
        public T1 Item1 { get; internal set; }
        /// <summary>
        /// Gets the 2nd captured element.
        /// </summary>
        public T2 Item2 { get; internal set; }
        /// <summary>
        /// Gets the 3rd captured element.
        /// </summary>
        public T3 Item3 { get; internal set; }
        /// <summary>
        /// Gets the 4th captured element.
        /// </summary>
        public T4 Item4 { get; internal set; }
        unsafe void ICapture.Call(IntPtr funcPtr)
        {
            Call((delegate* <T1, T2, T3, T4, void>)funcPtr);
        }
        unsafe T ICapture.CallRet<T>(IntPtr funcPtr)
        {
            return CallRet<T>((delegate* <T1, T2, T3, T4, T>)funcPtr);
        }
        internal unsafe void Call(delegate* <T1, T2, T3, T4, void> funcPtr)
        {
            funcPtr(Item1, Item2, Item3, Item4);
        }
        internal unsafe T CallRet<T>(delegate* <T1, T2, T3, T4, T> funcPtr)
        {
            return funcPtr(Item1, Item2, Item3, Item4);
        }
    }
    public partial struct CapturingDelegate
    {
        public static unsafe CapturingDelegate Create(delegate*<void> functionPointer)
        {
            return new (Utility.Capture.Instance, (IntPtr)functionPointer);
        }
        public static unsafe CapturingDelegate CreateRet<TRet>(delegate*<TRet> functionPointer)
        {
            return new (Utility.Capture.Instance, (IntPtr)functionPointer);
        }
        private static Capture<T1> CreateCaptureWith<T1>(T1 item1)
        {
            var capture = MemoryPool<Capture<T1>>.Acquire();
            capture.Item1 = item1;
            return capture;
        }
        public static unsafe CapturingDelegate Create<T1>(delegate*<T1, void> functionPointer, T1 item1)
        {
            return new (CreateCaptureWith(item1), (IntPtr)functionPointer);
        }
        public static unsafe CapturingDelegate CreateRet<T1, TRet>(delegate*<T1, TRet> functionPointer, T1 item1)
        {
            return new (CreateCaptureWith(item1), (IntPtr)functionPointer);
        }
        private static Capture<T1, T2> CreateCaptureWith<T1, T2>(T1 item1, T2 item2)
        {
            var capture = MemoryPool<Capture<T1, T2>>.Acquire();
            capture.Item1 = item1;
            capture.Item2 = item2;
            return capture;
        }
        public static unsafe CapturingDelegate Create<T1, T2>(delegate*<T1, T2, void> functionPointer, T1 item1, T2 item2)
        {
            return new (CreateCaptureWith(item1, item2), (IntPtr)functionPointer);
        }
        public static unsafe CapturingDelegate CreateRet<T1, T2, TRet>(delegate*<T1, T2, TRet> functionPointer, T1 item1, T2 item2)
        {
            return new (CreateCaptureWith(item1, item2), (IntPtr)functionPointer);
        }
        private static Capture<T1, T2, T3> CreateCaptureWith<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            var capture = MemoryPool<Capture<T1, T2, T3>>.Acquire();
            capture.Item1 = item1;
            capture.Item2 = item2;
            capture.Item3 = item3;
            return capture;
        }
        public static unsafe CapturingDelegate Create<T1, T2, T3>(delegate*<T1, T2, T3, void> functionPointer, T1 item1, T2 item2, T3 item3)
        {
            return new (CreateCaptureWith(item1, item2, item3), (IntPtr)functionPointer);
        }
        public static unsafe CapturingDelegate CreateRet<T1, T2, T3, TRet>(delegate*<T1, T2, T3, TRet> functionPointer, T1 item1, T2 item2, T3 item3)
        {
            return new (CreateCaptureWith(item1, item2, item3), (IntPtr)functionPointer);
        }
        private static Capture<T1, T2, T3, T4> CreateCaptureWith<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            var capture = MemoryPool<Capture<T1, T2, T3, T4>>.Acquire();
            capture.Item1 = item1;
            capture.Item2 = item2;
            capture.Item3 = item3;
            capture.Item4 = item4;
            return capture;
        }
        public static unsafe CapturingDelegate Create<T1, T2, T3, T4>(delegate*<T1, T2, T3, T4, void> functionPointer, T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new (CreateCaptureWith(item1, item2, item3, item4), (IntPtr)functionPointer);
        }
        public static unsafe CapturingDelegate CreateRet<T1, T2, T3, T4, TRet>(delegate*<T1, T2, T3, T4, TRet> functionPointer, T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new (CreateCaptureWith(item1, item2, item3, item4), (IntPtr)functionPointer);
        }
    }
}