using Microsoft.EntityFrameworkCore;
using Serilog;
using Usuarios.Data;

var builder = WebApplication.CreateBuilder(args);

// inyeccion de dependencias
builder.Services.AddDbContext<usuariosContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// configuracion de serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Logger(l => // Se va a escribir un archivo 
        l.Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Error) // Indicando el tipo de log que se va a crear (Error)
        .WriteTo.File("Logs/Log-Error-.txt", rollingInterval: RollingInterval.Day) // El nombre del archivo
    )
    .WriteTo.Logger(l => // Se va a escribir un archivo 
        l.Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Information) // Indicando el tipo de log que se va a crear (Error)
        .WriteTo.File("Logs/Log-.txt", rollingInterval: RollingInterval.Day) // El nombre del archivo
    )
    .CreateLogger(); // Finalmente se crea el Log con las configuraciones realizadas anteriormente.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
