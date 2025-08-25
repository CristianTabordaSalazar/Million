using MillionApi.Application;
using MillionApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
    builder.Services.AddControllers();
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",   // Next.js dev
            "http://127.0.0.1:3000"    // opcional, por si usas 127.0.0.1
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        // solo si usarás cookies/token en cookies:
        //.AllowCredentials()
        ;
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.UseInfrastructureAsync();
}

app.Run();