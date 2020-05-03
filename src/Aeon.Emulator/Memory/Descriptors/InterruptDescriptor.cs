﻿using System.Runtime.InteropServices;
using Aeon.Emulator.Memory;

namespace Aeon.Emulator
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InterruptDescriptor
    {
        private ushort offset1;
        private ushort selector;
        private byte unused;
        private byte typeAttributes;
        private ushort offset2;

        /// <summary>
        /// Casts a interrupt descriptor to a descriptor.
        /// </summary>
        /// <param name="descriptor">Interrupt descriptor to cast.</param>
        /// <returns>Resulting descriptor.</returns>
        public static implicit operator Descriptor(InterruptDescriptor descriptor)
        {
            unsafe
            {
                return *(Descriptor*)&descriptor;
            }
        }

        /// <summary>
        /// Gets the segment offset.
        /// </summary>
        public uint Offset => this.offset1 | (uint)(this.offset2 << 16);
        /// <summary>
        /// Gets the selector value.
        /// </summary>
        public ushort Selector => this.selector;
        /// <summary>
        /// Gets the descriptor attributes.
        /// </summary>
        public byte Attributes => this.typeAttributes;
        /// <summary>
        /// Gets the privilege level of the descriptor.
        /// </summary>
        public uint PrivilegeLevel => ((uint)this.typeAttributes >> 5) & 0b11u;
        /// <summary>
        /// Gets a value indicating whether the descriptor refers to a 32-bit code segment.
        /// </summary>
        public bool Is32Bit => (this.typeAttributes & 0x8) != 0;
        /// <summary>
        /// Gets a value indicating whether the descriptor refers to a trap.
        /// </summary>
        public bool IsTrap => (this.typeAttributes & 0x0F) == 0x0F || (this.typeAttributes & 0x0F) == 0x07;
    }
}
