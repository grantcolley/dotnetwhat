# dotnetwhat

# Table of Contents
- [Overview](#overview)
- [Memory Allocation](#memory-allocation)
  - [Stack](#stack)
  - [Heap](#heap)
    - [Gen0, Gen1 and Gen2](#gen0-gen1-and-gen2)
    - [LOH](#loh)
- [Glossary](#glossary)
- [References](#references)

# Overview

.NET is known as managed because it provides a runtime environment called the **Common Language Runtime (CLR)** to manage code execution. The **CLR** is a set of libraries for running .NET applications and is responsible for things like enforcing type safety and memory management. The **CLR** also **Just In Time (JIT)** compiles managed code into native processor-specific code on demand at runtime. Only code that is used gets **JIT** compiled to avoid wasting resources.

The **.NET SDK** is a set of libraries and tools for developing .NET applications. The code written by a developer is compiled into **Microsoft Intermediate language (MSIL)**, in the form of Portable Executable files such as *.exe* and *.dll* files. **Microsoft intermediate language (MSIL)** is CPU-independent instructions that can be converted to native (CPU-specific) code by the **CLR** as runtime.

.NET applications can be written in different languages and the language compiler must adhere to the rules laid out in the **Common Type System (CTS)** and **Common Language Specification (CLS)**. The **CTS** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also has a library containing the basic primitive types including char, bool, byte etc. The **CTS** also defines two main kinds of types that must be supported: value and reference types. The **CLS** is a subset of the **CTS** and defines a set of common features needed by applications.

When a .NET application is run the operating system loads the **CLR** which then loads the application assemblies into memory. The **CLR** initializes and creates the main application domain, which in turn creates the main thread with a default stack size of 1MB on a 32-bit system and 4MB on a 64-bit system. The main thread executes the applications entry point, typically the static Main method, and the application starts running. The **CLR** continues to provide services such as memory management, garbage collection, exception handling, and **JIT** compiling **MSIL** code into native code.

# Memory Allocation

> **Value and Reference Types and Variables**
>
> The main difference between value type and reference types are the way they are represented and how they get assigned between variables. **Variables** are simply slots of memory for storing types according to how they are represented.
>
> **Value type** objects are represented by the value of the object. When the value is assigned from one variable to another the value is copied and both variables will each contain their own copy of the value. Changing the value of one variable will not impact the value of the other variable.
>
> **Reference type** objects are represented by a reference to the actual object i.e. the object is stored at an address in memory and the reference points to the object. When the reference is assigned from one variable to another the reference is copied and both variables will point to the same object. Any changes to the object will be reflected by the variables pointing to it.

## Stack

## Heap
#### Gen0, Gen1 and Gen2

#### LOH


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


