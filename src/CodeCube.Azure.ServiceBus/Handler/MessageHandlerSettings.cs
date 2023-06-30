namespace CodeCube.Azure.ServiceBus.Handler;

public sealed class MessageHandlerSettings
{
    public string TopicName { get; set; }
    public string SubscriptionName { get; set; }
    public string QueueName { get; set; }
    public Type Handler { get; set; }
}