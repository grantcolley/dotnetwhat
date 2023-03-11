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

.NET is known as [managed](https://learn.microsoft.com/en-us/dotnet/standard/managed-code) because it provides a runtime environment called the **Common Language Runtime ([CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr))** to [manage code execution](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process). The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** is a set of libraries for running .NET applications and is responsible for things like enforcing type safety and memory management. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** also **Just In Time ([JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code))** compiles managed code into native processor-specific code on demand at runtime. Only code that is used gets **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to avoid wasting resources.

.NET applications can be written in different languages and the language compiler must adhere to the rules laid out in the **Common Type System ([CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))** and **Common Language Specification ([CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))**. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also has a library containing the basic primitive types including char, bool, byte etc. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** also defines two main kinds of types that must be supported: value types and reference types. The **[CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** is a subset of the **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** and defines a set of common features needed by applications.

The **[.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)** is a set of libraries and tools for developing .NET applications. .NET also has a large set of libraries called the [BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries), which provides implementation for many general types, algorithms, and utility functionality.
Code is compiled into **Microsoft Intermediate language ([MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil))**, in the form of Portable Executable files such as *.exe* and *.dll* files. **[MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)** is CPU-independent instructions that can be [JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code) compiled to native (CPU-specific) code by the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** as runtime.

When a .NET application is started the operating system loads the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** which then loads the application assemblies into memory. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** initializes and creates the main application domain, which in turn creates the main thread with a default stack size of 1MB on a 32-bit system and 4MB on a 64-bit system. The thread stack provides the thread context. The main thread executes the applications entry point, typically the static Main method, and the application starts running. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** continues to provide services such as memory management, garbage collection, exception handling, and **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiling **[MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)** code into native code.

The main thread creates the GUI and executes the [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows), which is responsible for processing and dispatching messages queued by the operating system, such as key presses and mouse clicks. Each user control is bound to the thread that creates it, which is typically the main thread, and cannot be updated by another. This is to ensure the [integrity of UI components](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8). 

The [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows), or message pump, looks something like this:

```C#
MSG msg;
while (GetMessage(&msg, NULL, 0, 0))
{ 
   TranslateMessage(&msg); 
   DispatchMessage(&msg); 
} 
```

The [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows) calls `GetMessage(&msg, NULL, 0, 0)` to check the message queue. If there is no message the thread is blocked until one arrives e.g. mouse move, mouse click or key press etc. When a message is placed in the queue the thread picks it off and calls `TranslateMessage(&msg);` to translate it into something meaningful. The message is then passed into `DispatchMessage(&msg);`, which routes it to the applicable even handler for processing e.g. `Button1_Click(object sender, EventArgs e)`. When the event has finished processing `GetMessage(&msg, NULL, 0, 0)` and the process is repeated until the application shuts down.


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
  * [Integrity of UI components](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8)
  * [Managed Code](https://learn.microsoft.com/en-us/dotnet/standard/managed-code)
  * [Managed Execution Process](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process)
  * [Performance](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance)
  * [SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)

* **Wikipedia**
  * [Message Loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows)

