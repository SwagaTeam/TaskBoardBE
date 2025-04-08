﻿using Messaging.Kafka.Services.Abstractions;
using Messaging.Kafka.Services.Implementations;
using Messaging.Kafka.Settings;

namespace Messaging.Kafka
{
    public static class Extensions
    {
        public static void AddProducer<TMessage>(this IServiceCollection serviceCollection, IConfigurationSection configurationSection)
        {
            serviceCollection.Configure<KafkaSettings>(configurationSection);
            serviceCollection.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();
        }

        public static void AddConsumer<TMessage, THandler>(
            this IServiceCollection serviceCollection, IConfigurationSection configurationSection)
            where THandler : class, IMessageHandler<TMessage>
        {
            serviceCollection.Configure<KafkaSettings>(configurationSection);
            serviceCollection.AddHostedService<KafkaConsumer<TMessage>>();
            serviceCollection.AddSingleton<IMessageHandler<TMessage>, THandler>();
        }
    }
}
