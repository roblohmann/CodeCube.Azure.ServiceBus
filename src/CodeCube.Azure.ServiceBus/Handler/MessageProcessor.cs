using Azure.Messaging;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using System.Text;

namespace CodeCube.Azure.ServiceBus.Handler
{
    internal sealed class MessageProcessor : IHostedService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusProcessorOptions _serviceBusProcessorOptions;
        private readonly string _queueName;
        private readonly IMessageHandler _messageHandler;

        private ServiceBusProcessor? _processor;

        public MessageProcessor(ServiceBusClient serviceBusClient, ServiceBusProcessorOptions serviceBusProcessorOptions, string queueName, IMessageHandler messageHandler)
        {
            _serviceBusClient = serviceBusClient;
            _serviceBusProcessorOptions = serviceBusProcessorOptions;
            _queueName = queueName;
            _messageHandler = messageHandler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _processor = _serviceBusClient.CreateProcessor(_queueName, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processor is not null)
                await _processor.DisposeAsync().ConfigureAwait(false);

            await _serviceBusClient.DisposeAsync().ConfigureAwait(false);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));

            var messageBody = Encoding.UTF8.GetString(args.Message.Body);

            if (!string.IsNullOrEmpty(messageBody))
            {
                var messageType = (string)args.Message.ApplicationProperties[MessageProperties.MessageType];
                var messageVersion = (string)args.Message.ApplicationProperties[MessageProperties.MessageTypeVersion];
                var occurredOn = Convert.ToDateTime(args.Message.ApplicationProperties[MessageProperties.OccurredOn]);
                var sourceApplication = (string)args.Message.ApplicationProperties[MessageProperties.SourceApplication];
                ;
                if (!args.Message.ApplicationProperties.TryGetValue(MessageProperties.OperationId, out var operationId))
                {
                    operationId = Guid.Empty;
                }

                string operatorId = null;
                if (args.Message.ApplicationProperties.TryGetValue(MessageProperties.OperatorId, out var operatorIdObject))
                {
                    operatorId = operatorIdObject.ToString();
                }
                var metaData = new MessageMetadata(occurredOn, messageType, messageVersion, sourceApplication, operatorId, operationId.ToString());

                try
                {

                    await _messageHandler.HandleMessage(metaData, messageBody, cancellationToken);

                    await args.CompleteMessageAsync(args.Message, cancellationToken);
                }
                catch (PermanentException e)
                {
                    Console.WriteLine($"PermanentError thrown while processing message, message will be dead lettered: {e}");
                    await args.DeadLetterMessageAsync(args.Message, e.Message, nameof(PermanentException), cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error thrown while processing message: {e}");
                    await args.AbandonMessageAsync(args.Message, null, cancellationToken);
                }
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            var errorEvent = new ErrorEvent(
                args.ErrorSource.ToString(),
                args.FullyQualifiedNamespace,
                args.EntityPath,
                args.Identifier,
                args.Exception.Message);

            return _messageHandler.HandleError(errorEvent);
        }
    }
}
