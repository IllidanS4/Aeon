﻿using System.Runtime.CompilerServices;

namespace Aeon.Emulator.Instructions.FPU
{
    internal static class Fdivr
    {
        [Opcode("D8/7 mf32", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide32(VirtualMachine vm, float value)
        {
            ReverseDivide64(vm, value);
        }

        [Opcode("DC/7 mf64|D8F8+ st|DCF0 st0", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide64(VirtualMachine vm, double value)
        {
            ref var st0 = ref vm.Processor.FPU.ST0_Ref;
            st0 = value / st0;
        }

        [Opcode("DCF1", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide1(VirtualMachine vm)
        {
            ref var value = ref vm.Processor.FPU.GetRegisterRef(1);
            value = vm.Processor.FPU.ST0_Ref / value;
        }
        [Opcode("DCF2", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide2(VirtualMachine vm)
        {
            ref var value = ref vm.Processor.FPU.GetRegisterRef(2);
            value = vm.Processor.FPU.ST0_Ref / value;
        }
        [Opcode("DCF3", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide3(VirtualMachine vm)
        {
            ref var value = ref vm.Processor.FPU.GetRegisterRef(3);
            value = vm.Processor.FPU.ST0_Ref / value;
        }
        [Opcode("DCF4", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide4(VirtualMachine vm)
        {
            ref var value = ref vm.Processor.FPU.GetRegisterRef(4);
            value = vm.Processor.FPU.ST0_Ref / value;
        }
        [Opcode("DCF5", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide5(VirtualMachine vm)
        {
            ref var value = ref vm.Processor.FPU.GetRegisterRef(5);
            value = vm.Processor.FPU.ST0_Ref / value;
        }
        [Opcode("DCF6", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide6(VirtualMachine vm)
        {
            ref var value = ref vm.Processor.FPU.GetRegisterRef(6);
            value = vm.Processor.FPU.ST0_Ref / value;
        }
        [Opcode("DCF7", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide7(VirtualMachine vm)
        {
            ref var value = ref vm.Processor.FPU.GetRegisterRef(7);
            value = vm.Processor.FPU.ST0_Ref / value;
        }
    }

    internal static class Fdivrp
    {
        [Opcode("DEF0", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide0(VirtualMachine vm)
        {
            vm.Processor.FPU.Pop();
        }

        [Opcode("DEF1", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide1(VirtualMachine vm)
        {
            Fdivr.ReverseDivide1(vm);
            vm.Processor.FPU.Pop();
        }
        [Opcode("DEF2", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide2(VirtualMachine vm)
        {
            Fdivr.ReverseDivide2(vm);
            vm.Processor.FPU.Pop();
        }
        [Opcode("DEF3", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide3(VirtualMachine vm)
        {
            Fdivr.ReverseDivide3(vm);
            vm.Processor.FPU.Pop();
        }
        [Opcode("DEF4", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide4(VirtualMachine vm)
        {
            Fdivr.ReverseDivide4(vm);
            vm.Processor.FPU.Pop();
        }
        [Opcode("DEF5", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide5(VirtualMachine vm)
        {
            Fdivr.ReverseDivide5(vm);
            vm.Processor.FPU.Pop();
        }
        [Opcode("DEF6", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide6(VirtualMachine vm)
        {
            Fdivr.ReverseDivide6(vm);
            vm.Processor.FPU.Pop();
        }
        [Opcode("DEF7", OperandSize = 16 | 32, AddressSize = 16 | 32)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseDivide7(VirtualMachine vm)
        {
            Fdivr.ReverseDivide7(vm);
            vm.Processor.FPU.Pop();
        }
    }
}
