using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.ShareLib
{
    public sealed class StartupConfigReader
    {
        public const string Position = "Reader";

        public StartupConfigReader() { }

        public string Type { get; set; } = string.Empty;

        public AddonDataProviderType ReaderType =>
            System.Enum.TryParse(Type, out AddonDataProviderType m)
            ? m
            : AddonDataProviderType.DXGI;
    }
}
