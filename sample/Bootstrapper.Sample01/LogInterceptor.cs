using System;
using System.ComponentModel;
using System.Diagnostics;
using Bootstrapper.Abstractions;
using Castle.DynamicProxy;
using Serilog;
using Serilog.Events;

namespace Bootstrapper.Sample.LogIntercept
{
    [DebuggerStepThrough]
    public class LogInterceptor : Interceptor
    {
        protected override void PreMethodProcess(IInvocation invocation)
        {
            var isDebugEnabled = Log.IsEnabled(LogEventLevel.Debug);
            var isVerboseEnabled = Log.IsEnabled(LogEventLevel.Verbose);

            if (!isDebugEnabled) return;

            Log.Debug("{Method}({Join})", invocation.Method, string.Join(",", invocation.Arguments));

            if (isVerboseEnabled)
                Log.Verbose("Input Arguments: {@Arguments}", invocation.Arguments);
        }

        protected override void PostMethodProcess(IInvocation invocation)
        {
            var isDebugEnabled = Log.IsEnabled(LogEventLevel.Debug);
            var isVerboseEnabled = Log.IsEnabled(LogEventLevel.Verbose);

            if (!isDebugEnabled) return;

            Log.Debug("Return of {FullName}.{Method}", invocation.TargetType.FullName, invocation.Method);

            if (isVerboseEnabled)
                Log.Verbose("Return Value: {@ReturnValue}", invocation.ReturnValue);
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
}
