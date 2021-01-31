using System.Collections.Generic;
using System.Linq;
using System.Threading;
using engenious.Utility;

namespace engenious.Helper
{
    public sealed class GlSynchronizationContext : SynchronizationContext
    {
        private Thread _currentThread;

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
        public override unsafe void Send(SendOrPostCallback callback, object state)
        {
            if (Thread.CurrentThread == _currentThread)
            {
                callback(state);
            }
            else
            {
                Add(CapturingDelegate.Create<SendOrPostCallback, object>(&SendOrPostCallbackCaller, callback, state), true, true).WaitOne();
            }
        }
        public override unsafe void Post(SendOrPostCallback callback, object state)
        {
            Add(CapturingDelegate.Create<SendOrPostCallback, object>(&SendOrPostCallbackCaller, callback, state), true);
        }
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
                    int newcapacity = (int)((long)_array.Count * 2);
                    if (newcapacity < _array.Count + 4)
                    {
                        newcapacity = _array.Count + 4;
                    }

                    int added = newcapacity - _array.Capacity;
                    _array.Capacity = newcapacity;
                    _array.InsertRange(_head, Enumerable.Repeat(new CallbackState(false), newcapacity / 2));
         
                    _tail = _head; // 178, 
                    _head += newcapacity / 2;
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

        public void WaitForWork()
        {
            _resetEvent.WaitOne();
        }

        internal void CancelWait()
        {
            _resetEvent.Set();
        }
    }
}

