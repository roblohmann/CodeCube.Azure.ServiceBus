namespace CodeCube.Azure.ServiceBus.Sender
{
    internal sealed class ServiceBusClaimCheckSender : IServiceBusMessageSender
    {
        public ServiceBusClaimCheckSender(string serviceBusConnectionString, string blobStorageConnectionString)
        {
            throw new NotImplementedException();
        }

        public Task SendMessage(string messageId, object messageObject, MessageMetadata metadata)
        {
            throw new NotImplementedException();
        }
    }
}
