using Auto.BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace Auto.BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Configuration, builder.Services);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }

        private static void ConfigureServices(ConfigurationManager configuration, IServiceCollection services)
        {
            ILoggerFactory logFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders().AddSerilog();
            });
            services.AddLogging(builder => {

                const string outputTemplate = "[{@t:HH:mm:ss:fff} {@l:u1}] {#if Length(SourceContext) > 0}[{Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1),-17}] {#end}{@m}\n{@x}";

                Log.Logger=new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.File(new ExpressionTemplate(outputTemplate),"out.log",rollingInterval: RollingInterval.Day)
                .WriteTo.Debug(new ExpressionTemplate(outputTemplate))
                .WriteTo.Console(new ExpressionTemplate(outputTemplate, theme: TemplateTheme.Literate))
                .CreateLogger();

                builder.Services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(logFactory.CreateLogger(string.Empty));
            });
            Microsoft.Extensions.Logging.ILogger log = logFactory.CreateLogger("Program");
            log.LogInformation(
            $"{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName} " +
            $"{DateTimeOffset.Now}");
        }
    }
}