namespace CodeCube.Azure.ServiceBus.Sender;

internal sealed class ServiceBusMessageSender : IServiceBusMessageSender
{
    public ServiceBusMessageSender(string serviceBusConnectionString)
    {
        throw new NotImplementedException();
    }

    public Task SendMessage(string messageId, object messageObject, MessageMetadata metadata)
    {
        throw new NotImplementedException();
    }
}