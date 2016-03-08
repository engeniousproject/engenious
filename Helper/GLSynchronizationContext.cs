using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace engenious
{
    sealed class GLSynchronizationContext : SynchronizationContext
    {
        BlockingCollection<KeyValuePair<SendOrPostCallback,object>> queue = new BlockingCollection<KeyValuePair<SendOrPostCallback,object>>();

        public override void Post(SendOrPostCallback d, object state)
        {
            queue.Add(new KeyValuePair<SendOrPostCallback,object>(d, state));
        }

        public void RunOnCurrentThread()
        {

            KeyValuePair<SendOrPostCallback, object> workItem;

            while (queue.Count > 0 && queue.TryTake(out workItem, Timeout.Infinite))
                workItem.Key(workItem.Value);

        }
    }
}

