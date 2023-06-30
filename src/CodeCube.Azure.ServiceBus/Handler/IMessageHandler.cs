namespace CodeCube.Azure.ServiceBus.Handler
{
    public interface IMessageHandler
    {
        /// <summary>
        /// This method is executed while handling servicebus messages.
        /// Implement your business logic here.
        ///
        /// If you want to deadletter a message throw a new <see cref="DeadletterException"/>.
        /// All other exceptiontypes are handled the default way, meaning they will be retried x-amount of times.
        /// </summary>
        /// <param name="metaData">The <see cref="MessageMetadata">Metadata</see> sent with the message.</param>
        /// <param name="messageBody">The full messagebody as json string.</param>
        /// <param name="cancellationToken"></param>
        /// <remarks>No need to complete the message, the package is handling this.</remarks>
        /// <returns></returns>
        Task HandleMessage(MessageMetadata metaData, string messageBody, CancellationToken cancellationToken);

        /// <summary>
        /// This method is executed when a message could not be properly handled.
        /// A default logmessage is provided, which can be logged as desired.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        Task HandleError(Exception exception, string logMessage);
    }
}
