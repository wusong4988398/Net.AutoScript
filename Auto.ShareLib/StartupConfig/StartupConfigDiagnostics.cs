using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.ShareLib
{
    public sealed class StartupConfigDiagnostics
    {
        public const string Position = "Diagnostics";

        public bool Enabled { get; set; }

        public StartupConfigDiagnostics() { }
    }
}
