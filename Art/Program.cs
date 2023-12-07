using Art.Abstractions.IRepositories;
using Art.Abstractions.IRepositories.IArtistRepos;
using Art.Abstractions.IRepositories.IPicturesRepos;
using Art.Abstractions.IServices;
using Art.Abstractions.IUnitOfWorks;
using Art.Data;
using Art.Implementations.Repositories.EntityRepos;
using Art.Implementations.Services;
using Art.Implementations.UnitOfWorks;
using AutoMapper;
using Google.Cloud.PubSub.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => options./*UseLazyLoadingProxies().*/UseSqlServer(builder.Configuration.GetConnectionString("ArtConStr")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IArtistService), typeof(ArtistService));
builder.Services.AddScoped(typeof(IPictureService), typeof(PictureService));

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped(typeof(IArtistRepository), typeof(ArtistRepository));
builder.Services.AddScoped(typeof(IPictureRepository), typeof(PictureRepository));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

builder.Services.AddLogging(l => { 
    l.ClearProviders();
    l.SetMinimumLevel(LogLevel.Information);
    l.AddConsole();
});
//middleware de de log islet

Logger? log = new LoggerConfiguration()
    .WriteTo.Console(LogEventLevel.Debug)
    .WriteTo.File("Logs/myJsonLogs.json")
    .WriteTo.File("Logs/mylogs.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")

.WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("ArtConStr"), sinkOptions: new MSSqlServerSinkOptions
{
    TableName = "ArtSeriLog",
    AutoCreateSqlTable = true
},
null, null, LogEventLevel.Warning, null,
columnOptions: new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
            new SqlColumn(columnName:"User_Id", SqlDbType.NVarChar)
    }
},
 null, null
 )
.Enrich.FromLogContext()
.MinimumLevel.Information()
.CreateLogger();

Log.Logger = log;
builder.Host.UseSerilog(log);
/*
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.MSSqlServer("ArtConStr", "Art",
            columnOptions: GetSqlColumnOptions(), restrictedToMinimumLevel: LogEventLevel.Information, batchPostingLimit: 1)
            .CreateLogger();
Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

   
*/
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
