using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Set up Serilog with monthly rolling logs
Log.Logger = new LoggerConfiguration()
    .Enrich.With(new StackTraceEnricher())
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DNS Management Api",
        Description = "API pour la gestion des enregistrements DNS.",
    });

    // Include XML comments for better documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // D�finition du sch�ma d�authentification Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. " +
                      "Exemple : \"Bearer 12345abcdef\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Exigence de s�curit� � NOUVELLE SYNTAXE .NET 10 / Swashbuckle 10
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        // La cl� doit utiliser **exactement** le m�me nom que dans AddSecurityDefinition
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// Jwt options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Authentification / JWT
var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
var keyBytes = Encoding.UTF8.GetBytes(jwtConfig.Key);

// Configure JWT Authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
    });

// Configure Authorization
builder.Services.AddAuthorization();

// Pour acc�der au HttpContext dans les services
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDomainService, DomainService>();
builder.Services.AddScoped<IRecordService, RecordService>();

builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IRecordRepository, EfRecordRepository>();
builder.Services.AddScoped<IDomainRepository, EfDomainRepository>();
builder.Services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

// Hasher de mot de passe
builder.Services.AddSingleton<IPasswordHasher, IdentityPasswordHasher>();
builder.Services.AddHostedService<LogRetentionService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// const string ProdCorsPolicy = "ProdCorsPolicy";
//
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(ProdCorsPolicy, policy =>
//     {
//         var apiOrigin = builder.Configuration["Cors:ApiOrigin"];
//         var appOrigin = builder.Configuration["Cors:AppOrigin"];
//
//         policy
//             .WithOrigins(appOrigin!, apiOrigin!)
//             .AllowAnyMethod()
//             .AllowAnyHeader();
//     });
// });

var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<ApplicationDbContext>();
await context.Database.MigrateAsync();

// if (app.Environment.IsProduction())
// {
//     app.UseCors(ProdCorsPolicy);
// }
// else
// {
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// }
    
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await DbInitializer.SeedAsync(context);

    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// Make Program accessible to integration tests
public partial class Program { }
