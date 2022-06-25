using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;
using TasksListApp.Models.DatabaseSettings;
using TasksListApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<TaskStoreDatabaseSettings>(
    builder.Configuration.GetSection(nameof(TaskStoreDatabaseSettings)));

builder.Services.AddSingleton<ITaskStoreDatabaseSettings>(sp=>
    sp.GetRequiredService<IOptions<TaskStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetValue<string>("TaskStoreDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { 
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}) ;

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

app.Run();
