using CaBlazorTemplate.Infrastructure.Persistence;
using CaBlazorTemplate.Server.Options;
using CaBlazorTemplate.Server.Services;

namespace CaBlazorTemplate.Server;

public class Startup
{
    public IConfiguration Configuration { get; private set; }
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;

    }
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddApplicationServices();
        services.AddInfrastructureServices(Configuration);
        services.AddPresentationServices(Configuration);

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();

            // Initialise and seed database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                _ = initialiser.InitialiseAsync();
                _ = initialiser.SeedAsync();
            }
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseExceptionHandler("/Error");
        }

        //ContentHeaderPolicy Config
        var contentHeaderPolicyOptions = new ContentHeaderPolicyOptions();
        Configuration.GetSection(ContentHeaderPolicyOptions.ContentHeaderPolicy).Bind(contentHeaderPolicyOptions);

        app.UseSecurityHeaders(
                SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment(),contentHeaderPolicyOptions));

        //app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseSwaggerUi3(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/v1/specification.json";
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();



        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapNotFound("/api/{**segment}");
            endpoints.MapFallbackToPage("/_Host");
        });

    }


}

