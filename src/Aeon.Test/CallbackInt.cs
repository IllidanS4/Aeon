﻿using System;
using System.Collections.Generic;
using System.IO;
using Aeon.Emulator;

namespace Aeon.Test
{
    public class CallbackInt : IInterruptHandler
    {
        private readonly int interrupt;
        private readonly Action callback;

        public CallbackInt(int interrupt, Action callback)
        {
            this.interrupt = interrupt;
            this.callback = callback;
        }

        public IEnumerable<InterruptHandlerInfo> HandledInterrupts => new InterruptHandlerInfo[] { this.interrupt };

        public void HandleInterrupt(int interrupt) => this.callback();

        void IDisposable.Dispose()
        {
        }
    }
}
