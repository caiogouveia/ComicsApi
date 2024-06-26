﻿namespace Startup.Custom
{
    public static class LocalizationSetup
    {
        public static void AddLocalizationSetup(this IServiceCollection services)
        {
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("en-US"),
                            new CultureInfo("en"),
                            new CultureInfo("pt-BR"),
                            new CultureInfo("pt"),
                        };

                options.DefaultRequestCulture = new RequestCulture(culture: "pt", uiCulture: "pt");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.SetDefaultCulture("pt-BR");
            });
        }
    }
}