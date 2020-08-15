﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aeon.Emulator.CommandInterpreter
{
    public sealed class ClsCommand : CommandStatement
    {
        public ClsCommand()
        {
        }

        internal override CommandResult Run(CommandProcessor processor) => processor.RunCommand(this);
    }
}
