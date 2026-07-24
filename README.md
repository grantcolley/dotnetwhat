# dotnetwhat

*Programming is communicating instructions. Semantics matter.*

> [!IMPORTANT]
> This is a collection of notes about **.NET**. The purpose of this `readme` is simply to consolidate content I found important or useful to me in some way, and condense them into a single page for my own personal reference.

### Table of Contents
- [Overview](#overview)
  - [CLR](#clr)
  - [CTS and CLS](#cts-and-cls)
  - [BCL](#bcl)
  - [.NET SDK](#net-sdk)
  - [CIL](#cil)
  - [Running an Application](#running-an-application)
      - [The Message Loop](#the-message-loop)
- [CPU Architecture](#cpu-architecture)
- [Memory](#memory)
  - [Variables](#variables) 
  - [Value Types](#value-types) 
  - [Reference Types](#reference-types)
  - [Memory Allocation](#memory-allocation)
  	- [Stack Memory](#stack-memory)
  	- [Heap Memory](#heap-memory)
  		- [Small Object Heap](#small-object-heap)
  		- [Large Object Heap (LOH)](#large-object-heap-loh)
  		- [Pinned Object Heap (POH)](#pinned-object-heap-poh)
  		- [Putting Heap Allocation Into Context](#putting-heap-allocation-into-context) 
  	- [Stack vs Heap](#stack-vs-heap)
    - [Workstation Heap vs Server Heaps](#workstation-heap-vs-server-heaps)
    	- [Workstation GC](#workstation-gc)
     	- [Server GC](#server-gc)
      	- [What about IIS / ASP.NET?](#what-about-iis--aspnet)
      	- [Physical vs Logical processors](#physical-vs-logical-processors)
      	- [Do AppDomains still exist?](#do-appdomains-still-exist)
      		- [.NET Framework](#net-framework)
      	 	- [In modern .NET](#in-modern-net)
  - [Releasing Memory](#releasing-memory)
  - [Releasing Unmanaged Resources](#releasing-unmanaged-resources)
  - [WeakReference Class](#weakreference-class)
  - [Memory and ASP.NET Core](#memory-and-aspnet-core)
  - [Memory Leaks and Memory Exceptions](#memory-leaks-and-memory-exceptions)
      - [HttpClient and IHttpClientFactory](#httpclient-and-ihttpclientfactory)
        - [HttpClient](#httpclient)
        - [IHttpClientFactory](#ihttpclientfactory)
      - [OutOfMemoryException](#outofmemoryexception)
      - [StackOverflowException](#stackoverflowexception)
  - [Accessing Memory underlying a Variable](#accessing-memory-underlying-a-variable)  
      - [unsafe and fixed](#unsafe-and-fixed)
      - [Memory\<T> and Span\<T>](#memoryt-and-spant)
  - [Manually Allocating Memory on the Stack](#manually-allocating-memory-on-the-stack)
- [The Memory Model](#the-memory-model) 
  - [Atomicity of Variables, Volatility and Interlocking](#atomicity-of-variables-volatility-and-interlocking)
      - [Atomic](#atomic)
      - [Interlocked](#interlocked)
      - [Volatile](#volatile)
      - [`Interlocked` vs `lock`](#interlocked-vs-lock)
      - [When to use them?](#when-to-use-them)
- [Stack Memory is Thread-safe (with caveats)](#stack-memory-is-thread-safe-with-caveats)
- [Types and Nullability](#types-and-nullability)
- [Concurrency](#concurrency)
  - [Parallelism vs Concurrency vs Asynchronous](#parallelism-vs-concurrency-vs-asynchronous)
  - [Threads](#threads)
  - [ThreadPool](#threadpool)
  - [Task and Task\<T>](#task-and-taskt)
  - [ValueTask\<T>](#valuetaskt)
  - [Task\<T> vs ValueTask\<T>](#taskt-vs-valuetaskt)
  - [`async/await`](#asyncawait)
      - [`async/await` Scheduling](#asyncawait-scheduling)
      - [Iterating with `async` Enumerables](#iterating-with-async-enumerables)
      - [Async Scenarios](#async-scenarios)
  - [Thread Safety](#thread-safety)
      - [Locks and Mutex](#locks-and-mutex)
      - [SemaphoreSlim](#semaphoreslim)
  - [FixedWindowRateLimiter](#fixedwindowratelimiter) 
- [What's in the CIL](#whats-in-the-cil)
  - [Method Parameters](#method-parameters)
  - [Boxing and Unboxing](#boxing-and-unboxing)
  - [Ref](#ref)
  - [Ref Locals](#ref-locals)
  - [Ref Returns](#ref-returns)
  - [Lambda](#lambda)
  - [Captured Variable](#captured-variable)
  - [More on Captured Variables](#more-on-captured-variables)
  - [Closing Over a Loop Variable](#closing-over-a-loop-variable)
     - [for loop](#for-loop)
     - [foreach loop](#foreach-loop)
- [How it Works Internally](#how-it-works-internally)
  - [Array](#array)
  - [List\<T>](#listt)
  - [Dictionary\<TKey,TValue>](#dictionarytkeytvalue)
  - [Records](#records)
  - [Enums](#enums)
- [Performance](#performance)
  - [Span\<T>](#spant)
  - [StringBuilder](#stringbuilder)
  - [Mark Members Static](#mark-members-static)
- [Language Integrated Query (Linq)](#language-integrated-query-linq)
  - [Query Operators](#query-operators)
  - [Deferred Execution](#deferred-execution)
  - [Fluent Syntax vs Query Expressions](#fluent-syntax-vs-query-expressions)
- [AI Agents in the IDE](#ai-agents-in-the-ide)
	- [GitHub Copilot Chat](#github-copilot-chat)
 	- [OpenAI Codex](#openai-codex)
- [CI/CD](#cicd)
- [Unit Testing](#unit-testing)
- [REST](#rest)
- [S.O.L.I.D Principles](#solid-principles)
  - [S — Single Responsibility Principle](#s--single-responsibility-principle)
  - [O — Open/Closed Principle](#o--openclosed-principle)
  - [L — Liskov Substitution Principle](#l--liskov-substitution-principle)
  - [I — Interface Segregation Principle](#i--interface-segregation-principle)
  - [D — Dependency Inversion Principle](#d--dependency-inversion-principle)
  - [Difference Between LSP and ISP](#difference-between-lsp-and-isp)
- [Big *O*](#big-o)
  - [TL;DR](#tldr)
  - [Rules of Thumb](#rules-of-thumb)
  - [Big *O* With Code Examples](#big-o-with-code-examples)
    - [Constant Time `O(1)`](#constant-time-o1)
    - [Linear Time `O(n)`](#linear-time-on)
    - [Logarithmic Time `O(log n)`](#logarithmic-time-olog-n)
    - [Quadratic Time `O(n²)`](#quadratic-time-on)
    - [Exponential Time `O(2ⁿ)`](#exponential-time-o2ⁿ)
  - [Big *O* Growth Comparison Table](#big-o-growth-comparison-table)
  - [Big *O* Summary](#big-o-summary)
- [Interview Prep](#interview-prep)
	- [Reverse an Array](#reverse-an-array)
	- [Rotate an array](#rotate-an-array)
 	  - [Three-reversal algorithm](#three-reversal-algorithm)
      - [Copy to new array](#copy-to-new-array) 
    - [Fibonnaci](#fibonnaci)
      - [Return a single number](#return-a-single-number)
      - [Iterate over a sequence](#iterate-over-a-sequence)
    - [Search algorithm](#search-algorithm)
    - [Sort algorithm](#sort-algorithm)
    - [Currency Converter](#currency-converter)
    - [Compute Latest Positions](#compute-latest-positions)
    - [Calculate Moving Average](#calculate-moving-average)
    - [Code Challenge](#code-challenge)
      - [Easy](#easy) 
        - [Two Sum](#two-sum)
        - [Rotate Array](#rotate-array)
        - [Valid Parentheses](#valid-parentheses)
        - [Remove Duplicates from Sorted Array](#remove-duplicates-from-sorted-array)
        - [Binary Search](#binary-search)
      - [Medium](#medium)
      	- [Reverse Words](#reverse-words)
        - [Longest Common Prefix](#longest-common-prefix)
        - [Merge Intervals](#merge-intervals)
        - [Group Anagrams](#group-anagrams)
        - [Maximum Subarray](#maximum-subarray)
        - [Find Missing Number](#find-missing-number)
        - [First Non-Repeating Character](#first-non-repeating-character)
      - [Hard](#hard)
      	- [Implement LRU Cache](#implement-lru-cache)
      - [Senior](#senior)
      	- [Implement Producer/Consumer](#implement-producerconsumer)
- [Glossary](#glossary)
- [References](#references)
  - [.NET Blogs](#net-blogs)
  - [Microsoft](#microsoft)
  - [Wikipedia](#wikipedia)

## Overview

The [pillars of the .NET](https://devblogs.microsoft.com/dotnet/why-dotnet/#the-pillars-of-the-net-stack) stack is the runtime, libraries and languages.

### CLR
.NET is known as [**managed**](https://learn.microsoft.com/en-us/dotnet/standard/managed-code) because it provides a runtime environment called the **Common Language Runtime ([CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr))** to [**manage code execution**](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process). The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** is a set of libraries for running .NET applications and is responsible for things like enforcing type safety and memory management. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** **Just In Time ([JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code))** compiles managed code into native processor-specific code on demand at runtime. Only code that is used gets **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to avoid wasting resources.

### CTS and CLS
.NET applications can be written in different languages, and each language compiler must adhere to the rules laid out in the **Common Type System ([CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))** and **Common Language Specification ([CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))**.

The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also has a library containing the basic primitive types including char, bool, byte etc. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** also defines the two main kinds of types that must be supported: value types and reference types.
The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** also includes rules for inheritance, interfaces, and virtual methods etc. that enables an object-oriented programming model.
The **[CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** is a subset of the **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** and defines a set of common features needed by applications.

### BCL
.NET has a large set of libraries called the **Base Class Library ([BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries))**, which provides implementation for many general types, algorithms, and utility functionality.

### .NET SDK
The **[.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)**, or *software development kit*, is a set of libraries and tools for developing .NET applications.

### CIL
Code is compiled into **Common Intermediate language ([CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language))**, in the form of Portable Executable files such as *.exe* and *.dll* files. **[CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)** is CPU-independent [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) for loading, storing, initializing, and calling methods on objects, arithmetic and logical operations, control flow, direct memory access, exception handling etc. **[CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)** is just in time **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to native, CPU-specific code, by the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** as runtime.

### Running an Application
When a .NET application is initialised the operating system loads the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)**. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** then loads the application's assemblies into memory, and reserves a contiguous region of virtual address space for the application called the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management#allocating-memory). The [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management#allocating-memory) can have an initial size of 2GB-4GB for 32-bit systems, and slightly larger for 64-bit systems. 

The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** initializes the process environment (default application domain), and starts the application’s main managed thread the application runs on. Each thread is allocated it's own stack memory, which is part of the thread context. Threads have a default stack size of 1MB. The main thread executes the application's entry point, typically the static *Main()* method, and the application starts running. 

The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** continues to provide services such as memory management, garbage collection, exception handling, and **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiling **[CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)** code into native code.

#### The Message Loop
The main thread creates the GUI and executes the [**message loop**](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows), which is responsible for processing and dispatching messages queued by the operating system, such as key presses and mouse clicks. Each user control is bound to the thread that created it, typically the main thread, and cannot be updated by another. This is to ensure the [**integrity of UI components**](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model). 

The [**message loop**](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows), or message pump, looks something like this:

```C#
// complexity removed for brevity

MSG msg;
while (GetMessage(&msg, NULL, 0, 0))
{ 
   TranslateMessage(&msg); 
   DispatchMessage(&msg); 
} 
```

The [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows) calls `GetMessage(&msg, NULL, 0, 0)`, to check the message queue. If there is no message the thread is blocked until one arrives e.g. a mouse move, mouse click or key press etc. When a message is placed in the queue the thread picks it off and calls `TranslateMessage(&msg);` to translate it into something meaningful. The message is then passed into `DispatchMessage(&msg);`, which routes it to the applicable even handler for processing e.g. `Button1_Click(object sender, EventArgs e)`. When the event has finished processing `GetMessage(&msg, NULL, 0, 0)` and the process is repeated until the application shuts down.

## CPU Architecture
`x86` and `x64` refers to the CPU architecture, and by extension the instruction set and operating system/process model. 

Every process runs in its own ***virtual address space***. A reference (pointer) in your code is just an address in that space. How large that total space is depends on whether the process is `32 bit (x86)` or `64 bit (x64)`. The entire ***virtual address space*** of the process (stack, heap, code, OS mappings, etc.) must fit inside the available space.

`x86` is shorthand for `32-bit` processors and memory addresses (pointers) are `4 bytes` (`32 bits`) long. This means the maximum number of unique memory addresses `32-bit` processors (`x86`) can access = `2³²` = `4GB`.

`x64` is shorthand for `64-bit` processors and memory addresses (pointers) are `8 bytes` (`64 bits`) long. This means the theoretical maximum directly addressable memory `64-bit` processors (`x64`) can access = `2⁶⁴` = `16 exabytes`!.

> [!NOTE]
>
> A memory address is just a binary number. 
> 
> On a `32-bit` CPU (`x86`), addresses are `32 bits` wide, meaning the CPU can generate numbers from 0 to 2³² - 1.
>
> This is exactly `4,294,967,296` unique addresses = `4GB` of **byte** addressable space.
>
> Each unique address corresponds to `1 byte` of memory. So `2³²` addresses = `4GB`
>
> This is a hardware design limitation of a `32-bit` system. The CPU simply cannot generate addresses larger than `32-bits`. If you try access memory beyond the highest `32-bit` number, it "wraps around".

Both Windows and Linux `32-bit` are limited to 4GB total address space.
- Windows default split is `2GB` user (process memory, heap, stack, code etc.) allocation and `2GB` kernel allocation.
- Linux default split is `3GB` user (process memory, heap, stack, code etc.) allocation and `1GB` kernel allocation.

> [!TIP]
>
> **Why this matters?**
>
> A database server like **SQL Server** can benifit from `x64` because it may want to cache hundreds of `GB` of data in memory. References are `16 bytes`, and `x64` objects are larger, requiring more bytes of memory because of object and header alignment (padding). On the other hand, because there is more available memory there are less GC collections. 
>
>  A small desktop app with no big memory needs may perform better on `x86`, because its objects are smaller with less memory overhead per object. References are `4 bytes`, and objects require less bytes of memory because of object and header alignment (padding). On the other hand, because address space is capped at ~2GB the GC must work in a tighter space, which can lead to more fragmentation, and more frequent collections.

## Memory

### Variables
[**Variables**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables) are simply named storage locations in memory. C# is a type-safe language, and the C# compiler guarantees that values stored in variables are always of the appropriate type. Variables store [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) and [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), the main difference between them are the way they are handled in memory.

### Value Types
[**Value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) objects include numeric types (`int`, `decimal` etc.), `char`, `bool`, `enum` and `DateTime`. Custom value types can be created using a [struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct).

[**Value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) variables store the actual value of the type in the variable e.g. `Int32 abc = 5;` will create a storage location named `abc` that can store a 32 bit `integer`, and then assign `abc` the value `5`. 
No additional type information is stored with a value type, as the type information is known at compile-time and embedded in the generated IL code.

When value type variables are assigned from one variable to another, or as an argument to a method, the value is copied. The new variable will have its own copy of the value and changing the value of one variable will not impact the value of the other variable.

> [!IMPORTANT]
> 
>  Built‑in C# primitives like `int`, `double`, `bool`, and `char` are immutable because the runtime and language design guarantee that their values can never change after creation. Any “change” you see is actually the creation of a new value.
>
> The .NET runtime (CLR) defines primitives as atomic value units.
>
> Arithmetic produces new values, not mutations.
>
> > But remember: Immutability of the value ≠ immutability of the variable.
> >
> > This means while a new value is created, the variable is is an address in memory and simply updated to store the new value.

>  [!Note]
> 
>  **[**Value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) live where they are created.**
>
> While local variables and parameters that are [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) will be stored on the **stack**, if a [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object contains a member that is a [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) then that [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) member will be stored on the heap with that [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object.
<br>

### Reference Types
[**Reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) objects come in two parts: an object which is stored in heap memory, and a reference pointing to that object. When the reference is assigned from one variable to another, the reference is copied and both variables will point to the same object. Therefore, unlike variables for [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), multiple variables can point to the same [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object in heap memory. Operations to properties on the object via one variable is visible the other variable, because it's the same object that is modified.
<br>

>  [!Note] 
>  
> ### A piece of paper with the address of a house written on it.
> 
> The house is a reference type object in heap memory. The address is the reference pointing to where that object is located in heap memory. The piece of paper is the variable containing the address pointing to the object in heap memory. 
> 
> If you copy the same address to another piece of paper (another variable), you now have two variables pointing to the same object in heap memory. If you were to paint the door of the house green, both pieces of paper still point to the same house, which now has a green door.

### Memory Allocation
.NET uses both stack and heap because it needs fast automatic memory for execution (stack) and flexible shared memory for objects and data structures (heap).

#### Stack Memory
> The Stack: Fast, Automatic, Short-Lived Memory
> - The stack is used for method call frames, local variables, and method parameters
> - Extremely fast (just moves a pointer)
> - Automatically cleaned up when a method exits (no garbage collection)
> - Thread-local (each thread has its own stack)
> - Limited in size (1MB)

There is one Stack per Thread because each thread needs its own independent execution context

When code execution enters a method, both the parameters passed into the method and the local variables declared in the method, are allocated on the threads **stack** memory. For [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) local variables, the actual value of the type is stored in **stack** memory. For [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) local variables, only the reference to the object is stored in the **stack** memory, while the object itself is stored on the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#memory-allocation). 

Local variables and method parameters are pushed onto the **stack** in the order they are created and popped off the **stack** on a last in first out (LIFO) basis. Local variables and parameters are scoped to the method in which they are created and when the executing code leaves the method they are popped off the **stack**, therefore the **stack** is self-maintaining and no garbage collection is required. 

> Why .NET Needs the Stack
> - High performance
> - Deterministic lifetime
> - Cheap memory management
> - Safe multi-threading by default

#### Heap Memory
> The Heap: Flexible, Shared, Long-Lived Memory
> - Reference types (class, object, array, string)
> - Objects that outlive a single method
> - Shared data between methods and threads
> - Dynamically sized
> - Slower to allocate than stack
> - Requires garbage collection

Local variables and method parameters that are [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) push the reference, or "pointer" to the object, onto the stack however, the object itself is always stored on the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap). While each thread has it's own stack memory, all threads share the same [**heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) memory. This allows multiple variables across different threads to reference the same object in the shared managed heap.

> Why .NET Needs the Heap
> - Dynamic lifetimes
> - Build complex object graphs
> - Share objects across threads
> - Polymorphism (Use OOP properly)
> - Large data support

The [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) consists of three heaps. The small object heap, the [**large object heap (LOH)**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap) for objects that are 85,000 bytes (85kb) and larger, which are usually arrays, and the [**Pinned Object Heap (POH)**](https://devblogs.microsoft.com/dotnet/internals-of-the-poh).

##### Small Object Heap
The small object heap is divided into three generations, 0, 1, and 2, so it can handle short-lived and long-lived objects separately for optimization reasons.
- Gen 0 - newly allocated objects that are short lived. Garbage collection is most frequent on Gen 0. 
- Gen 1 - objects that survive a collection of Gen 0 are promoted to Gen 1, which serves as a buffer between short-lived objects and long-lived objects.
- Gen 2 - objects that survive a collection of Gen 1 are considered long-lived objects and promoted to Gen 2.

##### Large Object Heap (LOH)
The [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap) is sometimes referred to as generation 3. If an object is greater than or equal to 85,000 bytes (85kb) in size, usually arrays, it's considered a large object and allocated on the [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap). This number was determined by performance tuning.

##### Pinned Object Heap (POH)
The [**Pinned Object Heap (POH)**](https://devblogs.microsoft.com/dotnet/internals-of-the-poh) is a special heap introduced in `.NET 5` for objects that are intentionally pinned for long periods of time.
Normally, the .NET GC moves objects around in memory during collection/compaction.
Pinned means: “GC is not allowed to move this object.”
During a GC collection pinned objects become obstacles. Because the GC cannot move the pinned object, so holes/fragments develop around it.
Instead of pinning objects in the normal GC heap, the POH isolates long-lived pinned objects into a separate heap.
- pinned allocations can live in the POH
- fragmentation is contained there
- normal heaps compact more efficiently

The POH is managed by the GC and collected automatically, but it is:
- not compacted
- designed specifically for pinned allocations
- separate from the normal SOH/LOH layout

Example pinning an object in the POH
```C#
byte[] buffer = GC.AllocateArray<byte>(4096, pinned: true);
```

##### Putting Heap Allocation Into Context
To put into context what goes onto the [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap), 85,000 bytes is the equivalent of the following:
|Type|85,000 bytes (85kb)|
|:--------|:-------------------------------|
|string|A string with 42,500 16bit characters, equivalent to approx. 10 x A4 pages of text|
|32 bit object reference|An array containing 21,250 references to objects on a 32 bit system|
|64 bit object reference|An array containing 10,625 references to objects on a 64 bit system|
|Int32|An array containing 21,250 integers|
|Int64|An array containing 10,625 longs|
|Decimal 128 bits (16 bytes)|An array containing 5,312 decimals|

> [!IMPORTANT]
> Stack memory is limited to 1MB.
>
> The initial size of the heap is 2GB-4GB for 32-bit systems, and slightly larger for 64-bit systems. The heap can grow (and shrink) according to the demands of the application, and is only limited by the available system memory and any restrictions imposed by the operating system and hardware.

#### Stack vs Heap
- **Stack:**
	- fixed size
 	- generally faster memory allocation and access than heap
  	- memory is often close together (contiguous). 
 	- self maintaining, memory push/popped on a LIFO basis
  	- [thread-safe with some caveats](https://github.com/grantcolley/dotnetwhat?tab=readme-ov-file#stack-memory-is-thread-safe-with-caveats)
- **Heap:**
	- dynamically sized
	- generally slower memory allocation and access than stack
 	- subject to fragmentation over time
 	- requires the garbage collector (GC) to clean up, which can cause delays
 	- not thread-safe as memory is shared among threads

If you need large memory allocation or recursive data structures, prefer heap allocation (`class` or `new`), not large `struct` or `Span<T>` on the stack.

#### Workstation Heap vs Server Heaps
##### Workstation GC
Workstation GC = 1 Application = 1 process = 1 managed heap
- Typically 1 managed heap per process

##### Server GC
Server GC = 1 Application = 1 process = multiple heaps (1 per logical processor)
- If you have 8 logical processors → 8 heaps in that process
- Dedicated GC threads handle collection, in parallel

##### What about IIS / ASP.NET?
- Under Internet Information Services (IIS), each application pool runs in its own worker process (`w3wp.exe`)
- Each worker process gets its own GC + heaps
- Multiple apps can share a pool → then they share the same GC/heaps

> [!TIP]
>
> So with IIS, apps in the same pool can allocate objects into the same managed heap, and the GC collects across that whole worker process. However, they do not normally “share objects” directly in a safe application-level sense but the underlying memory belongs to the same process. A memory leak, high allocation rate, or crash in one app can affect the others in that pool.
>
> Practical takeaway: for isolation and predictable memory behavior, put important apps in separate application pools.

##### Physical vs Logical processors
- A physical processor (core) is an actual hardware core on the CPU.
- A logical processor is what the OS exposes for scheduling threads.
- The OS schedules threads on logical processors
- With technologies like hyper-threading (Simultaneous Multithreading), one physical core can present multiple logical processors.

Example:
- 4 physical cores, no hyper-threading → 4 logical processors
- 4 physical cores with hyper-threading → 8 logical processors

##### Do AppDomains still exist?
Yes — but their role changed dramatically.

> [!TIP]
>
> Heaps are per process, not per AppDomain. Applications have their own process. Inside that process, Server GC creates one heap per logical processor. Applications do not share heap(s).

###### .NET Framework
.NET Framework supported multiple AppDomains inside a single process.
An **AppDomain** provided:
- isolation between applications/plugins
- separate assembly loading
- unloading of code
- some security boundaries

A single process could host multiple AppDomains, each running different applications/components. This was common in ASP.NET on IIS.

###### In modern .NET
In modern .NET (.NET Core / .NET 5+ / .NET 8)
From .NET 8, there is still technically an AppDomain type, but there is effectively one AppDomain per process.
The modern replacements for the traditional `AppDomain` is:
- AssemblyLoadContext → dynamic assembly loading/unloading
- processes
- containers

So when discussing GC architecture today process boundaries matter much more than AppDomains.
 
How many apps run in a process? Usually one application = one process

Examples:
- ASP.NET Core web app → one process
- Windows service → one process
- console app → one process

Each process gets:
- its own CLR/runtime instance
- its own managed heaps
- its own GC

> [!TIP]
> A “single heap” still contains:
> - Gen 0
> - Gen 1
> - Gen 2
> - LOH (Large Object Heap)
> - POH (Pinned Object Heap in newer .NET versions)

### Releasing Memory
[**Garbage collection**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#what-happens-during-a-garbage-collection) is the process of releasing and compacting [**heap memory**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) and occurs most frequently in Gen0. The [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap#loh-performance-implications) and Gen 2 are collected together, if either one's threshold is exceeded, a generation 2 collection is triggered.

Both Gen0 and Gen2 collections compact the memory, however, the large object heap (LOH) isn't compacted unless you use the [GCSettings.LargeObjectHeapCompactionMode](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.gcsettings.largeobjectheapcompactionmode) property to compact the large object heap on demand.

**Phases of Garbage Collection**
- **Suspension:** *all managed threads are suspended except for the thread that triggered the garbage collection*
- **Mark:** *the garbage collector starts at each root and follows every object reference and marks those as seen. Roots include static fields, local variables on a thread's stack, CPU registers, GC handles, and the finalize queue*
- **Compact:** *relocate objects next to each other to reduce fragmentation of the heap. Then update all references to point to the new locations*
- **Resume:** *manage threads are allowed to resume*

> [!TIP]
>
> Reference Counting
>
> Every object stores the number of references pointing to it. When the count reaches zero, the object is immediately freed.

[**Workstation GC**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#workstation-gc) collection occurs on the user thread that triggered the garbage collection and remains at the same priority.

[**Server GC**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#server-gc) collection occurs on multiple dedicated threads. On Windows, these threads run at `THREAD_PRIORITY_HIGHEST` priority level. A heap and a dedicated thread to perform garbage collection are provided for each logical CPU

[**Background GC**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/background-gc) applies only to generation 2 collections and is enabled by default. Gen 0 and 1 are collected as needed while a Gen 2 collection is in progress. Background garbage collection is performed on one or more dedicated threads, depending on whether it's workstation or server GC.

### Releasing Unmanaged Resources
The most common types of [**unmanaged resources**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/unmanaged) are objects that wrap operating system resources, such as files, windows, network connections, or database connections. Although the garbage collector is able to track the lifetime of an object that encapsulates an unmanaged resource, it doesn't know how to release and clean up the unmanaged resource.

The `protected virtual void Dispose(bool disposing)` method executes in two distinct scenarios. If disposing equals true, the method has been called by a user's code and both managed and unmanaged resources can be disposed. If disposing equals false, the method has been called from inside the finalizer and you should not reference other managed objects as only unmanaged resources can be disposed in this scenario.

If you use unmanaged resources you should implement the [**dispose pattern**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose) to free memory used by unmanaged resources. The `Dispose()` method should not be virtual as it musn't be overriden by a derived class. When disposing is finished it should call `GC.SuppressFinalize` to take the object off the finalization queue and prevents finalization code from executing a second time.

> [!WARNING]
>
> Finalizers are dangerous. Objects with finalizers get placed on a queue after a collection and a single thread works the queue one at a time. Any blocking code in a finalizer will block the queue.
> 
> Use finalizers sparingly and only when absolutely necessary. You should only explicitly provide a finalizer (destructor) when your class directly uses unmanaged resources (like file handles, native memory pointers, OS handles) that need to be cleaned up if the consumer forgets to call `Dispose()`.
>
> Only Provide a finalizer if:
> 1. Your class directly owns unmanaged resources, and
> 2. You want to ensure cleanup happens even if `Dispose()` is never called.

> [!NOTE]
>
> Why `Dispose()` should NOT be virtual:
> - It's part of the IDisposable interface, which defines void `Dispose()` as the method signature.
> - Consumers of your class or framework code expect a non-overridable, predictable behavior.
> - Making it virtual would allow derived classes to override and forget to call `base.Dispose()`, breaking the cleanup chain.
> - The correct extensibility point is `Dispose(bool disposing)`, which safely allows subclasses to add their own cleanup logic while preserving the disposal sequence.

```C#
    public class Foo: IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;

        // Track whether Dispose has been called.
        private bool disposed = false;

        // Don't make Dispose() virtual. It mustn't be overridden by a derived class.
        public void Dispose()
        {
            Dispose(true);

			// Only call GC.SuppressFinalize if explicitly implementing a finaliser.
            // GC.SuppressFinalize takes this object off the finalization queue
            // and prevents finalization code from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // 
        // 1. If disposing equals true, the method has been called by a 
        // user's code. Both managed and unmanaged resources can be disposed.
        // 
        // 2. If disposing equals false, the method has been called 
        // from inside the finalizer and you should not reference
        // other managed objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if(!this.disposed)
            {
                if(disposing)
                {
                    //TODO: Dispose managed resources here.
                }

                // Dispose unmanaged resources.
                CloseHandle(handle);
                handle = IntPtr.Zero;

                // TODO: set large fields to null.

                disposed = true;
            }
        }

        // Use interop to call the method necessary
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // The finalizer will run only if the Dispose method doesn't get called.
        // Do not provide finalizer in types derived from this class.
        ~Foo()
        {
            Dispose(false);
        }
    }
```


### WeakReference Class
The [WeakReference](https://learn.microsoft.com/en-us/dotnet/api/system.weakreference) class references an object while still allowing it to be collected by garbage collection under memory pressure. This can be useful for caching.
[IMemoryCache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/memory#use-imemorycache) uses `WeakReference`.

#### Memory and ASP.NET Core
When an **ASP.NET Core** app starts, the GC allocates heap segments where each segment is a contiguous range of memory.
**Transient** objects that are referenced during the life of a web request are short lived and remain in gen 0. Application level **singletons** will migrate to generation 2.
**GC.Collect** should not be done by production **ASP.NET Core apps**.
**Server GC** is the default **GC** for **ASP.NET Core** apps and are optimized for the server. The **GC** mode can be set explicitly in the project file or in the `runtimeconfig.json` file of the published app. 
```XML
<PropertyGroup>
  <ServerGarbageCollection>true</ServerGarbageCollection>
</PropertyGroup>
```

>  [!Note]
>
> **Server GC** `gen0` collections are less frequent than **Workstation GC**.
> 
> On a typical web server environment, CPU usage is more important than memory, therefore the Server GC is better. If memory utilization is high and CPU usage is relatively low, the Workstation GC might be more performant. For example, high density hosting several web apps where memory is scarce e.g. docker containers.
>
> See the following about [*GC using Docker and small containers*](https://learn.microsoft.com/en-us/aspnet/core/performance/memory#gc-using-docker-and-small-containers)
> 
> *...When multiple containerized apps are running on one machine, Workstation GC might be more performant than Server GC.*

### Memory Leaks and Memory Exceptions
#### HttpClient and IHttpClientFactory
##### HttpClient
Incorrectly using `HttpClient` can result in a resource leak. `HttpClient` implements `IDisposable`, but should not be disposed on every invocation. Rather, `HttpClient` should be reused.

Even when an `HttpClient` instances is disposed, the actual network connection takes some time to be released by the operating system. By continuously creating new connections, socket exhaustion can occur as each client connection requires its own client socket. 

One way to prevent socket exhaustion is to reuse the same `HttpClient` instance, however, this exposes another issue, stale DNS. This is where the DNS record still points to the old IP address of a device. HttpClient only resolves DNS entries when the connection is created, and doesn't track any time to live, specified by the DNS server.

To get around both socket exhaustion and stale DNS, create a singleton (or static) HttpClient instance and set the `PooledConnectionLifetime ` to the desired interval, which will recycle the connection.

```C#
var handler = new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(15) // Recreate every 15 minutes
};

HttpClient sharedClient = new HttpClient(handler);
```

##### IHttpClientFactory
`IHttpClientFactory` creates `HttpClient` instances and manages the pooling and lifetime of underlying `HttpClientHandler` instances. Automatic management avoids common DNS problems that occur when manually managing `HttpClient` lifetimes, including socket exhaustion and stale DNS.

`IHttpClientFactory` manages the lifetime of `HttpClientHandler` instances separately from instances of `HttpClient` that it creates. The `HttpClientHandler` instances are cached, defaulted to 2 mins, before being recycled.

Pooling `HttpClientHandler` helps reduce the risk of socket exhaustion and the refreshing process solve the DNS update problem by ensuring we don’t have long lived instances of `HttpClientHandler` and connections hanging around. 

When you call any of the `AddHttpClient` extension methods, you're adding the `IHttpClientFactory` and related services to the `IServiceCollection`.

```C#
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("name-client", httpClient =>
{
    httpClient.BaseAddress = new Uri("my-base_uri");
});
```

#### OutOfMemoryException
[**OutOfMemoryException**](https://learn.microsoft.com/en-us/dotnet/api/system.outofmemoryexception) is thrown when there isn't enough memory to continue the execution of a program. [“Out Of Memory” Does Not Refer to Physical Memory](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/out-of-memory-does-not-refer-to-physical-memory). The most common reason is there isn't a contiguous block of memory large enough for the required allocation size. Another common reason is attempting to expand a `StringBuilder` object beyond the length defined by its `StringBuilder.MaxCapacity` property.

#### StackOverflowException
In .NET, each thread has a stack of a fixed size, which can vary dependening on the platform, or be configured manually. Once a thread is created, the stack size is not resized. If you exceed it, you get a `StackOverflowException`, which is fatal and cannot be caught in .NET.

The most common reason for `StackOverflowException`:
- Deep or infinite recursion
- Large local (stack-allocated) arrays or structs

### Accessing Memory underlying a Variable 
C# code is called "verifiably safe code" because .NET tools can verify that the code is safe. Safe code creates managed objects and doesn't allow you to access memory directly. C# does, however, still allow direct memory access. `.NET Core 2.1` introduced `Memory<T>` and `Span<T>` which provide a type safe way to work with a contiguous block of memory. Prior to that, memory could be directly accessed by writing unsafe code using `unsafe` and `fixed`. The examples below show how, despite being [immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings), a [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) can be modified by directly accessing the memory storing it. The first example uses [unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) code with the `unsafe` and `fixed` keywords. The second example uses `Memory<T>` and `Span<T>`.

A [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) is a reference type with value type semantics. [Strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) store text as a readonly collection of [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char) objects. [Strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) are [immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings) i.e. once created they cannot be modified. If a [strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) variable is updated, a new [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) is created and the original is released for disposal by the garabage collector. 

#### unsafe and fixed
[Unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) code is written with the `unsafe` keyword, where you can directly access memory using [pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types). A [pointer](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types) is simply a variable that holds the memory address of another type or variable. The variable also needs to be [fixed](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/fixed) or "pinned", so the garbage collector can't move it while compacting the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap). 

[Unsafe code](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) isn't necessarily dangerous; it's just code whose safety cannot be verified.

>  [!Note]
> 
>  In order to use the `unsafe` block you must set [AllowUnsafeBlocks](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/language#allowunsafeblocks) in the project file to `true`.
>  ```XML
>  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
>  ```

In the following [C# code](https://github.com/grantcolley/dotnetwhat/blob/15f618ccf2d8f0eef09fa42f3971b1e03aa0108d/tests/TestCases.cs#L46) an immutable string is mutated by directly accessing it's values in memory using `unsafe` and `fixed`. The `unsafe` keyword allows us to create a [pointer](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types) `char* ptr` using the `fixed` statement, which gives us direct access to the value in the variable `source`, allowing us to directly replace each character in memory with a character from the variable `target`.
>  [!Warning]
> This example works because the number of characters in `source` and `target` are equal.
 ```C#
        [TestMethod]
        public void Unsafe()
        {
            // Arrange
            string source = "Hello";
            string target = "World";

            // Act
            Mutate_Using_Fixed(source, target);

            // Assert
            Assert.AreEqual(target, source);
        }

        public static void Mutate_Using_Fixed(string source, string target)
        {
            unsafe
            {
                fixed(char* ptr = source)
                {
                    for (int i = 0; i < source.Length; i++) 
                    {
                        ptr[i] = target[i];
                    }
                }
            }
        }
```

#### Memory\<T> and Span\<T>
[Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) is a [ref struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) that provides a type-safe representation of a contiguous region of memory. [Memory\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.memory-1) is similar to [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) in that it provides a type-safe representation of a contiguous region of memory, however, it is not a [ref struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) so can be placed on the managed heap. This means it doesn't share the same restrictions as [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) and can be a field in a class or used across `await` and `yield` boundaries.

The following [C# code](https://github.com/grantcolley/dotnetwhat/blob/15f618ccf2d8f0eef09fa42f3971b1e03aa0108d/tests/TestCases.cs#L60) shows how an immutable string, can be mutated by directly accessing it in memory using `Memory<T>` and `Span<T>`.

```C#
        [TestMethod]
        public void Direct_Memory_Span()
        {
            // Arrange
            string source = "Hello";
            string target = "World";

            // Act
            MutateString.Mutate_Using_Memory_Span(source, target);

            // Assert
            Assert.AreEqual(target, source);
        }

        public static void Mutate_Using_Memory_Span(string source, string target)
        {
            var memory = MemoryMarshal.AsMemory(source.AsMemory());

            for (int i = 0; i < source.Length; i++)
            {
                ref char c = ref memory.Span[i];
                c = target[i];
            }
        }
```

### Manually Allocating Memory on the Stack
[stackalloc](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/stackalloc) allocates a block of memory on the stack. Because the memory is allocated on the stack it is not [garbage collected](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/) so it doesn't have to be pinned with the `fixed` statement and is automatically discarded when the method returns.

>  [!Warning]
>
>  Allocating too much memory on the stack can result in a [StackOverflowException](https://learn.microsoft.com/en-us/dotnet/api/system.stackoverflowexception) being thrown when the execution stack exceeds the stack size.

When working with [pointer types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types) `stackalloc` must use the `unsafe` context, as can been seen in this [example](https://github.com/grantcolley/dotnetwhat/blob/810ce35178fffb9ec70ad3e29a93e76c8c7754c8/tests/TestCases.cs#L95). 
```C#
            int length = 3;
            unsafe
            {
                int* numbers = stackalloc int[length];
                for (var i = 0; i < length; i++)
                {
                    numbers[i] = i;
                }
            }
```

The [preferred approach](https://github.com/grantcolley/dotnetwhat/blob/810ce35178fffb9ec70ad3e29a93e76c8c7754c8/tests/TestCases.cs#L117) is to assign a stack allocated memory block to a [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span) which doesn't require the `unsafe` keyword.
```C#
            int length = 3;
            Span<int> numbers = stackalloc int[length];
            for (var i = 0; i < length; i++)
            {
                numbers[i] = i;
            }
```


## The Memory Model

> [!IMPORTANT]
>
> The following is an extract from [**Atomic memory accesses**](https://github.com/dotnet/runtime/blob/main/docs/design/specs/Memory-model.md#atomic-memory-accesses)
> 
> Memory accesses to properly aligned data of primitive and Enum types with sizes up to the platform pointer size are always atomic. The value that is observed is always a result of complete read and write operations.
>
> Primitive types: `bool`, `char`, `int8`, `uint8`, `int16`, `uint16`, `int32`, `uint32`, `int64`, `uint64`, `float32`, `float64`, native `int`, native unsigned `int`.
> 
> Managed references are always aligned to their size on the given platform and accesses are atomic.

>  [!Note]
>
> The C# Language Specification states:
>
> [9.6 Atomicity of variable references](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables#96-atomicity-of-variable-references)
>
>*…Reads and writes of the following data types shall be atomic: bool, char, byte, sbyte, short, ushort, uint, int, float, and reference types. In addition, reads and writes of enum types with an underlying type in the previous list shall also be atomic. Reads and writes of other types, including long, ulong, double, and decimal, as well as user-defined types, need not be atomic….*

> [!Note]
> 
> Why Architecture Matters
>
> *"The CLI guarantees that reads and writes of variables of value types that are the size (or smaller) of the processor’s natural pointer size are atomic."*
> <br>
> Read *[Atomicity, volatility and immutability are different, part two](https://ericlippert.com/2011/05/31/atomicity-volatility-and-immutability-are-different-part-two/) by Eric Lippert*
> 
> So, read operations of a 64-bit `long` on 64-bit systems are already atomic; however, on a 32-bit system a 64-bit `long` is usually stored as two 32-bit chunks, so read operations are typically done in two 32-bit steps.

### Atomicity of Variables, Volatility and Interlocking

##### Atomic
Atomic simply means a read from memory, or a write to memory will be done in one single step. So, when you assign a variable, the assignment happens in a single step, and likewise with reading a variable i.e. assigning only half a variable value in one step is not atomic, and likewise with reading only half a variable.

This looks innocent, but it is not atomic. It actually performs three operations.
```C#
count++;

// It is not atomic. It actually performs three operations:
// Read count
// Add one
// Write count
```

> [!TIP]
> 
> The following methods perform atomic memory accesses regardless of the platform when the location of the variable is managed by the runtime.
> - `System.Threading.Interlocked` methods
> - `System.Threading.Volatile` methods
> 
> Example: `Volatile.Read<double>(ref location)` on a 32 bit platform is atomic, while an ordinary read of location may not be.

> [!Warning]
>
> **Atomic reads and writes and thread safety**
>
>Atomic reads and writes do not mean the variable is thread safe. It is entirely possible for one thread on a CPU to read a variable, while another is concurrently writing to it, resulting in the value returned in a corrupted state. As a result i.e. a reading thread could observe a torn value consisting of pieces of different values.
>
>Also, in the case of reference types, the atomicity is only on the reading of the reference, not the object itself, which can be accessed and modified by other threads.
>
>Locking limits access to a variable to a single thread at a time and is the safest way to prevent race conditions and ensure data consistency when multiple threads attempt to read or write shared data concurrently.

##### Interlocked
`Interlocked` performs atomic read-modify-write operations.

```C#
// Instead of this (performs three operations):
count++;

// Do this (performs the increment as one indivisible instruction):
Interlocked.Increment(ref count);
```
`Interlocked` methods:
```C#
Interlocked.Increment(ref counter);
Interlocked.Decrement(ref counter);
Interlocked.Exchange(ref state, 1);
Interlocked.CompareExchange(ref state, 1, 0); //Change to 1 only if current value equals 0.
```

##### Volatile
volatile tells the compiler and CPU:

> "Always read this value directly from memory, and don't reorder accesses around it."

Example:
```C#
private volatile bool _stopRequested;

_stopRequested = true;

while (!_stopRequested)
{
    // work
}
```
Without `volatile`, the compiler or CPU might optimise this into:
```C#
bool cached = _stopRequested;

while (!cached)
{
    // work
}
```
`volatile` prevents that.

`Volatile` guarantees:
- latest writes become visible
- reads are not cached in registers
- memory ordering around the access

##### Interlocked vs lock
```C#
// Consider this...
lock (_gate)
{
    count++;
}

// versus this...
Interlocked.Increment(ref count);
```
`Interlocked` is:
- much faster
- no blocking
- no context switching
But only works for simple operations.

If you need to update several variables together, use a `lock`.

##### When to use them?
| Problem                                      | Solution                                                   |
| -------------------------------------------- | ---------------------------------------------------------- |
| Simple read/write of a naturally atomic type | Nothing extra (atomicity is already provided)              |
| Publish a flag between threads               | `volatile` (or `Volatile.Read`/`Volatile.Write`)           |
| Counter                                      | `Interlocked.Increment()`                                  |
| Swap references                              | `Interlocked.Exchange()`                                   |
| One-time initialization                      | `Interlocked.CompareExchange()`                            |
| Update multiple values consistently          | `lock` or another synchronization primitive                |
| Queue, cache, dictionary, list modifications | `lock`, `ReaderWriterLockSlim`, or a concurrent collection |

> [!TIP]
>
> In new C# code, `volatile` is relatively uncommon. The `System.Threading.Volatile` class (`Volatile.Read` and `Volatile.Write`) is often preferred because it makes memory-ordering intent explicit at the point of access. For most state coordination, you'll more commonly reach for `Interlocked` or a `lock`, depending on whether you need a single atomic operation or to protect a larger critical section.

## Stack Memory is Thread-safe (with caveats)
Stack memory is thread-safe per thread because each thread has its own call stack that no other thread can access. This means that local variables, method call frames, and arguments passed to methods are stored in a stack that's private to the thread.

There are caveats to be aware of:
- **Captured variables in closures:** local variable captured by a lambda or anonymous method might get hoisted to the heap, and not remain stack-allocated.
- **Ref/out parameters and unsafe code:** passing references (e.g., `ref`, `out`, or pointers) from the stack to another thread, breaks thread-safety.
- **Async/await:** local variables declared before an `await` might be moved to the heap, invalidating stack-safety.

> [!WARNING]
>
> Stack memory is thread-local and not shared by design. However, you can break this isolation by doing something like:
>
> async/await
> ```C#
> async Task Demo()
> {
>     int counter = 0;
>     await Task.Delay(100); // 'counter' may now be on the heap
>     counter++;
> }
> ```
>
> Ref/out
> ```C#
> void Dangerous(ref int x)
> {
>    Task.Run(() =>
>    {
>        x = 42; // Accessing another thread's stack memory
>    });
>}
> ```

## Types and Nullability
Reference types can be assigned the literal `null`, meaning it doesn't point to any object on the heap.

Value types on the other hand must contain a value, and so a bitwise zeroing occurs when it is default-initialized - which basically means the runtime sets all its bits in memory to zero. So the default for both `int` and `bool` is effectively `0`, which in the case of `bool` is the same as `false`.

You can create a nullable Value type using a special struct `Nullable<T>` e.g. `Nullable<int>`. A bitwise zeroing occurs when a `Nullable<T>` is default-initialized, setting both `value` and `hasFlag` to zero. While the default for `value` is still a bitwise zeroing, it gives the appearance of being `null` because it isn't accessible.

> [!Note]
>
> Local variables that are `Nullable<T>` are still allocated on the stack.

> [!Important]
>
> When the variable is set to another value a new instance of `Nullable<T>` is created.
>
> ```C#
> int? n = default;
> Console.WriteLine(n.HasValue); // False
> 
> n = 5;
> Console.WriteLine(n.HasValue); // True
> Console.WriteLine(n.Value);    // 5
> ```

> [!Tip]
>
> `Nullable<T>` value types are particularly useful when working with databases and nullable data types.

```C#
namespace System
{
    [Serializable]
    public struct Nullable<T> where T : struct
    {
        private bool hasValue;
        internal T value;

        public Nullable(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public bool HasValue
        {
            get { return hasValue; }
        }

        public T Value
        {
            get
            {
                if (!hasValue)
                    throw new InvalidOperationException("Nullable object must have a value.");
                return value;
            }
        }

        public static implicit operator Nullable<T>(T value)
        {
            return new Nullable<T>(value); // changing the value creates a new instance
        }
    }
}
```

## Concurrency
The operating system runs code on threads. Threads execute independently from each other and are each allocated stack memory for their context. This is where a method's local variables and arguments are stored.
Threads can run concurrently. Physical concurrency is when multiple threads are run in parallel on multiple CPU's. Logical concurrency is when multiple threads are interleaved on a single CPU.

>  [!Note]
> 
> By default, there is no persistent relation between threads and specific CPU cores. The operating system's scheduler is responsible for managing which core a thread runs on, and it typically moves threads between cores to balance the workload and optimize performance.
>
> 
> Read [About Processes and Threads](https://learn.microsoft.com/en-us/windows/win32/procthread/about-processes-and-threads)
> 
> *...A thread is the entity within a process that can be scheduled for execution. All threads of a process share its virtual address space and system resources. In addition, each thread maintains exception handlers, a scheduling priority, thread local storage, a unique thread identifier, and a set of structures the system will use to save the thread context until it is scheduled. The thread context includes the thread's set of machine registers, the kernel stack, a thread environment block, and a user stack in the address space of the thread's process. Threads can also have their own security context, which can be used for impersonating clients....*
> 

#### Parallelism vs Concurrency vs Asynchronous
**Concurrency** is about dealing with multiple tasks at once, but not necessarily simultaneously, typically by switching between them. Concurrency is achieved through multithreading. You might have multiple threads interleaving execution on a single CPU core.

Key Point: Tasks are in progress at the same time, but may not run literally at the same moment.
```C#
Thread t1 = new Thread(SomeWork);
Thread t2 = new Thread(SomeOtherWork);
t1.Start();
t2.Start();
```

**Parallelism** is a type of concurrency where multiple tasks run at the same time, often on multiple cores. It’s about doing multiple things simultaneously to speed up computation. Parallelism is used for data processing or CPU-bound operations.

Key Point: Tasks are executed in true simultaneous fashion (especially on multicore CPUs) to improve performance.
```C#
Parallel.For(0, 100, i =>
{
    // This work happens in parallel
    DoWork(i);
});
```

**Asynchronous** programming is about non-blocking operations — your code can perform work while waiting for I/O-bound operations (like file `I/O`, `HTTP` requests, or database queries) to complete.

Key Point: It’s not necessarily multithreaded or parallel — it's about allowing other work to continue while waiting.
```C#
public async Task GetDataAsync()
{
    var result = await httpClient.GetStringAsync("https://example.com");
    Console.WriteLine(result);
}
```

#### Threads
When creating a [Thread](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread), pass into it's constructor a callback to the code to execute. The [Thread](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread) can then be configured e.g. set its `thread.IsBackground = true`. Start running a [Thread](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread) by calling `thread.Start()`, optionally passing into it a parameter of type `object`.

>  [!Note]
>
> *Threads don't return values. You can call a method that has parameter of type `object` e.g. `object stateInfo`
> but the return type of the method must be void.*

[Threads](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread) are only suitable for long running code and when it’s properties need to be configured. Do not use [Threads](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread) for asynchronous code or short running code because creating and destroying [Threads](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread) is costly

```C#
        public void RunThread()
        {
            var message = "Hello World!";

            var thread = new Thread(WriteToConsole);
            thread.IsBackground = true;
            thread.Start(message);            
        }

        private static void WriteToConsole(object stateInfo)
        {
            Console.WriteLine(stateInfo);
        }
```

#### ThreadPool
The [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) contains a pool of pre-existing threads waiting in the background. They are optimised for short running code where the same thread can pick up multiple tasks one after the other. When all thread on the [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) is in use then any new requests must wait until one becomes free. Unlike when you create a new thread, you can't change the properties of an existing thread from the [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool).

>  [!Note]
>
> If the [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) is used for long running code then the thread is taken out of rotation.

>  [!Warning]
> 
> *When [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) threads are rotated they do not clear local storage or fields marked with the [ThreadStaticAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.threadstaticattribute). Therefore, if a method examines thread local storage or fields marked with the ThreadStaticAttribute it may find values left over from previous use of the [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) thread.*
>

The [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) uses background threads that do not keep the application running if all foreground threads finish.

```C#
        public void RunThreadFromThreadPool()
        {
            var message = "Hello World!";

            ThreadPool.QueueUserWorkItem(WriteToConsole, message);
        }

        private static void WriteToConsole(object stateInfo)
        {
            Console.WriteLine(stateInfo);
        }
```

Updating a UI control on the UI thread can be done by calling the controls `Dispatcher` like this:

```C#
private void button1_Click(object sender, RoutedEventArgs e)
{
    ThreadPool.QueueUserWorkItem(_ =>
    {
        string message = ComputeMessage();

        button1.Dispatcher.InvokeAsync(() =>
        {
            button1.Content = message;
        });
    });
}
```

#### Task and Task\<T>
A **Task** is a data structure that represents the eventual completion of an asynchronous operation.

[Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) represents an asynchronous operation while [Task\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) represents and asynchronous operation that returns a value of type `T`.

>  [!TIP]
> 
> Read [How Async/Await Really Works in C#](https://devblogs.microsoft.com/dotnet/how-async-await-really-works/)
>
> *...At its heart, a Task is just a data structure that represents the eventual completion of some asynchronous operation (other frameworks call a similar type a “promise” or a “future”)....*

Calling [Task.Run](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run) or [Task.Factory.StartNew](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskfactory.startnew) will execute a method on the [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool). A task exposes a `GetAwaiter` method, which gets an awaiter to await the task i.e. let the caller know when the task is finished. The `awaiter` also lets the caller attach a *Continuation*, which tells what needs to be executed next. 
Ultimately, the task is able to tell you if a thread on the [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) has completed executing the method, if an exception occurred and, crucially, because a task supports a continuation, it can tell what needs to be called on completion. 
The [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool) executes the method while task synchronises everything to ensure the continuation is invoked.

[Task.Run](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run) queues the specified method to run on the ThreadPool using the default task scheduler and default [TaskCreationOptions](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcreationoptions), and returns a Task or Task<T> handle for that method.

[Task.Factory.StartNew](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskfactory.startnew) gives you fine grained control including specifying [TaskCreationOptions](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcreationoptions), passing parameters such as a [CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken), and controlling the [Task Scheduler]( https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskscheduler).

A [Task Scheduler]( https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskscheduler) ensures that the work of a task is eventually executed. The default task scheduler uses the ThreadPool. 

>  [!Note]
> 
> Read [Task.Run vs Task.Factory.StartNew](https://devblogs.microsoft.com/pfxteam/task-run-vs-task-factory-startnew/)
>
> *...Task.Run in no way obsoletes Task.Factory.StartNew, but rather should simply be thought of as a quick way to use Task.Factory.StartNew without needing to specify a bunch of parameters.  It’s a shortcut...*

```C#
        public void RunTask()
        {
            var message = "Hello World!";

            _ = Task.Run(() => WriteToConsole(message));

            // this does the same thing as Task.Run()
            _ = Task.Factory.StartNew(() => WriteToConsole(message),
                    CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        private static void WriteToConsole(string stateInfo)
        {
            Console.WriteLine(stateInfo);
        }
```

Tasks use [AggregateException](https://learn.microsoft.com/en-us/dotnet/api/system.aggregateexception) to consolidate multiple failures into a single, throwable exception object. Each exception can be handled by calling [AggregateException.Handle](https://learn.microsoft.com/en-us/dotnet/api/system.aggregateexception.handle). [AggregateException.Flatten](), on the otherhand, recursively flattens all instances of [AggregateException](https://learn.microsoft.com/en-us/dotnet/api/system.aggregateexception) exceptions that are *inner exceptions* of the current AggregateException instance.

```C#
try
{
    Func<int, int, int> divide = (x, y) => x / y;
    var task = Task.Run(() => divide(10, 0));
    var result = task.Result;
}
catch (AggregateException ae)
{
    ae.Handle(e =>
    {
        if (e is DivideByZeroException)
        {
            // do something...
        }
        else
        {
            // do something else...
        }
    });
}
```


|Type|Description|
|----------------------------:|-------------------------------------------------------------------------------------|
|[Task.CompletedTask](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.completedtask)|Gets a task that has already completed successfully.|
|[Task.FromResult\<TResult>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.fromresult)|Creates a Task\<TResult> that's completed successfully returning the specified \<TResult>. The method is commonly used when the return value of a task is immediately known without executing a longer code path.|
|[TaskCompletionSource\<TResult>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource-1)|Represents the producer side of a [Task\<TResult>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1). In many scenarios, it is useful to enable a [Task\<TResult>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) to represent an external asynchronous operation. TaskCompletionSource<TResult> is provided for this purpose. It enables the creation of a task that can be handed out to consumers. It doesn't tie up a thread.|

#### ValueTask\<T>
[Value Task\<T>](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/) is the struct equivalent of `Task<T>`, although much more limited than `Task<T>`. It was created to help improve asynchronous performance where decreased allocation overhead is important.

`ValueTask<T>` is a struct that can represent either:
- a synchronously available result, or
- an existing `Task<T>`.

Why does `ValueTask<T>` exist?
Imagine the following cache. Every cache hit still creates a `Task<Customer>`, which requires an allocation on the heap.
```C#
public async Task<Customer> GetCustomerAsync(int id)
{
    if (_cache.TryGetValue(id, out var customer))
        return customer;

    return await LoadCustomerFromDatabaseAsync(id);
}
```
With `ValueTask<Customer>` a cache hit produces no heap allocation.
```C#
public ValueTask<Customer> GetCustomerAsync(int id)
{
    if (_cache.TryGetValue(id, out var customer))
        return ValueTask.FromResult(customer);

    return new ValueTask<Customer>(LoadCustomerFromDatabaseAsync(id));
}
```

> [!TIP]
> Calling `.AsTask()` may allocate, reducing the benefit of `ValueTask<T>`.

#### Task\<T> vs ValueTask\<T>
When should you use each?

Use `Task<T>` (about 95% of the time). Most application code should simply return `Task<T>`.

Examples:
- ASP.NET Core controllers
- Web APIs
- Services
- Database access
- File I/O
- Network I/O

Use `ValueTask<T>` when all of the following are true:
- the method is called very frequently,
- it often completes synchronously,
- profiling shows `Task` allocations are a measurable cost,
- callers only `await` the result once.

Typical examples include:
- high-performance libraries
- networking frameworks
- parsers
- channels
- pipelines
- caches
- object pools

Rule of thumb
- Default to `Task<T>`.
- Use `ValueTask<T>` only after profiling demonstrates that avoiding `Task` allocations provides a meaningful performance benefit. It is a specialized optimization, not a general replacement for `Task<T>`.

#### `async/await`
A [Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) exposes a `GetAwaiter` method to which the caller can attach a *Continuation*.

The `await` keyword simplifies attaching the continuation.

Consider the following:

```C#

Task task = Task.Run(() => "Hello World!")
    .ContinueWith(antecedent =>
    {
        Console.WriteLine(antecedent.Result);
    });

// using async/await the code above can be simplified into the following...

string message = await Task.Run(() => "Hello World!");
Console.WriteLine(message);

// because behind the scene the compiler does something along the lines of this...

Task<string> task = Task.Run(() => "Hello World!");
TaskAwaiter<string> awaiter = task.GetAwaiter();
awaiter.OnCompleted(() =>
{
    string message = awaiter.GetResult();
    Console.WriteLine(message);
});
```

![async/await flowchart](/readme-images/async-await-flowchart.png?raw=true "async/await flowchart")

By default, awaiting a task will attempt to capture the scheduler from `SynchronisationContext.Current` or `TaskScheduler.Current`. When the callback is ready to be invoked, it’ll use the captured scheduler if available. 
`ConfigureAwait(continueOnCapturedContext: false)` avoids forcing the callback to be invoked on the original context or scheduler. ConfigureAwait(continueOnCapturedContext: true)
`ConfigureAwait(true)` does nothing meaninglful, except to explicitly show not using `ConfigureAwait(false)` is inentional e.g. to silence static analysis warnings.

##### `async/await` Scheduling
Three core concepts involved in async/await scheduling in .NET:
- `SynchronizationContext` — “After an `await`, do I need to resume on a specific thread?”
- `TaskScheduler` — “Who schedules my continuation?”
- **ThreadPool / Threads** — “Where does code actually execute?”

Understanding how these three interact fully explains where your async code runs and why.

`SynchronizationContext` — “After an `await`, do I need to resume on a specific thread?”
If a `SynchronizationContext.Current` exists the runtime captures it and resumes the continuation through that context.

`TaskScheduler` — “Who schedules my continuation?”
If no `SynchronizationContext` exists `await` falls back to the `TaskScheduler.Default` and continuation runs on a ThreadPool thread.

**ThreadPool / Threads** — “Where does code actually execute?”
- **Threads** are the physical execution units of your program which includes:
	 - Stack
	 - CPU core
	 - Instruction execution.
- **ThreadPool** is a shared pool of worker threads, dynamically sized and optimized for short-lived work, and esed by `Async` continuations and `Task.Run`.

>  [!Note]
>
> Code after the `await` is not guaranteed to always run on the same thread `await` was called.
>
> Calling `await` on a UI thread is a special case. If `await` is called on the UI thread, code that runs after the await will continue on the UI thread.  
>
> Async methods do NOT stay on one stack.
> When an await happens:
> - The current stack frame is unwound
> - State is stored on the heap
> - When resumed, execution may continue on:
> 	- A different thread
> 	- A different stack
> 
> When an await completes, the runtime decides where to resume based on:
> - Is there a `SynchronizationContext`?
> - Did you use `ConfigureAwait(false)`?
> 
> `ConfigureAwait(false)` is the default behavior when there is no `SynchronizationContext`, and explicitly tells .NET “I do not care about resuming on the original context.” and will:
> - Skip context capture,
> - Always resume on a ThreadPool thread
> - Never resume on the UI thread
> 
> `ConfigureAwait(true)` is the default behavior on the UI thread because it has `SynchronizationContext`. UI apps have a `SynchronizationContext` because UI components can only be accessed on the UI thread and the context forces the continuation back to that thread.

>  [!Note]
>
> In .NET, asynchronous I/O operations are built on top of lower-level system APIs that handle the actual I/O operations in a non-blocking manner. These system APIs are often part of the operating system and are exposed through various mechanisms depending on the platform (Windows, Linux, macOS, etc.).
>
> .NET abstracts these lower-level APIs through its own asynchronous I/O APIs, which are part of the Base Class Library (BCL). Here are some examples:
> * **FileStream**: The `FileStream` class in .NET provides methods like `ReadAsync` and `WriteAsync` that internally use platform-specific asynchronous I/O mechanisms.
> * **Sockets**: The `Socket` class provides methods like `ReceiveAsync` and `SendAsync`, which are built on top of the underlying network APIs provided by the OS.
> * **HttpClient**: The `HttpClient` class for HTTP operations uses asynchronous methods for network I/O, relying on the lower-level HTTP stack provided by the OS.
>
> These lower-level APIs allow .NET to provide a high-level, easy-to-use abstraction for performing efficient asynchronous I/O operations e.g. using `async/await`

##### Iterating with `async` Enumerables

> [!TIP]
>
> Read [Iterating with Async Enumerables](https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8)

##### Async Scenarios
If your code implements I/O-bound scenarios to support network data requests, database access, or file system read/writes, asynchronous programming is the best approach. You can also write asynchronous code for CPU-bound scenarios like expensive calculations.

The `Task` and `Task<T>` objects represent the core of asynchronous programming. These objects are used to model asynchronous operations by supporting the `async` and `await` keywords. In most cases, the model is fairly simple for both I/O-bound and CPU-bound scenarios. Inside an async method:
- I/O-bound code starts an operation represented by a `Task` or `Task<T>` object within the async method.
- CPU-bound code starts an operation on a background thread with the `Task.Run` method.

> [!TIP]
>
> Read [Async scenarios](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/async-scenarios)

#### Thread Safety
##### Locks and Mutex
Locking limits access to a variable to a single thread at a time and is the safest way to prevent race conditions and ensure data consistency when multiple threads attempt to read or write shared data concurrently.

Mutex, or "mutual exclusion" is synchronizing access to shared state from competing threads by first locking it, then releasing the lock when it is finished. Competing threads must wait for the lock to be release, before accessing the shared state.

```C#
private object _lockObj = new object();
private int _counter = 0;

public void Multithread_Increment()
{
    lock(_lockObj)
    {
        _counter++;
    }
}
```

##### SemaphoreSlim
`SemaphoreSlim` is a lightweight synchronization primitive in .NET that limits the number of threads (or asynchronous operations) that can access a resource at the same time.

Unlike `lock`, which only allows one thread into a critical section, `SemaphoreSlim` can allow `N` concurrent threads.

A `lock` cannot be awaited, `SemaphoreSlim` was designed specifically to support asynchronous code and can be awaited. Using `SemaphoreSlim` as an async lock is one of the most common uses.

```C#
private readonly SemaphoreSlim _semaphore = new(initialCount: 1, maxCount: 1);

public async Task DoWorkAsync()
{
    await _semaphore.WaitAsync();

    try
    {
        // Only one caller can be here.
    }
    finally
    {
		// Always call Release() in a finally block.
        _semaphore.Release();
    }
}
```
> [!WARNING]
>
> Always call `Release()` in a `finally` block.

#### FixedWindowRateLimiter
`FixedWindowRateLimiter` is part of the `System.Threading.RateLimiting` namespace (introduced in `.NET 7`). It limits the number of operations allowed during a fixed time window. Once the window expires, the permit count is reset.

It is useful for scenarios such as:
- Limiting API requests
- Restricting concurrent background jobs
- Throttling access to expensive resources
- Preventing excessive retries

This example allows 5 operations every 10 seconds.
```C#
using System;
using System.Threading.RateLimiting;

FixedWindowRateLimiter rateLimiter = new(
    new FixedWindowRateLimiterOptions
    {
        PermitLimit = 5,
        Window = TimeSpan.FromSeconds(10),
        QueueLimit = 0,
        AutoReplenishment = true
    });

for (int i = 1; i <= 10; i++)
{
    using RateLimitLease lease = await rateLimiter.AcquireAsync();

    if (lease.IsAcquired)
    {
        Console.WriteLine($"{DateTime.Now:T} Request {i} allowed");
    }
    else
    {
        Console.WriteLine($"{DateTime.Now:T} Request {i} rejected");
    }
}
```

## What's in the CIL

#### Method Parameters
Arguments can be passed to [**method parameters**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters) by value or by reference.
**Passing by value**, which is the default for both [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) and [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), means the argument passes a copy of the variable into the method. **Passing by reference**, using the [ref](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref) keyword, means the argument passes the address of the variable into the method.

>  [!Note]
>
> Parameters can also be passed using the [**out**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier) keyword and the [**in**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier) keyword. Both pass by [ref](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref), however each has slightly different behavior.  
>
> With the [**out**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier) keyword an argument is passed by [ref](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref) and it must be assigned a value inside the called method.
>
> With the [**in**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier) keyword an argument is passed by [ref](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref) but it cannot be modified inside the called method.

Example [C# code](https://github.com/grantcolley/dotnetwhat/blob/810ce35178fffb9ec70ad3e29a93e76c8c7754c8/tests/TestCases.cs#L7) passing arguments to method parameters **by value** and **by reference** and the compiled [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions):
``` C#
  // C# code
  MyClass myClass = new MyClass();
  int param = 123;
  Foo foo = new Foo();

  myClass.Method1(param, foo);
  myClass.Method2(ref param, ref foo);

  // Compiled into CIL 
  .locals init (class [dotnetwhat.library]dotnetwhat.library.MyClass V_0,
           int32 V_1,
           class [dotnetwhat.library]dotnetwhat.library.Foo V_2)  
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

In the code listing above we see the [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) for loading a class called `MyClass` and two variables, an `int32` with the value `123` and an instance of a class called `Foo`. We first pass these variables **by value** to `MyClass.Method1(int32, Foo)`. We then pass the same variables **by reference** to `MyClass.Method1(int32&, Foo&)`.

In lines `IL_0011` and `IL_0012` we load a copies of the variables onto the **stack** with the instructions `ldloc.1` and `ldloc.2`. In line `IL_0013` we call `MyClass.Method1(int32, Foo)` and pass the copies of the variables into the method **by value**.

In lines `IL_001a` and `IL_001c` we load the address of the variables onto the **stack** with the instructions `ldloca.s   V_1` and `ldloca.s   V_2`. In line `IL_001e` we call `MyClass.Method1(int32&, Foo&)` and pass the variables addresses into the method **by refence**.    

#### Boxing and Unboxing
In **C#** the [Type System](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system) specifies the value of any type can be treated as an `object`, which all types derive from.

[**Boxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) is the process of converting a [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) to an `object`, or an interface implemented by the [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types). It does this by wrapping the value in a [**System.Object**](https://learn.microsoft.com/en-us/dotnet/api/system.object) instance and stores it on the [**heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap). 

[**Unboxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) is the explicit conversion of the value of the `object`, or interface type, to a value type.

> [!IMPORTANT]
>
> Boxing is not about where the data lives, it’s about:
> - Treating a value type as a reference type
>   
> Value types can live:
> - On the stack
> - Inline inside heap objects
> - Inline inside arrays
> - Inline inside other structs
>   
> All without boxing.

[**Boxing and Unboxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) can be expensive. [**Boxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) involves creating and allocating a new object on the heap, and casting when setting it's value. [**Unboxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) involves first checking the value of the `object` is a boxed value of the [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), then copying the value from the instance into the [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types).

> [!NOTE]
> 
> Examples of unintentional [**boxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) can occur when working with `strings` e.g. when using `String.Format()` and `String.Concat()` etc. Ways around this is to use string interpolation instead, or always call `.ToString()` of the value type. In both cases boxing doesn't happen.

Example [C# code](https://github.com/grantcolley/dotnetwhat/blob/810ce35178fffb9ec70ad3e29a93e76c8c7754c8/tests/TestCases.cs#L25) comparing writing the value of an integer to a string, both with and without calling `Int32.ToString()` and using string interpolation, and the compiled [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions):
```C#
  // C# code
  int localInt = 5;

  string string1 = string.Format("{0}", localInt);
  string string2 = string.Format("{0}", localInt.ToString());
  string string3 = string.Concat("Foo", localInt);
  string string4 = string.Concat("Foo", localInt.ToString());
  string string5 = $"{localInt}";
  
  // Compiled into CIL 
  .locals init (int32 V_0,
           string V_1,
           string V_2,
           string V_3,
           string V_4,
           string V_5,
           valuetype [System.Runtime]System.Runtime.CompilerServices.DefaultInterpolatedStringHandler V_6)
  IL_0000:  nop
  IL_0001:  ldc.i4.5
  IL_0002:  stloc.0
  IL_0003:  ldstr      "{0}"
  IL_0008:  ldloc.0
  IL_0009:  box        [System.Runtime]System.Int32
  IL_000e:  call       string [System.Runtime]System.String::Format(string,
                                                                    object)
  IL_0013:  stloc.1
  IL_0014:  ldstr      "{0}"
  IL_0019:  ldloca.s   V_0
  IL_001b:  call       instance string [System.Runtime]System.Int32::ToString()
  IL_0020:  call       string [System.Runtime]System.String::Format(string,
                                                                    object)
  IL_0025:  stloc.2
  IL_0026:  ldstr      "Foo"
  IL_002b:  ldloc.0
  IL_002c:  box        [System.Runtime]System.Int32
  IL_0031:  call       string [System.Runtime]System.String::Concat(object,
                                                                    object)
  IL_0036:  stloc.3
  IL_0037:  ldstr      "Foo"
  IL_003c:  ldloca.s   V_0
  IL_003e:  call       instance string [System.Runtime]System.Int32::ToString()
  IL_0043:  call       string [System.Runtime]System.String::Concat(string,
                                                                    string)
  IL_0048:  stloc.s    V_4
  IL_004a:  ldloca.s   V_6
  IL_004c:  ldc.i4.0
  IL_004d:  ldc.i4.1
  IL_004e:  call       instance void [System.Runtime]System.Runtime.CompilerServices.DefaultInterpolatedStringHandler::.ctor(int32,
                                                                                                                             int32)
  IL_0053:  ldloca.s   V_6
  IL_0055:  ldloc.0
  IL_0056:  call       instance void [System.Runtime]System.Runtime.CompilerServices.DefaultInterpolatedStringHandler::AppendFormatted<int32>(!!0)
  IL_005b:  nop
  IL_005c:  ldloca.s   V_6
  IL_005e:  call       instance string [System.Runtime]System.Runtime.CompilerServices.DefaultInterpolatedStringHandler::ToStringAndClear()
  IL_0063:  stloc.s    V_5
  IL_0065:  ret
```
In the code listing above we see the [**CIL instruction**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) for [**boxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) in line `IL_0009` for `String.Format()`, and line `IL_002c` for `String.Concat()`. We can see no [**boxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) occurs when using `Int32.ToString()` in lines `IL_001b` and `IL_003e`. We can also see in line `IL_0056` no boxing occurs when using string interpolation.

#### Ref
The use of [ref](https://learn.microsoft.com/en-gb/dotnet/csharp/language-reference/keywords/ref) results in copying a pointer to the underlying storage rather than copying the data referenced by that pointer. [**Value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) are “copy by value” by default. [ref](https://learn.microsoft.com/en-gb/dotnet/csharp/language-reference/keywords/ref) provides a “copy by reference” behavior, which can provide significant performance benefits.

#### Ref Locals
A [ref local](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals) is a variable that refers to other storage.

In this [C# code](https://github.com/grantcolley/dotnetwhat/blob/810ce35178fffb9ec70ad3e29a93e76c8c7754c8/tests/TestCases.cs#L60) variable `b` holds a copy of `a`. Variable `c`, however, refers to the same storage location as `c`. When we set `c` to 7 then `a` is now also 7 because they are both refering to the same storage location. `b` on the other hand is still 5 because it has its own copy. We can see the [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) below.
```C#
  // C# code
  int a = 5;

  int b = a;
  ref int c = ref a;
  c = 7;

  // Compiled into CIL 
  .locals init (int32 V_0,     // local variable `a`
           int32 V_1,          // local variable `b`           
           int32& V_2)         // local variable `c`
  IL_0000:  nop
  IL_0001:  ldc.i4.5           // pushes 5 onto the stack
  IL_0002:  stloc.0            // pops 5 off the stack into local variable `a`
  IL_0003:  ldloc.0            // pushes the value of `a` onto the stack
  IL_0004:  stloc.1            // pops the value from stack into local variable `b`
  IL_0005:  ldloca.s   V_0     // pushes the address of `a` onto the stack
  IL_0007:  stloc.2            // pops the address of `a` from stack into local variable `c`
  IL_0008:  ldloc.2            // pushes the value of `c` onto the stack
  IL_0009:  ldc.i4.7           // pushes 7 onto the stack
  IL_000a:  stind.i4           // pops the value 7 from the stack into the address of `c`
  IL_000b:  ret

Console.WriteLine("c: " + c); // Output: c: 7
Console.WriteLine("a: " + a); // Output: a: 7
Console.WriteLine("b: " + b); // Output: b: 5
```

#### Ref Returns
[Ref return](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values) values are returned by a method by reference i.e. the address of the value is returned rather than the value itself. If the returned value is stored in a [ref local](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals) it can be modifed and the change is reflected in the called method. If a [ref return](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values) value returned by a method isn't stored in a [ref local](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals) then it stores a copy of the value stored at the address in the [ref return](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values).

In the [C# code](https://github.com/grantcolley/dotnetwhat/blob/810ce35178fffb9ec70ad3e29a93e76c8c7754c8/tests/TestCases.cs#L77) below `decimal a = myClass.GetCurrentPrice()` returns the current price by value i.e. `a` is only a copy of the current price returned by `myClass.GetCurrentPrice()`. Changes to `a` will only be applied to itself.

On the other hand `ref decimal b = ref myClass.GetCurrentPriceByRef()` returns the address of the current price i.e. `b` is now pointing to the same current price as the one returned by `myClass.GetCurrentPriceByRef()`. Changes to variable `b` will be reflected in the current price retunred by `myClass.GetCurrentPriceByRef()` because they are both pointing to a value at the same address.

Finally `a = myClass.GetCurrentPriceByRef();` returns the address of the current price, however, because variable `a` is not a [ref local](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals) it only stores a copy of the value in the address of current price.   

We can see in the [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) below line `IL_0008:  callvirt` calls `MyClass::GetCurrentPrice()` which returns a `System.Decimal` by value i.e. a copy of the current price. Line `IL_000f:  callvirt` calls `MyClass::GetCurrentPriceByRef()` which returns `System.Decimal&` by ref i.e. the address of the current price. Finally we see in line `IL_002f:  ldobj` a copy of the value in the address is stored.

```C#
  // C# code
  MyClass myClass = new MyClass();

  decimal a = myClass.GetCurrentPrice();
  ref decimal b = ref myClass.GetCurrentPriceByRef();
  b = 567.89m;
  a = myClass.GetCurrentPriceByRef();
  
  // Compiled into CIL 
  .locals init (class [dotnetwhat.library]dotnetwhat.library.MyClass V_0,
           valuetype [System.Runtime]System.Decimal V_1,
           valuetype [System.Runtime]System.Decimal& V_2)
  IL_0000:  nop
  IL_0001:  newobj     instance void [dotnetwhat.library]dotnetwhat.library.MyClass::.ctor()
  IL_0006:  stloc.0
  IL_0007:  ldloc.0
  IL_0008:  callvirt   instance valuetype [System.Runtime]System.Decimal [dotnetwhat.library]dotnetwhat.library.MyClass::GetCurrentPrice()
  IL_000d:  stloc.1
  IL_000e:  ldloc.0
  IL_000f:  callvirt   instance valuetype [System.Runtime]System.Decimal& [dotnetwhat.library]dotnetwhat.library.MyClass::GetCurrentPriceByRef()
  IL_0014:  stloc.2
  IL_0015:  ldloc.2
  IL_0016:  ldc.i4     0xddd5
  IL_001b:  ldc.i4.0
  IL_001c:  ldc.i4.0
  IL_001d:  ldc.i4.0
  IL_001e:  ldc.i4.2
  IL_001f:  newobj     instance void [System.Runtime]System.Decimal::.ctor(int32,
                                                                           int32,
                                                                           int32,
                                                                           bool,
                                                                           uint8)
  IL_0024:  stobj      [System.Runtime]System.Decimal
  IL_0029:  ldloc.0
  IL_002a:  callvirt   instance valuetype [System.Runtime]System.Decimal& [dotnetwhat.library]dotnetwhat.library.MyClass::GetCurrentPriceByRef()
  IL_002f:  ldobj      [System.Runtime]System.Decimal
  IL_0034:  stloc.1
  IL_0035:  ret
```

#### Lambda
A [Lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions) expression is used to create an anonymous function. Input parameters go to the left of the [lambda operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-operator) `=>` while the lambda expression or statement block goes on the right.

An [expression lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#expression-lambdas) returns the result of the expression. A [statement lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#statement-lambdas) resembles an expression lambda except that its statements are enclosed in braces.

Lambda expressions can be used in any code that requires instances of delegate types or expression trees, for example as an argument to the [Task.Run(Action)](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run?view=net-7.0#system-threading-tasks-task-run(system-action)) or when you write [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/).

```C#
 (input parameters) => expression / { /* statement block */ }
```

In the following [example](https://github.com/grantcolley/dotnetwhat/blob/main/src/Multiplier.cs) we use lambda to multiply two parameters and return the result. We can see in [IL Disassembler](https://learn.microsoft.com/en-us/dotnet/framework/tools/ildasm-exe-il-disassembler) the compiler converts the [lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions) expression into a private nested container class (inside the red box), with a ``System.Func`3<int32,int32,int32>`` [delegate](https://learn.microsoft.com/en-us/dotnet/api/system.func-3), and a method `<Multiply>b__0_0 : int32(int32,int32)` for the multiplication routine. The final listing shows the [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) output for the original `Multiply(int32 value1, int32 value2)` that consumes the [lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions) expression.

```C#
    public class Multiplier
    {
        public int Multiply(int value1, int value2)
        {
            Func<int, int, int> local = (v1, v2) => v1 * v2;

            return local(value1, value2);
        }
    }
```

![CIL for Lambda Multiply Routine](/readme-images/Lambda_multiply.png?raw=true "CIL for Lambda Multiply Routine")

```C#
.method assembly hidebysig instance int32 
        '<Multiply>b__0_0'(int32 v1,
                           int32 v2) cil managed
{
  // Code size       4 (0x4)
  .maxstack  8
  IL_0000:  ldarg.1
  IL_0001:  ldarg.2
  IL_0002:  mul
  IL_0003:  ret
} // end of method '<>c'::'<Multiply>b__0_0'
```

```C#
.method public hidebysig instance int32  Multiply(int32 value1,
                                                  int32 value2) cil managed
{
  // Code size       46 (0x2e)
  .maxstack  3
  .locals init (class [System.Runtime]System.Func`3<int32,int32,int32> V_0,
           int32 V_1)
  IL_0000:  nop
  IL_0001:  ldsfld     class [System.Runtime]System.Func`3<int32,int32,int32> dotnetwhat.library.Multiplier/'<>c'::'<>9__0_0'
  IL_0006:  dup
  IL_0007:  brtrue.s   IL_0020
  IL_0009:  pop
  IL_000a:  ldsfld     class dotnetwhat.library.Multiplier/'<>c' dotnetwhat.library.Multiplier/'<>c'::'<>9'
  IL_000f:  ldftn      instance int32 dotnetwhat.library.Multiplier/'<>c'::'<Multiply>b__0_0'(int32,
                                                                                              int32)
  IL_0015:  newobj     instance void class [System.Runtime]System.Func`3<int32,int32,int32>::.ctor(object,
                                                                                                   native int)
  IL_001a:  dup
  IL_001b:  stsfld     class [System.Runtime]System.Func`3<int32,int32,int32> dotnetwhat.library.Multiplier/'<>c'::'<>9__0_0'
  IL_0020:  stloc.0
  IL_0021:  ldloc.0
  IL_0022:  ldarg.1
  IL_0023:  ldarg.2
  IL_0024:  callvirt   instance !2 class [System.Runtime]System.Func`3<int32,int32,int32>::Invoke(!0,
                                                                                                  !1)
  IL_0029:  stloc.1
  IL_002a:  br.s       IL_002c
  IL_002c:  ldloc.1
  IL_002d:  ret
} // end of method Multiplier::Multiply
```

#### Captured Variable
[Lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#capture-of-outer-variables-and-variable-scope-in-lambda-expressions) expressions can refer to variables declared outside of it's scope e.g. the [lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#capture-of-outer-variables-and-variable-scope-in-lambda-expressions) expression can refer to a variable that is outside the lambda expression but local to the method that contains the lambda expression. These outer variables consumed by a lambda expression are called captured variables. [Captured variables](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#capture-of-outer-variables-and-variable-scope-in-lambda-expressions) won't be garbage-collected until the delegate that references it becomes eligible for garbage collection.

>  [!Warning]
> 
> A lambda expression can't directly capture a parameter that has been passed by [ref](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref).

>  [!Note]
> 
> Captured variables is the same as "closed" variables. When a function references a variable that is declared externally to it, the variable is "closed over" when the function is formed i.e. the variable is bound to the function so it remains accessible to the function. When the C# compiler detects a closure it creates a compiler generated class containing the delegate and the associated local variables.

Key points to note:
- Closures close over variables, not over values.
- Captured variables are evaluated when a delegate is invoked, not when it is created.

In the following [example](https://github.com/grantcolley/dotnetwhat/blob/main/src/CapturedVariable.cs) a [lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#capture-of-outer-variables-and-variable-scope-in-lambda-expressions) expression increments a captured variable and returns the result. We can see in [IL Disassembler (ILDASM)](https://learn.microsoft.com/en-us/dotnet/framework/tools/ildasm-exe-il-disassembler) the compiler converts the [lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions) expression into a private nested container class (inside the red box). The container class contains a public field `myLocalValue : public int32` i.e. this is where the compiler moves the captured variable that is to be incremented, thereby ensuring the captured variable won't be garbage-collected until the containing class is garbage collected, which is only elegible for collection when the lambda is out of scope.

```C#
    public class CapturedVariable
    {
        public int IncrementLocalVariable()
        {
            int myLocalValue = 0;

            Func<int> increment = () => myLocalValue++;

            increment(); // Captured variable is evaluated when the delegate is invoked

            return myLocalValue; // returns 1
        }
    }
```
![CIL for incrementing a Captured Variable](/readme-images/Captured_variable.png?raw=true "CIL for incrementing a Captured Variable")
```C#
.method public hidebysig instance int32  IncrementLocalVariable() cil managed
{
  // Code size       45 (0x2d)
  .maxstack  2
  .locals init (class dotnetwhat.library.CapturedVariable/'<>c__DisplayClass0_0' V_0,
           class [System.Runtime]System.Func`1<int32> V_1,
           int32 V_2)
  IL_0000:  newobj     instance void dotnetwhat.library.CapturedVariable/'<>c__DisplayClass0_0'::.ctor()
  IL_0005:  stloc.0
  IL_0006:  nop
  IL_0007:  ldloc.0
  IL_0008:  ldc.i4.0
  IL_0009:  stfld      int32 dotnetwhat.library.CapturedVariable/'<>c__DisplayClass0_0'::myLocalValue
  IL_000e:  ldloc.0
  IL_000f:  ldftn      instance int32 dotnetwhat.library.CapturedVariable/'<>c__DisplayClass0_0'::'<IncrementLocalVariable>b__0'()
  IL_0015:  newobj     instance void class [System.Runtime]System.Func`1<int32>::.ctor(object,
                                                                                       native int)
  IL_001a:  stloc.1
  IL_001b:  ldloc.1
  IL_001c:  callvirt   instance !0 class [System.Runtime]System.Func`1<int32>::Invoke()
  IL_0021:  pop
  IL_0022:  ldloc.0
  IL_0023:  ldfld      int32 dotnetwhat.library.CapturedVariable/'<>c__DisplayClass0_0'::myLocalValue
  IL_0028:  stloc.2
  IL_0029:  br.s       IL_002b
  IL_002b:  ldloc.2
  IL_002c:  ret
} // end of method CapturedVariable::IncrementLocalVariable
```

#### More on Captured Variables
Consider the following example where `seed` is captured by a closure, not re-created on each call to `natural()`.

```C#
Func<int> natural = Natural();
Console.WriteLine (natural()); // 0
Console.WriteLine (natural()); // 1

static Func<int> Natural()
{
	int seed = 0;
	return () => seed++; // Returns a closure
}
```

**What’s happening step by step**
```C#
Func<int> natural = Natural();
```
- `Natural()` is called once.
- Inside `Natural`, the local variable `seed` is created and initialized to 0.
- The method returns a lambda: `() => seed++`.
- That lambda captures `seed`.

At this point, `seed` does not live on the stack anymore. The C# compiler lifts it into a hidden object (often called a *closure object*) on the heap.

**Each call to `natural()`**
```C#
Console.WriteLine(natural()); // 0
Console.WriteLine(natural()); // 1
```
- You are invoking the same delegate instance each time.
- That delegate holds a reference to the same captured `seed` variable.
- `seed++` increments the value stored in that closure object.
 
So:

- First call → returns 0, then increments to 1
- Second call → returns 1, then increments to 2

`seed` is not reset because `Natural()` is not being called again.

> [!TIP]
> 
> - Closures capture variables, not values.
> - Only if you called `Natural()` again, a new closure would be created in which `seed` is set to 0.


#### Closing Over a Loop Variable
The behavior for closing over loop variables is the same for `for` loops and `while` loops, where the loop variable is logically outside the loop, and therefore closures will close over the same copy of the variable. However, it is different for `foreach` loops, where the loop variable of a `foreach` will be logically inside the loop, and therefore closures will close over a fresh copy of the variable each time.

The examples below show the generated [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) for the `for` loop and the `foreach` loop for comparison.

>  [!Note]
> 
> the container class `<>c__DisplayClass0_0` generated for the `for` loop, `while` loop and `foreach` loop is identical. 

##### for loop
The loop variable of a `for` loop will be logically outside the loop, and therefore closures will close over the same copy of the variable. In the [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) we can see in line `IL_000e:` an instance of the container class `<>c__DisplayClass0_0` is created outside the loop and the same instance is referenced inside the loop with each iteration.

```C#
        public string For()
        {
            StringBuilder sb = new StringBuilder();

            var funcs = new List<Func<int>>(2);

            for(int i = 0; i < 2; i++)
            {
                funcs.Add(() => i); // same copy of the closed variable is updated
            }

            sb.Append(funcs[0]().ToString()); // closed variable evaluated when delegate is invoked
            sb.Append(funcs[1]().ToString()); // closed variable evaluated when delegate is invoked

			// IMPORTANT -> because i++ is a post increment, i equals 2 after the for loop is finished.

            return sb.ToString(); // returns 22
        }
````
![CIL for loop](/readme-images/Looping_For.png?raw=true "CIL for loop")
```C#
.method public hidebysig instance string 
        Loop() cil managed
{
  .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
  // Code size       148 (0x94)
  .maxstack  3
  .locals init (class [System.Runtime]System.Text.StringBuilder V_0,
           class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>> V_1,
           class dotnetwhat.library.Looping_For/'<>c__DisplayClass0_0' V_2,
           int32 V_3,
           bool V_4,
           string V_5)
  IL_0000:  nop
  IL_0001:  newobj     instance void [System.Runtime]System.Text.StringBuilder::.ctor()
  IL_0006:  stloc.0
  IL_0007:  ldc.i4.2
  IL_0008:  newobj     instance void class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::.ctor(int32)
  IL_000d:  stloc.1
  IL_000e:  newobj     instance void dotnetwhat.library.Looping_For/'<>c__DisplayClass0_0'::.ctor()
  IL_0013:  stloc.2
  IL_0014:  ldloc.2
  IL_0015:  ldc.i4.0
  IL_0016:  stfld      int32 dotnetwhat.library.Looping_For/'<>c__DisplayClass0_0'::i
  IL_001b:  br.s       IL_0042
  IL_001d:  nop
  IL_001e:  ldloc.1
  IL_001f:  ldloc.2
  IL_0020:  ldftn      instance int32 dotnetwhat.library.Looping_For/'<>c__DisplayClass0_0'::'<Loop>b__0'()
  IL_0026:  newobj     instance void class [System.Runtime]System.Func`1<int32>::.ctor(object,
                                                                                       native int)
  IL_002b:  callvirt   instance void class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::Add(!0)
  IL_0030:  nop
  IL_0031:  nop
  IL_0032:  ldloc.2
  IL_0033:  ldfld      int32 dotnetwhat.library.Looping_For/'<>c__DisplayClass0_0'::i
  IL_0038:  stloc.3
  IL_0039:  ldloc.2
  IL_003a:  ldloc.3
  IL_003b:  ldc.i4.1
  IL_003c:  add
  IL_003d:  stfld      int32 dotnetwhat.library.Looping_For/'<>c__DisplayClass0_0'::i
  IL_0042:  ldloc.2
  IL_0043:  ldfld      int32 dotnetwhat.library.Looping_For/'<>c__DisplayClass0_0'::i
  IL_0048:  ldc.i4.2
  IL_0049:  clt
  IL_004b:  stloc.s    V_4
  IL_004d:  ldloc.s    V_4
  IL_004f:  brtrue.s   IL_001d
  IL_0051:  ldloc.0
  IL_0052:  ldloc.1
  IL_0053:  ldc.i4.0
  IL_0054:  callvirt   instance !0 class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::get_Item(int32)
  IL_0059:  callvirt   instance !0 class [System.Runtime]System.Func`1<int32>::Invoke()
  IL_005e:  stloc.3
  IL_005f:  ldloca.s   V_3
  IL_0061:  call       instance string [System.Runtime]System.Int32::ToString()
  IL_0066:  callvirt   instance class [System.Runtime]System.Text.StringBuilder [System.Runtime]System.Text.StringBuilder::Append(string)
  IL_006b:  pop
  IL_006c:  ldloc.0
  IL_006d:  ldloc.1
  IL_006e:  ldc.i4.1
  IL_006f:  callvirt   instance !0 class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::get_Item(int32)
  IL_0074:  callvirt   instance !0 class [System.Runtime]System.Func`1<int32>::Invoke()
  IL_0079:  stloc.3
  IL_007a:  ldloca.s   V_3
  IL_007c:  call       instance string [System.Runtime]System.Int32::ToString()
  IL_0081:  callvirt   instance class [System.Runtime]System.Text.StringBuilder [System.Runtime]System.Text.StringBuilder::Append(string)
  IL_0086:  pop
  IL_0087:  ldloc.0
  IL_0088:  callvirt   instance string [System.Runtime]System.Object::ToString()
  IL_008d:  stloc.s    V_5
  IL_008f:  br.s       IL_0091
  IL_0091:  ldloc.s    V_5
  IL_0093:  ret
} // end of method Looping_For::Loop
```

##### foreach loop
The loop variable of a `foreach` will be logically inside the loop, and therefore closures will close over a fresh copy of the variable each time. In the [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) we can see in line `IL_002d:` a new instance of the container class `<>c__DisplayClass0_0` is created inside the loop on each iteration.

```C#
        public string ForEach()
        {
            StringBuilder sb = new StringBuilder();

            var vals = new List<int> { 1, 2 };
            var funcs = new List<Func<int>>();

            foreach (int v in vals)
            {
                funcs.Add(() => v); // a fresh copy of the closed variable with each iteration
            }

            sb.Append(funcs[0]().ToString()); // Closed variable evaluated when delegate is invoked
            sb.Append(funcs[1]().ToString()); // Closed variable evaluated when delegate is invoked

            return sb.ToString(); // returns 12
        }
```
![CIL foreach loop](/readme-images/Looping_Foreach.png?raw=true "CIL foreach loop")
```C#
.method public hidebysig instance string 
        Loop() cil managed
{
  .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
  // Code size       183 (0xb7)
  .maxstack  3
  .locals init (class [System.Runtime]System.Text.StringBuilder V_0,
           class [System.Collections]System.Collections.Generic.List`1<int32> V_1,
           class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>> V_2,
           valuetype [System.Collections]System.Collections.Generic.List`1/Enumerator<int32> V_3,
           class dotnetwhat.library.Looping_Foreach/'<>c__DisplayClass0_0' V_4,
           int32 V_5,
           string V_6)
  IL_0000:  nop
  IL_0001:  newobj     instance void [System.Runtime]System.Text.StringBuilder::.ctor()
  IL_0006:  stloc.0
  IL_0007:  newobj     instance void class [System.Collections]System.Collections.Generic.List`1<int32>::.ctor()
  IL_000c:  dup
  IL_000d:  ldc.i4.1
  IL_000e:  callvirt   instance void class [System.Collections]System.Collections.Generic.List`1<int32>::Add(!0)
  IL_0013:  nop
  IL_0014:  dup
  IL_0015:  ldc.i4.2
  IL_0016:  callvirt   instance void class [System.Collections]System.Collections.Generic.List`1<int32>::Add(!0)
  IL_001b:  nop
  IL_001c:  stloc.1
  IL_001d:  newobj     instance void class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::.ctor()
  IL_0022:  stloc.2
  IL_0023:  nop
  IL_0024:  ldloc.1
  IL_0025:  callvirt   instance valuetype [System.Collections]System.Collections.Generic.List`1/Enumerator<!0> class [System.Collections]System.Collections.Generic.List`1<int32>::GetEnumerator()
  IL_002a:  stloc.3
  .try
  {
    IL_002b:  br.s       IL_0058
    IL_002d:  newobj     instance void dotnetwhat.library.Looping_Foreach/'<>c__DisplayClass0_0'::.ctor()
    IL_0032:  stloc.s    V_4
    IL_0034:  ldloc.s    V_4
    IL_0036:  ldloca.s   V_3
    IL_0038:  call       instance !0 valuetype [System.Collections]System.Collections.Generic.List`1/Enumerator<int32>::get_Current()
    IL_003d:  stfld      int32 dotnetwhat.library.Looping_Foreach/'<>c__DisplayClass0_0'::v
    IL_0042:  nop
    IL_0043:  ldloc.2
    IL_0044:  ldloc.s    V_4
    IL_0046:  ldftn      instance int32 dotnetwhat.library.Looping_Foreach/'<>c__DisplayClass0_0'::'<Loop>b__0'()
    IL_004c:  newobj     instance void class [System.Runtime]System.Func`1<int32>::.ctor(object,
                                                                                         native int)
    IL_0051:  callvirt   instance void class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::Add(!0)
    IL_0056:  nop
    IL_0057:  nop
    IL_0058:  ldloca.s   V_3
    IL_005a:  call       instance bool valuetype [System.Collections]System.Collections.Generic.List`1/Enumerator<int32>::MoveNext()
    IL_005f:  brtrue.s   IL_002d
    IL_0061:  leave.s    IL_0072
  }  // end .try
  finally
  {
    IL_0063:  ldloca.s   V_3
    IL_0065:  constrained. valuetype [System.Collections]System.Collections.Generic.List`1/Enumerator<int32>
    IL_006b:  callvirt   instance void [System.Runtime]System.IDisposable::Dispose()
    IL_0070:  nop
    IL_0071:  endfinally
  }  // end handler
  IL_0072:  ldloc.0
  IL_0073:  ldloc.2
  IL_0074:  ldc.i4.0
  IL_0075:  callvirt   instance !0 class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::get_Item(int32)
  IL_007a:  callvirt   instance !0 class [System.Runtime]System.Func`1<int32>::Invoke()
  IL_007f:  stloc.s    V_5
  IL_0081:  ldloca.s   V_5
  IL_0083:  call       instance string [System.Runtime]System.Int32::ToString()
  IL_0088:  callvirt   instance class [System.Runtime]System.Text.StringBuilder [System.Runtime]System.Text.StringBuilder::Append(string)
  IL_008d:  pop
  IL_008e:  ldloc.0
  IL_008f:  ldloc.2
  IL_0090:  ldc.i4.1
  IL_0091:  callvirt   instance !0 class [System.Collections]System.Collections.Generic.List`1<class [System.Runtime]System.Func`1<int32>>::get_Item(int32)
  IL_0096:  callvirt   instance !0 class [System.Runtime]System.Func`1<int32>::Invoke()
  IL_009b:  stloc.s    V_5
  IL_009d:  ldloca.s   V_5
  IL_009f:  call       instance string [System.Runtime]System.Int32::ToString()
  IL_00a4:  callvirt   instance class [System.Runtime]System.Text.StringBuilder [System.Runtime]System.Text.StringBuilder::Append(string)
  IL_00a9:  pop
  IL_00aa:  ldloc.0
  IL_00ab:  callvirt   instance string [System.Runtime]System.Object::ToString()
  IL_00b0:  stloc.s    V_6
  IL_00b2:  br.s       IL_00b4
  IL_00b4:  ldloc.s    V_6
  IL_00b6:  ret
} // end of method Looping_Foreach::Loop


```

## How it Works Internally
#### Array
An array is a contiguous block of memory that stores elements of the same type.

Internally, an array in .NET includes:
- A header with metadata (including type info, length, etc.)
- The actual element data, stored in a flat, contiguous memory region
```
[ Object Header | Length | Element0 | Element1 | ... ]
```
Arrays are zero-based and have fixed length.
\
Indexing is `O(1)` because of the fixed-size element type and contiguous layout.
\
The JIT compiler can apply bounds-checking optimizations for performance in safe code.
```C#
int[] numbers = new int[3] { 10, 20, 30 };
```
```
+------------------------+--------------------+------------+------------+------------+
| Object Header (12–16B) | Length (Int32 = 3) | Value[0]   | Value[1]   | Value[2]   |
+------------------------+--------------------+------------+------------+------------+
|     Type pointer       |         3          |    10      |    20      |    30      |
+------------------------+--------------------+------------+------------+------------+

```


#### List\<T>
The `List<T>` class is a generic dynamic array — it stores its elements in a contiguous block of memory and resizes as needed.

`List<T>` is initialized with a default capacity or a user-specified capacity. When elements are added beyond the current capacity, it resizes the array (usually doubling its capacity).

Key Internal Fields:
```C#
private T[] _items;    // Underlying array buffer
private int _size;     // Current number of elements in the list
private int _version;  // Used to track changes for enumerator safety
```

#### Dictionary\<TKey,TValue>
The `Dictionary<TKey, TValue>` class remains a hash table-based implementation. `Dictionary<TKey, TValue>` uses `Buckets` and `Entries`. Each bucket contains the index of the first `entry` in the `entries` array that belongs to that `hash bucket`. Objects that share the same `hash bucket` form a linked list using the `next` field, linking to the next entry (like a singly-linked list).
- **Buckets (int[] or Span<int>)** – An array of indexes into the entries array.
- **Entries (Entry<TKey, TValue>[])** – An array of structs that contain:
	- **int hashCode**
	- **int next** (index of the next entry in case of collision, forming a linked list)
	- **TKey** key
	- **TValue** value

Each bucket contains the index of the first entry in the entries array that belongs to that hash bucket. If there are collisions, the next field links to the next entry (like a singly-linked list).

A collision happens when two different keys produce the same bucket index (after hashing and modulo). Collisions are inevitable so each bucket in the hash table doesn't just hold one entry, it points to a linked list (chain) of entries that hash to that same bucket.

**Internally:**
- There's a `buckets[]` array, where each element holds an index into the `entries[]` array.
- The `entries[]` array contains entries that have key-value pairs and a next field (like a linked list pointer).

**When a collision happens:**
- The new entry is added to `entries[]`.
- The previous entry’s next field is updated to point to the index of the new one.

**Add an Entry**:
- Compute hash code from `TKey` (via `GetHashCode()`).
- Map it to a bucket index.
- Insert into `entries[]`, update bucket, and manage collision via chaining.

**TryGetValue / ContainsKey**:
- Compute hash code from `TKey` (via `GetHashCode()`).
- Map it to a bucket index.
- Traverse the linked list in entries (via next) to find the key.

In .NET 9.0 `Dictionary<TKey, TValue>` uses a hash table with chaining (linked list). It is optimized for `O(1)` average-time complexity on lookup, insert, and remove.

The `Entry` structure:
```C#
struct Entry<TKey, TValue>
{
    public int hashCode;    // Lower 31 bits of hash code, -1 if unused
    public int next;        // Index of next entry, -1 if last
    public TKey key;        // Key of entry
    public TValue value;    // Value of entry
}
```
Here is a step-by-step example of how `Hash Codes` works in Dictionary.
```C# 
var dict = new Dictionary<string, int>();
dict["apple"] = 42;

// step 1 - generate a hashcode from the key
int hashCode = "apple".GetHashCode();

// step 2 - enure the hashcode is non-negative
hashCode = -123456789 & 0x7FFFFFFF

// step 3 - the hash code is then modulo'd by the number of buckets to find the right one
int bucketIndex = hashCode % buckets.Length;
```

#### Records
Record is a compile time contruct that represent a class or struct that works with immutable (read-only) data, and has structural equality semantics i.e. if the values are erqual, the object is equal. At runtime records are treated as classes or structs with additional rules enforcing immutability and structural equality semantics. Records are useful for simply storing and passing read-only values. Records can implement intrerfaces and in the case of class Records, can inherit from another base class Record.

```C#
public record Point{}; // Point is a class

public record struct Point{}; // Point is a struct
```

#### Enums
Enums are value type where you specify a group of named numeric constants. Enums are stored in memory exactly as its underlying integral type, with no extra metadata per value. Adding the The `[Flags]` attribute does not change memory layout at all.

How it works
- Every enum has an underlying integral type
- If you don’t specify one, the default is int (System.Int32)
- At runtime and in memory, the enum value is just that integral value

```C#
[Flags]
enum Permissions : int
{
    Read = 1,
    Write = 2,
    Execute = 4
}

// The enum above is the equivalent in memory to:
int permission = 1;

// Enums can be boxed
object o = Permissions.Read;
```

> [!Note]
>
> An enum variable can hold a value that is not one of its named constants. This is intentional and follows directly from how enums work.

```C#
enum Status
{
    None = 0,
    Started = 1,
    Finished = 2
}

Status s = (Status)42;   // perfectly legal
```

## Performance
<!--
        https://blog.marcgravell.com/2017/04/spans-and-ref-part-1-ref.html
        // https://blog.marcgravell.com/2022/05/unusual-optimizations-ref-foreach-and.html
-->

#### Span\<T>
[Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) is a [ref struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) that provides type-safe access to a contiguous region of memory. [Ref structs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) can only be allocated on the stack and not the heap. [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) can, however, point to heap memory, stack memory and unmanaged memory. [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) can wrap an entire contiguous block of memory or it can point to any contiguous range within it, using [slicing](https://learn.microsoft.com/en-us/dotnet/api/system.span-1?view=net-7.0#spant-and-slices).

>  [!Note]
>
>Because [ref structs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) can only be allocated on the stack and not the heap they can't do anything that may cause them to be allocated on the heap. For example, [ref structs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) can't be a field of a class, implement an interface or be boxed. [Ref struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) variables also can't be captured by a [lambda expression](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions), [local function](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/local-functions) or [async methods](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async) or be used in [iterators](https://learn.microsoft.com/en-us/dotnet/csharp/iterators). 

In the following [C# code](https://github.com/grantcolley/dotnetwhat/blob/0a0d44e57e78ce2c8bc8e8546a677b23501d1643/src/TextParser.cs) we [benchmark](https://github.com/grantcolley/dotnetwhat/blob/main/benchmarks/TextParserBenchmarks.cs) parsing text to return the last word in a sentence using `LastOrDefault`, `Substring` and `ReadOnlySpan<char>`. The results cleary show `ReadOnlySpan<char>` outperforms LINQ's `LastOrDefault` and `Substring`.

```C#
    public class TextParser
    {
        public string Get_Last_Word_Using_LastOrDefault(string paragraph)
        {
            var words = paragraph.Split(" ");

            var lastWord = words.LastOrDefault();

            return lastWord?.Substring(0, lastWord.Length - 1) ?? string.Empty;
        }

        public string Get_Last_Word_Using_Substring(string paragraph)
        {
            var lastSpaceIndex = paragraph.LastIndexOf(" ", StringComparison.Ordinal);

            var position = lastSpaceIndex + 1;
            var wordLength = paragraph.Length - position - 1;

            return lastSpaceIndex == -1
                ? string.Empty
                : paragraph.Substring(position, wordLength);
        }

        public ReadOnlySpan<char> Get_Last_Word_Using_Span(ReadOnlySpan<char> paragraph)
        {
            var lastSpaceIndex = paragraph.LastIndexOf(' ');

            var position = lastSpaceIndex + 1;
            var wordLength = paragraph.Length - position - 1;

            return lastSpaceIndex == -1
                ? ReadOnlySpan<char>.Empty
                : paragraph.Slice(position, wordLength);
        }
    }
``` 

![Benchmark ReadOnlySpan\<char>](/readme-images/TextParser.png?raw=true "Benchmark Span<T>")

#### StringBuilder
A [StringBuiler](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder) represents a mutable sequence of characters by maintaining a buffer to accommodate expansion. Expansion beyond the buffer involves creating a new, larger buffer and copying the original buffer to it. The default capacity of a [StringBuiler](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder) is 16 characters, and its default maximum capacity is [Int32.MaxValue](https://learn.microsoft.com/en-us/dotnet/api/system.int32.maxvalue). Each time the number of characters required exceeds the capacity, the capacity doubles in size e.g. capacity starts at 16, then doubles to 32, then to 64, then 128 etc. until eventually the maximum capacity of 2,147,483,647 is reached an an either a [ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception) or an [OutOfMemoryException](https://learn.microsoft.com/en-us/dotnet/api/system.outofmemoryexception) exception is thrown.

Generally [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder) performans better than [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/), however, it does depend on the size of the string, the amount of memory to be allocated for the new string, the system on which the code is executing, and the type of operation.

In the following [C# code](https://github.com/grantcolley/dotnetwhat/blob/main/src/TextBuilder.cs) we [benchmark](https://github.com/grantcolley/dotnetwhat/blob/main/benchmarks/TextBuilderBenchmarks.cs) concatenating strings versus using `StringBuilder.Append`. The results clearly show [StringBuilder](https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder) significantly outperforms [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/) as the number of concatenations increases.

```C#
    public class TextBuilder
    {
        public string StringConcatenateTwoStrings(string sentence)
        {
            sentence += sentence;

            return sentence;
        }

        public string StringConcatenateFiveStrings(string sentence)
        {
            for (int i = 0; i < 5; i++) 
            {
                sentence += sentence;
            }

            return sentence;
        }

        public string StringConcatenateTenStrings(string sentence)
        {
            for (int i = 0; i < 10; i++)
            {
                sentence += sentence;
            }

            return sentence;
        }

        public string StringBuilderAppendTwoStrings(string sentence)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(sentence);
            stringBuilder.Append(sentence);

            return stringBuilder.ToString();
        }

        public string StringBuilderAppendFiveStrings(string sentence)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                stringBuilder.Append(sentence);
            }

            return stringBuilder.ToString();
        }

        public string StringBuilderAppendTenStrings(string sentence)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                stringBuilder.Append(sentence);
            }

            return stringBuilder.ToString();
        }
    }
```

![Benchmark StringBuilder](/readme-images/TextBuilder.png?raw=true "Benchmark StringBuilder")

#### Mark Members Static
Mark those members that do not reference instance data or call instance methods can be marked as static. This will prevent a runtime check to see if the object pointer is not null resulting in a performance gain.

See [CA1822: Mark members as static](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822)

## Language Integrated Query (Linq)
Linq enables you to query any collection (sequence) that implements `IEnumerable<T>`. The standard query operators are implemented as extension methods on `IEnumerable<T>`.

#### Query Operators
- Filtering operators: `Where()`, `Take()`, `Skip()` etc...
- Projecting operators: `Select()`, `SelectMany()` etc...
- Joining operators: `Join()`, `GroupJoin()`, `Zip()` etc...
- Grouping operators: `GroupBy()`, `Chunk()` etc...
- Ordering operators:  `OrderBy()`, `ThenBy()`, `OrderByDescending()` ect...
- Aggregation operators: `Count()`, `Min()`, `Max()`, `Average()` etc...
- Element operators: `First()`, `Last()`, `FirstOrDefault()`, `LastOrDefault` etc...
- Quantifiers: `Contains()`, `Any()`, `All()` etc...
- Set operators: `Concat()`, `Union()`, `Intersect()`, `Except()` etc...
- Conversion operators (export): `ToList()`, `ToDictionary()`, `ToHashSet()`  etc...
- Conversion operators (import): `OfTYype<T>()`, `Cast<T>()`  etc...

#### Deferred Execution
Most query operators execute when enumerated, not when constructed. This is known as deferred execution. All standard query operators provide deferred execution with the exception of:
- Operators that return a single element or scalar value like quantifiers, element, and aggregation operators  e.g. `Contains()`, `First()` and `Count()`
- Conversion operators e.g. `ToList()`

#### Fluent Syntax vs Query Expressions
Fluent syntax are extension methods on `IEnumerable<T>` which allows you to chain query operators to build complex queries. A query expression is special language support that starts with `from` and ends with either a `select` or `group` clause.

> [!NOTE]
> 
> The compiler translates query expressions into fluent syntax.

```C#
// Fluent syntax
IEnumerable<string> query = names.Where (n = > n.Contains ("a"))
								 .OrderBy (n = > n.Length)
								 .Select (n = > n.ToUpper());

// Query expression
IEnumerable<string> query = from n in names
							where n.Contains("a")
							orderby n.Length
							select n.ToUpper();
```

## AI Agents in the IDE
#### GitHub Copilot Chat
GitHub Copilot. Use the Copilot free plan in Visual Studio. GitHub Pro subscription does not include Copilot Pro.

#### OpenAI Codex
Codex – OpenAI’s coding agent. Visual Studio Code offers the best integration with your ChatGPT Pro subscription as long as you install the official Codex extension and login using your ChatGPT account.

## CI/CD

## Unit Testing

## REST
**REST (REpresentational State Transfer)** is a widely used architectural style for designing networked applications, particularly APIs, that allows client-server communication using HTTP methods like `GET`, `POST`, `PUT`, and `DELETE`. It was introduced by Roy Fielding in 2000 to improve web efficiency through constraints like statelessness, uniform interfaces, and cacheability. 

## S.O.L.I.D Principles
**SOLID** is a set of five object-oriented design principles that help make C# code easier to maintain, test, and extend.

#### S — Single Responsibility Principle
A class should have one reason to change. This means it should focus on a single, cohesive responsibility.

Example - Instead of one `Invoice` class printing, saving, and calculating everything, responsibilities are split.
```C#
// ❌ Violates SRP: OrderService handles persistence, validation, and notifications
public class OrderService
{
    public void PlaceOrder(Order order)
    {
        // validate
        // save to DB
        // send confirmation email
    }
}

// ✅ SRP: Each class has one responsibility
public class OrderValidator
{
    public bool IsValid(Order order) => /* validation logic */;
}

public class OrderRepository
{
    public void Save(Order order) => /* DB logic */;
}

public class EmailNotifier
{
    public void SendConfirmation(Order order) => /* email logic */;
}

public class OrderService
{
    private readonly OrderValidator _validator;
    private readonly OrderRepository _repo;
    private readonly EmailNotifier _notifier;

    public OrderService(OrderValidator validator, OrderRepository repo, EmailNotifier notifier)
    {
        _validator = validator;
        _repo = repo;
        _notifier = notifier;
    }

    public void PlaceOrder(Order order)
    {
        if (!_validator.IsValid(order)) return;
        _repo.Save(order);
        _notifier.SendConfirmation(order);
    }
}
```

#### O — Open/Closed Principle
Classes should be open for extension but closed for modification. This means you should be able to add new functionality to a system without changing existing, tested code. 

Example - You can add new discounts without changing `PriceCalculator`.
```C#
// Base abstraction
public interface IDiscount
{
    decimal Apply(decimal price);
}

// Extensions
public class StudentDiscount : IDiscount
{
    public decimal Apply(decimal price) => price * 0.9m;
}

public class SeniorDiscount : IDiscount
{
    public decimal Apply(decimal price) => price * 0.8m;
}

// Client code stays unchanged
public class PriceCalculator
{
    public decimal Calculate(decimal price, IDiscount discount)
    {
        return discount.Apply(price);
    }
}
```
You can add new discount types without touching `PriceCalculator`.

#### L — Liskov Substitution Principle
A subclass should be able to replace its base class without breaking the program. The issue is about correct behavior when substituting subclasses. It is better to avoid inheritance when the behavior does not truly match.

Bad Example - Setting width also changes height, thereby changing the expected behaviour of the base class. 
```C#
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int Area => Width * Height;
}

public class Square : Rectangle
{
    public override int Width
    {
        set
        {
            base.Width = value;
            base.Height = value;
        }
    }

    public override int Height
    {
        set
        {
            base.Width = value;
            base.Height = value;
        }
    }
}
```

Better Example - Avoid inheritance when the behavior does not truly match. Now each shape behaves correctly on its own.
```C#
public interface IShape
{
    int Area { get; }
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public int Area => Width * Height;
}

public class Square : IShape
{
    public int Side { get; set; }

    public int Area => Side * Side;
}
```

#### I — Interface Segregation Principle
Clients should not be forced to depend on methods they do not use. The issue is fat interfaces. It is better to split interfaces into focused contracts.

Bad Example - Fat interfaces. A modern printer might not support faxing.
```C#
public interface ISmartDevice
{
    void Print();
    void Scan();
    void Fax();
}

public class BasicPrinter : ISmartDevice
{
    public void Print()
    {
        Console.WriteLine("Printing");
    }

    public void Scan()
    {
        throw new NotImplementedException();
    }

    public void Fax()
    {
        throw new NotImplementedException();
    }
}
```

Better Example - It is better to split interfaces into focused contracts. Now classes implement only what they need.
```C#
public interface IPrinter
{
    void Print();
}

public interface IScanner
{
    void Scan();
}

public interface IFaxMachine
{
    void Fax();
}

public class BasicPrinter : IPrinter
{
    public void Print()
    {
        Console.WriteLine("Printing");
    }
}

public class OfficePrinter : IPrinter, IScanner, IFaxMachine
{
    public void Print()
    {
        Console.WriteLine("Printing");
    }

    public void Scan()
    {
        Console.WriteLine("Scanning");
    }

    public void Fax()
    {
        Console.WriteLine("Faxing");
    }
}
```

#### D — Dependency Inversion Principle
High-level code should depend on abstractions (interfaces), not concrete classes. It ensures high-level business logic remains decoupled from low-level implementation details. Dependencies can be injected into a class.

Example - `NotificationService` depends on `IEmailService`, not directly on `EmailService`.
```C#
public interface IEmailService
{
    void Send(string message);
}

public class EmailService : IEmailService
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class NotificationService
{
    private readonly IEmailService _emailService;

    public NotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public void NotifyUser()
    {
        _emailService.Send("Hello!");
    }
}
```

#### Difference Between LSP and ISP
**Liskov Substitution Principle (LSP)** and **Interface Segregation Principle (ISP)** are commonly confused because both often involve interfaces and inheritance.
The key difference is:
- **LSP** is about correct behavior when substituting subclasses
- **ISP** is about keeping interfaces small and focused

| Principle | Focus                   | Problem                                    | Ask  														   |
| --------- | ----------------------- | ------------------------------------------ | ------------------------------------------------------------- |
| LSP       | Inheritance correctness | Subclass changes expected behavior         | “Can I safely use the child anywhere the parent is expected?” |
| ISP       | Interface design        | Classes forced to implement unused methods | “Am I forcing classes to implement things they don’t need?”   |

## Big *O*
Big *O* notation is a way to describe how fast or slow your code runs as the input size grows. It gives you a basic idea of your code's performance and scalability.

It doesn't measure actual time (like milliseconds); it measures how the number of operations grows relative to the input size.

#### TL;DR
| Big O          | Meaning                                                  | Example                                                         |
| -------------- | -------------------------------------------------------- | --------------------------------------------------------------- |
| **O(1)**       | Constant time – super fast, doesn’t depend on input size | `list[0];` or `dictionary.ContainsKey("foo")`                   |
| **O(n)**       | Linear – time grows with input size                      | `foreach (var item in list) { ... }`                            |
| **O(n²)**      | Quadratic – nested loops, gets slow fast                 | `foreach (var a in list) foreach (var b in list) { ... }`       |
| **O(log n)**   | Logarithmic – very efficient, divide and conquer         | Binary search: `list.BinarySearch(item);`                       |
| **O(n log n)** | Typical of efficient sorts                               | `list.Sort();` (uses TimSort in .NET)                           |
| **O(2ⁿ)**      | Exponential – extremely slow for large inputs            | Recursive solutions like solving the Fibonacci sequence naively |

#### Rules of Thumb:
- Favor `O(1)` and `O(log n)` when you can.
- Be cautious with `O(n²)` and worse – especially with nested loops.

> [!IMPORTANT]
> 
> **Is 2n Big O of n?**
>
> Before we get started, it's important to remember that the "big O" in **Big O** stands for order of, so we are really only concerned with changes in the order of magnitude. This means we don't have to worry about constants. It also means that `O(n)`, `O(2n)`, `O(10n)` are all just `O(n)`.

#### Big *O* with Code Examples
##### Constant Time `O(1)`
Doesn’t depend on input size. You know exactly where the item is so go straight to it.

e.g. Accessing an element in an array by index. Fast, no matter how big the array is. You know exactly where the item is so go straight to it.
```C#
int[] array = {1, 2, 3, 4, 5};

int GetValue(int index)
{
    return array[index]; // Always takes the same amount of time
}
```

##### Linear Time `O(n)`
Time grows linearly with input size. Items are not sorted so check one by one from start to finish.

e.g. Looping through a list or array, checking each item one by one from start to finish.
```C#
int[] array = {1, 2, 3, 4, 5};

bool Contains(int value)
{
    foreach (var item in array)
    {
        if (item == value) return true;
    }

    return false;
}
```

##### Logarithmic Time `O(log n)`
Every step of the algorithm cuts the problem in half so instead of checking every item, you’re skipping a big chunk with each move. Super fast even with big input sizes. Typical used in binary search.

e.g. Binary search in a sorted array. Each loop cuts the array size in half
```C#
int[] sortedArray = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

int BinarySearch(int value)
{
    int left = 0;
    int right = sortedArray.Length - 1;

    while (left <= right)
    {
        int mid = (left + right) / 2;

        if (sortedArray[mid] == value)
            return mid;
        else if (sortedArray[mid] < value)
            left = mid + 1;
        else
            right = mid - 1;
    }

    return -1;
}
```

##### Quadratic Time `O(n²)`
The work your code does grows a lot faster than the input size grows. Specifically, if you double the input, the work grows four times. If you triple it, it grows nine times — like squaring the size. Typically used in some sorting algorithms like bubble sort or selection sort

e.g. Nested loops over the same data set. Gets much slower as numbers grows.
- If `numbers = 10`, it prints 100 lines.
- If `numbers = 100`, it prints 10,000 lines!
```C#
void PrintAllPairs(int[] numbers)
{
    foreach (var a in numbers)
    {
        foreach (var b in numbers)
        {
            Console.WriteLine($"{a}, {b}");
        }
    }
}
```

##### Exponential Time `O(2ⁿ)`
Time doubles with each extra input, resulting in explosive growth, making it extremely slow.

e.g. Recursive Fibonacci, where each call creates two more calls, like a tree branching rapidly.
```C#
int Fibonacci(int n)
{
    if (n <= 1) return n;
    return Fibonacci(n - 1) + Fibonacci(n - 2);
}
```
> [!TIP]
> 
> The Fibonacci sequence is the series of numbers where each number is the sum of the two preceding numbers.
>
> e.g. `0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610`
> 
> Here is a more efficient implementation of Fibonacci which can be called like this `foreach (int i in Fib())`
> ```C#
> public static IEnumerable<int> Fibonacci()
> {
>    int prev = 0, next = 1;
>     yield return prev;
>     yield return next;
> 
>     while (true)
>     {
>         int sum = prev + next;
>         yield return sum;
>         prev = next;
>         next = sum;
>     }
> }
> ```

#### Big O Growth Comparison Table
| **Big O**    | **Name**    | **Example** in English | **C# Code Pattern**          | **Growth for n = 10, 20, 30** |
| ------------ | ----------- | ---------------------- | ---------------------------- | ----------------------------- |
| **O(1)**     | Constant    | Always takes same time | `dict.ContainsKey(key)`      | 1, 1, 1                       |
| **O(log n)** | Logarithmic | Cuts problem in half   | Binary Search                | \~4, \~5, \~6                 |
| **O(n)**     | Linear      | One step per item      | `foreach (var item in list)` | 10, 20, 30                    |
| **O(n²)**    | Quadratic   | Compare every pair     | Nested loops                 | 100, 400, 900                 |
| **O(2ⁿ)**    | Exponential | Try all combinations   | Recursive brute-force        | 1,024; 1,048,576; >1 billion  |

#### Big *O* Summary
- `O(1)`: Super fast, doesn't care about size
- `O(log n)`: Excellent, scales well
- `O(n)`: Grows steadily
- `O(n²)`: Slows down fast with large input
- `O(2ⁿ)`: Unusable beyond ~20 items

## Interview Prep
- C# language knowledge
  - LINQ
  - yield return
  - async/await
  - Task vs ValueTask
  - lock, Interlocked, volatile
  - Records vs classes
  - Generics
  - readonly struct
  - CancellationToken
- Collections
  - IEnumerable
  - List
  - Dictionary
  - HashSet
  - Queue
  - Stack
  - LinkedList
  - PriorityQueue<TElement, TPriority>
  - Span
  - IReadOnlyList<T>
- Data Structures
  - Trees
  - Graphs
- Memory Management & Garbage Collection
  - What kind of algorithm is applied when garbage collection happens?
  - What would you do if garbage collection is a performance bottleneck?
  - What would you do if you have lots of objects in memory?
  - How would you optimise that?
  - Object pooling
  - Memory-related questions around:
    - Memory<T>
	- Span<T>
	- Memory buffers
  - Memory and garbage collection in high-performance systems
- Concurrency, Performance & Scalability
  - Difference between:`async / await`, concurrency and parallelism
  - Highly scalable systems
  - Memory and garbage collection in high-performance systems
- Coding Challenge
  - Arrays and strings
    - Two Sum
    - Rotate Array
    - Reverse Words
    - Longest Common Prefix 
  - Algorithms
    - Binary Search
    - Merge Intervals
    - Maximum Subarray
    - Sliding Window  
  - Producer-consumer using Channel<T>.
- Debugging & Profiling
  - How do you debug an API request?
  - Talking about profiling
  - How you would make a specific optimisation
- SQL / Databases
  - What types of joins are there?
  - CTE vs temporary tables
  - What is a view?
  - Can you put an index on a view?
  - How do you debug a query to make it faster?
  - Types of indexes
  - Clustered vs non-clustered indexes
  - Why use an index?

### Reverse an Array
`Array.Reverse` has a time complexity of `O(n)`, where `n` is the number of elements being reversed.
Internally, it swaps elements from the two ends of the range until it reaches the middle.
```C#
int left = 0;
int right = array.Length - 1;

while (left < right)
{
    int tmp = array[left];
    array[left] = array[right];
    array[right] = tmp;
    left++;
    right--;
}
```
Swaps elements from the two ends of the range until it reaches the middle.
- create `while (left < right)` loop that continues until the left position meets the right position.
- store the left element in a temp variable `int tmp = array[left];`
- replace left element with right element `array[left] = array[right];`
- replace right element with tmp variable (original left element) `array[right] = tmp;`
- increment left `left++;` and decrement right `right--;`
- continue until left and right are equal

### Rotate an array
#### Three-reversal algorithm
The purpose of `k = k % n;` is to normalize the rotation amount so that it is always between `0` and `n - 1`.
This works because rotating an array by its own length leaves it unchanged.

For an array of length 5:
```
Rotate by 0 == Rotate by 5 == Rotate by 10 == Rotate by 15
Rotate by 1 == Rotate by 6 == Rotate by 11 == Rotate by 21
Rotate by 2 == Rotate by 7 == Rotate by 12 == Rotate by 22
```

Taking the remainder (%) removes these complete rotations.

```C#
public static void Rotate(int[] nums, int k)
{
    if (nums == null || nums.Length == 0)
        return;

    int n = nums.Length;

    // Normalize rotation amount so it's always between 0 and n - 1.
    // Taking the remainder (%) removes these complete rotations. 
    k = k % n;
									
    Array.Reverse(nums);
    Array.Reverse(nums, 0, k);
    Array.Reverse(nums, k, n - k);
}
```

The three-reversal algorithm does this:
```C#
// e.g. in an array 1,2,3,4,5 where n = 3:

Array.Reverse(nums);             // O(n) 	 -> 5,4,3,2,1  first, reverse entire array
Array.Reverse(nums, 0, k);       // O(k) 	 -> 3,4,5,2,1  then, reverse beginning section up to k
Array.Reverse(nums, k, n - k);   // O(n - k) -> 3,4,5,1,2  finally, reverse remaining section after k 
```

Total time: `O(n)`
```
O(n) + O(k) + O(n - k)
= O(2n)
= O(n)
```
The constant factor is about twice that of a single pass, but asymptotically it is still `O(n)` and uses `O(1)` additional memory.

#### Copy to new array
If allocating a new array is acceptable then.

```C#
public static int[] Rotate(int[] nums, int k)
{
    if (nums == null || nums.Length == 0)
        return null;

    int n = nums.Length;

    // Normalize rotation amount so it's always between 0 and n - 1.
    // Taking the remainder (%) removes these complete rotations. 
    k = k % n;

    int[] rotated = new int[nums.Length];

    for (int i = 0; i < n; i++)
    {
        int position = (i + k) % n;
        rotated[position] = nums[i];
    }

    return rotated;
}

```

Copy to new array algorithm does this:

**Step 1: Start with the current index**

Loop through over every element in the current the array, where `i` is the current index in the original array.

**Step 2: Calculating the index in the destination array - shift it right by `k`**

The current element at `i` should be placed after rotating the array to the right by `k` positions.

**Step 3: Wrap around the end**

This is where `% n` comes in. The modulo operator is what makes the indices "wrap around" instead of going out of bounds.

So, if the result extends past the end of the array, you wrap it back to the beginning. This guarantees that newIndex is always between 0 and n - 1, making it a valid array index.

`newIndex = (oldIndex + k) % arrayLength`

```CSharp
// e.g. in an array 1,2,3,4,5 where n = 3:

for (int i = 0; i < n; i++)
{
    // when i = 3, then (i + k) = 6
    // and 6 % 5 = 1
    // So index 6 "wraps around" to index 1.
    int position = (i + k) % n;

    rotated[position] = nums[i];
}
```

Total time: `O(n)`
| Operation     | Complexity         |
| ------------- | ------------------ |
| `nums.Length` | O(1)               |
| `new int[n]`  | O(n)               |
| `k = k % n`   | O(1)               |
| `for` loop    | O(n)               |
| Loop body     | O(1) per iteration |
| `return`      | O(1)               |


### Fibonnaci
The Fibonacci sequence is a famous series of numbers where each number is the sum of the two numbers before it. It starts like this: (0, 1, 1, 2, 3, 5, 8, 13, 21), and so on.

#### Return a single number
```CSharp
		// Return a single number
        public int Fibonacci(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException("Input cannot be negative", nameof(n));
            }
            else if (n == 0)
            {
                return 0;
            }
            else if (n == 1)
            {
                return 1;
            }
            int prev = 0, next = 1, sum = 0;
            for (int i = 2; i <= n; i++)
            {
                sum = prev + next;
                prev = next;
                next = sum;
            }
            return sum;
        }
```
Steps
- return `0` and `1` immediately
- set variables `int prev = 0, next = 1, sum = 0;`
- the `for` loop starts at 2 `for (int i = 2; i <= n; i++)`
- set sum `sum = prev + next;`, then `prev = next;` and `next = sum;`
- at the end of the loop return final `sum`

**Time Complexity: `O(n)`**

The key part is the loop, which executes `n - 1` times (approximately `n` times).

Each iteration performs a fixed number of constant-time operations:
- one addition
- three assignments
- one comparison
- loop increment

Since the work per iteration is constant `O(1) × n = O(n)`

**Space Complexity: `O(1)`**

The algorithm uses only three integer variables:
```C#
htt prev = 0;
int next = 1;
int sum = 0;
```
Regardless of how large n becomes, it doesn't allocate any additional memory.

Therefore space = `O(1)`

#### Iterate over a sequence
```C#
		// iterate over a sequence
        public static IEnumerable<int> Fibonacci(int n)
        {
            int prev = 0, next = 1;

            yield return prev;
            yield return next;

            for (int i = 2; i <= n; i++)
            {
                int sum = prev + next;
                yield return sum;
                prev = next;
                next = sum;
            }
        }
```

**Time Complexity: `O(n)`**

The loop executes approximately `n - 1` times, and each iteration performs:
- one addition `O(1)`
- three assignments, each `O(1)`
- one yield return `O(1)`

Therefore `O(1) × n = O(n)`.

**Space Complexity: `O(1)`**

The iterator maintains only a few local variables:
```C#
int prev = 0;
int next = 1;
int sum;
```
The compiler transforms the method into a state machine that stores these variables between calls to `MoveNext()`, but the amount of state does not grow with `n`.

Therefore the extra space is `O(1)`

### Search algorithm
| Algorithm                             | Requires Sorted Data | Average Time | Worst Time  | Typical Use                   |
| ------------------------------------- | -------------------- | ------------ | ----------- | ----------------------------- |
| Linear Search                         | No                   | O(n)         | O(n)        | Small or unsorted collections |
| Binary Search                         | Yes                  | O(log n)     | O(log n)    | Large sorted collections      |
| Hash Lookup (`Dictionary`, `HashSet`) | No                   | O(1)         | O(n) (rare) | Fast key-based lookups        |

Which should you use?
- Linear Search: Small collections or unsorted data.
- Binary Search: Large, sorted collections.
- `Dictionary<TKey, TValue>`: Fast lookup by key.
- `HashSet<T>`: Fast existence checks (e.g., "does this item exist?").

### Sort algorithm



### Currency Converter
The following class has been written to convert an amount to USD.
`IExchangeService` is an interface to a vendor supplied API for getting the spot rate for a currency.
This class will be handed to other teams. Can it be written better?

```C#
    public interface IExchangeRateProvider
    {
        Task<decimal?> GetSpotRateAsync(string code);
    }

    public static class  ConvertToUSDService
    {
        public static decimal Convert(string currencyCode, decimal amount, IExchangeRateProvider exchangeRateProvider)
        {
            var rate = exchangeRateProvider.GetSpotRateAsync(currencyCode).Result;

            var convertedAmount = amount * rate.Value;

            return convertedAmount;
        }
    }
```
Key improvements:
- Change static to instance class that implements an interace that can be injected, and mocked in unit tests.
- Validate the `currencyCode` and `amount` to avoid unnecesary calls to the exchange rate provider.
- Use await `_exchangeRateProvider.GetSpotRateAsync` without blocking (await all the way down).
- Cache spot rates for a short time, to reduce calls to the exchange rate provider.
- Use a `SemaphoreSlim` per currency to prevent multiple concurrent calls to the exchange rate provider for the same currency.
- Introduce error handling.
- Return a structured and meaningful response with `Response`.

```C#
    // Rather than use a static class for the service, create an interface that
    // can be injected into consuming classes, and can be mocked in unit tests.
    public interface IUSDConversionService
    {
        Task<Result> ConvertAsync(string currencyCode, decimal amount);
    }

    // Example cache interface with expiry
    public interface ICache<T>
    {
        T Get(string key);
        void Set(string key, T value, TimeSpan expiration);
    }

    // Return a structured response, including when unsuccessful.
    public record Result (bool Success, decimal? ConvertedAmount = null, string Message = "");

    public class USDConversionService : IUSDConversionService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ICache<decimal?> _cache;

        private ConcurrentDictionary<string, SemaphoreSlim> _semaphore = [];

        // Constructor injection of the exchange rate provider
        public USDConversionService(IExchangeRateProvider exchangeRateProvider, ICache<decimal?> cache)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _cache = cache;
        }

        // Async method to convert currency to USD using the exchange rate provider
        public async Task<Result> ConvertAsync(string currencyCode, decimal amount)
        {
            // Fail fast - add validation for currencyCode and amount
            if (string.IsNullOrWhiteSpace(currencyCode) 
                || currencyCode.Length != 3
                || currencyCode.All(char.IsLetter) == false)
            {
                return new Result(false, Message: "Invalid currency code. Must be 3 letters.");
            }

            if (amount == 0)
            {
                return new Result(false, Message: "Amount cannot be 0");
            }

            try
            {
                decimal? rate = await GetRateAsync(currencyCode);

                if (rate == null)
                {   
                    return new Result(false, Message: "Failed to retrieve exchange rate");
                }

                decimal convertedAmount = amount * rate.Value;

                return new Result(true, convertedAmount);
            }
            catch(Exception ex)
            {
                return new Result(false, Message: ex.Message);
            }
        }

        private async Task<decimal?> GetRateAsync(string currencyCode)
        {
            // First check the cache for the exchange rate before calling the provider.
            decimal? cachedRate = _cache.Get(currencyCode);

            if (cachedRate.HasValue)
            {
                // Return the cached rate if it exists, no
                // need to call the exchange rate provider.
                return cachedRate;
            }

            // Semaphore prevents multiple concurrent calls to the exchange rate provider for the same currency.
            // Semaphore per currency code for better performance.
            SemaphoreSlim semaphore = _semaphore.GetOrAdd(currencyCode, new SemaphoreSlim(1, 1));

            // short timeout
            bool access = await semaphore.WaitAsync(TimeSpan.FromSeconds(5));

            if(access == false)
            {
                // If we can't get access to the semaphore within the timeout, throw an exception.
                throw new OperationCanceledException("Request timed out");
            }

            try
            {
                // Check the cache again after aquiring the lock, before calling the provider.
                cachedRate = _cache.Get(currencyCode);

                if (cachedRate.HasValue)
                {
                    // Return the cached rate if it exists, no
                    // need to call the exchange rate provider.
                    return cachedRate;
                }

                decimal? rate = await _exchangeRateProvider.GetSpotRateAsync(currencyCode);

                if (rate != null)
                {
                    // Cache the rate for 5 minutes to reduce calls to the exchange rate provider.
                    _cache.Set(currencyCode, rate, TimeSpan.FromMinutes(5));
                }

                return rate;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
```

### Compute Latest Positions
Fix the following code. It does not compile and isn't efficient. Can you figure out why and adjust the code?

```C#
    record Trade(int TradeId, int SecurityId, DateTime TradeDate, string BuyOrSell, int Quantity);
    record Position(int SecurityId, int Quantity);

    public class PositionCalculator
    {
        public List<Position> ComputeLatestPositions(List<Trade> trades, ImmutableList<Position> previousPositions = null)
        {
            foreach (var t in trades)
            {
                lock (this)
                {
                    var previousPosition = previousPositions.First(p => p.SecurityId == t.TradeId);
                    previousPosition.Quantity += t.Quantity;
                }
            }

            return null;
        }
    }
```

Key improvements:
- Use immutable records correctly. Create a new position because `previousPosition.Quantity` can't be updated.
- Use the correct lookup key `SecurityId` instead of `p.SecurityId == t.TradeId`.
- Handle missing positions by adding positions for a new security instead of `previousPositions.First(...)`, which throws when no existing position exists.
- Correctly processes `BUY` and `SELL` by adding `BUY` and subtract `SELL` quantities, and ignore invalid trade directions.
- Add validation to prevent unexpected `NullReferenceExceptions`, so `trades` can't be null, and `null` trade entries are ignored.
- Improve performance by using a dictionary keyed by `SecurityId` for `O(n + m)`.
- Return a `result` instead of `null`.
- Remove unnecessary thread synchronization while remaining thread-safe. The method is naturally thread-safe because it operates entirely on local variables so there is no shared mutable state. Using `lock(this)` is bad practice because external code can also lock the same object, causing deadlocks.
- Use more flexible interface `IReadOnlyList<T>` instead of `ImmutableList<T>`, allowing users to pass in from a wider range of source type including `List<Trade>`, `Trade[]`, `ImmutableList<Trade>` and `ReadOnlyCollection<Trade>`. 

> [!IMPORTANT]
>
> Use a dictionary keyed by `SecurityId` for `O(n + m)` where the complexity is approximately `O(n + m)`. position. Don't search positions with `First()` inside a loop which is `O(n * m)`.
>
> List lookup using `First(...)`   -> `O(number of trades * number of positions)` 
> Dictionary keyed by `SecurityId` -> `O(number of trades + number of positions)` 

> [!TIP]
>
> When using a Dictionary, use `ContainsKey(...)` which requires a single lookup, instead of two with `ContainsKey(...)` followed by `result[key]`.
>
> ```C#
>   // GOOD - one lookup
>   TryGetValue(...)
>
>   // BAD - two lookups
>   ContainsKey(...)
>   result[key]
> ``` 

```C#
    public record Trade(int TradeId, int SecurityId, DateTime TradeDate, string BuyOrSell, long Quantity);
    public record Position(int SecurityId, long Quantity);

    public class PositionCalculator
    {
        public List<Position> ComputeLatestPositions(
            IReadOnlyList<Trade> trades,
            IReadOnlyList<Position> previousPositions = null)
        {
            ArgumentNullException.ThrowIfNull(trades);

            var result = previousPositions != null
                ? previousPositions.ToDictionary(p => p.SecurityId, p => p.Quantity)
                : new Dictionary<int, long>();

            foreach (var t in trades)
            {
                if (t is null)
                {
                    continue;
                }

                long amount;

                if (string.Equals(t.BuyOrSell, "BUY", StringComparison.OrdinalIgnoreCase))
                {
                    amount = t.Quantity;
                }
                else if (string.Equals(t.BuyOrSell, "SELL", StringComparison.OrdinalIgnoreCase))
                {
                    amount = -t.Quantity;
                }
                else
                {
                    continue;
                }

                if (result.TryGetValue(t.SecurityId, out var quantity))
                {
                    result[t.SecurityId] = quantity + amount;
                }
                else
                {
                    result.Add(t.SecurityId, amount);
                }
            }

            return result
                .Select(kv => new Position(kv.Key, kv.Value))
                .ToList();
        }
    }
```

### Calculate Moving Average
Write a moving average calculator class for a single security.

Think about how this class should be implemented, for example:
- should it use a push based or pull based approach?
- it should be unit testable

```C#
// Interface used to abstract the system clock.
// This allows time to be mocked during unit testing.
public interface IClock
{
    DateTime GetCurrentDateTime();
}

// Implements the Observer pattern so prices can be pushed into
// the calculator as they arrive.
internal sealed class MovingAverageCalculator : IObserver<decimal>
{
    // The moving average time window.
    private readonly TimeSpan _duration;

    // Clock abstraction for testability.
    private readonly IClock _clock;

    // Queue of prices with the time they were received.
    // Old prices can be removed efficiently from the front.
    private readonly Queue<(decimal Value, DateTime Timestamp)> _values =
        new Queue<(decimal, DateTime)>();

    // Synchronises access if prices are pushed/read concurrently.
    private readonly object _gate = new();

    // Running total of all values currently in the queue.
    // This allows O(1) average calculation.
    private decimal _sum = 0;

    // Constructor.
    public MovingAverageCalculator(TimeSpan duration, IClock clock)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(duration),
                "Duration must be greater than zero.");
        }

        _duration = duration;
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    // Returns the current moving average.
    public decimal GetMovingAverage()
    {
        lock (_gate)
        {
            // Remove any expired values first.
            RemoveExpiredPrices();

            // Return the average if there are values,
            // otherwise return zero.
            return _values.Count > 0
                ? _sum / _values.Count
                : 0;
        }
    }

    // Attempts to get the current moving average.
    // Returns false when no prices are currently available.
    public bool TryGetMovingAverage(out decimal average)
    {
        lock (_gate)
        {
            // Remove any expired values first.
            RemoveExpiredPrices();

            if (_values.Count == 0)
            {
                average = 0;
                return false;
            }

            average = _sum / _values.Count;
            return true;
        }
    }

    // Called whenever a new price arrives.
    public void OnNext(decimal value)
    {
        lock (_gate)
        {
            // Add the new value with the current timestamp.
            _values.Enqueue((value, _clock.GetCurrentDateTime()));

            // Update the running total.
            _sum += value;

            // Remove expired prices.
            RemoveExpiredPrices();
        }
    }

    // Required by IObserver<T>.
    // No implementation needed for this exercise.
    public void OnCompleted()
    {
    }

    // Required by IObserver<T>.
    public void OnError(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);
    }

    // Removes all values older than the configured duration.
    private void RemoveExpiredPrices()
    {
        DateTime now = _clock.GetCurrentDateTime();

        while (_values.Count > 0 &&
               now - _values.Peek().Timestamp > _duration)
        {
            // Remove the oldest value from the running total.
            _sum -= _values.Peek().Value;

            // Remove it from the queue.
            _values.Dequeue();
        }
    }
}
```
**Review**
- Push-based design using `IObserver<decimal>`.
- Queue-based storage for efficient `O(1)` append/removal.
- `O(1)` running average calculation using `_sum / _values.Count`.
- Time-window expiry so only prices inside the configured duration are used.
- Injected clock abstraction for deterministic unit testing.
- Constructor validation for invalid duration and null dependencies.
- Thread safety using a private lock.
- Clear empty-state handling via `TryGetMovingAverage`.
- Sealed class to prevent unnecessary inheritance.

### Code Challenge
#### Easy
##### Two Sum
Given an array of integers and a target, return the indices of the two numbers that add up to the target.
```C#
Input:
nums = [2, 7, 11, 15]
target = 9

Output:
[0, 1]

Test output:
TwoSum([2, 7, 11, 15], 9)   -> [0, 1]
TwoSum([3, 2, 4], 6)        -> [1, 2]
TwoSum([3, 3], 6)           -> [0, 1]
TwoSum([1, 2, 3], 7)        -> Exception ("No solution exists.")
```
Skills
- `Dictionary<TKey, TValue>`
- `O(n)` lookup
```C#
    public static int[] TwoSum(int[] nums, int target)
    {
        ArgumentNullException.ThrowIfNull(nums);

        // Maps a number to its index.
        Dictionary<int, int> seen = new();

        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];

            // If we've already seen the complement,
            // we've found the required pair.
            if (seen.TryGetValue(complement, out int index))
            {
                return [index, i];
            }

            // Store the current value and its index.
            seen[nums[i]] = i;
        }

        throw new InvalidOperationException("No solution exists.");
    }
```
Steps
- Create `Dictionary<int, int>`
- Loop through the array and for each item `int complement = target - nums[i];`
- Check if the complement is in the dictionary `if (seen.TryGetValue(complement, out int index))`
  	- then return its value and the current element position `return [index, i];`
  	- else, add the value and its position to the dictionary `seen[nums[i]] = i;`

Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(n)** |

The algorithm performs a single pass through the array. Each dictionary lookup and insertion is `O(1)`n average, giving an overall time complexity of `O(n)` while using `O(n)` additional space.

##### Rotate Array
Rotate an array to the right by `k` positions.
```C#
Input:
nums = [1, 2, 3, 4, 5, 6, 7]
k = 3

Output:
[5, 6, 7, 1, 2, 3, 4]

Test output:
Rotate([1, 2, 3, 4, 5, 6, 7], 3) -> [5, 6, 7, 1, 2, 3, 4]
Rotate([-1, -100, 3, 99], 2)     -> [3, 99, -1, -100]
Rotate([1], 10)                  -> [1]
Rotate([1, 2], 0)                -> [1, 2]
```
Skills
- Array manipulation
- Modulo arithmetic
- In-place algorithm
- `O(n)` traversal
```C#
    public static void Rotate(int[] nums, int k)
    {
        ArgumentNullException.ThrowIfNull(nums);

        if (k < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        if (nums.Length <= 1)
        {
            return;
        }

        // Reduce the number of rotations to the minimum required.
        k %= nums.Length;

        if (k == 0)
        {
            return;
        }

        // Reverse the entire array.
        Reverse(nums, 0, nums.Length - 1);

        // Reverse the first k elements.
        Reverse(nums, 0, k - 1);

        // Reverse the remaining elements.
        Reverse(nums, k, nums.Length - 1);
    }

    private static void Reverse(int[] nums, int left, int right)
    {
        while (left < right)
        {
            (nums[left], nums[right]) = (nums[right], nums[left]);
            left++;
            right--;
        }
    }
```
Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(1)** |

The algorithm performs three linear passes over the array using the reverse technique. Since each element is visited a constant number of times, the overall time complexity is `O(n)` while requiring only `O(1)` additional space because the rotation is performed in-place.

##### Valid Parentheses
Given a string containing only the characters `(`, `)`, `{`, `}`, `[` and `]`, determine whether the brackets are correctly balanced. Every opening bracket must be closed by the same type of bracket, and in the correct order.
```C#
Input:
s = "([{}])"

Output:
true

Test output:
IsValid("()")       -> true
IsValid("()[]{}")   -> true
IsValid("([{}])")   -> true
IsValid("(]")       -> false
IsValid("([)]")     -> false
IsValid("((")       -> false
IsValid("]")        -> false
IsValid("")         -> true
```
Skills
- `Stack<T>`
- Character processing
- `O(n)` traversal
```C#
    public static bool IsValid(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        Stack<char> stack = new();

        foreach (char character in input)
        {
            switch (character)
            {
                case '(':
                case '[':
                case '{':
                    stack.Push(character);
                    break;

                case ')':
                    if (stack.Count == 0 || stack.Pop() != '(')
                    {
                        return false;
                    }
                    break;

                case ']':
                    if (stack.Count == 0 || stack.Pop() != '[')
                    {
                        return false;
                    }
                    break;

                case '}':
                    if (stack.Count == 0 || stack.Pop() != '{')
                    {
                        return false;
                    }
                    break;

                default:
                    throw new ArgumentException(
                        $"Invalid character '{character}'.",
                        nameof(input));
            }
        }

        // All opening brackets must have been matched.
        return stack.Count == 0;
    }
```
Steps
- Create a `Stack<char> stack = new();`
- Iterate over the sequence
  - if opening parenthesis `push` onto the stack
  - if closing parenthesis:
    - if `stack.Count == 0`, then fail
    - `stack.Pop()` must be the corresponding opening parenthesis, else fail
- at the end of the sequence `stack.Count` must be 0 to succeed, else fail 
 
Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(n)** |

The algorithm processes the input string once. Each opening bracket is pushed onto the stack and each closing bracket results in at most one pop operation. In the worst case (all opening brackets), the stack stores every character, resulting in `O(n)` time and `O(n)` auxiliary space.

##### Remove Duplicates from Sorted Array
Given a sorted array of integers, remove the duplicates in-place such that each unique element appears only once. Return the number of unique elements. The first `k` elements of the array should contain the unique values.
```C#
Input:
nums = [1, 1, 2, 2, 3]

Output:
k = 3
nums = [1, 2, 3, _, _]

Test output:
RemoveDuplicates([1, 1, 2])                    -> 2, [1, 2, _]
RemoveDuplicates([0, 0, 1, 1, 1, 2, 2, 3, 3])  -> 4, [0, 1, 2, 3, _, _, _, _, _]
RemoveDuplicates([1])                          -> 1, [1]
RemoveDuplicates([])                           -> 0, []
```
Skills
- Two-pointer technique
- In-place array manipulation
- `O(n)` traversal
- `O(1)` extra space
```C#
    public static int RemoveDuplicates(int[] nums)
    {
        ArgumentNullException.ThrowIfNull(nums);

        if (nums.Length == 0)
        {
            return 0;
        }

        // The index where the next unique value will be written.
        int writeIndex = 1;

        for (int readIndex = 1; readIndex < nums.Length; readIndex++)
        {
            // Copy only unique values forward.
            if (nums[readIndex] != nums[writeIndex - 1])
            {
                nums[writeIndex] = nums[readIndex];
                writeIndex++;
            }
        }

        return writeIndex;
    }
```
Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(1)** |

The algorithm uses two pointers: one to read through the array and one to write the next unique value. Each element is examined exactly once, resulting in `O(n)` time complexity. Since the duplicates are removed in-place without allocating another array, the extra space complexity is `O(1)`.

##### Binary Search
Given a sorted array of integers and a target value, return the index of the target if it exists; otherwise, return `-1`.
```C#
Input:
nums = [1, 3, 5, 7, 9]
target = 7

Output:
3

Test output:
BinarySearch([1, 3, 5, 7, 9], 7) -> 3
BinarySearch([1, 3, 5, 7, 9], 1) -> 0
BinarySearch([1, 3, 5, 7, 9], 9) -> 4
BinarySearch([1, 3, 5, 7, 9], 4) -> -1
BinarySearch([], 5)              -> -1
```
Skills
- Binary search
- Divide and conquer
- `O(log n)` lookup
- Sorted arrays
```C#
    public static int BinarySearch(int[] nums, int target)
    {
        ArgumentNullException.ThrowIfNull(nums);

        int left = 0;
        int right = nums.Length - 1;

        while (left <= right)
        {
            // Prevent integer overflow when calculating the midpoint.
            int middle = left + ((right - left) / 2);

            if (nums[middle] == target)
            {
                return middle;
            }

            if (nums[middle] < target)
            {
                left = middle + 1;
            }
            else
            {
                right = middle - 1;
            }
        }

        return -1;
    }
```
Complexity
| Operation |   Complexity |
| --------- | -----------: |
| Time      | **O(log n)** |
| Space     |     **O(1)** |

Binary search repeatedly halves the search range until the target is found or no elements remain. Since the search space is reduced by half on each iteration, the algorithm runs in `O(log n)` time while requiring only `O(1)` additional space. It is one of the most efficient algorithms for searching a sorted array.

#### Medium
##### Reverse Words
Reverse Words (Medium)
Given a string containing one or more words separated by spaces, return a new string with the words in reverse order.
Assume words are separated by one or more spaces. The output should contain a single space between each word and no leading or trailing spaces.
```C#
Input:
"The quick brown fox"

Output:
"fox brown quick The"

Test output:
ReverseWords("The quick brown fox") -> "fox brown quick The"
ReverseWords("Hello World")         -> "World Hello"
ReverseWords("  a good   example ") -> "example good a"
ReverseWords("ChatGPT")             -> "ChatGPT"
ReverseWords("")                    -> ""
```
Skills
- String manipulation
- Arrays
- `StringSplitOptions`
- `O(n)` traversal
```C#
    public static string ReverseWords(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.Length == 0)
        {
            return string.Empty;
        }

        // Split the input into words, ignoring extra whitespace.
        string[] words = input.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        // Reverse the order of the words.
        Array.Reverse(words);

        // Join the words together with a single space.
        return string.Join(' ', words);
    }
```
Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(n)** |

The algorithm splits the input into words, reverses the array of words, and joins them back together. Each character is processed a constant number of times, resulting in `O(n)` time complexity. An additional array of words is created during the split operation, so the auxiliary space complexity is `O(n)`.

> [!TIP]
> 
> Interview follow-up:
>
> A common follow-up question is to solve this in-place using a `char[]` without calling `Split()` or `Array.Reverse()`. The in-place solution first reverses the entire character array, then reverses each individual word, achieving `O(n)` time while using `O(1)` additional space (excluding the character array itself).

##### Longest Common Prefix
Given an array of strings, return the longest common prefix shared by all the strings. If there is no common prefix, return an empty string.
```C#
Input:
["flower", "flow", "flight"]

Output:
"fl"

Test output:
LongestCommonPrefix(["flower", "flow", "flight"]) -> "fl"
LongestCommonPrefix(["dog", "racecar", "car"])    -> ""
LongestCommonPrefix(["interspecies", "interstellar", "interstate"]) -> "inters"
LongestCommonPrefix(["hello"])                    -> "hello"
LongestCommonPrefix([])                           -> ""
```
Skills
- String manipulation
- Character comparison
- `O(n × m)` traversal
> Where `n` is the number of strings and `m` is the length of the shortest string.
```C#
        public static string LongestCommonPrefix3(string[] strings)
        {
            if (strings == null || strings.Length == 0)
            {
                return string.Empty;
            }

            string? first = strings[0];

            if (first == null)
            {
                return string.Empty;
            }

			// outer loop over each character in the first string
            for (int pos = 0; pos < first.Length; pos++)
            {
                char expected = first[pos];

				// inner loop over each subsequent string in array
                for (int i = 1; i < strings.Length; i++)
                {
                    string? current = strings[i];

                    if (current == null ||
                        current.Length <= pos ||
                        current[pos] != expected)
                    {
						// return common prefix as soon as we no longer have a match
                        return first.Substring(0, pos);
                    }
                }
            }

            return first; // return first string as common prefix
        }
```
Steps
- outer loop over each character in the first string
  - inner loop over each subsequent string in array
	- if any string is null, shorter than current `pos`, char at `pos` doesn't match then `return strings[0].Substring(0, pos);` 
- if outer loop finished, return the whole first string as common prefix `return strings[0];`

Complexity
| Operation |   Complexity |
| --------- | -----------: |
| Time      | **O(n × m)** |
| Space     |     **O(1)** |

The algorithm starts by assuming the first string is the common prefix. It then compares each character position across all strings. It stops immediately when it finds a mismatch or reaches the end of the first string, resulting in `O(n × m)`, where `n` is the number of strings and `m` is the length of the first string.

##### Merge Intervals
Given an array of intervals where each interval is represented as `[start, end]`, merge all overlapping intervals and return the resulting non-overlapping intervals.
```C#
Input:
[[1, 3], [2, 6], [8, 10], [15, 18]]

Output:
[[1, 6], [8, 10], [15, 18]]

Test output:
MergeIntervals([[1, 3], [2, 6], [8, 10], [15, 18]])
-> [[1, 6], [8, 10], [15, 18]]

MergeIntervals([[1, 4], [4, 5]])
-> [[1, 5]]

MergeIntervals([[1, 10], [2, 3], [4, 8]])
-> [[1, 10]]

MergeIntervals([[1, 2]])
-> [[1, 2]]

MergeIntervals([])
-> []
```
Skills
- Sorting
- Interval comparison
- `List<T>`
- `O(n log n)` processing
```C#
    public static int[][] MergeIntervals(int[][] intervals)
    {
        ArgumentNullException.ThrowIfNull(intervals);

        if (intervals.Length == 0)
        {
            return [];
        }

        foreach (int[] interval in intervals)
        {
            if (interval is null || interval.Length != 2)
            {
                throw new ArgumentException(
                    "Each interval must contain exactly two values.",
                    nameof(intervals));
            }

            if (interval[0] > interval[1])
            {
                throw new ArgumentException(
                    "An interval's start value cannot be greater than its end value.",
                    nameof(intervals));
            }
        }

        // Sort the intervals by their start value.
        Array.Sort(intervals, static (left, right) =>
            left[0].CompareTo(right[0]));

        List<int[]> mergedIntervals = [];

        // Copy the first interval so the input interval is not modified.
        int[] currentInterval =
        [
            intervals[0][0],
            intervals[0][1]
        ];

        for (int i = 1; i < intervals.Length; i++)
        {
            int[] nextInterval = intervals[i];

            if (nextInterval[0] <= currentInterval[1])
            {
                // The intervals overlap, so extend the current interval.
                currentInterval[1] = Math.Max(
                    currentInterval[1],
                    nextInterval[1]);
            }
            else
            {
                // The intervals do not overlap, so store the current interval.
                mergedIntervals.Add(currentInterval);

                currentInterval =
                [
                    nextInterval[0],
                    nextInterval[1]
                ];
            }
        }

        // Add the final interval.
        mergedIntervals.Add(currentInterval);

        return [.. mergedIntervals];
    }
```
Complexity
| Operation |     Complexity |
| --------- | -------------: |
| Time      | **O(n log n)** |
| Space     |       **O(n)** |

The intervals are first sorted by their start values, which takes `O(n log n)` time. The algorithm then makes one linear pass through the sorted intervals, merging overlaps in `O(n)` time.

The returned collection may contain up to n intervals, resulting in `O(n)` output space. Excluding the returned result, the algorithm uses `O(1)` additional working space, although `Array.Sort` may use implementation-dependent stack space.

##### Group Anagrams
Given an array of strings, group together all strings that are anagrams of each other. Two words are anagrams if they contain the same characters with the same frequencies.

The groups may be returned in any order.

Description
```C#
Input:
["eat", "tea", "tan", "ate", "nat", "bat"]

Output:
[
    ["eat", "tea", "ate"],
    ["tan", "nat"],
    ["bat"]
]

Test output:
GroupAnagrams(["eat", "tea", "tan", "ate", "nat", "bat"])
-> [["eat", "tea", "ate"], ["tan", "nat"], ["bat"]]

GroupAnagrams([""])
-> [[""]]

GroupAnagrams(["a"])
-> [["a"]]

GroupAnagrams(["abc", "bca", "cab", "xyz"])
-> [["abc", "bca", "cab"], ["xyz"]]

GroupAnagrams([])
-> []
```
Skills
- `Dictionary<TKey, TValue>`
- String manipulation
- Sorting
- `O(n × k log k)` processing
> Where `n` is the number of strings and `k` is the average length of a string.
```C#
    public static IList<IList<string>> GroupAnagrams(string[] words)
    {
        ArgumentNullException.ThrowIfNull(words);

        Dictionary<string, List<string>> groups = new();

        foreach (string word in words)
        {
            ArgumentNullException.ThrowIfNull(word);

            // Create a canonical key by sorting the characters.
            char[] characters = word.ToCharArray();
            Array.Sort(characters);

            string key = new(characters);

            if (!groups.TryGetValue(key, out List<string>? group))
            {
                group = [];
                groups.Add(key, group);
            }

            group.Add(word);
        }

        List<IList<string>> result = [];

        foreach (List<string> group in groups.Values)
        {
            result.Add(group);
        }

        return result;
    }
```
Complexity
| Operation |         Complexity |
| --------- | -----------------: |
| Time      | **O(n × k log k)** |
| Space     |       **O(n × k)** |

For each of the `n` input strings, the algorithm sorts its `k` characters to produce a canonical dictionary key, which takes `O(k log k)` time. Dictionary lookups and insertions are `O(1)` on average, resulting in an overall time complexity of `O(n × k log k)`.

The dictionary stores every input string along with its corresponding key, requiring `O(n × k)` additional space.

> [!TIP]
>
> Interview follow-up:
>
> An optimized solution avoids sorting each word by constructing a frequency-count key (for example, the counts of `'a'` through `'z'`). This reduces the time complexity to `O(n × k)` while using a slightly more complex key-generation algorithm.

##### Maximum Subarray
Given an array of integers, find the contiguous subarray with the largest sum and return that sum.

The subarray must contain at least one element.

For each element, you have two choices:
- Extend the previous subarray (if it was positive, it helps you)
- Start a new subarray at the current element (if the previous sum was negative, it hurts you)

This greedy/dynamic decision at every step ensures the global optimum emerges naturally.
```C#
Input:
nums = [-2, 1, -3, 4, -1, 2, 1, -5, 4]

Output:
6

Subarray:
[4, -1, 2, 1]

Test output:
MaximumSubarray([-2, 1, -3, 4, -1, 2, 1, -5, 4]) -> 6
MaximumSubarray([1])                               -> 1
MaximumSubarray([5, 4, -1, 7, 8])                 -> 23
MaximumSubarray([-3, -2, -5])                     -> -2
MaximumSubarray([-2, 1])                          -> 1
```
Skills
- Kadane's algorithm
- Dynamic programming
- Running totals
- `O(n)` traversal
```C#
    public static int MaximumSubarray(int[] nums)
    {
        ArgumentNullException.ThrowIfNull(nums);

        if (nums.Length == 0)
        {
            throw new ArgumentException(
                "The array must contain at least one element.",
                nameof(nums));
        }

        // The maximum sum of a subarray ending at the current position.
        int currentSum = nums[0];

        // The maximum sum found anywhere in the array.
        int maximumSum = nums[0];

        for (int i = 1; i < nums.Length; i++)
        {
            // Either extend the existing subarray or start a new one
            // from the current element.
            currentSum = Math.Max(
                nums[i],
                currentSum + nums[i]);

            // Record the largest sum found so far.
            maximumSum = Math.Max(maximumSum, currentSum);
        }

        return maximumSum;
    }
```
Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(1)** |

Kadane's algorithm examines each element once. At every position, it decides whether to extend the current subarray or begin a new subarray from the current element.

The algorithm stores only the current sum and the best sum found, resulting in `O(n)` time complexity and `O(1)` additional space.

##### Find Missing Number
Given an array containing `n - 1` distinct integers in the range `1` to `n`, find the missing number.

You may assume that exactly one number is missing.
```C#
Input:
nums = [1, 2, 3, 5]

Output:
4

Test output:
FindMissingNumber([1, 2, 3, 5])          -> 4
FindMissingNumber([2, 3, 1, 5])          -> 4
FindMissingNumber([1])                   -> 2
FindMissingNumber([2])                   -> 1
FindMissingNumber([1, 2, 4, 5, 6])       -> 3
```
Skills
- Arithmetic series
- Array traversal
- `O(n)` traversal
- `O(1)` extra space
```C#
    public static int FindMissingNumber(int[] nums)
    {
        ArgumentNullException.ThrowIfNull(nums);

        // The complete sequence contains one more value than the array.
        int n = nums.Length + 1;

        // Calculate the expected sum of the sequence 1..n.
        int expectedSum = n * (n + 1) / 2;

        // Calculate the actual sum of the values present.
        int actualSum = 0;

        foreach (int number in nums)
        {
            actualSum += number;
        }

        return expectedSum - actualSum;
    }
```
Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(1)** |

The algorithm calculates the expected sum of the integers from `1` to `n` using the arithmetic series formula and subtracts the sum of the values present in the array. Each element is visited exactly once, resulting in `O(n)` time complexity and `O(1)` additional space.

> [!TIP]
>
> Interview follow-up:
>
> An alternative solution uses the bitwise XOR operator. XOR-ing all numbers from `1` to `n` with every element in the array causes matching values to cancel out, leaving only the missing number. This approach also runs in `O(n)` time and `O(1)` space, while avoiding the possibility of integer overflow when summing very large sequences.

##### First Non-Repeating Character
Description
```C#
Input:
"swiss"

Output:
'w'

Test output:
FirstNonRepeatingCharacter("swiss")   -> 'w'
FirstNonRepeatingCharacter("aabbcc")  -> null
FirstNonRepeatingCharacter("leetcode")-> 'l'
FirstNonRepeatingCharacter("aabbcd")  -> 'c'
FirstNonRepeatingCharacter("")        -> null
```
Skills
- `Dictionary<TKey, TValue>`
- Character counting
- `O(n)` traversal
- Hash table lookup
```C#
    public static char? FirstNonRepeatingCharacter(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        Dictionary<char, int> characterCounts = [];

        // Count the occurrences of each character.
        foreach (char character in input)
        {
            if (characterCounts.TryGetValue(character, out int count))
            {
                characterCounts[character] = count + 1;
            }
            else
            {
                characterCounts.Add(character, 1);
            }
        }

        // Return the first character that occurs exactly once.
        foreach (char character in input)
        {
            if (characterCounts[character] == 1)
            {
                return character;
            }
        }

        return null;
    }
```
Complexity
| Operation | Complexity |
| --------- | ---------: |
| Time      |   **O(n)** |
| Space     |   **O(k)** |

The algorithm makes two passes over the string. The first pass counts the occurrences of each character using a dictionary, and the second pass finds the first character whose count is one.

Each dictionary lookup and update is `O(1)` on average, giving an overall time complexity of `O(n)`, where `n` is the length of the string. The dictionary stores at most k distinct characters, resulting in `O(k)` additional space, where `k` is the size of the character set encountered in the input.

#### Hard
##### Implement LRU Cache
Design a Least Recently Used cache with a fixed capacity.

The cache must support:
- `Get(key)`: return the value associated with the key, or `-1` if the key does not exist.
- `Put(key, value)`: insert or update a value.
- When the cache exceeds its capacity, remove the least recently used item.
```C#
Input:
capacity = 2

Put(1, 10)
Put(2, 20)
Get(1)
Put(3, 30)
Get(2)
Put(4, 40)
Get(1)
Get(3)
Get(4)

Output:
10
-1
-1
30
40

Test output:
LruCache cache = new(2);

cache.Put(1, 10);
cache.Put(2, 20);

cache.Get(1) -> 10

cache.Put(3, 30); // Evicts key 2.

cache.Get(2) -> -1

cache.Put(4, 40); // Evicts key 1.

cache.Get(1) -> -1
cache.Get(3) -> 30
cache.Get(4) -> 40
```
Skills
- `Dictionary<TKey, TValue>`
- `LinkedList<T>`
- Cache eviction
- `O(1)` lookup and update
```C#
public sealed class LruCache
{
    /// <summary>
    /// Represents a cache entry.
    /// </summary>
    private readonly record struct CacheEntry(int Key, int Value);

    private readonly int _capacity;

    // Maps each key to its node in the linked list.
    private readonly Dictionary<int, LinkedListNode<CacheEntry>> _entries = [];

    // The most recently used item is stored at the front.
    // The least recently used item is stored at the back.
    private readonly LinkedList<CacheEntry> _usageOrder = [];

    public LruCache(int capacity)
    {
        if (capacity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        _capacity = capacity;
    }

    /// <summary>
    /// Returns the value associated with a key and marks the entry as recently used.
    /// </summary>
    public int Get(int key)
    {
        if (!_entries.TryGetValue(key, out LinkedListNode<CacheEntry>? node))
        {
            return -1;
        }

        MoveToFront(node);

        return node.Value.Value;
    }

    /// <summary>
    /// Inserts or updates a cache entry and marks it as recently used.
    /// </summary>
    public void Put(int key, int value)
    {
        if (_entries.TryGetValue(key, out LinkedListNode<CacheEntry>? existingNode))
        {
            // Update the existing entry.
            existingNode.Value = new CacheEntry(key, value);
            MoveToFront(existingNode);
            return;
        }

        // Insert the new entry at the front because it is now
        // the most recently used item.
        LinkedListNode<CacheEntry> newNode =
            _usageOrder.AddFirst(new CacheEntry(key, value));

        _entries.Add(key, newNode);

        if (_entries.Count > _capacity)
        {
            RemoveLeastRecentlyUsed();
        }
    }

    /// <summary>
    /// Moves an existing entry to the front of the usage list.
    /// </summary>
    private void MoveToFront(LinkedListNode<CacheEntry> node)
    {
        _usageOrder.Remove(node);
        _usageOrder.AddFirst(node);
    }

    /// <summary>
    /// Removes the least recently used cache entry.
    /// </summary>
    private void RemoveLeastRecentlyUsed()
    {
        LinkedListNode<CacheEntry>? leastRecentlyUsed = _usageOrder.Last;

        if (leastRecentlyUsed is null)
        {
            return;
        }

        _usageOrder.RemoveLast();
        _entries.Remove(leastRecentlyUsed.Value.Key);
    }
}
```
Complexity
| Operation |       Complexity |
| --------- | ---------------: |
| `Get`     | **O(1)** average |
| `Put`     | **O(1)** average |
| Space     |  **O(capacity)** |

The dictionary provides constant-time average lookup by key. The linked list maintains usage order and supports constant-time insertion, removal, and movement when a node reference is already known.

The front of the list contains the most recently used entry, while the back contains the least recently used entry. When the cache exceeds its capacity, the final node is removed from both the linked list and dictionary.

#### Senior
##### Implement Producer/Consumer
Implement a thread-safe producer/consumer queue that allows multiple producers to publish items and one or more consumers to process them asynchronously.

The implementation should support cancellation, bounded capacity, backpressure, graceful completion, and exception propagation.

When the channel reaches its capacity, producers asynchronously wait until a consumer makes space available.
```C#
Capacity:
3

Produced:
1, 2, 3, 4, 5

Consumed:
1, 2, 3, 4, 5

Test output:
Produced: 1
Produced: 2
Consumed: 1
Produced: 3
Consumed: 2
Produced: 4
Consumed: 3
Produced: 5
Consumed: 4
Consumed: 5
Processing completed.
```
Skills
- `Channel<T>`
- Asynchronous producer/consumer processing
- Backpressure
- `CancellationToken`
- Graceful completion
```C#
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

/// <summary>
/// Provides a bounded asynchronous producer/consumer queue.
/// </summary>
/// <typeparam name="T">The type of item stored in the queue.</typeparam>
public sealed class ProducerConsumerQueue<T>
{
    private readonly Channel<T> _channel;

    /// <summary>
    /// Initializes a new bounded producer/consumer queue.
    /// </summary>
    /// <param name="capacity">
    /// The maximum number of items that may be waiting in the queue.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="capacity"/> is less than one.
    /// </exception>
    public ProducerConsumerQueue(int capacity)
    {
        if (capacity < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity),
                "Capacity must be greater than zero.");
        }

        BoundedChannelOptions options = new(capacity)
        {
            // Producers wait asynchronously when the queue is full.
            FullMode = BoundedChannelFullMode.Wait,

            // Allow multiple concurrent producers.
            SingleWriter = false,

            // Allow multiple concurrent consumers.
            SingleReader = false,

            // Prevent continuations from running synchronously inside channel operations.
            AllowSynchronousContinuations = false
        };

        _channel = Channel.CreateBounded<T>(options);
    }

    /// <summary>
    /// Asynchronously writes an item to the queue.
    /// </summary>
    /// <param name="item">The item to enqueue.</param>
    /// <param name="cancellationToken">
    /// A token used to cancel the write operation.
    /// </param>
    /// <returns>A task representing the asynchronous write operation.</returns>
    public ValueTask WriteAsync(T item, CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(item, cancellationToken);
    }

    /// <summary>
    /// Reads all items from the queue until it is completed.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel reading.
    /// </param>
    /// <returns>
    /// An asynchronous sequence containing queued items.
    /// </returns>
    public IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }

    /// <summary>
    /// Marks the queue as complete so that no more items can be written.
    /// </summary>
    /// <param name="exception">
    /// An optional exception that caused production to stop.
    /// </param>
    /// <returns>
    /// <c>true</c> if the queue was completed by this call; otherwise,
    /// <c>false</c>.
    /// </returns>
    public bool TryComplete(Exception? exception = null)
    {
        return _channel.Writer.TryComplete(exception);
    }

    /// <summary>
    /// Gets a task that completes when the queue has been fully drained.
    /// </summary>
    public Task Completion => _channel.Reader.Completion;
}

public static class Example
{
    /// <summary>
    /// Runs a producer/consumer demonstration.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>A task representing the complete workflow.</returns>
    public static async Task RunAsync(CancellationToken cancellationToken = default)
    {
        ProducerConsumerQueue<int> queue = new(capacity: 3);

        Task producerTask = ProduceAsync(queue, cancellationToken);

        Task[] consumerTasks =
        [
            ConsumeAsync(
                consumerId: 1,
                queue,
                cancellationToken),

            ConsumeAsync(
                consumerId: 2,
                queue,
                cancellationToken)
        ];

        try
        {
            await producerTask;

            // Signal that no more items will be written.
            queue.TryComplete();
        }
        catch (Exception exception)
        {
            // Complete the channel with the producer exception so consumers
            // and Completion observe the failure.
            queue.TryComplete(exception);
            throw;
        }

        // Wait for every consumer to finish draining the queue.
        await Task.WhenAll(consumerTasks);

        // Observe successful completion or any channel exception.
        await queue.Completion;

        Console.WriteLine("Processing completed.");
    }

    /// <summary>
    /// Produces sample values and writes them to the queue.
    /// </summary>
    /// <param name="queue">The queue that receives produced values.</param>
    /// <param name="cancellationToken">
    /// A token used to cancel production.
    /// </param>
    /// <returns>A task representing the producer operation.</returns>
    private static async Task ProduceAsync(ProducerConsumerQueue<int> queue, CancellationToken cancellationToken)
    {
        for (int value = 1; value <= 5; value++)
        {
            // WriteAsync waits when the bounded queue is full.
            await queue.WriteAsync(value, cancellationToken);

            Console.WriteLine($"Produced: {value}");
        }
    }

    /// <summary>
    /// Reads and processes values until the queue is completed.
    /// </summary>
    /// <param name="consumerId">The consumer identifier.</param>
    /// <param name="queue">The queue from which values are read.</param>
    /// <param name="cancellationToken">
    /// A token used to cancel consumption.
    /// </param>
    /// <returns>A task representing the consumer operation.</returns>
    private static async Task ConsumeAsync(int consumerId, ProducerConsumerQueue<int> queue, CancellationToken cancellationToken)
    {
        await foreach (int value in queue.ReadAllAsync(cancellationToken))
        {
            Console.WriteLine($"Consumer {consumerId} consumed: {value}");

            // Simulate asynchronous processing.
            await Task.Delay(
                TimeSpan.FromMilliseconds(100),
                cancellationToken);
        }
    }
}
```
Complexity
| Operation |       Complexity |
| --------- | ---------------: |
| Enqueue   | **O(1)** average |
| Dequeue   | **O(1)** average |
| Space     |  **O(capacity)** |

Each channel write and read is constant time on average. The bounded channel stores no more than the configured capacity, so queued-item storage is O(capacity).

`BoundedChannelFullMode.Wait` provides backpressure: when the queue is full, producers wait asynchronously rather than dropping items or allocating an unbounded amount of memory. Calling `TryComplete()` prevents further writes while allowing consumers to process all remaining items before they finish.

##### Next Challenge
Description
```C#
Input:

Output:

e.g.

```
Skills
- 
```C#
```
Complexity

## Glossary
* **Background GC** *- applies only to generation 2 collections and is enabled by default*
* **Base Class Library  (BCL)** *- a standard set of class libraries providing implementation for general functionality*
* **Boxing** *- the process of converting value types to objects or an interface implemented by the value type*
* **Char** *- a type representing a Unicode UTF-16 character* 
* **Common Intermediate Language (CIL)** *- instructions for loading, storing, initializing, and calling methods on objects, arithmetic and logical operations, control flow, direct memory access, exception handling etc*
* **Common Language Runtime (CLR)** *- .NET runtime responsible for managing code execution, memory and type safety etc.*
* **Common Language Specification (CLS)** *- subset of CTS that defines a set of common features needed by applications*
* **Common Type System (CTS)** *- defines rules all languages must follow when it comes to working with types*
* **Fixed** *- declares a pointer to a variable and fixes or "pins" it, so the garbage collection can't relocate it*
* **Garbage Collection** *- the process of releasing and compacting heap memory*
* **ILDASM.exe** *- IL Disassembler (ILDASM*
* **in Keyword** *- an argument is passed by reference, however it cannot be modified in the called method*
* **Just-In-Time compilation (JIT)** *- at runtime the JIT compiler translates MSIL into native code, which is processor specific code*
* **Lambda** *- lambda expression used to create anonymous functions*
* **Large Object Heap (LOH)** *- contains objects that are 85,000 bytes and larger, which are usually arrays*
* **LINQ** *- the name for a set of technologies based on the integration of query capabilities directly into the C# language*
* **Memory\<T>** *- similar to Span\<T> provides a type-safe representation of a contiguous region of memory, but unlike Span\<T> can be placed on the managed heap*
* **Managed Code** *- code whose execution is managed by a runtime*
* **Managed Heap** *- a segment of memory for storing and managing objects. All threads share the same heap*
* **Message Loop** *- responsible for processing and dispatching messages queued by the operating system, such as key presses and mouse clicks*
* **Method Parameters** *- arguments passed my value or by reference. Default is by value.*
* **.NET SDK** *-a set of libraries and tools for developing .NET applications*
* **out Keyword** *- an argument is passed by reference, however a value must be assigned to it in the called method*
* **OutOfMemoryException** *- is thrown when there is not enough memory to continue the execution of a program*
* **Pointers** *- a variable that holds the memory address of another type or variable, allowing direct access to it in memory*
* **ref Keyword** *- an argument passes a variables address into a method, rather than a copy of the variable*
* **Reference types** *- objects represented by a reference that points to where the object is stored in memory*
* **Ref Locals** *- variables that refers to other storage i.e. reference another variables storage*
* **Ref Returns** *- values returned by a method by reference i.e. modifying it will change the value in the called code*
* **Ref Structs** *- struct declared using the ref modifier and can only be allocated on the stack and not the managed heap*
* **Safe Handle** *- represents a wrapper class for operating system handles*
* **Span\<T>** *- provides a type-safe representation of a contiguous region of memory including heap, stack and unmanaged memory*
* **Stack** *- stores local variables and method parameters. Each thread has it's own stack memory which gives it context* 
* **stackalloc** *- allocates a block of memory on the stack*
* **StackOverflowException** *- thrown when the execution stack exceeds the stack size*
* **String** *- a reference type that stores text in a readonly collection of char objects. Strings are therefore immutable.*
* **Struct** *- a value type structure that can encapsulate data and related functionality*
* **System.Object** *- the base class of all .NET classes*
* **Thread** *- threads execute application code*
* **ThreadPool** *- a pool of threads that can be used to execute tasks*
* **ThreadStaticAttribute** *- A static field marked with ThreadStaticAttribute is not shared between threads. Each executing thread has a separate instance*
* **Unboxing** *- the process of explicitly converting an objects value, or interface type, to a value type*
* **Unmanaged resources** *- common types include files, windows, network connections, or database connections*
* **Unsafe code** *- allows direct access to memory using pointers*
* **Value types** *- objects represented by the value of the object*
* **Variables** *- represent storage locations*

## References
### **.NET Blogs**
  * [About Processes and Threads](https://learn.microsoft.com/en-us/windows/win32/procthread/about-processes-and-threads)
  * [All About Span: Exploring a New .NET Mainstay](https://learn.microsoft.com/en-us/archive/msdn-magazine/2018/january/csharp-all-about-span-exploring-a-new-net-mainstay)
  * [How Async/Await Really Works in C#](https://devblogs.microsoft.com/dotnet/how-async-await-really-works/)
  * [“Out Of Memory” Does Not Refer to Physical Memory](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/out-of-memory-does-not-refer-to-physical-memory)
  * [What is .NET, and why should you choose it?](https://devblogs.microsoft.com/dotnet/why-dotnet/)

### **Microsoft**
  * [Background GC](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/background-gc)
  * [**Boxing and Unboxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing)
  * [BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries)
  * [Char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char)
  * [CIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)
  * [CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)
  * [CTS & CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)
  * [Dispose Pattern](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose)
  * [Fixed](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/fixed)
  * [Garbage Collection](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#what-happens-during-a-garbage-collection)
  * [IL Disassembler (ILDASM)](https://learn.microsoft.com/en-us/dotnet/framework/tools/ildasm-exe-il-disassembler)
  * [in Keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier)
  * [Integrity of UI Components](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8)
  * [Lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions)
  * [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)
  * [LOH](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap)
  * [Managed Code](https://learn.microsoft.com/en-us/dotnet/standard/managed-code)
  * [Managed Execution Process](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process)
  * [Managed Heap](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap)
  * [Memory Management](https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management)
  * [Memory\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.memory-1)
  * [Method Parameters](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters)
  * [out Keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier)
  * [“Out Of Memory” Does Not Refer to Physical Memory](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/out-of-memory-does-not-refer-to-physical-memory)
  * [OutOfMemoryException](https://learn.microsoft.com/en-us/dotnet/api/system.outofmemoryexception)
  * [Performance](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance)
  * [Pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types)
  * [ref Keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref)
  * [Reference Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types)
  * [Ref Locals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals)
  * [Ref Returns](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values)
  * [Ref Structs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct)
  * [Safe Handle](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.safehandle)
  * [SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)
  * [Server GC](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#server-gc)
  * [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)
  * [stackalloc](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/stackalloc)
  * [StackOverflowException](https://learn.microsoft.com/en-us/dotnet/api/system.stackoverflowexception)
  * [String](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings)
  * [Struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct)
  * [System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)
  * [Thread](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread)
  * [ThreadPool](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool)
  * [ThreadStaticAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.threadstaticattribute)
  * [Unmanaged Resources](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/unmanaged)
  * [Unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code)
  * [Value Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types)
  * [Variables](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables)
  * [Workstation GC](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#workstation-gc)

### **Wikipedia**
  * [CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)
  * [CIL Instructions](https://en.wikipedia.org/wiki/List_of_CIL_instructions)
  * [Message Loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows)


