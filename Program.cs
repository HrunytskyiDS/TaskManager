using FluentMigrator.Runner;
using LazyCache;
using TaskManager;
using TaskManager.Endpoints;
using TaskManager.Services;

var builder = WebApplication.CreateBuilder(args);

// SQLite connection
var connectionString = "Data Source=taskmanager.db";

// Add FluentMigrator
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSQLite()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(TaskManager.Migrations.CreateTasksTable).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAppCache, CachingService>();
builder.Services.AddSingleton<TaskService>();
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

// Run migrations
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();

    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>(); 
    initializer.Execute();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterTaskEndpoints();

app.Run();