﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aeon.Emulator.CommandInterpreter
{
    public sealed class PrintEnvironmentCommand : CommandStatement
    {
        internal override CommandResult Run(CommandProcessor processor) => processor.RunCommand(this);
    }
}
