![](https://xpandshields.azurewebsites.net/nuget/v/Xpand.XAF.Modules.SequenceGenerator.svg?&style=flat) ![](https://xpandshields.azurewebsites.net/nuget/dt/Xpand.XAF.Modules.SequenceGenerator.svg?&style=flat)

[![GitHub issues](https://xpandshields.azurewebsites.net/github/issues/eXpandFramework/expand/SequenceGenerator.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc+label%3AStandalone_xaf_modules+SequenceGenerator) [![GitHub close issues](https://xpandshields.azurewebsites.net/github/issues-closed/eXpandFramework/eXpand/SequenceGenerator.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aclosed+sort%3Aupdated-desc+label%3AStandalone_XAF_Modules+SequenceGenerator)
# About 

The `SequenceGenerator` updates Business Objects members with unique sequential values.

## Details

---

**Credits:** to [Brokero](https://www.brokero.ch/de/startseite/) that [sponsor](https://github.com/sponsors/apobekiaris) the initial implementation of this module.

---

The `SequenceGenerator` module is a well tested implementation variation of the [E2829](https://supportcenter.devexpress.com/ticket/details/e2829/how-to-generate-a-sequential-number-for-a-persistent-object-within-a-database). The module can be configure to generate unique numerical sequences per ObjectType/memberName combination. 

In details: when any XAF database transaction starts an [Explicit UnitOfWork](https://docs.devexpress.com/XPO/8921/concepts/explicit-units-of-work) is used to acquire a lock to the `SequenceStorage` table. If the table is already locked the it retries until success, if not it queries the table for all the object types that match the objects inside the transaction and assigns their binding members (e.g. a long SequenceNumber member). After the XAF transaction completes with success or with a failure the database lock is released. A long sequential number is generated only one time for new objects.

##### <u>Configuration</u>
You can configure the Sequence binding at runtime by creating instances of the `SequenceStorage` BO as shown in the next screencast.

<twitter>![hfvTo7UsCI](https://user-images.githubusercontent.com/159464/80309035-f918e500-87da-11ea-8f52-7799457213cf.gif)</twitter>

The SequenceStorage table is a normal XAF BO, therefore it is possible to create sequence bindings in code by creating instances of that object. However we do not recommend creating instances directly but use the provided API (possibly in a [ModuleUpdater](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater)). The API respects additional constrains and validations.

To generate the configuration of the screencast you can use the next snippet.

```cs
objectSpace.SetSequence<Order>(order => order.OrderID,2000);
objectSpace.SetSequence<Accessory>(accessory => accessory.AccesoryID,1000);
```

To share the same sequence between types use the `SequenceStorage.CustomType` member.

To observe the generation results you may use a call like the next one:

```cs
SequenceGeneratorService.Sequence.OfType<Order>()
.Do(DoWithOrder)
.Subscribe();

SequenceGeneratorService.Sequence.OfType<Accessory>()
.Do(DoWithAccessory)
.Subscribe();
```

---

**Limitations:**

The module works only for MSSQL, MySql, and Oracle databases.

**Possible future improvements:**

1. Provide logic to allow re-generation of a sequence for e.g. when an object is deleted or per demand.
2. Support all database providers.
3. Additional constrains e.g. based on view, on model, on object state etc.

Let us know if you want us to implement them for you, or if you have other ideas and needs.

---

## Installation 
1. First you need the nuget package so issue this command to the `VS Nuget package console` 

   `Install-Package Xpand.XAF.Modules.SequenceGenerator`.

    The above only references the dependencies and nexts steps are mandatory.

2. [Ways to Register a Module](https://documentation.devexpress.com/eXpressAppFramework/118047/Concepts/Application-Solution-Components/Ways-to-Register-a-Module)
or simply add the next call to your module constructor
    ```cs
    RequiredModuleTypes.Add(typeof(Xpand.XAF.Modules.SequenceGeneratorModule));
    ```
## Versioning
The module is **not bound** to **DevExpress versioning**, which means you can use the latest version with your old DevExpress projects [Read more](https://github.com/eXpandFramework/XAF/tree/master/tools/Xpand.VersionConverter).

The module follows the Nuget [Version Basics](https://docs.microsoft.com/en-us/nuget/reference/package-versioning#version-basics).
## Dependencies
`.NetFramework: net461`

|<!-- -->|<!-- -->
|----|----
|**DevExpress.Persistent.Base**|**Any**
 |**DevExpress.ExpressApp**|**Any**
 |**DevExpress.ExpressApp.Validation**|**Any**
 |**DevExpress.ExpressApp.Xpo**|**Any**
|Fasterflect.Xpand|2.0.7
 |JetBrains.Annotations|2019.1.3
 |System.Reactive|4.3.2
 |Xpand.Extensions|2.201.29
 |Xpand.Extensions.Reactive|2.201.29
 |Xpand.Extensions.XAF|2.201.29
 |Xpand.Extensions.XAF.Xpo|2.201.29
 |[Xpand.XAF.Modules.Reactive](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/src/Modules/Xpand.XAF.Modules.Reactive)|2.201.29
 |[Xpand.VersionConverter](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/tools/Xpand.VersionConverter)|2.201.7

## Issues-Debugging-Troubleshooting

To `Step in the source code` you need to `enable Source Server support` in your Visual Studio/Tools/Options/Debugging/Enable Source Server Support. See also [How to boost your DevExpress Debugging Experience](https://github.com/eXpandFramework/DevExpress.XAF/wiki/How-to-boost-your-DevExpress-Debugging-Experience#1-index-the-symbols-to-your-custom-devexpresss-installation-location).

If the package is installed in a way that you do not have access to uninstall it, then you can `unload` it with the next call at the constructor of your module.
```cs
Xpand.XAF.Modules.Reactive.ReactiveModuleBase.Unload(typeof(Xpand.XAF.Modules.SequenceGenerator.SequenceGeneratorModule))
```



### Tests

The module is tested on Azure for each build with these [tests](https://github.com/eXpandFramework/Packages/tree/master/src/Tests/SequenceGenerator)

