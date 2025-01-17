﻿using System;

namespace Aeon.Emulator.Video
{
    /// <summary>
    /// Emulates the VGA Sequencer registers.
    /// </summary>
    public sealed class Sequencer : VideoComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sequencer"/> class.
        /// </summary>
        public Sequencer()
        {
        }

        /// <summary>
        /// Gets or sets the Reset register.
        /// </summary>
        public byte Reset { get; set; }
        /// <summary>
        /// Gets or sets the Clocking Mode register.
        /// </summary>
        public byte ClockingMode { get; set; }
        /// <summary>
        /// Gets or sets the Map Mask register.
        /// </summary>
        public byte MapMask { get; set; }
        /// <summary>
        /// Gets or sets the Character Map Select register.
        /// </summary>
        public byte CharacterMapSelect { get; set; }
        /// <summary>
        /// Gets or sets the Sequencer Memory Mode register.
        /// </summary>
        public SequencerMemoryMode SequencerMemoryMode { get; set; }

        /// <summary>
        /// Returns the current value of a sequencer register.
        /// </summary>
        /// <param name="address">Address of register to read.</param>
        /// <returns>Current value of the register.</returns>
        public byte ReadRegister(SequencerRegister address)
        {
            return address switch
            {
                SequencerRegister.Reset => this.Reset,
                SequencerRegister.ClockingMode => this.ClockingMode,
                SequencerRegister.MapMask => this.MapMask,
                SequencerRegister.CharacterMapSelect => this.CharacterMapSelect,
                SequencerRegister.SequencerMemoryMode => (byte)this.SequencerMemoryMode,
                _ => 0
            };
        }
        /// <summary>
        /// Writes to a sequencer register.
        /// </summary>
        /// <param name="address">Address of register to write.</param>
        /// <param name="value">Value to write to register.</param>
        public void WriteRegister(SequencerRegister address, byte value)
        {
            switch (address)
            {
                case SequencerRegister.Reset:
                    this.Reset = value;
                    break;

                case SequencerRegister.ClockingMode:
                    this.ClockingMode = value;
                    break;

                case SequencerRegister.MapMask:
                    this.MapMask = value;
                    ExpandRegister(value, this.ExpandedMapMask);
                    break;

                case SequencerRegister.CharacterMapSelect:
                    this.CharacterMapSelect = value;
                    break;

                case SequencerRegister.SequencerMemoryMode:
                    this.SequencerMemoryMode = (SequencerMemoryMode)value;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Map Mask register expanded to four booleans.
        /// </summary>
        public readonly bool[] ExpandedMapMask = new bool[4];
    }

    [Flags]
    public enum SequencerMemoryMode : byte
    {
        None = 0,
        ExtendedMemory = 2,
        OddEvenWriteAddressingDisabled = 4,
        Chain4 = 8,
    }
}
