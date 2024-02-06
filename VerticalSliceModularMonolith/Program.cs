using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using System.Globalization;
using VerticalSliceModularMonolith.GraphQL;
using VerticalSliceModularMonolith.Infrastructure;
using VerticalSliceModularMonolith.Shared;
using VerticalSliceModularMonolith.Modules.Usuarios;
using VerticalSliceModularMonolith.Modules.Livros;
using VerticalSliceModularMonolith;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
try
{
    builder.Host.UseSerilog(Log.Logger);

    var origins = builder.Configuration["Cors:Origins"];
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .WithOrigins(origins?.Split(",").Select(x => x.Trim()).ToArray() ?? new string[] { })
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SigningKey"]!);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddShared();
    builder.Services.AddUsuarios();
    builder.Services.AddLivros();

    builder.Services.AddGraphQLConfiguration();

    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    });

    builder.Services.AddHostedService<TestesLocaisHostedService>();

    builder.Services.AddControllers().AddControllersAsServices();

    var app = builder.Build();

    var cultureInfo = new CultureInfo("pt-BR");
    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
    cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

    app.UseCors("AllowAll");

    app.UseResponseCompression();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseInfrastructure();

    app.UseGraphQL();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

public partial class Program { }
