﻿using System;

namespace Aeon.Emulator
{
    /// <summary>
    /// Defines a device which supports 8-bit DMA transfers.
    /// </summary>
    public interface IDmaDevice8 : IVirtualDevice
    {
        /// <summary>
        /// Gets the DMA channel of the device.
        /// </summary>
        int Channel { get; }

        /// <summary>
        /// Writes bytes of data to the DMA device.
        /// </summary>
        /// <param name="source">Bytes to write to the device.</param>
        /// <returns>Number of bytes actually written to the device.</returns>
        int WriteBytes(ReadOnlySpan<byte> source);
        /// <summary>
        /// Invoked when a transfer is completed in single-cycle mode.
        /// </summary>
        void SingleCycleComplete();
    }
}
