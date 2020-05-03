﻿using System;
using System.Runtime.CompilerServices;

namespace Aeon.Emulator.Instructions.FPU
{
    internal static class Fcos
    {
        [Opcode("D9FF", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Cosine(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            st0 = Math.Cos(st0);
        }
    }
}
