
using DataAcces.DataAcces;
using DataAcces.DataAccess.SPs;
using DataAcces.Logica;
using DataAccess.SPs;
using Logica;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DatabaseHelper>();  // Registrando DatabaseHelper
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UsuarioSP>();
builder.Services.AddScoped<GalagaSP>();
builder.Services.AddScoped<AmistadSP>();
builder.Services.AddScoped<FutbolitoSP>();
builder.Services.AddScoped<LogicaUsuario>();
builder.Services.AddScoped<LogicaGalaga>();
builder.Services.AddScoped<LogicaAmistad>();
builder.Services.AddScoped<LogicaFutbolito>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",builder =>
        {
            builder.SetIsOriginAllowed(_ => true)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
