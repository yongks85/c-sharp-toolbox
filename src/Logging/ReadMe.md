Intercept Object with Serilog 
Verbose: Each intercepted class function and their input output paramerter will be logged
Debug: Each intercepted class function call will be logged


How to use:
Add the following to the bootstrapper
```
var config = new LoggerConfiguration();
config.MinimumLevel.Verbose();
config.WriteTo.Console();
LogSetup.Register(registrator, config);
```

