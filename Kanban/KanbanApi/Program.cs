using KanbanApi.Data;
using KanbanApi.Interfaces;
using KanbanApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add the DbContext to the service container with SQL Server configuration
builder.Services.AddDbContext<KanbanDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services for dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IBoardColumnService, BoardColumnService>();
builder.Services.AddScoped<ICardItemService, CardItemService>();

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
