using ImTools;

namespace Bootstrapper.Abstractions;

public static class Extensions
{
    private static readonly DefaultProxyBuilder ProxyBuilder = new DefaultProxyBuilder();

    public static void Intercept<TService, TInterceptor>(this IRegistrator registrator, object serviceKey = null)
        where TInterceptor : class, IInterceptor
    {
        registrator.Intercept<TInterceptor>(typeof(TService), serviceKey);
    }

    public static void Intercept<TInterceptor>(this IRegistrator registrator, Type serviceType, object serviceKey = null)
        where TInterceptor : class, IInterceptor
    {

        Type proxyType;
        if (serviceType.IsInterface())
        {
            proxyType = ProxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
                serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
        }
        else if (serviceType.IsClass())
        {
            proxyType = ProxyBuilder.CreateClassProxyTypeWithTarget(
                serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
        }
        else
        {
            throw new ArgumentException(
                $"Intercepted service type {serviceType} is not a supported, cause it is nor a class nor an interface");
        }

        registrator.Register(serviceType, proxyType,
            made: Made.Of(pt => pt.PublicConstructors().FindFirst(ctor => ctor.GetParameters().Length != 0),
                Parameters.Of.Type<IInterceptor[]>(typeof(TInterceptor[]))),
            setup: Setup.DecoratorOf(useDecorateeReuse: true, decorateeServiceKey: serviceKey));
    }
}