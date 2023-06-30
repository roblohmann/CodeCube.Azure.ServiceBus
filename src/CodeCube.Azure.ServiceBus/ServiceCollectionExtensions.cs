using CodeCube.Azure.ServiceBus.Handler;
using CodeCube.Azure.ServiceBus.Sender;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCube.Azure.ServiceBus
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHostedServicebusProcessor(this IServiceCollection serviceCollection, Action<ServiceBusMessageHandlerOptions> configureOptions)
        {
            var servicebusOptions = new ServiceBusMessageHandlerOptions();
            configureOptions.Invoke(servicebusOptions);

            serviceCollection.AddTransient(servicebusOptions.HandlerSettings.Handler);
            
            serviceCollection.AddHostedService(serviceProvider =>
            {
                var handler = serviceProvider.GetRequiredService(servicebusOptions.HandlerSettings.Handler) as IMessageHandler;

                return new MessageProcessor(servicebusOptions, handler);
            });
        }

        public static void AddServiceBusSender(this IServiceCollection serviceCollection, string serviceBusConnectionString, string blobStorageConnectionString = null)
        {
            serviceCollection.AddSingleton<IServiceBusMessageSender>(serviceProvider =>
            {
                if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
                {
                    throw new ArgumentNullException($"{nameof(serviceBusConnectionString)} is empty");
                }

                if (!string.IsNullOrWhiteSpace(blobStorageConnectionString))
                {
                    return new ServiceBusClaimCheckSender(serviceBusConnectionString, blobStorageConnectionString);
                }

                return new ServiceBusMessageSender(serviceBusConnectionString);
            });
        }
    }
}