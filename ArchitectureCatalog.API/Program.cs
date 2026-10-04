using ArchitectureCatalog.API.Repositories;
using ArchitectureCatalog.API.Services.ArchitecturalStyles;
using ArchitectureCatalog.API.Services.Buildings;
using ArchitectureCatalog.API.Services.Comments;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MySqlConnection>(_ =>
    new MySqlConnection(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IArchitecturalStyleRepository, ArchitecturalStyleRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IArchitecturalStyleService, ArchitecturalStyleService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
