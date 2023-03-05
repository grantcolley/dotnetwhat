# dotnetwhat

# Table of Contents
- [Overview](#overview)
- [Glossary](#glossary)
- [References](#references)

# Overview
.NET provides a runtime environment called the **Common Language Runtime CLR** to manage code execution. The **CLR** is a set of libraries for running .NET applications and uses a garbage collector to manage memory. It is also responsible for things like enforcing memory safety and type safety. 

The **CLR** supports many languages, each of which must adhere to rules laid out in the **Common Type System CTS** and **Common Language Specification CLS**.
The **CTS** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also contains a library containing the basic primitive types including char, bool, byte etc. The **CTS** also defines two main kinds of types that must be supported: value and reference types. The **CLS** is a subset of the **CTS** and defines a set of common features needed by applications.

> **Value and Reference Types and Variables**
>
> The main difference between value type and reference types are the way they are represented and how they get assigned between variables. **Variables** are simply slots of memory that store those types according to how they are represented.
>
> **Value type** objects are represented by the value of the object they represent. When the value is assigned from one variable to another the value is copied and both variables will each contain their own value and changing the value of one will not impact the value of the other.
>
> **Reference type** objects are represented by a reference to the actual value of the object i.e. the object is stored in memory and a reference is the address that points to the object. When the reference is assigned from one variable to another the reference is copied and both variables will point to the same object and any changes to the object will be reflected by the variables pointing to it.

The .NET SDK is a set of libraries and tools for developing .NET applications. 

# Glossary
* **Base Class Library  BCL** *- a standard set of class libraries providing implementation for general functionality*
* **Common Language Runtime CLR** *- .NET runtime responsible for managing code execution, memory and type safety etc.*
* **Common Language Specification CLS** *- subset of CTS that defines a set of common features needed by applications*
* **Common Type System CTS** *- defines rules all languages must follow when it comes to working with types*
* **Just-in-time compilation JIT** *- at runtime the JIT compiler translates MSIL into native code, which is processor specific code*
* **Managed Code** *- code whose execution is managed by a runtime*
* **.NET SDK** *-a set of libraries and tools for developing .NET applications*
* **Reference types** *- objects represented by a reference that points to where the object is stored in memory*
* **Value types** *- objects represented by the value of the object*
* **Variables** *- slot of memory that stores value and reference type objects*

# References
* **Microsoft**
  * [BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries)
  * [CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)
  * [CTS & CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)
  * [managed code](https://learn.microsoft.com/en-us/dotnet/standard/managed-code)


