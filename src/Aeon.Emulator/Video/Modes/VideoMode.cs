﻿using System;

namespace Aeon.Emulator.Video
{
    /// <summary>
    /// Provides information about an emulated video mode.
    /// </summary>
    public abstract class VideoMode
    {
        /// <summary>
        /// The size of a video RAM plane in bytes.
        /// </summary>
        public const int PlaneSize = 65536;
        /// <summary>
        /// The size of a display page in bytes.
        /// </summary>
        public const int DisplayPageSize = 0x1000 / 2;

        private readonly CrtController crtController;
        private readonly AttributeController attributeController;
        private readonly Dac dac;

        private protected VideoMode(int width, int height, int bpp, bool planar, int fontHeight, VideoModeType modeType, VideoHandler video)
        {
            this.Width = width;
            this.Height = height;
            this.OriginalHeight = height;
            this.BitsPerPixel = bpp;
            this.IsPlanar = planar;
            this.FontHeight = fontHeight;
            this.VideoModeType = modeType;
            this.dac = video.Dac;
            this.crtController = video.CrtController;
            this.attributeController = video.AttributeController;
            this.VideoRam = GetVideoRamPointer(video);

            InitializeFont(video.VirtualMachine.PhysicalMemory);
        }
        private protected VideoMode(int width, int height, VideoMode baseMode)
        {
            this.Width = width;
            this.Height = height;
            this.BitsPerPixel = baseMode.BitsPerPixel;
            this.IsPlanar = baseMode.IsPlanar;
            this.FontHeight = baseMode.FontHeight;
            this.VideoModeType = baseMode.VideoModeType;
            this.dac = baseMode.dac;
            this.crtController = baseMode.crtController;
            this.attributeController = baseMode.attributeController;
            this.VideoRam = baseMode.VideoRam;
        }

        /// <summary>
        /// Gets the width of the emulated video mode in pixels or characters.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Gets the height of the emulated video mode in pixels or characters.
        /// </summary>
        public int Height { get; internal set; }
        /// <summary>
        /// Gets the original height of the emulated video mode in pixels or characters.
        /// </summary>
        /// <remarks>
        /// This remains set to the original height of the video mode before any changes due
        /// to modifying the value of the vertical end register.
        /// </remarks>
        public int OriginalHeight { get; }
        /// <summary>
        /// Gets the bits per pixel of the emulated video mode.
        /// </summary>
        public int BitsPerPixel { get; }
        /// <summary>
        /// Gets the width of the screen in pixels, even in text modes.
        /// </summary>
        public int PixelWidth => this.VideoModeType == VideoModeType.Graphics ? this.Width : this.Width * 8;
        /// <summary>
        /// Gets the height of the screen in pixels, even in text modes.
        /// </summary>
        public int PixelHeight => this.VideoModeType == VideoModeType.Graphics ? this.Height : this.Height * this.FontHeight;
        /// <summary>
        /// Gets a value which specifies whether the video mode is text-only or graphical.
        /// </summary>
        public VideoModeType VideoModeType { get; }
        /// <summary>
        /// Gets the number of bytes between rows of pixels.
        /// </summary>
        public virtual int Stride => crtController.Offset * 2;
        /// <summary>
        /// Gets the number of bytes from the beginning of video memory where the display data starts.
        /// </summary>
        public virtual int StartOffset => crtController.StartAddress;
        /// <summary>
        /// Gets the number of pixels to shift the output display horizontally.
        /// </summary>
        public int HorizontalPanning => attributeController.HorizontalPixelPanning;
        /// <summary>
        /// Gets the value to add to StartOffest.
        /// </summary>
        public int BytePanning => (crtController.PresetRowScan >> 5) & 0x3;
        /// <summary>
        /// Gets the value of the LineCompare register.
        /// </summary>
        public int LineCompare => crtController.LineCompare | ((crtController.Overflow & (1 << 4)) << 4) | ((crtController.MaximumScanLine & (1 << 6)) << 3);
        /// <summary>
        /// Gets the value of the StartVerticalBlanking register.
        /// </summary>
        public int StartVerticalBlanking => crtController.StartVerticalBlanking | ((crtController.Overflow & (1 << 3)) << 5) | ((crtController.MaximumScanLine & (1 << 5)) << 4);
        /// <summary>
        /// Gets a pointer to the emulated video RAM.
        /// </summary>
        public IntPtr VideoRam { get; }
        /// <summary>
        /// Gets the current EGA/VGA compatibility map.
        /// </summary>
        public byte[] InternalPalette => attributeController.InternalPalette;
        /// <summary>
        /// Gets the current VGA color palette.
        /// </summary>
        public ReadOnlySpan<uint> Palette => this.dac.Palette;
        /// <summary>
        /// Gets a value indicating whether the display mode is planar.
        /// </summary>
        public bool IsPlanar { get; }
        /// <summary>
        /// Gets the currently active display page index.
        /// </summary>
        public int ActiveDisplayPage { get; internal set; }
        /// <summary>
        /// Gets the height of the mode's font in pixels.
        /// </summary>
        public int FontHeight { get; }
        /// <summary>
        /// Gets the current font for the video mode.
        /// </summary>
        public byte[] Font { get; } = new byte[4096];
        /// <summary>
        /// Gets the video mode's width in mouse virtual screen units.
        /// </summary>
        public virtual int MouseWidth => this.PixelWidth;

        /// <summary>
        /// Gets a value indicating whether the display mode has a cursor.
        /// </summary>
        internal virtual bool HasCursor => false;

        /// <summary>
        /// Reads a byte from an address in video memory.
        /// </summary>
        /// <param name="offset">Address of byte to read.</param>
        /// <returns>Byte at specified address.</returns>
        internal abstract byte GetVramByte(uint offset);
        /// <summary>
        /// Writes a byte to an address in video memory.
        /// </summary>
        /// <param name="offset">Address where byte will be written.</param>
        /// <param name="value">Value to write to specified address.</param>
        internal abstract void SetVramByte(uint offset, byte value);
        /// <summary>
        /// Reads a 16-bit word from an address in video memory.
        /// </summary>
        /// <param name="offset">Address of word to read.</param>
        /// <returns>Word at specified address.</returns>
        internal abstract ushort GetVramWord(uint offset);
        /// <summary>
        /// Writes a 16-bit word to an address in video memory.
        /// </summary>
        /// <param name="offset">Address where word will be written.</param>
        /// <param name="value">Value to write to specified address.</param>
        internal abstract void SetVramWord(uint offset, ushort value);
        /// <summary>
        /// Reads a 32-bit doubleword from an address in video memory.
        /// </summary>
        /// <param name="offset">Address of doubleword to read.</param>
        /// <returns>Doubleword at specified address.</returns>
        internal abstract uint GetVramDWord(uint offset);
        /// <summary>
        /// Writes a 32-bit doubleword to an address in video memory.
        /// </summary>
        /// <param name="offset">Address where doubleword will be written.</param>
        /// <param name="value">Value to write to specified address.</param>
        internal abstract void SetVramDWord(uint offset, uint value);
        /// <summary>
        /// Writes a character at a position on the screen with the current font.
        /// </summary>
        /// <param name="x">Column of character to write.</param>
        /// <param name="y">Row of character to write.</param>
        /// <param name="index">Index of character to write.</param>
        /// <param name="foreground">Foreground color of character to write.</param>
        /// <param name="background">Background color of character to write.</param>
        internal abstract void WriteCharacter(int x, int y, int index, byte foreground, byte background);
        /// <summary>
        /// Performs any necessary initialization upon entering the video mode.
        /// </summary>
        /// <param name="video">The video device.</param>
        internal virtual void InitializeMode(VideoHandler video)
        {
            video.VirtualMachine.PhysicalMemory.Bios.CharacterPointHeight = (ushort)this.FontHeight;

            unsafe
            {
                byte* ptr = (byte*)VideoRam.ToPointer();
                for (int i = 0; i < VideoHandler.TotalVramBytes; i++)
                    ptr[i] = 0;
            }

            int stride;

            if (this.VideoModeType == VideoModeType.Text)
            {
                video.TextConsole.Width = this.Width;
                video.TextConsole.Height = this.Height;
                stride = this.Width * 2;
            }
            else
            {
                video.TextConsole.Width = this.Width / 8;
                video.TextConsole.Height = this.Height / this.FontHeight;
                if (this.BitsPerPixel < 8)
                    stride = this.Width / 8;
                else
                    stride = this.Width;
            }

            crtController.Overflow = (1 << 4);
            crtController.MaximumScanLine = (1 << 6);
            crtController.LineCompare = 0xFF;
            crtController.Offset = (byte)(stride / 2u);
            crtController.StartAddress = 0;
            video.Graphics.BitMask = 0xFF;
        }
        /// <summary>
        /// Returns a pointer to video RAM for the display mode.
        /// </summary>
        /// <param name="video">Current VideoHandler instance.</param>
        /// <returns>Pointer to the mode's video RAM.</returns>
        internal virtual IntPtr GetVideoRamPointer(VideoHandler video) => video.VideoRam;

        /// <summary>
        /// Copies the current font from emulated memory into a buffer.
        /// </summary>
        /// <param name="memory">Current PhysicalMemory instance.</param>
        private void InitializeFont(PhysicalMemory memory)
        {
            uint offset;
            int length;
            switch (this.FontHeight)
            {
                case 8:
                    offset = PhysicalMemory.Font8x8Offset;
                    length = 8 * 256;
                    break;

                case 14:
                    offset = PhysicalMemory.Font8x14Offset;
                    length = 16 * 256;
                    break;

                case 16:
                    offset = PhysicalMemory.Font8x16Offset;
                    length = 16 * 256;
                    break;

                default:
                    throw new InvalidOperationException("Unsupported font height.");
            }

            var ptr = memory.GetPointer(PhysicalMemory.FontSegment, offset);
            System.Runtime.InteropServices.Marshal.Copy(ptr, this.Font, 0, length);
        }
    }

    /// <summary>
    /// Specifies whether a video mode is text-only or graphical.
    /// </summary>
    public enum VideoModeType
    {
        /// <summary>
        /// The video mode is text-only.
        /// </summary>
        Text,
        /// <summary>
        /// The video mode is graphical.
        /// </summary>
        Graphics
    }
}
