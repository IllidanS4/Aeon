﻿namespace Aeon.Emulator.Video.Modes
{
    /// <summary>
    /// Implements functionality for chained 8-bit 256-color VGA modes.
    /// </summary>
    internal sealed class Vga256 : VideoMode
    {
        private unsafe readonly byte* videoRam;

        public Vga256(int width, int height, VideoHandler video)
            : base(width, height, 8, false, 8, VideoModeType.Graphics, video)
        {
            unsafe
            {
                this.videoRam = (byte*)video.VideoRam.ToPointer();
            }
        }

        public override int MouseWidth => this.PixelWidth * 2;

        internal override byte GetVramByte(uint offset)
        {
            unsafe
            {
                return videoRam[offset];
            }
        }
        internal override void SetVramByte(uint offset, byte value)
        {
            unsafe
            {
                videoRam[offset] = value;
            }
        }
        internal override ushort GetVramWord(uint offset)
        {
            unsafe
            {
                return *(ushort*)(videoRam + offset);
            }
        }
        internal override void SetVramWord(uint offset, ushort value)
        {
            unsafe
            {
                *(ushort*)(videoRam + offset) = value;
            }
        }
        internal override uint GetVramDWord(uint offset)
        {
            unsafe
            {
                return *(uint*)(videoRam + offset);
            }
        }
        internal override void SetVramDWord(uint offset, uint value)
        {
            unsafe
            {
                *(uint*)(videoRam + offset) = value;
            }
        }
        internal override void WriteCharacter(int x, int y, int index, byte foreground, byte background)
        {
            //throw new NotImplementedException();
        }
    }
}
