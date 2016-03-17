using Autofac;
using NServiceBus.Faults;

namespace Amazon.Store
{
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Persistence;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            var builder = new ContainerBuilder();
			// builder.RegisterType<AccountsContext>().As<IAccountsContext>();
            var container = builder.Build();

            configuration.EndpointName("amazon.store");
            configuration.UsePersistence<NHibernatePersistence>().ConnectionString(@"server=.\sqlexpress;database=amazon;integrated security=sspi;");
            configuration.UseTransport<MsmqTransport>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.UseContainer<AutofacBuilder>(f => f.ExistingLifetimeScope(container));
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<TimeoutManager>();
            configuration.EnableInstallers();

            var conventions = configuration.Conventions();
            conventions.DefiningCommandsAs(f => f.Namespace != null && f.Namespace.StartsWith("Amazon.Messages") && f.FullName.EndsWith("Command"));
            conventions.DefiningEventsAs(f => f.Namespace != null && f.Namespace.StartsWith("Amazon.Messages") && f.FullName.EndsWith("Event"));
        }
    }
}
