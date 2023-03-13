# dotnetwhat

*Programming is communicating instructions. Communication is all about meaning, semantics matter.*

### Table of Contents
- [Overview](#overview)
- [Value Types, Reference Types and Variables](#value-types-reference-types-and-variables)
- [Memory Allocation](#memory-allocation)
  - [Heap](#heap)
    - [Gen0, Gen1 and Gen2](#gen0-gen1-and-gen2)
    - [LOH](#loh)
- [Glossary](#glossary)
- [References](#references)

## Overview

.NET is known as [managed](https://learn.microsoft.com/en-us/dotnet/standard/managed-code) because it provides a runtime environment called the **Common Language Runtime ([CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr))** to [manage code execution](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process). The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** is a set of libraries for running .NET applications and is responsible for things like enforcing type safety and memory management. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** also **Just In Time ([JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code))** compiles managed code into native processor-specific code on demand at runtime. Only code that is used gets **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to avoid wasting resources.

.NET applications can be written in different languages and the language compiler must adhere to the rules laid out in the **Common Type System ([CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))** and **Common Language Specification ([CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))**. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also has a library containing the basic primitive types including char, bool, byte etc. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** also defines two main kinds of types that must be supported: value types and reference types. The **[CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** is a subset of the **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** and defines a set of common features needed by applications.

The **[.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)** is a set of libraries and tools for developing .NET applications. .NET also has a large set of libraries called the **Base Class Library ([BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries))**, which provides implementation for many general types, algorithms, and utility functionality.
Code is compiled into **Microsoft Intermediate language ([MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil))**, in the form of Portable Executable files such as *.exe* and *.dll* files. **[MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)** is CPU-independent instructions for loading, storing, initializing, and calling methods on objects, arithmetic and logical operations, control flow, direct memory access, exception handling etc. **[MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)** is **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to native (CPU-specific) code by the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** as runtime.

When a .NET application is initialised the operating system loads the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)**. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** then loads the application assemblies into memory and reserves a contiguous region of address space for the application called the [managed heap](https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management#allocating-memory). The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** also creates the main application domain, which in turn creates the main thread with a default stack size of 1MB on a 32-bit system and 4MB on a 64-bit system. Every thread is allocated it's own stack memory which provides the thread context. The main thread executes the application's entry point, typically the static Main method, and the application starts running. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** continues to provide services such as memory management, garbage collection, exception handling, and **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiling **[MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)** code into native code.

The main thread creates the GUI and executes the [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows), which is responsible for processing and dispatching messages queued by the operating system, such as key presses and mouse clicks. Each user control is bound to the thread that created it, typically the main thread, and cannot be updated by another. This is to ensure the [integrity of UI components](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8). 

The [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows), or message pump, looks something like this:

```C#
// complexity removed for brevity

MSG msg;
while (GetMessage(&msg, NULL, 0, 0))
{ 
   TranslateMessage(&msg); 
   DispatchMessage(&msg); 
} 
```

The [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows) calls `GetMessage(&msg, NULL, 0, 0)` to check the message queue. If there is no message the thread is blocked until one arrives e.g. mouse move, mouse click or key press etc. When a message is placed in the queue the thread picks it off and calls `TranslateMessage(&msg);` to translate it into something meaningful. The message is then passed into `DispatchMessage(&msg);`, which routes it to the applicable even handler for processing e.g. `Button1_Click(object sender, EventArgs e)`. When the event has finished processing `GetMessage(&msg, NULL, 0, 0)` and the process is repeated until the application shuts down.

## Value Types, Reference Types and Variables

The main difference between [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) and [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) are the way they are represented and how they get assigned between variables.

[**Variables**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables) represent storage locations. C# is a type-safe language and each variable has a type that determines what values can be stored in the variable.

[**Value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) objects are represented by the value of the object. When the value is assigned from one variable to another the value is copied and both variables will each contain their own copy of the value. Changing the value of one variable will not impact the value of the other variable.

[**Reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) objects are represented by a reference to the actual object i.e. the object is stored at an address in memory and the reference points to the object. When the reference is assigned from one variable to another the reference is copied and both variables will point to the same object. Unlike variables for [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), multiple variables can point to the same [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object therefore operations on one variable can affect the object referenced by the other variable.
<br>

>  *A pimped up version of an analogy about [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) by Jon Skeet on [.NET Rocks!](https://www.dotnetrocks.com/details/881) (34m 42s)*
>
> ### A piece of paper has the address of a house written on it
> 
> The house is a reference type object in memory. The address is the reference pointing to where that object is located in memory. The piece of paper is the variable containing the address to the object in memory. 
> 
> If you copy the same address to another piece of paper, you now have two variables pointing to the same object in memory. If you were to paint the door of the house green, both pieces of paper still point to the same house which now has a green door.
> 
> You cross out the address on the first piece of paper and replace it with the address of another house. Now each piece of paper (variables) have different addresses (references) each pointing to different houses (objects). 
> 
> You throw away the second piece of paper with the address to the original house. Now no piece of paper (variable) points to the original house (object). If the garbage collector came along and finds a house (object) with no piece of paper (variable) pointing to it, the house is torn down to make space for a new object e.g. an array of flats.
<br>

## Memory Allocation
When code execution enters a method, parameters passed into the method and local variables are allocated on the threads **stack** memory. For [value type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) variables the value of the type is stored on the **stack**. For [reference type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) variables the reference to the object is stored on the **stack**, while the object is stored on the [heap](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#memory-allocation). 
<br>

> ***NOTE: [Value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) live where they are created.***
>
> While local variables and parameters that are [value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) will be stored on the **stack**, if a [reference type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object contains a member that is a [value type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) then that [value type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) member will be stored on the heap with that [reference type](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object.
<br>

Local variables and parameters are pushed onto the **stack** in the order they are created and popped off the **stack** on a last in first out (LIFO) basis. Local variables and parameters are scoped to the method in which they are created. The **stack** is self-maintaining so when the executing code leaves the method they are popped off the **stack**.



Arguments can be passed to [method parameters](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters) by value or by reference. 
**Passing by value** means passing a copy of the variable to the method. **Passing by reference** means passing access to the variable to the method by passing in the address of the variable. By default arguments are passed by value for both [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) and [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types).

```
  IL_0000:  nop
  IL_0001:  newobj     instance void [dotnetwhat.library]dotnetwhat.library.MyClass::.ctor()
  IL_0006:  stloc.0
  IL_0007:  ldc.i4.s   123
  IL_0009:  stloc.1
  IL_000a:  newobj     instance void [dotnetwhat.library]dotnetwhat.library.Foo::.ctor()
  IL_000f:  stloc.2
  IL_0010:  ldloc.0
  IL_0011:  ldloc.1
  IL_0012:  ldloc.2
  IL_0013:  callvirt   instance void [dotnetwhat.library]dotnetwhat.library.MyClass::Method1(int32,
                                                                                             class [dotnetwhat.library]dotnetwhat.library.Foo)
  IL_0018:  nop
  IL_0019:  ldloc.0
  IL_001a:  ldloca.s   V_1
  IL_001c:  ldloca.s   V_2
  IL_001e:  callvirt   instance void [dotnetwhat.library]dotnetwhat.library.MyClass::Method2(int32&,
                                                                                             class [dotnetwhat.library]dotnetwhat.library.Foo&)
  IL_0023:  nop
  IL_0024:  ret
```

In the code listing above we see the **[MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)** for loading a class called `MyClass` and two variables, an `int32` with the value `123` and an instance of a class called `Foo`. We first pass these variables **by value** to `MyClass.Method1(int32, Foo)`. We then pass the same variables **by reference** to `MyClass.Method1(int32&, Foo&)`.

In lines `IL_0011` and `IL_0012` we load a copies of the variables onto the **stack** with the instructions `ldloc.1` and `ldloc.2`. In line `IL_0013` we call `MyClass.Method1(int32, Foo)` and pass the copies of the variables into the method **by value**.

In lines `IL_001a` and `IL_001c` we load the address of the variables onto the **stack** with the instructions `ldloca.s   V_1` and `ldloca.s   V_2`. In line `IL_001e` we call `MyClass.Method1(int32&, Foo&)` and pass the variables addresses into the method **by refence**.    


### Heap
#### Gen0, Gen1 and Gen2

#### LOH


## Glossary
* **Base Class Library  (BCL)** *- a standard set of class libraries providing implementation for general functionality*
* **Common Language Runtime (CLR)** *- .NET runtime responsible for managing code execution, memory and type safety etc.*
* **Common Language Specification (CLS)** *- subset of CTS that defines a set of common features needed by applications*
* **Common Type System (CTS)** *- defines rules all languages must follow when it comes to working with types*
* **Just-In-Time compilation (JIT)** *- at runtime the JIT compiler translates MSIL into native code, which is processor specific code*
* **Managed Code** *- code whose execution is managed by a runtime*
* **Message Loop** *- responsible for processing and dispatching messages queued by the operating system, such as key presses and mouse clicks*
* **Method Parameters** *- arguments passed my value or by reference. Default is by value.*
* **Microsoft Intermediate Language (MSIL)** *- instructions for loading, storing, initializing, and calling methods on objects, arithmetic and logical operations, control flow, direct memory access, exception handling etc*
* **.NET SDK** *-a set of libraries and tools for developing .NET applications*
* **Reference types** *- objects represented by a reference that points to where the object is stored in memory*
* **Value types** *- objects represented by the value of the object*
* **Variables** *- represent storage locations*

## References
* **Microsoft**
  * [BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries)
  * [CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)
  * [CTS & CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)
  * [Integrity of UI Components](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8)
  * [Managed Code](https://learn.microsoft.com/en-us/dotnet/standard/managed-code)
  * [Managed Execution Process](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process)
  * [Memory Management](https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management)
  * [Method Parameters](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters)
  * [MSIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)
  * [Performance](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance)
  * [Reference Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types)
  * [SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)
  * [Value Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types)
  * [Variables](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables)

* **Wikipedia**
  * [Message Loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows)

* **[ChatGPT](https://chat.openai.com/chat)**
