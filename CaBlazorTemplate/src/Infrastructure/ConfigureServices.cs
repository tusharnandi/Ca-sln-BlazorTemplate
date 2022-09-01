using CaBlazorTemplate.Application.Common.Interfaces;
using CaBlazorTemplate.Infrastructure.Files;
using CaBlazorTemplate.Infrastructure.Identity;
using CaBlazorTemplate.Infrastructure.Persistence;
using CaBlazorTemplate.Infrastructure.Persistence.Interceptors;
using CaBlazorTemplate.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        //services.Configure<SqlConnectionStringsOptions>(configuration.GetSection(SqlConnectionStringsOptions.SqlConnectionStrings));//Db

        services.AddLogging(builder => { builder.AddSeq(configuration.GetSection("Seq")); });

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CaBlazorTemplateDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetValue<string>("SqlConnectionStrings:db:CaBlazorTemplateDb:key1"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddMvcCore().AddApiExplorer();

        //services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        return services;
    }
}
