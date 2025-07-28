using PhiRedactor.Application.Orchestrators;
using PhiRedactor.Application.Orchestrators.Interfaces;
using PhiRedactor.Application.Services;
using PhiRedactor.Application.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string[]? allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>();

if (allowedOrigins is not null)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });


builder.Services.AddTransient<IRedactorOrchestrator, PhiRedactorOrchestrator>();
builder.Services.AddTransient<IFileService, TextFileService>();
builder.Services.AddTransient<IRegexService, PhiRegexService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddConsole();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (allowedOrigins is not null)
    app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
