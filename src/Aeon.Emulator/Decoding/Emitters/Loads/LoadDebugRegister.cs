using System;
using System.Reflection.Emit;

namespace Aeon.Emulator.Decoding.Emitters
{
    internal sealed class LoadDebugRegister : Emitter
    {
        public LoadDebugRegister(EmitStateInfo state)
            : base(state)
        {
        }

        public override Type MethodArgType => this.ReturnType == EmitReturnType.Value ? typeof(uint) : typeof(uint).MakePointerType();

        public override void EmitLoad()
        {
            //// Reg is the middle 3 bits of the ModR/M byte.
            //int reg = (*ip & 0x38) >> 3;

            LoadProcessor();

            //il.LoadLocal(ipPointer);
            LoadIPPointer();

            il.Emit(OpCodes.Ldind_U1);
            if (Compatibility.Bmi1IsSupported)
            {
                il.LoadConstant(0x0303);
                il.Emit(OpCodes.Call, Infos.Intrinsics.BitFieldExtract);
            }
            else
            {
                il.LoadConstant(0x38);
                il.Emit(OpCodes.And);
                il.LoadConstant(3);
                il.Emit(OpCodes.Shr_Un);
            }

            il.Emit(OpCodes.Call, Infos.Processor.GetDebugRegisterPointer);
            if (this.ReturnType == EmitReturnType.Value)
                il.Emit(OpCodes.Ldind_U4);
        }
    }
}
