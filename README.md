# dotnetwhat

# Table of Contents
- [Overview](#overview)
- [Glossary](#glossary)
- [References](#references)

# Overview

The **.NET SDK** is a set of libraries and tools for developing .NET applications. .NET applications can be written in a number of different languages. The compiler of each language must adhere to the rules laid out in the **Common Type System (CTS)** and **Common Language Specification (CLS)**.

The **CTS** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also contains a library containing the basic primitive types including char, bool, byte etc. The **CTS** also defines two main kinds of types that must be supported: value and reference types. The **CLS** is a subset of the **CTS** and defines a set of common features needed by applications.

> **Value and Reference Types and Variables**
>
> The main difference between value type and reference types are the way they are represented and how they get assigned between variables. **Variables** are simply slots of memory for storing types according to how they are represented.
>
> **Value type** objects are represented by the value of the object. When the value is assigned from one variable to another the value is copied and both variables will each contain their own copy of the value. Changing the value of one variable will not impact the value of the other variable.
>
> **Reference type** objects are represented by a reference to the actual object i.e. the object is stored at an address in memory and the reference points to the object. When the reference is assigned from one variable to another the reference is copied and both variables will point to the same object. Any changes to the object will be reflected by the variables pointing to it.

The managed code is compiled into **Microsoft intermediate language (MSIL)**, which is CPU-independent instructions that can be converted to native (CPU-specific) code.

.NET provides a runtime environment called the **Common Language Runtime (CLR)** to manage code execution. The **CLR** is a set of libraries for running .NET applications and uses a garbage collector to manage memory. It is also responsible for things like enforcing memory safety and type safety. 
The **CLR** also ensures before each method is run for the first time it’s **MSIL** is **Just In Time (JIT)** compiled to native processor-specific code. The **JIT** compiled native code for that method then gets reused.

When a .NET application is run the operating system loads the CLR, which is responsible for managing the execution of .NET code. The CLR initializes and creates the main application domain, which in turn creates the main thread. The CLR loads the applications assemblies into memory. The CLR then starts JIT compiling the MSIL code into machine code that can be executed by the computers CPU. Finally, the main thread executes the applications entry point, typically the static Main method. The **CLR** continues to provide services such as memory management, garbage collection, exception handling, and JIT compiling any method being executed for the first time.

# Glossary
* **Base Class Library  (BCL)** *- a standard set of class libraries providing implementation for general functionality*
* **Common Language Runtime (CLR)** *- .NET runtime responsible for managing code execution, memory and type safety etc.*
* **Common Language Specification (CLS)** *- subset of CTS that defines a set of common features needed by applications*
* **Common Type System (CTS)** *- defines rules all languages must follow when it comes to working with types*
* **Just-In-Time compilation (JIT)** *- at runtime the JIT compiler translates MSIL into native code, which is processor specific code*
* **Managed Code** *- code whose execution is managed by a runtime*
* **Microsoft Intermediate Language (MSIL)** *- instructions for loading, storing, initializing, and calling methods on objects, arithmetic and logical operations, control flow, direct memory access, exception handling etc*
* **.NET SDK** *-a set of libraries and tools for developing .NET applications*
* **Reference types** *- objects represented by a reference that points to where the object is stored in memory*
* **Value types** *- objects represented by the value of the object*
* **Variables** *- slot of memory that stores value and reference type objects*

# References
* **Microsoft**
  * [BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries)
  * [CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)
  * [CTS & CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)
  * [Managed Code](https://learn.microsoft.com/en-us/dotnet/standard/managed-code)
  * [Managed Execution Process](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process)
  * [Performance](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance)
  * [SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)


