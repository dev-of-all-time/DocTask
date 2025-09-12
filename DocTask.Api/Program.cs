using DockTask.Api.Configurations;
using DockTask.Api.Handlers;
using DocTask.Data;
using DocTask.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
builder.Services.AddControllers();
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
//debug test
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//     var task32 = await context.Tasks.FirstOrDefaultAsync(t => t.TaskId == 32);
//     if (task32 != null)
//         Console.WriteLine($"Found task 32: {task32.Title}");
//     else
//         Console.WriteLine("Task 32 not found");
// }
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseExceptionHandler(_ => {});
app.UseHttpsRedirection();

app.Run();