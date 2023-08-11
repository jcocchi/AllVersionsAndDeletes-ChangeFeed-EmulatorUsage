# Enable all versions and deletes change feed mode on a container in the emulator

This sample project enables all versions and deletes change feed mode on a container in the [Azure Cosmos DB emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21). Because the emulator doesn't support [continuous backups with point in time restore](https://learn.microsoft.com/en-us/azure/cosmos-db/continuous-backup-restore-introduction), which is required for development against an account provisioned in Azure, retention for the change feed is set as part of the container properties instead.

10 minutes is the maximum change feed retention for all versions and deletes mode using the emulator, and setting the retention period on the container will allow you to read change feed in this mode. However, this doesn't guarantee you will see 10 minutes of retention. The retention setting is a maximum retention value only as the Azure Cosmos DB emulator regularly purges logs to manage resource consumption. It's recommended you only read the change feed from `now()` when using all versions and deletes mode with the emulator.

After running this sample for a given container, you will be able to use the existing all versions and deletes mode samples to consume the change feed.

- [.NET pull model sample](https://github.com/Azure/azure-cosmos-dotnet-v3/tree/master/Microsoft.Azure.Cosmos.Samples/Usage/CFPullModelAllVersionsAndDeletesMode)
- [Java pull model sample](https://github.com/Azure-Samples/azure-cosmos-java-sql-api-samples/blob/main/src/main/java/com/azure/cosmos/examples/changefeedpull/SampleChangeFeedPullModelForAllVersionsAndDeletesMode.java)
- [Java Change feed processor sample](https://github.com/Azure-Samples/azure-cosmos-java-sql-api-samples/blob/main/src/main/java/com/azure/cosmos/examples/changefeed/SampleChangeFeedProcessorForAllVersionsAndDeletesMode.java)

> Note: If you are using the preview of all versions and deletes change feed mode on an account provisioned in Azure with continuous backups enabled, the retention period is equal to the backup window configured on the account. No further action is needed in this scenario, and attempting to use this sample will throw an error. Sign up for the preview [here](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/change-feed-modes?tabs=all-versions-and-deletes#get-started).

## Setup

Rename the `appsettings.sample.json` file to `appsettings.json` and fill in the `CosmosConnectionString` with your Azure Cosmos DB emulator connection string. Fill in the other attributes according to your preferred configuration. Default values are provided. This sample sets the change feed retention on new or existing containers.

## Run the application

Run the application in Visual Studio with `CTRL + F5` or from the command line.

```cmd
cd AllVersionsAndDeletes-EmulatorUsage
dotnet run
```
