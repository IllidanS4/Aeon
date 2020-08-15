﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aeon.Emulator.CommandInterpreter
{
    public sealed class ExitCommand : CommandStatement
    {
        internal override CommandResult Run(CommandProcessor processor) => CommandResult.Exit;
    }
}
