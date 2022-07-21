using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ToDoList.Api.Pipelines;
using ToDoList.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly()).ForEach(pair =>
{
    builder.Services.AddScoped(typeof(IValidator), pair.ValidatorType);
    builder.Services.AddScoped(pair.InterfaceType, pair.ValidatorType);
});
builder.Services.AddMediatR(Assembly.GetExecutingAssembly())
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<DatabaseContext>(settings =>
    settings.UseNpgsql(builder.Configuration.GetConnectionString("Main"), sqlOpt => sqlOpt.CommandTimeout(300))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.Database.Migrate();
}

app.Run();
