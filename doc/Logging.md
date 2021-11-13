# Logging Interception with DryIoc & Serilog

Easily Intercept Object with Serilog Logging

## Logging Levels
Debug: Each intercepted class function call will be logged
Verbose: Each intercepted class function and their input output parameter will be logged \

## How to use:
Add the following to the bootstrapper
```
var config = new LoggerConfiguration();
config.MinimumLevel.Verbose();
config.WriteTo.Console();
LogSetup.Register(registrator, config);
```

