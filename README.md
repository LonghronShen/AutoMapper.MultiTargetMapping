# AutoMapper.MultiTargetMapping

[![Build status](https://ci.appveyor.com/api/projects/status/tgvc6o5xwqop5dbs?svg=true)](https://ci.appveyor.com/project/LonghronShen/automapper-multitargetmapping)
![Nuget](https://img.shields.io/nuget/dt/Sprintor.AutoMapper.MultiTargetMapping.svg)

A simple multi-target mapping enhancement for *AutoMapper*.

## The Problem

Assuming that we have three types:

```C#
public class AModel
{

    public string Field1 { get; set; }

    public string Field2 { get; set; }

}

public class BModel
{

    public string Field3 { get; set; }

    public string Field4 { get; set; }

}

public class CModel
{

    public string Field5 { get; set; }

    public string Field6 { get; set; }

}
```

And we have defined the mapping [`AModel => BModel`] and [`AModel => CModel`].

A source object like this:

```C#
var source = new AModel()
{
    Field1 = "a",
    Field2 = "b"
};
```

If we want to map the source object into serveral other types, we can call the Mapper.Map function servaral times to achieve this, like:

```C#
var b = Mapper.Map<AModel, BModel>(source);
var c = Mapper.Map<AModel, CModel>(source);
...
```

Repeating these codes are so boring.

With the help of `System.Tuple` and `Destruction assignment in C# 7` features, using this library, we can now wrap things up to:

```C#
var (b1, b2) = MultiTargetMapper.Map<Tuple<BModel, BModel>>(this.Source);

// Now b1 and b2 are newly constructed objects which are converted from the source object using AutoMapper, and they are both of type BMdoel.

// No more than 8 items are OK using this way.
var (b3, b4, c1) = MultiTargetMapper.Map<Tuple<BModel, BModel, CModel>>(this.Source);
...
```

Since the predefined `System.Tuple` can hold no more than 8 items, we provide a way to make a dynamic object which contains what you want.

Sadly, since the dynamically created object's type is only known at runtime,and the destruction assignment syntax just works in compilation time, we cannot use a strongly-typed way to do so.

```C#
// If you want to map as many as possible destinations, do like this.
// This way you can get more than 8 items while Tuple just holds no more than 8 items.
var destination = MultiTargetMapper.MapDynamic(this.Source,
    typeof(BModel), typeof(BModel), typeof(CModel), typeof(CModel));

// The destination object is of dynamic type, so we cannot do a destuction over it to make new variables.
```

**Note**: The `dynamic` feature is only available on *.NET Framework 4.0*, *.NET Standard 1.1*, *.NET Standard 2.0* and *Portable Class Library, portable-net45+win8+wpa81, Profile 111*.

For more general scenarios, we also provide a batch mapping method:

```C#
var destinations = MultiTargetMapper.Map(this.Source, typeof(BModel), typeof(BModel), typeof(CModel), typeof(CModel));

// The destinations object is of IList<object>. You can iterate the collection to fetch each destination object. Ordering is the same as the given types array.
```

## Platform Support

This library helps supporting frameworks like:

- .NET Framework 3.5
- .NET Framework 4.0
- .NET Framework 4.7.1
- .NETStandard 1.1
- .NETStandard 2.0
- Windows Phone 8.0 (Silverlight)
- Windows Phone 8.1 (UWP)
- Windows 8 (UWP)
- Silverlight 5