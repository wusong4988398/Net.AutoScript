using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.ShareLib
{
    public sealed class StartupConfigNpcOverlay
    {
        public const string Position = "Overlay";

        public StartupConfigNpcOverlay() { }

        public bool Enabled { get; set; }

        public bool ShowTargeting { get; set; }
        public bool ShowSkinning { get; set; }
        public bool ShowTargetVsAdd { get; set; }
    }
}
