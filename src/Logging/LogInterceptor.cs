using System.ComponentModel;
using System.Diagnostics;

using Bootstrapper.Abstractions;

using Castle.DynamicProxy;

using Serilog.Events;

namespace Logging;

//TODO: To Improve:
//Show NAmespace when enter or exit or both?
//show return type?
//If no return value, do not show?

[DebuggerStepThrough]
public class LogInterceptor : Interceptor
{
    protected override void PreMethodProcess(IInvocation invocation)
    {
        var isDebugEnabled = Log.IsEnabled(LogEventLevel.Debug);
        var isVerboseEnabled = Log.IsEnabled(LogEventLevel.Verbose);

        if (!isDebugEnabled) return;

        Log.Debug("Enter {Method}({Join})", invocation.Method.Name, string.Join(",", invocation.Arguments));

        if (isVerboseEnabled)
            Log.Verbose("Input Arguments: {@Arguments}", invocation.Arguments);
    }

    protected override void PostMethodProcess(IInvocation invocation)
    {
        var isDebugEnabled = Log.IsEnabled(LogEventLevel.Debug);
        var isVerboseEnabled = Log.IsEnabled(LogEventLevel.Verbose);

        if (!isDebugEnabled) return;

        if (isVerboseEnabled)
        {
            Log.Debug("Exit {FullName}.{Method}", invocation.TargetType.FullName, invocation.Method.Name);
            Log.Verbose("Return Value: {@ReturnValue}", invocation.ReturnValue);
        }
    }

    protected override void ExceptionProcess(IInvocation invocation, Exception exception)
    {
        if (exception is WarningException)
        {
            Log.Warning("{e}", exception);
            return;
        }

        Log.Error("{e}", exception);
    }
}