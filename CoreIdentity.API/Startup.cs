using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using CoreIdentity.Persistence;
using CoreIdentity.Application;
using CoreIdentity.API.Filters;
using FluentValidation.AspNetCore;
using CoreIdentity.Application.Common.Models;
using CoreIdentity.Application.Common.Interfaces;

namespace CoreIdentity.API;

public class Startup
{
    private ILoggerFactory _loggerFactory;

    public Startup(IConfiguration configuration)
    {
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Information);
        });

        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Default Policy
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000/")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });

        services.AddAuthorization();
        var logger = _loggerFactory.CreateLogger(typeof(Startup));

        //then reset the logger after printing
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Information);
        });
        logger = _loggerFactory.CreateLogger(typeof(Startup));

        var test2 = Configuration.GetSection("Jwt");
        // Service Layers 
        string connString = Configuration.GetConnectionString("CoreIdentityDb");

        services.AddConfigurations(Configuration);
        services.AddPersistenceLayer(connString);
        services.AddApplicationLayer();

        //services.AddControllers(opts => opts.Filters.Add(new AuthorizeFilter()));
        services.AddControllers();

        services.AddApiVersioning(setup =>
        {
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
        });

        services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Badger CoreIdentity API", Version = "version 1.0" });
            opts.SwaggerDoc("v2", new OpenApiInfo { Title = "Badger CoreIdentity API", Version = "version 2.0" });

            //opts.OperationFilter<FileUploadOperation>();
            //opts.OperationFilter<OptionalRouteParameterOperationFilter>();
            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "CoreIdentity", "CoreIdentity" }
                    }
            });

            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // opts.IncludeXmlComments(xmlPath);
        });

        services.AddAuthentication();

        services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddFluentValidation();

        services.AddMemoryCache();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Shows UseCors with CorsPolicyBuilder.
        app.UseCors(builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });


        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseAuthentication();

        //app.UseHttpsRedirection();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<CoreIdentityDbContext>();
            await DbSeeder.SeedAsync(context);
        }
    }
}
