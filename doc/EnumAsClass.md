# Enumeration As Class

Enumeration As Class is a enum replacement. <br/>
By converting the enum value type to a value object, we can have a cleaner code implementation <br/>
This work is based on [this Microsoft Article](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types) which you should visit for more details on why we do this. <br/>
Additional functionality has been added on to ease the use of using enummeration as a class <br/>

# Getting Started

## Declaration
Inherit ``EnumAsclass`` class on to use this. <br/>
Each definition will contain and interger index and string name which needs to be explicitly declared. <br/>
The defination should be public static for access <br/>
Constructor should be private to prevent external instantiation of the value object <br/>

```
 public class SimpleSample : EnumAsClass
{
    public static readonly SimpleSample SimpleSampleIndex1 = new SimpleSample(1, "Index A");
    public static readonly SimpleSample SimpleSampleIndex2 = new SimpleSample(2, "Index B");

    private SimpleSample(int index, string name) : base(index, name) { }
}
```
## Configuration
There are a few variables that can set to change the behavior of the value object <br/>

### ExecuteWhenNoMatch

**Default value: null <br />**

Action to execute when no match is found. <br/>
Can be used to throw exception or add logging when no matching definition is found <br />
Will return value of `NoMatchResult` after execution (when no exception is thrown by ExecuteWhenNoMatch)

### NoMatchResult

Default value: Func that returns null <br />
Set this value to return a specific type when casting match is not found

### MultipleResultSameAsNoMatch
Default value: false <br />

In the rare event that same index or name is declared, multiple matches will occured <br/>
When MultipleResultSameAsNoMatch is set as false, the 1st of the multiple match will be returned. <br/>
If true, the execution of `ExecuteWhenNoMatch` will occur. <br />
This is followed by return value of `NoMatchResult` (when no exception is thrown by ExecuteWhenNoMatch)

## Implicit Conversion 
The base class already implemented implicit conversion to integer and string. 

However, addtional implementation of implicit converstion from interger and string is required:
```
public static implicit operator SimpleSample(int index) => FromIndex<SimpleSample>(index);
public static implicit operator SimpleSample(string name) => FromName<SimpleSample>(name);
```

## Indexers
Indexers can also be added to allow convenient access to the enumeration like this: ```DerivedClass[2];```
```
 public IEnumerable<DerivedClass> this[uint index] => AsBitFlags<DerivedClass>(index);

 public DerivedClass this[int index] => FromIndex<DerivedClass>(index);

 public DerivedClass this[string name] => FromName<DerivedClass>(name);
```

