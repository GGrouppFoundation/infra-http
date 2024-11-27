using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GarageGroup.Infra;

partial class PrimaryHandler
{
    public static Dependency<SocketsHttpHandler> UseSocketsHttpHandler(
        this Dependency<ISocketsHttpHandlerProvider, ISocketsHttpHandlerConfiguration> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateSocketsHttpHandler);

        static SocketsHttpHandler CreateSocketsHttpHandler(
            ISocketsHttpHandlerProvider provider, ISocketsHttpHandlerConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(configuration);

            return InnerCreateSocketsHttpHandler(provider, configuration);
        }
    }

    public static Dependency<SocketsHttpHandler> UseSocketsHttpHandler(
        Func<IServiceProvider, ISocketsHttpHandlerConfiguration> configurationResolver)
    {
        ArgumentNullException.ThrowIfNull(configurationResolver);
        return InnerUseSocketsHttpHandler(configurationResolver);
    }

    public static Dependency<SocketsHttpHandler> UseStandardSocketsHttpHandler()
    {
        return InnerUseSocketsHttpHandler(ResolveConfiguration);

        static SocketsHttpHandlerConfiguration ResolveConfiguration(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            return serviceProvider.GetSocketsHttpHandlerConfigurationFromEnvironment(string.Empty);
        }
    }
}