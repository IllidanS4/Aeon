using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aeon.Emulator
{
    internal static class Compatibility
    {
        public const MethodImplOptions AggressiveOptimization = (MethodImplOptions)512;

        public static readonly bool Bmi1IsSupported =
            Type.GetType("System.Runtime.Intrinsics.X86.Bmi1", false)
            ?.GetProperty("IsSupported", BindingFlags.Public | BindingFlags.Static)
            ?.GetValue(null) is bool b ? b : false;


        public const bool Bmi1IsSupportedDirect = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Bmi1BitFieldExtract(uint value, byte start, byte length)
        {
            return Bmi1BitFieldExtract(value, (ushort)(start | (length << 8)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Bmi1BitFieldExtract(uint value, ushort control)
        {
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MathScaleB(double x, int n)
        {
            return x * Math.Pow(2, n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MathLog2(double d)
        {
            return Math.Log(d, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MathClamp(int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long MathClamp(long value, long min, long max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static readonly Encoding EncodingLatin1 = Encoding.GetEncoding("ISO-8859-1");

        public static void Write(this Stream stream, ArraySegment<byte> segment)
        {
            if(segment.Count > 0)
            {
                stream.Write(segment.Array, segment.Offset, segment.Count);
            }
        }

        public static void Write(this Stream stream, ReadOnlySpan<byte> span)
        {
            switch(span.Length)
            {
                case 0:
                    break;
                case 1:
                    stream.WriteByte(span[0]);
                    break;
                case 2:
                    stream.WriteByte(span[0]);
                    stream.WriteByte(span[1]);
                    break;
                case 3:
                    stream.WriteByte(span[0]);
                    stream.WriteByte(span[1]);
                    stream.WriteByte(span[2]);
                    break;
                case 4:
                    stream.WriteByte(span[0]);
                    stream.WriteByte(span[1]);
                    stream.WriteByte(span[2]);
                    stream.WriteByte(span[3]);
                    break;
                case 5:
                    stream.WriteByte(span[0]);
                    stream.WriteByte(span[1]);
                    stream.WriteByte(span[2]);
                    stream.WriteByte(span[3]);
                    stream.WriteByte(span[4]);
                    break;
                case 6:
                    stream.WriteByte(span[0]);
                    stream.WriteByte(span[1]);
                    stream.WriteByte(span[2]);
                    stream.WriteByte(span[3]);
                    stream.WriteByte(span[4]);
                    stream.WriteByte(span[5]);
                    break;
                case 7:
                    stream.WriteByte(span[0]);
                    stream.WriteByte(span[1]);
                    stream.WriteByte(span[2]);
                    stream.WriteByte(span[3]);
                    stream.WriteByte(span[4]);
                    stream.WriteByte(span[5]);
                    stream.WriteByte(span[6]);
                    break;
                case 8:
                    stream.WriteByte(span[0]);
                    stream.WriteByte(span[1]);
                    stream.WriteByte(span[2]);
                    stream.WriteByte(span[3]);
                    stream.WriteByte(span[4]);
                    stream.WriteByte(span[5]);
                    stream.WriteByte(span[6]);
                    stream.WriteByte(span[7]);
                    break;
                default:
                    var arr = ArrayPool<byte>.Shared.Rent(span.Length);
                    try
                    {
                        span.CopyTo(arr);
                        stream.Write(arr, 0, span.Length);
                    } finally
                    {
                        ArrayPool<byte>.Shared.Return(arr);
                    }
                    break;
            }
        }

        public static int Read(this Stream stream, Span<byte> span)
        {
            int read;
            switch(span.Length)
            {
                case 0:
                    return 0;
                case 1:
                    read = stream.ReadByte();
                    if(read == -1) return 0;
                    span[0] = (byte)read;
                    return 1;
                case 2:
                    read = stream.ReadByte();
                    if(read == -1) return 0;
                    span[0] = (byte)read;
                    read = stream.ReadByte();
                    if(read == -1) return 1;
                    span[1] = (byte)read;
                    return 2;
                case 3:
                    read = stream.ReadByte();
                    if(read == -1) return 0;
                    span[0] = (byte)read;
                    read = stream.ReadByte();
                    if(read == -1) return 1;
                    span[1] = (byte)read;
                    read = stream.ReadByte();
                    if(read == -1) return 2;
                    span[2] = (byte)read;
                    return 3;
                case 4:
                    read = stream.ReadByte();
                    if(read == -1) return 0;
                    span[0] = (byte)read;
                    read = stream.ReadByte();
                    if(read == -1) return 1;
                    span[1] = (byte)read;
                    read = stream.ReadByte();
                    if(read == -1) return 2;
                    span[2] = (byte)read;
                    read = stream.ReadByte();
                    if(read == -1) return 3;
                    span[3] = (byte)read;
                    return 4;
                default:
                    var arr = ArrayPool<byte>.Shared.Rent(span.Length);
                    try
                    {
                        read = stream.Read(arr, 0, span.Length);
                        arr.AsSpan().Slice(0, span.Length).CopyTo(span);
                    } finally
                    {
                        ArrayPool<byte>.Shared.Return(arr);
                    }
                    return read;
            }
        }

        public static bool TryPeek<T>(this Stack<T> stack, out T result)
        {
            if(stack.Count > 0)
            {
                result = stack.Peek();
                return true;
            }
            result = default;
            return false;
        }
    }
}
