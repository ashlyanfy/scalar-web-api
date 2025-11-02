using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Public API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "¬ведите токен в формате: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
          Array.Empty<string>() }
    });

   
    c.DocumentFilter<HideInternalPathsFilter>();
});


var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-secret-signing-key-min-32-chars-long-123456";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false; 
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true
        };
    });

var app = builder.Build();


app.MapSwagger("/openapi/{documentName}.json").AllowAnonymous(); 
app.MapScalarApiReference("/docs").AllowAnonymous();            

// пайплайн
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



public sealed class HideInternalPathsFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, Swashbuckle.AspNetCore.SwaggerGen.DocumentFilterContext context)
    {
        if (swaggerDoc.Paths is null) return;

        var prefixes = new[] { "/internal", "/cron", "/health" };
        var toRemove = swaggerDoc.Paths.Keys
            .Where(p => prefixes.Any(pref => p.StartsWith(pref, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var key in toRemove)
            swaggerDoc.Paths.Remove(key);
    }
}
