using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Game;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Auto.Core
{
    public static class DependencyInjection
    {
        public static bool AddWoWProcess(this IServiceCollection services, ILogger log)
        {
            services.AddSingleton<CancellationTokenSource>();
            services.AddSingleton<WowProcess>();

            //services.AddSingleton<AddonConfigurator>();

            var sp = services.BuildServiceProvider(
                new ServiceProviderOptions { ValidateOnBuild = true });

            WowProcess process = sp.GetRequiredService<WowProcess>();


            return true;
        }
    }
}
