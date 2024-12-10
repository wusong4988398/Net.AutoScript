﻿

using Auto.ShareLib;

namespace Auto.BlazorServer
{
    public  static class DependencyInjection
    {
        public static void AddStartupConfigurations(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.Configure<StartupConfigPid>(configuration.GetSection(StartupConfigPid.Position));
            services.Configure<StartupConfigDiagnostics>(configuration.GetSection(StartupConfigDiagnostics.Position));
            services.Configure<StartupConfigPathing>(configuration.GetSection(StartupConfigPathing.Position));
            services.Configure<StartupConfigReader>(configuration.GetSection(StartupConfigReader.Position));
            services.Configure<StartupConfigNpcOverlay>(configuration.GetSection(StartupConfigNpcOverlay.Position));
        }
    }
}
