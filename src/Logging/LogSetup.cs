using DryIoc;

namespace Logging;

public static class LogSetup
{

    public static void Register(IRegistrator registrator, LoggerConfiguration config)
    {
        Log.Logger = config.CreateLogger();
        registrator.Register<LogInterceptor>();
    }

}