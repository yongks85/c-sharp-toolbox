# Bootstrapper

Quick way to add structure and common application level features to new applications.
Base on my personal experience, this portion of the code is always done in hind sight.
That resulted in messy, hard maintain code on the long term development.
Refactoring is also seldom done as it is low in Business Value
<br/>
While always creating new application, type and referring to the same boiler code for start up gets annoying as well

## Feature
- Add DryIoc for Dependency Injection
    - Allow Microsoft Dependency Injection extensions used by other libraries
    - Make Module based registration more visible 
    - Extend for code interception
- Application level exception handling
- Argument parser (Future)
- Multi Assembly scanning for registration (Future)

## Examples
  - Sample01
    - Log Interception 
    - Simple MediatR example
    - How to use bootstrapper in Console App (Net core)
    