﻿// Copyright (c) 2013-2015 Jae-jun Kang
// See the file LICENSE for details.

using System;
using System.Collections.Generic;
using System.Threading;

namespace x2
{
    public class SingleThreadFlow : EventBasedFlow
    {
        protected Thread thread;

        public SingleThreadFlow()
            : this(new UnboundedQueue<Event>())
        {
        }
        
        public SingleThreadFlow(IQueue<Event> queue)
            : base(queue)
        {
            thread = null;
        }

        public SingleThreadFlow(string name)
            : this(name, new UnboundedQueue<Event>())
        {
        }

        public SingleThreadFlow(string name, IQueue<Event> queue)
            : this(queue)
        {
            this.name = name;
        }

        public override Flow StartUp()
        {
            lock (syncRoot)
            {
                if (thread == null)
                {
                    SetUp();
                    caseStack.SetUp(this);
                    thread = new Thread(Run);
                    thread.Name = name;
                    thread.Start();
                    queue.Enqueue(new FlowStart());
                }
            }
            return this;
        }

        public override void ShutDown()
        {
            lock (syncRoot)
            {
                if (thread == null)
                {
                    return;
                }
                queue.Close(new FlowStop());
                thread.Join();
                thread = null;

                caseStack.TearDown(this);
                TearDown();
            }
        }

        private void Run()
        {
            currentFlow = this;
            equivalent = new EventEquivalent();
            events = new List<Event>();
            handlerChain = new List<Handler>();

            while (true)
            {
                queue.Dequeue(events);
                if (events.Count == 0)
                {
                    break;
                }
                for (int i = 0, count = events.Count; i < count; ++i)
                {
                    Dispatch(events[i]);
                }
                events.Clear();
            }

            handlerChain = null;
            events = null;
            equivalent = null;
            currentFlow = null;
        }
    }
}
