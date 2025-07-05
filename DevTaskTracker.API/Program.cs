using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Application.IServices;
using DevTaskTracker.Application.Mappers;
using DevTaskTracker.Application.Services.Member;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Common;
using DevTaskTracker.Infrastructure.Persistence;
using DevTaskTracker.Infrastructure.Services;
using DevTaskTracker.Infrastructure.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DevTaskTracker.Infrastructure") // DbContext is in Infrastructure 
    )
    //.LogTo(msg=>File.AppendAllText("ef-queries.text", msg), LogLevel.Information)
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
);
// Add Response Compression
builder.Services.AddResponseCompression();


// Add custom token service
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<IMemberRepository, MemberServiceRepository>();
builder.Services.AddScoped<IMemberServices, MemberServices>();
builder.Services.AddScoped<ITask, TaskService>();
builder.Services.AddScoped<IEmailChecker, EmailChecker>();
builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();
//Add Auto Mapper 
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

//builder.Services.AddScoped<ICommonImplementations, CommonImplementations>();
builder.Services.AddScoped(typeof(ICommonImplementations<>), typeof(CommonImplementations<>));

// In-Memory Cache

builder.Services.AddMemoryCache();

// Add Identity with default token providers
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Taskly", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });       
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

// Add Controllers and Swagger/OpenAPI support
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Rate Liniter
builder.Services.AddRateLimiter(_=>_.AddFixedWindowLimiter("default", options =>
{
    options.PermitLimit = 100;
    options.Window =TimeSpan.FromSeconds(1);
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    options.QueueLimit = 50;
}));

// Configure the HTTP request pipeline  // MIddleware

var app = builder.Build();

app.UseCors("Taskly");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeeder.SeedRolesAsync(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    app.UseRateLimiter();
app.UseHttpsRedirection();

app.UseResponseCompression();
app.UseAuthentication();  // IMPORTANT: Use before UseAuthorization
app.UseAuthorization();

app.MapControllers();

//app.Run("https://0.0.0.0:7215");
app.Run("http://0.0.0.0:5000");// For Docker config








































//using DevTaskTracker.Application.Services;
//using DevTaskTracker.Domain.Entities;
//using DevTaskTracker.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add DbContext with SQL Server
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//    b=> b.MigrationsAssembly("DevTaskTracker.Infrastructure") // The DbContext class is present in Infrastructure project
//    ));
//// Add Token Service 
//builder.Services.AddScoped<ITokenService, TokenService>();


//// Add Identity with default token providers
//builder.Services.AddIdentity<AppUser, IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddDefaultTokenProviders();



//// Add Controllers and Swagger/OpenAPI support
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthentication();  // Add this if you're going to use Identity/JWT
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
