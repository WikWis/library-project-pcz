using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var allowFrontendOrigin = "_allowFrontendOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowFrontendOrigin,
        policy =>
        {
            policy.WithOrigins("http://localhost:4321")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddProblemDetails();

builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();

// konfiguracja jwt - domyślny schemat autrentykacji
builder.Services.AddAuthentication(options =>
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
    };
});

builder.Services.AddAuthorization();

//Tutaj rejestracja serwisów
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPieceService, PieceService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<ITitleService, TitleService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
builder.Services.AddScoped<IAuthService, AuthService>(); //własny serwis autoryzacji

//AutoMapery np.
builder.Services.AddAutoMapper(_ => { }, typeof(Program).Assembly,  typeof(BaseService).Assembly);

var app = builder.Build();

// Tutaj seedery np.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var mapper = services.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    
    try
    {
        var context = services.GetRequiredService<MyDbContext>();
        context.Database.Migrate(); // Tworzy tabele w kontenerze SQL Server, jeśli jeszcze ich nie ma

        await RoleAndUserSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Błąd inicjalizacji bazy danych, ról i danych testowych.");
    }
    
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors(allowFrontendOrigin);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();