﻿using System;

namespace Aeon.Emulator.Video
{
    [Flags]
    internal enum SequencerMemoryMode : byte
    {
        None = 0,
        ExtendedMemory = 2,
        OddEvenWriteAddressingDisabled = 4,
        Chain4 = 8
    }
}
