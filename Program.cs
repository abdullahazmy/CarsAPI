using CarsAPI.DTOs;
using CarsAPI.Models;
using CarsAPI.Repository;
using CarsAPI.Repository.Interfaces;
using CarsAPI.Service;
using EdufyAPI.Models.Roles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();


#region Configure Swagger with JWT authentication
// 🔹 Configure Swagger with JWT authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AskAMuslim API", Version = "v1" });

    // Add JWT support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // This line tells Swagger to use enum names
    c.UseInlineDefinitionsForEnums();
});

#endregion

// return enums as names instead of numbers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 🔹 Add the connection string to the configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


#region Identity

// 🔹 Configure Identity with roles
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 🔹 Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"]
    };
});

#endregion


#region Register services for dependency injection
// 🔹 Register UnitOfWork and Generic Repository for dependency injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IFavouriteService, FavouriteService>();
// This ensures that:
//A new instance of UnitOfWork(and consequently DbContext) is created per request.
//Once the request is complete, the instance is disposed of properly. This is important to prevent memory leaks.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles)); // Register AutoMapper
builder.Services.AddLogging(); // This ensures logging is available

#endregion


# region Cores

// 🔹 Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

/*
  // 🔹 Configure CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
});
    * Don't forget to edit the appsettings.json file to include the AllowedOrigins key
 */
#endregion

//builder.Services.AddScoped<FileUploadHelper>();

var app = builder.Build();

app.UseDefaultFiles(); // Enables serving index.html by default
app.UseStaticFiles();

app.MapFallbackToFile("index.html"); // For React Router support (SPA)

#region Middleware Configuration
// 🔹 Configure middleware


// Swagger 
app.UseSwagger();
app.UseSwaggerUI();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ask A Muslim API V1");
});




app.UseCors("AllowAll");

app.UseHttpsRedirection();
// 🔹 Enable Authentication & Authorization middleware
app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
