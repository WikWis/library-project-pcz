using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.Interfaces;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();

//Tutaj rejestracja serwisów np. builder.Services.AddScoped<IJakisService, JakisService>();

/* Jeśli frontend i backend na innym localhoscie, to odkomentować.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
*/

//AutoMapery np.
builder.Services.AddAutoMapper(_ => { }, typeof(Program).Assembly,  typeof(BaseService).Assembly);

builder.Services.AddControllers();
builder.Services.AddAuthentication(/* Konfiguracja JWT pod frontend */);
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();

var app = builder.Build();

// Tutaj seedery np.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var mapper = services.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    /*
    try
    {
        var context = services.GetRequiredService<MyDbContext>();
        AdminSeeder.SeedAdmin(services).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Błąd inicjalizacji ról i danych testowych.");
    }
    */
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();