using Api.AntiFraud.BackgroundServices;
using Application;
using ExternalServices;
using Persistence;

//
var builder = Host.CreateApplicationBuilder(args);
var persistenceConfiguration = builder.Configuration.GetSection("Persistence");
var externalServicesConfiguration = builder.Configuration.GetSection("ExternalServices");
var applicationConfiguration = builder.Configuration.GetSection("Application");
builder.Services.AddPersistence(persistenceConfiguration);
builder.Services.AddExternalServices(externalServicesConfiguration);
builder.Services.AddAplication(applicationConfiguration);
builder.Services.AddHostedService<AccountTransactionValidatorBackgroundService>();

//
var host = builder.Build();
host.Run();