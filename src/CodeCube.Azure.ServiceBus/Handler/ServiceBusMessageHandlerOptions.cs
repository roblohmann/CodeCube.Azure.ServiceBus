using Azure.Messaging.ServiceBus;

namespace CodeCube.Azure.ServiceBus.Handler;

public sealed class ServiceBusMessageHandlerOptions
{
    public ServiceBusMessageHandlerOptions()
    {
        ServiceBusProcessorOptions = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1
        };
    }

    public string ConnectionString { get; set; }
    public string BlobStorageConnectionString { get; set; }
    public ServiceBusProcessorOptions ServiceBusProcessorOptions { get; set; }
    public MessageHandlerSettings HandlerSettings { get; set; }
}