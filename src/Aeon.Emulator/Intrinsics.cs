using System.Runtime.CompilerServices;

namespace Aeon.Emulator
{
    public static class Intrinsics
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ExtractBits(uint value, byte start, byte length, uint mask)
        {
            if (Compatibility.Bmi1IsSupportedDirect)
                return Compatibility.Bmi1BitFieldExtract(value, start, length);
            else
                return (value & mask) >> start;
        }
    }
}
