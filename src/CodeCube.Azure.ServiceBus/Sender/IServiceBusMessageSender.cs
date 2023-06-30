namespace CodeCube.Azure.ServiceBus.Sender;

public interface IServiceBusMessageSender
{
    Task SendMessage(string messageId, object messageObject, MessageMetadata metadata);
}