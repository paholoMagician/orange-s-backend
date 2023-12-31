
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using orange.Controllers;
using orangebackend6;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json").Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<orangeContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews().AddNewtonsoftJson(
    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
IServiceCollection serviceCollection = builder.Services.AddDbContext<orangeContext>(opt => opt.UseInMemoryDatabase(databaseName: "bin3"));

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 1024;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.WithOrigins( "http://localhost:4200",
                                            "https://9bf141c93e33.ngrok.app",
                                            "http://localhost:5078",
                                            "http://localhost:60004",
                                            "https://orangetaskapp.web.app",
                                            "http://localhost:3358", "*")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials() );

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();
app.UseAuthorization();

app.MapHub<ComentariosHub>("/hubs/ComentariosHub");
app.MapHub<tablaHub>("/hubs/tablaHub");

app.Run();
