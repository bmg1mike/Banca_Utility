using Banca.UtilityService.API;
using Banca.UtilityService.Application;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider serviceProvider, LoggerConfiguration config) =>
{
    config.ReadFrom
        .Configuration(context.Configuration)
        .ReadFrom.Services(serviceProvider);

});
// Add services to the container.
builder.Services.AddHttpClient("Utility").AddTransientHttpErrorPolicy(x => x.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(builder.Configuration.GetValue<double>("PollyConfig:RetryTime")), retryCount: builder.Configuration.GetValue<int>("PollyConfig:RetryCount"))))
           .AddTransientHttpErrorPolicy(x => x.CircuitBreakerAsync(builder.Configuration.GetValue<int>("PollyConfig:HandledEventsAllowedBeforeBreaking"), TimeSpan.FromSeconds(builder.Configuration.GetValue<double>("PollyConfig:breakerTime"))));

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionMiddleware();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
