using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.ShareLib
{
    public sealed class StartupConfigPid
    {
        public const string Position = "Process";

        public StartupConfigPid() { }

        public int Id { get; set; }
    }
}
