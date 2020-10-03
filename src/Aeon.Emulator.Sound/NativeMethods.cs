﻿using System;
using System.Runtime.InteropServices;

namespace Aeon.Emulator.Sound
{
    internal static class NativeMethods
    {
        [DllImport("winmm.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern uint midiOutOpen(out IntPtr lphmo, uint uDeviceID, IntPtr dwCallback, IntPtr dwCallbackInstance, uint dwFlags);
        [DllImport("winmm.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern uint midiOutClose(IntPtr hmo);
        [DllImport("winmm.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern uint midiOutShortMsg(IntPtr hmo, uint dwMsg);
        [DllImport("winmm.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false)]
        public static extern uint midiOutReset(IntPtr hmo);

        public const uint MIDI_MAPPER = 0xFFFFFFFF;
    }
}
