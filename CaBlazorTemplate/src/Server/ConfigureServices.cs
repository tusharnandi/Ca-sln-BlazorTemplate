using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NSwag;
using NSwag.Generation.Processors.Security;
using CaBlazorTemplate.Application.Common.Interfaces;
using CaBlazorTemplate.Server.Services;
using Blazored.Toast;

namespace CaBlazorTemplate.Server;
public static class ConfigureServices
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MsGraphService>();
        services.AddScoped<MsGraphClaimsTransformation>();
        services.AddScoped<CaeClaimsChallengeService>();

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-TOKEN";
            options.Cookie.Name = "__Host-X-XSRF-TOKEN";
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddHttpClient();
        services.AddOptions();

        services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureB2C")
        .EnableTokenAcquisitionToCallDownstreamApi(Array.Empty<string>())
        .AddInMemoryTokenCaches();

        services.Configure<MicrosoftIdentityOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Events.OnTokenValidated = async context =>
            {
                IServiceProvider? applicationServices = null;
                if (applicationServices != null && context.Principal != null)
                {
                    using var scope = applicationServices.CreateScope();
                    context.Principal = await scope.ServiceProvider
                        .GetRequiredService<MsGraphClaimsTransformation>()
                        .TransformAsync(context.Principal);
                }
            };
        });

        services.AddRazorPages().AddMvcOptions(options =>
        {
            //var policy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();
            //options.Filters.Add(new AuthorizeFilter(policy));
        }).AddMicrosoftIdentityUI();
        

        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "CaBlazorTemplate Blazor Web API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });
            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        services.AddBlazoredToast();

        return services;
    }
}

