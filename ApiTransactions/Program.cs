using Application;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using ExternalServices;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Inject dependencies
var persistenceConfiguration = builder.Configuration.GetSection("Persistence");
var externalServicesConfiguration = builder.Configuration.GetSection("ExternalServices");
var applicationConfiguration = builder.Configuration.GetSection("Application");
builder.Services.AddPersistence(persistenceConfiguration);
builder.Services.AddExternalServices(externalServicesConfiguration);
builder.Services.AddAplication(applicationConfiguration);

//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Create DB if not exists
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
    db.Database.EnsureCreated();
}

// Create Topics (only one for this case)
var kafkaHost = externalServicesConfiguration["KafkaClient:RemoteHost"];
var kafkaPort = externalServicesConfiguration["KafkaClient:RemotePort"];
var kafkaTopics = new string[] {
    externalServicesConfiguration["KafkaClient:AccountTransactionsTopic"],
    externalServicesConfiguration["KafkaClient:AccountTransactionsTopicFails"]
};
using var adminClient = new AdminClientBuilder(
    new AdminClientConfig
    {
        BootstrapServers = $"{kafkaHost}:{kafkaPort}",
    }
).Build();
var kafkaMetadata = adminClient.GetMetadata(TimeSpan.FromSeconds(15));
foreach (var topic in kafkaTopics)
{
    bool kafkaTopicExists = kafkaMetadata.Topics.Exists(t => t.Topic == topic && !t.Error.IsError);
    if (!kafkaTopicExists)
    {
        await adminClient.CreateTopicsAsync(new[]
        {
            new TopicSpecification
            {
                Name = topic,
                NumPartitions = 3,
                ReplicationFactor = 1
            }
        });
    }
}

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
