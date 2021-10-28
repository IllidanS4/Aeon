﻿using System;
using System.Runtime.CompilerServices;

namespace Aeon.Emulator.Instructions.FPU
{
    internal static class Fyl2x
    {
        [Opcode("D9F1", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void ScaleLog2(Processor p)
        {
            ref var st1 = ref p.FPU.GetRegisterRef(1);

            // ST(1) = ST(1) * Log2(ST(0))
            st1 *= Compatibility.MathLog2(p.FPU.ST0_Ref);
            p.FPU.Pop();
        }
    }
}
