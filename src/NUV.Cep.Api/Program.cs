﻿using Common.Helpers;


namespace CBL.Startup.Custom
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine($"***** {nameof(CreateHostBuilder)} ****");
                var host = CreateHostBuilder(args).Build();

                Console.WriteLine("VAULT access checking ...");
                var service = (IConfiguration)host.Services.GetService(typeof(IConfiguration));
                var loginVault = service.GetSection("ApisCredentials:Username")?.Value;
                Console.WriteLine($"VAULT access is: {!string.IsNullOrWhiteSpace(loginVault)}");

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                var errorMessage = "Erro na inicialização ou finalização da aplicação = ";

                if (ex.Message.Contains("ORA-") ||
                   ex.Message.Contains("SQLCODE"))
                {
                    errorMessage = "Erro na inicialização ou durante a execução do EF = ";
                }
                errorMessage += ex.Message;

                Console.WriteLine(errorMessage);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    Console.WriteLine(Environment.GetEnvironmentVariable("ENVTEST"));
                    Console.WriteLine($"**** Environment: {context.HostingEnvironment.EnvironmentName} ****");

                    var pathEnvFile = string.Empty;
                    if (args.NotNullOrZero())
                    {
                        pathEnvFile = args.FirstOrDefault(x => x.StartsWith("--env="))?.Replace("--env=", "");
                    }
                    if (string.IsNullOrWhiteSpace(pathEnvFile))
                    {
                        pathEnvFile = Path.Combine(
                            context.HostingEnvironment.ContentRootPath, ".env");
                    }
                    CustomDotEnv.LoadDotEnv(pathEnvFile);

                    var configureProxy = new WebProxyConfigureMethod();
                    WebRequest.DefaultWebProxy = configureProxy.GetProxyWithVariable();
                    Console.WriteLine($"Proxy configured: {configureProxy.HttpProxyField} no proxy: {string.Join(",", configureProxy.HttpNoProxyField ?? new string[] { })}");

                    var buildConfig = config.Build();

                    Console.WriteLine($"**** LogLevelDefault: {buildConfig.GetSection("Logging:LogLevel:Default")?.Value} ****");

                    var keyvaultDns = buildConfig.GetSection("AzureKeyVault:Dns")?.Value;
                    Console.WriteLine($"KeyVault credential is {!string.IsNullOrWhiteSpace(keyvaultDns)}");

                    Uri.TryCreate(keyvaultDns, UriKind.RelativeOrAbsolute, out Uri vaultDns);
                    var vaultTenantId = buildConfig.GetSection("AzureKeyVault:TenantId")?.Value;
                    var vaultClientId = buildConfig.GetSection("AzureKeyVault:ClientId")?.Value;
                    var vaultClientSecret = buildConfig.GetSection("AzureKeyVault:ClientSecret")?.Value;

                    TokenCredential credential = new
                        ClientSecretCredential(
                            tenantId: vaultTenantId,
                            clientId: vaultClientId,
                            clientSecret: vaultClientSecret
                        );

                    config.AddAzureKeyVault(vaultDns, credential);

                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<Startup>();
                    }

                    Console.WriteLine($"**** {nameof(IHostBuilder)} configured ****");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return host;
        }
    }
}