using GrafanaConnector;
using GrafanaConnector.Services;

var builder = WebApplication.CreateBuilder(args);

// Add options to the container.
builder.Services.AddOptions<ConnectorOptions>();

// additional providers here needed.
// allow environment variables to override values from other providers.
builder.Configuration.AddEnvironmentVariables(prefix: "GRAF_").AddCommandLine(args);
builder.Services.Configure<ConnectorOptions>(options => builder.Configuration.GetSection("ConnectorOptions").Bind(options));

// Add services to the container.
builder.Services.AddSingleton<TwinClientService>();
builder.Services.AddSingleton<IRecodingStrategyService, RecodingStrategyService>();
builder.Services.AddHostedService<RecodingManagementService>();

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