# Configuration Manager

This library allows configuration or settings for your application. \
It also can be used for standalone domain level persistent configuration

The template can be defined in 2 ways:
 - In code
	Create a class inheriting `ISettingDefinition`  

 - JSON
	Create a json file with the definition to load


## Default Value
If or any reason, the value is not set, default value will apply.

## Accessing configuration
To set or read value, do the following: `Configuration["Section"].["Sub section"]`

## Setup

### Definition

#### Modifiers
Readonly values cannot be changed and is designed to use the value from the definition \
Specifying a value does not change its value \
Typical use case is when we want the setting on/off in only for a specific duration \
(e.g development feature that is now released) \

Indexable if False, will not be output to the json file \
However, specifying the value manually in the output file will change its value \
Intention is to use as "hidden" setting (e.g Preview feature) \

### Persistence
The configuration allow loading and saving the values to different type of persistence

#### Json file
If a incorrect name is specified and does not match the definition, it will be ignored\
If a setting is ReadOnly, it will be ignore as well \

#### In Memory
Using for snapshot or testing, the setting is serialized as a string.
