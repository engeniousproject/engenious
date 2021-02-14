using System.Collections.Generic;
using System.Linq;
using System.Threading;
using engenious.Utility;

namespace engenious.Helper
{
    /// <summary>
    /// A synchronization context for opengl commands.
    /// </summary>
    public sealed class GlSynchronizationContext : SynchronizationContext
    {
        private Thread? _currentThread;

        private struct CallbackState
        {
            public CallbackState(bool doWait)
            {
                WaitHandle = new AutoResetEvent(false);
                DoWait = false;
                HasOwnership = false;
                Callback = default;
            }
            public CapturingDelegate Callback;
            public AutoResetEvent WaitHandle;
            public bool HasOwnership;
            public bool DoWait;
        }

        private readonly object _syncRoot = new();

        private int _count, _head, _tail;
        private readonly List<CallbackState> _array = new();
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);

        /// <inheritdoc />
        public GlSynchronizationContext()
        {
            for (int i = 0; i < 128; i++)
            {
                _array.Add(new CallbackState(false));
            }
        }

        private static void SendOrPostCallbackCaller(SendOrPostCallback callback, object state)
        {
            callback(state);
        }

        /// <inheritdoc />
        public override unsafe void Send(SendOrPostCallback callback, object? state)
        {
            if (Thread.CurrentThread == _currentThread)
            {
                callback(state);
            }
            else
            {
                Add(CapturingDelegate.Create<SendOrPostCallback, object>(&SendOrPostCallbackCaller, callback, state!), true, true).WaitOne();
            }
        }

        /// <inheritdoc />
        public override unsafe void Post(SendOrPostCallback callback, object? state)
        {
            Add(CapturingDelegate.Create<SendOrPostCallback, object>(&SendOrPostCallbackCaller, callback, state!), true);
        }
        
        /// <summary>
        /// Send a <see cref="CapturingDelegate"/> in the worker queue to execute synchronized in this context
        /// and waits for it to be completed.
        /// </summary>
        /// <param name="callback">The callback to call in this context.</param>
        /// <param name="passOwnership">
        /// Whether to pass the ownership of the <paramref name="callback"/>.
        /// <remarks>If you do not pass this ownership you need to cleanup the delegate.</remarks>
        /// </param>
        public void Send(CapturingDelegate callback, bool passOwnership)
        {
            if (Thread.CurrentThread == _currentThread)
            {
                callback.Call();
                if (passOwnership)
                    callback.Capture.Dispose();
            }
            else
            {
                Add(callback, passOwnership, true).WaitOne();
            }
        }

        /// <summary>
        /// Posts a <see cref="CapturingDelegate"/> in the worker queue to execute synchronized in this context.
        /// </summary>
        /// <param name="callback">The callback to call in this context.</param>
        /// <param name="passOwnership">
        /// Whether to pass the ownership of the <paramref name="callback"/>.
        /// <remarks>If you do not pass this ownership you need to cleanup the delegate.</remarks>
        /// </param>
        /// <returns>
        /// A reset event that gets set when the callback got executed.
        /// <remarks>This <see cref="AutoResetEvent"/> gets reused and therefore automatically gets invalid after it was set once.</remarks>
        /// </returns>
        public AutoResetEvent Post(CapturingDelegate callback, bool passOwnership)
        {
            return Add(callback, passOwnership, true);
        }
        private AutoResetEvent Add(CapturingDelegate callback, bool passOwnership, bool wait = false)
        {
            lock (_syncRoot)
            {
                if (_count == _array.Count)
                {
                    int newCapacity = (int)((long)_array.Count * 2);
                    if (newCapacity < _array.Count + 4)
                    {
                        newCapacity = _array.Count + 4;
                    }

                    int added = newCapacity - _array.Capacity;
                    _array.Capacity = newCapacity;
                    _array.InsertRange(_head, Enumerable.Repeat(new CallbackState(false), newCapacity / 2));
         
                    _tail = _head; // 178, 
                    _head += newCapacity / 2;
                }
                var item = _array[_tail];
                item.Callback = callback;
                item.DoWait = wait;
                item.HasOwnership = passOwnership;
                _array[_tail] = item;
                _tail = (_tail + 1) % _array.Count;
                _count++;

                _resetEvent.Set();
                return item.WaitHandle;
            }
        }

        private bool TryTake(ref CallbackState callbackState)
        {
            if (_count == 0)
                return false;
            lock (_syncRoot)
            {
                if (_count == 0)
                    return false;
                callbackState = _array[_head];
                _head = (_head + 1) % _array.Count;
                _count--;
                return true;
            }
        }
        /// <summary>
        /// Executes all pending work on the current thread and returns afterwards.
        /// </summary>
        public void RunOnCurrentThread()
        {
            _currentThread = Thread.CurrentThread;
            var workItem = default(CallbackState);
            while (TryTake(ref workItem))
            {
                var callbackState = workItem;

                callbackState.Callback.Call();
                if (callbackState.DoWait)
                    callbackState.WaitHandle.Set();
                if (callbackState.HasOwnership)
                    callbackState.Callback.Capture.Dispose();
            }

        }
        
        internal void WaitForWork()
        {
            _resetEvent.WaitOne();
        }

        internal void CancelWait()
        {
            _resetEvent.Set();
        }
    }
}

