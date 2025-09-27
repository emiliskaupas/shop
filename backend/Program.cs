using backend.Data;
using Microsoft.EntityFrameworkCore;
using backend.Services;
using Shop.Shared.Notifications;

var builder = WebApplication.CreateBuilder(args);
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(conn));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
