using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

using CaBlazorTemplate.Server;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.AddServerHeader = false;
                });

                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((context, config) => {
                var builtConfig = config.Build();
                var keyvaultUriList = builtConfig.GetSection("KeyVault");
                if (keyvaultUriList != null)
                {
                    //Console.WriteLine("KeyVault reading...");
                    foreach (var pair in keyvaultUriList.GetChildren())
                    {
                        Console.WriteLine($"{pair.Key}={pair.Value} secrets mapping to configuration");
                        try
                        {
                            config.AddAzureKeyVault(new Uri(pair.Value), new DefaultAzureCredential(),
                                options: new AzureKeyVaultConfigurationOptions()
                                { ReloadInterval = new TimeSpan(24, 0, 0) });

                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"Error: failed to Connect {pair.Value}");

                        }
                        //TODO: handle error like access permission

                    }
                }
            });

}