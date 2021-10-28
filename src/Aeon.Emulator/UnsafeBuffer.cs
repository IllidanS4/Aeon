using System;
using System.Runtime.InteropServices;

namespace Aeon.Emulator
{
    internal class UnsafeBuffer<T> where T : unmanaged
    {
        private readonly T[] array;
        private readonly GCHandle handle;

        public UnsafeBuffer(int length)
        {
            this.array = new T[length];
            handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        }

        public unsafe T* ToPointer() => (T*)handle.AddrOfPinnedObject();
        public void Clear() => Array.Clear(this.array, 0, this.array.Length);

        ~UnsafeBuffer()
        {
            handle.Free();
        }
    }
}
