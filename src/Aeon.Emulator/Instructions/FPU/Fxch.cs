﻿using System.Runtime.CompilerServices;

namespace Aeon.Emulator.Instructions.FPU
{
    internal static class Fxch
    {
        [Opcode("D9C8", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        public static void Exchange0(VirtualMachine vm)
        {
        }

        [Opcode("D9C9", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void Exchange1(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            ref var st1 = ref p.FPU.GetRegisterRef(1);
            double temp = st0;
            st0 = st1;
            st1 = temp;
        }

        [Opcode("D9CA", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void Exchange2(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            ref var st2 = ref p.FPU.GetRegisterRef(2);
            double temp = st0;
            st0 = st2;
            st2 = temp;
        }

        [Opcode("D9CB", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void Exchange3(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            ref var st3 = ref p.FPU.GetRegisterRef(3);
            double temp = st0;
            st0 = st3;
            st3 = temp;
        }

        [Opcode("D9CC", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void Exchange4(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            ref var st4 = ref p.FPU.GetRegisterRef(4);
            double temp = st0;
            st0 = st4;
            st4 = temp;
        }

        [Opcode("D9CD", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void Exchange5(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            ref var st5 = ref p.FPU.GetRegisterRef(5);
            double temp = st0;
            st0 = st5;
            st5 = temp;
        }

        [Opcode("D9CE", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void Exchange6(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            ref var st6 = ref p.FPU.GetRegisterRef(6);
            double temp = st0;
            st0 = st6;
            st6 = temp;
        }

        [Opcode("D9CF", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | Compatibility.AggressiveOptimization)]
        public static void Exchange7(Processor p)
        {
            ref var st0 = ref p.FPU.ST0_Ref;
            ref var st7 = ref p.FPU.GetRegisterRef(7);
            double temp = st0;
            st0 = st7;
            st7 = temp;
        }
    }
}
