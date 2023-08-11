using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

// Validate input
var connectionString = config.GetConnectionString("CosmosConnectionString");
ValidateInput(connectionString, "Please provide a connection string in appsettings.json");

var dbName = config.GetSection("DatabaseName").Value;
var containerName = config.GetSection("ContainerName").Value;
var containerPK = config.GetSection("ContainerPartitionKey").Value; 
var retention = ValidateRetention(config.GetSection("ContainerChangeFeedRetention").Value, "Optionally, enter an integer for the number of minutes in the retention period as the third command line argument. Maximum retention is 10 and minimum is 1. When not set, the default value is 10.");

Console.WriteLine($"Setting change feed retention period of {retention} minutes on \n\tdatabase: {dbName} \n\tcontainer: {containerName}");

var client = new CosmosClient(connectionString);

await client.CreateDatabaseIfNotExistsAsync(dbName);

var containerProps = new ContainerProperties(containerName, partitionKeyPath: containerPK);

// Set change feed retention for a new container
containerProps.ChangeFeedPolicy.FullFidelityRetention = TimeSpan.FromMinutes(retention);
var container = await client.GetDatabase(dbName).CreateContainerIfNotExistsAsync(containerProps);

// This was an existing container, so we need to update the change feed retention
if (container.StatusCode == HttpStatusCode.OK)
{
    var containerConfig = await container.Container.ReadContainerAsync();
    containerProps = containerConfig.Resource;

    // Set new retention policy for container
    containerProps.ChangeFeedPolicy.FullFidelityRetention = TimeSpan.FromMinutes(retention);
    await container.Container.ReplaceContainerAsync(containerProps);
}

Console.WriteLine($"Change feed retention period successfully set.");

// Helper functions
void ValidateInput(string input, string message){
    if (input == null || input == "")
    {
        throw new Exception(message);
    }
}

int ValidateRetention(string input, string message)
{
    if (input == "")
        return 10;
    
    if (int.TryParse(input, out int retention))
    {
        if (retention > 0 && retention <= 10)
        {
            return retention;
        }
    }

    throw new Exception(message);
}
