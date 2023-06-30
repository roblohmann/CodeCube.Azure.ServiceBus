namespace CodeCube.Azure.ServiceBus;

public sealed class MessageMetadata
{
    public MessageMetadata(DateTime occurredOn, string messageType, string messageTypeVersion, string sourceApplication, string operatorId, string operationId)
    {
        OccurredOn = occurredOn;
        MessageType = ValidateFilled(nameof(messageType), messageType);
        MessageTypeVersion = ValidateFilled(nameof(messageTypeVersion), messageTypeVersion);
        SourceApplication = ValidateFilled(nameof(sourceApplication), sourceApplication);
        OperatorId = operatorId;
        OperationId = operationId;
    }


    public DateTime OccurredOn { get; }

    public string MessageType { get; }

    public string MessageTypeVersion { get; }

    public string SourceApplication { get; }

    public string OperatorId { get; }

    public string OperationId { get; }

    private static string ValidateFilled(string paramName, string s)
    {
        if (string.IsNullOrWhiteSpace(s) || char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]))
        {
            throw new ArgumentOutOfRangeException(paramName, s, "Value should be filled and not start or end in whitespace");
        }

        return s;
    }
}