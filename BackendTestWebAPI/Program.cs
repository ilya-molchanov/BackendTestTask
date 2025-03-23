using BackendTest.Data;
using BackendTest.Data.Repositories;
using BackendTest.Data.Repositories.Interfaces;
using BackendTest.WebApi.Filters.Exception;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.Add(new ServiceDescriptor(
    typeof(INodeRepository),
    typeof(NodeRepository),
    ServiceLifetime.Transient
));

// Database variables
//var dbHost = "localhost,1433"; // Environment.GetEnvironmentVariable("DB_HOST");
//var dbName = Environment.GetEnvironmentVariable("DB_NAME");
//var dbPass = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
//var connString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPass};Encrypt=False";

// var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddDbContext<NodesDbContext>
(options =>
{
    options.UseSqlServer("Data Source=localhost,1433;Initial Catalog=testappdb;User ID=sa;Password=password@12345#;Encrypt=False", //connString, // builder.Configuration.GetConnectionString("DefaultConnection")
    sqlOptions => sqlOptions.MigrationsAssembly("BackendTest.Data"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetService<NodesDbContext>();

//    db.Database.Migrate();
//}

app.Run();
