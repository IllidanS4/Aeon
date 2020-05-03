﻿using System;
using System.Runtime.CompilerServices;

namespace Aeon.Emulator.Instructions.FPU
{
    internal static class Fscale
    {
        [Opcode("D9FD", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
        public static void Scale(VirtualMachine vm)
        {
            var fpu = vm.Processor.FPU;
            ref var st0 = ref fpu.ST0_Ref;
            st0 = Math.ScaleB(st0, (int)fpu.GetRegisterRef(1));
        }
    }
}
