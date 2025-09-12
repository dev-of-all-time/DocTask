using System.Text;
using DockTask.Api.Configurations;
using DockTask.Api.Handlers;
using DocTask.Data;
using DocTask.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddControllerConfiguration();

// Authorization (tích hợp role-based)
builder.Services.AddAuthentication("JwtAuth")  // Set default scheme
    .AddJwtBearer("JwtAuth", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:AccessSecretKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Manager", policy => policy.RequireRole("Admin","Manager"));
    options.AddPolicy("Staff", policy => policy.RequireRole("Admin", "Manager", "Staff"));
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddApplicationContainer();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Kết nối đến SQL Server thành công!");
        }
        else
        {
            Console.WriteLine("Không thể kết nối đến SQL Server.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Lỗi khi kết nối đến SQL Server: {ex.Message}");
    }
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseJwtAuthentication();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler(_ => {});
app.UseHttpsRedirection();


app.Run();