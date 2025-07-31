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
- [Memory](#memory)
  - [Variables](#variables) 
  - [Value Types](#value-types) 
  - [Reference Types](#reference-types) 
  - [Memory Allocation](#memory-allocation)
  - [Stack vs Heap](#stack-vs-heap)
  - [Releasing Memory](#releasing-memory)
  - [Releasing Unmanaged Resources](#releasing-unmanaged-resources)
  - [WeakReference Class](#weakreference-class)
  - [Memory and ASP.NET Core](#memory-and-aspnet-core)
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
      - [Atomicity and Thread Safety](#atomicity-and-thread-safety)
- [Stack Memory is Thread-safe (with caveats)](#stack-memory-is-thread-safe-with-caveats) 
- [Concurrency](#concurrency)
  - [Parallelism vs Concurrency vs Asynchronous](#parallelism-vs-concurrency-vs-asynchronous)
  - [Threads](#threads)
  - [ThreadPool](#threadpool)
  - [Task and Task\<T>](#task-and-taskt)
  - [ValueTask\<T>](#value-taskt)
  - [Async Await](#async-await)
  - [Thread Safety](#thread-safety)
      - [Locks and Mutex](#locks-and-mutex)   
- [What's in the CIL](#whats-in-the-cil)
  - [Method Parameters](#method-parameters)
  - [Boxing and Unboxing](#boxing-and-unboxing)
  - [Ref](#ref)
  - [Ref Locals](#ref-locals)
  - [Ref Returns](#ref-returns)
  - [Lambda](#lambda)
  - [Captured Variable](#captured-variable)
  - [Closing Over a Loop Variable](#closing-over-a-loop-variable)
     - [for loop](#for-loop)
     - [foreach loop](#foreach-loop)
- [How it Works - Internal Structure](#how-it-works---internal-structure)
  - [Dictionary\<TKey,TValue>](#dictionarytkeytvalue)
  - [List\<T>](#listt)
- [Performance](#performance)
  - [Span\<T>](#spant)
  - [StringBuilder](#stringbuilder)
  - [Mark Members Static](#mark-members-static)
- [Big *O*](#big-o)
  - [Key Terms](#key-terms)
  - [Common Big *O* Examples (with C# context)](#common-big-o-examples-with-c-context)
  - [Big *O* Code Examples](#big-o-code-examples)
- [Glossary](#glossary)
- [References](#references)

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

The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** also creates the application domain, which in turn creates the main thread the application runs on. Each thread is allocated it's own stack memory, which is part of the thread context. Threads have a default stack size of 1MB. The main thread executes the application's entry point, typically the static *Main()* method, and the application starts running. 

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

## Memory

### Variables
[**Variables**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables) are simply named storage locations in memory. C# is a type-safe language, and the C# compiler guarantees that values stored in variables are always of the appropriate type. Variables store [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) and [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), the main difference between them are the way they are handled in memory.

### Value Types
[**Value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) objects include numeric types (`int`, `decimal` etc.), `char`, `bool`, `enum` and `DateTime`. Custom value types can be created using a [struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct).

[**Value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) variables store the actual value of the type in the variable e.g. `Int32 abc = 5;` will create a storage location named `abc` that can store a 32 bit `integer`, and then assign `abc` the value `5`. 
No additional type information is stored with a value type, as the type information is known at compile-time and embedded in the generated IL code.

When value type variables are assigned from one variable to another, or as an argument to a method, the value is copied. The new variable will have its own copy of the value and changing the value of one variable will not impact the value of the other variable.

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
>  *An analogy about [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) by [Jon Skeet](https://codeblog.jonskeet.uk/category/csharp/) on [**.NET Rocks!**](https://www.dotnetrocks.com/details/881) (34m 42s)*
>
> ### A piece of paper with the address of a house written on it.
> 
> The house is a reference type object in heap memory. The address is the reference pointing to where that object is located in heap memory. The piece of paper is the variable containing the address pointing to the object in heap memory. 
> 
> If you copy the same address to another piece of paper (another variable), you now have two variables pointing to the same object in heap memory. If you were to paint the door of the house green, both pieces of paper still point to the same house, which now has a green door.
> 
> You cross out the address on the first piece of paper and replace it with the address of another house. Now each piece of paper (variables) have different addresses (references), each pointing to different houses (objects). 
> 
> You throw away the second piece of paper with the address to the original house. Now no piece of paper (variable) points to the original house (object). When the garbage collector comes along and finds a house (object) with no piece of paper (variable) pointing to it, the house is torn down to free up the memory allocated for it on the heap.
<br>

#### Memory Allocation
When code execution enters a method, both the parameters passed into the method and the local variables declared in the method, are allocated on the threads **stack** memory. For [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) local variables, the actual value of the type is stored in **stack** memory. For [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) local variables, only the reference to the object is stored in the **stack** memory, while the object itself is stored in the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#memory-allocation). 

Local variables and method parameters are pushed onto the **stack** in the order they are created and popped off the **stack** on a last in first out (LIFO) basis. Local variables and parameters are scoped to the method in which they are created and when the executing code leaves the method they are popped off the **stack**, therefore the **stack** is self-maintaining.

Local variables and method parameters that are [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) push the reference, or "pointer" to the object, onto the stack however, the object itself is always stored on the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap). While each thread has it's own stack memory, all threads share the same [**heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) memory. This allows multiple variables across different threads to reference the same object in the shared managed heap.

The [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) consists of two heaps, the small object heap and the [**large object heap (LOH)**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap) for objects that are 85,000 bytes (85kb) and larger, which are usually arrays.
The small object heap is divided into three generations, 0, 1, and 2, so it can handle short-lived and long-lived objects separately for optimization reasons.
- Gen 0 - newly allocated objects that are short lived. Garbage collection is most frequent on Gen 0. 
- Gen 1 - objects that survive a collection of Gen 0 are promoted to Gen 1, which serves as a buffer between short-lived objects and long-lived objects.
- Gen 2 - objects that survive a collection of Gen 1 are considered long-lived objects and promoted to Gen 2.

The [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap) is sometimes referred to as generation 3. If an object is greater than or equal to 85,000 bytes (85kb) in size, it's considered a large object and allocated on the [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap). This number was determined by performance tuning.

To put into context what goes onto the [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap), 85,000 bytes is the equivalent of the following:
|Type|85,000 bytes (85kb)|
|:--------|:-------------------------------|
|string|A string with 42,500 16bit characters, equivalent to approx. 9 x A4 pages of text|
|32 bit object reference|An array containing 21,250 references to objects on a 32 bit system|
|64 bit object reference|An array containing 10,625 references to objects on a 64 bit system|
|Int32|An array containing 21,250 integers|
|Int64|An array containing 10,625 longs|
|Decimal(16 bytes)|An array containing 5,312 decimals|

The initial size of the heap 2GB-4GB for 32-bit systems, and slightly larger for 64-bit systems. The heap can grow (and shrink) according to the demands of the application. The size the heap can grow to is limited by the available system memory and any restrictions imposed by the operating system and hardware.

#### Stack vs Heap
- **Stack:** fast, fixed-size, thread-local
- **Heap:** slower, dynamically sized, shared among threads
If you need large memory allocation or recursive data structures, prefer heap allocation (`class` or `new`), not large `struct` or `Span<T>` on the stack.

#### Releasing Memory
[**Garbage collection**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#what-happens-during-a-garbage-collection) is the process of releasing and compacting [**heap memory**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) and occurs most frequently in Gen0. The [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap#loh-performance-implications) and Gen 2 are collected together, if either one's threshold is exceeded, a generation 2 collection is triggered.

Both Gen0 and Gen2 collections compact the memory, however, the large object heap (LOH) isn't compacted unless you use the [GCSettings.LargeObjectHeapCompactionMode](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.gcsettings.largeobjectheapcompactionmode) property to compact the large object heap on demand.

**Phases of Garbage Collection**
- **Suspension:** *all managed threads are suspended except for the thread that triggered the garbage collection*
- **Mark:** *the garbage collector starts at each root and follows every object reference and marks those as seen. Roots include static fields, local variables on a thread's stack, CPU registers, GC handles, and the finalize queue*
- **Compact:** *relocate objects next to each other to reduce fragmentation of the heap. Then update all references to point to the new locations*
- **Resume:** *manage threads are allowed to resume*

[**Workstation GC**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#workstation-gc) collection occurs on the user thread that triggered the garbage collection and remains at the same priority.

[**Server GC**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#server-gc) collection occurs on multiple dedicated threads. On Windows, these threads run at `THREAD_PRIORITY_HIGHEST` priority level. A heap and a dedicated thread to perform garbage collection are provided for each logical CPU

[**Background GC**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/background-gc) applies only to generation 2 collections and is enabled by default. Gen 0 and 1 are collected as needed while a Gen 2 collection is in progress. Background garbage collection is performed on one or more dedicated threads, depending on whether it's workstation or server GC.

#### Releasing Unmanaged Resources
The most common types of [**unmanaged resources**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/unmanaged) are objects that wrap operating system resources, such as files, windows, network connections, or database connections. Although the garbage collector is able to track the lifetime of an object that encapsulates an unmanaged resource, it doesn't know how to release and clean up the unmanaged resource.

The `protected virtual void Dispose(bool disposing)` method executes in two distinct scenarios. If disposing equals true, the method has been called by a user's code and both managed and unmanaged resources can be disposed. If disposing equals false, the method has been called from inside the finalizer and you should not reference other managed objects as only unmanaged resources can be disposed in this scenario.

If you use unmanaged resources you should implement the [**dispose pattern**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose) to free memory used by unmanaged resources. The `Dispose()` method should not be virtual as it musn't be overriden by a derived class. When disposing is finished it should call `GC.SuppressFinalize` to take the object off the finalization queue and prevents finalization code from executing a second time.

>  [!Warning]
>
>  Finalizers are dangerous. Objects with finalizers get placed on a queue after a collection and a single thread works the queue one at a time. Any blocking code in a finalizer will block the queue. 

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

#### WeakReference Class
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

#### Accessing Memory underlying a Variable 
C# code is called "verifiably safe code" because .NET tools can verify that the code is safe. Safe code creates managed objects and doesn't allow you to access memory directly. C# does, however, still allow direct memory access. `.NET Core 2.1` introduced `Memory<T>` and `Span<T>` which provide a type safe way to work with a contiguous block of memory. Prior to that, memory could be directly accessed by writing unsafe code using `unsafe` and `fixed`. The examples below show how, despite being [immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings), a [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) can be modified by directly accessing the memory storing it. The first example uses [unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) code with the `unsafe` and `fixed` keywords. The second example uses `Memory<T>` and `Span<T>`.

A [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) is a reference type with value type semantics. [Strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) store text as a readonly collection of [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char) objects. [Strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) are [immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings) i.e. once created they cannot be modified. If a [strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) variable is updated, a new [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) is created and the original is released for disposal by the garabage collector. 

##### unsafe and fixed
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

##### Memory\<T> and Span\<T>
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

#### Manually Allocating Memory on the Stack
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
> Memory accesses to properly aligned data of primitive and Enum types with size with sizes up to the platform pointer size are always atomic. The value that is observed is always a result of complete read and write operations.
>
> Primitive types: `bool`, `char`, `int8`, `uint8`, `int16`, `uint16`, `int32`, `uint32`, `int64`, `uint64`, `float32`, `float64`, native `int`, native unsigned `int`.
>
> Managed references are always aligned to their size on the given platform and accesses are atomic.
>
> The following methods perform atomic memory accesses regardless of the platform when the location of the variable is managed by the runtime.
> - `System.Threading.Interlocked` methods
> - `System.Threading.Volatile` methods
> 
> Example: `Volatile.Read<double>(ref location)` on a 32 bit platform is atomic, while an ordinary read of location may not be.

### Atomicity of Variables, Volatility and Interlocking

##### Atomic
Atomic simply means a read from memory, or a write to memory will be done in one single step. So, when you assign a variable, the assignment happens in a single step, and likewise with reading a variable i.e. assigning only half a variable value in one step is not atomic, and likewise with reading only half a variable.

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

##### Atomicity and Thread Safety
> [!Warning]
>
> **Atomic reads and writes and thread safety**
>
>Atomic reads and writes do not mean the variable is thread safe. It is entirely possible for one thread on a CPU to read a variable, while another is concurrently writing to it, resulting in the value returned in a corrupted state. As a result i.e. a reading thread could observe a torn value consisting of pieces of different values.
>
>Also, in the case of reference types, the atomicity is only on the reading of the reference, not the object itself, which can be accessed and modified by other threads.
>
>Locking limits access to a variable to a single thread at a time and is the safest way to prevent race conditions and ensure data consistency when multiple threads attempt to read or write shared data concurrently.

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

>  [!Note]
> 
> Read [How Async/Await Really Works in C#](https://devblogs.microsoft.com/dotnet/how-async-await-really-works/)
>
> *...At its heart, a Task is just a data structure that represents the eventual completion of some asynchronous operation (other frameworks call a similar type a “promise” or a “future”)....*

![async/await flowchart](/readme-images/async-await-flowchart.png?raw=true "async/await flowchart")

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

#### Value Task\<T>
[Value Task\<T>](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/) is the struct equivalent of Task\<T>, altough much more limited than Task\<T>. It was created to help improve asynchronous performance where decreased allocation overhead is important.

#### Async Await
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

By default, awaiting a task will attempt to capture the scheduler from `SynchronisationContext.Current` or `TaskScheduler.Current`. When the callback is ready to be invoked, it’ll use the captured scheduler if available. 
`ConfigureAwait(continueOnCapturedContext: false)` avoids forcing the callback to be invoked on the original context or scheduler. ConfigureAwait(continueOnCapturedContext: true)
`ConfigureAwait(true)` does nothing meaninglful, except to explicitly show not using `ConfigureAwait(false)` is inentional e.g. to silence static analysis warnings.

>  [!Note]
>
> Code after the `await` is not guaranteed to always run on the same thread `await` was called.
>
> Calling `await` on a UI thread is a special case. If `await` is called on the UI thread, code that runs after the await will continue on the UI thread.  

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

#### Thread Safety
##### Locks and Mutex
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

[**Boxing and Unboxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) can be expensive. [**Boxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) involves creating and allocating a new object on the heap, and casting when setting it's value. [**Unboxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) involves first checking the value of the `object` is a boxed value of the [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), then copying the value from the instance into the [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types).

Examples of unintentional [**boxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) can occur when working with `strings` e.g. when using `String.Format()` and `String.Concat()` etc. Ways around this is to use string interpolation instead, or always call `.ToString()` of the value type.

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

## How it Works - Internal Structure
#### Dictionary\<TKey,TValue>
The `Dictionary<TKey, TValue>` class remains a hash table-based implementation. `Dictionary<TKey, TValue>` uses `Buckets` and `Entries`. Each bucket contains the index of the first `entry` in the `entries` array that belongs to that `hash bucket`. Objects that share the same `hash bucket` form a linked list using the `next` field, linking to the next entry (like a singly-linked list).
- **Buckets (int[] or Span<int>)** – An array of indexes into the entries array.
- **Entries (Entry<TKey, TValue>[])** – An array of structs that contain:
	- **int hashCode**
	- **int next** (index of the next entry in case of collision, forming a linked list)
	- **TKey** key
	- **TValue** value

Each bucket contains the index of the first entry in the entries array that belongs to that hash bucket. If there are collisions, the next field links to the next entry (like a singly-linked list).

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

#### List\<T>
The `List<T>` class is a generic dynamic array — it stores its elements in a contiguous block of memory and resizes as needed.

`List<T>` is initialized with a default capacity or a user-specified capacity. When elements are added beyond the current capacity, it resizes the array (usually doubling its capacity).

Key Internal Fields:
```C#
private T[] _items;    // Underlying array buffer
private int _size;     // Current number of elements in the list
private int _version;  // Used to track changes for enumerator safety
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

## Big *O*
Big *O* notation is a way to describe how fast or slow your code runs as the input size grows. It gives you a basic idea of your code's performance and scalability.

It doesn't measure actual time (like milliseconds); it measures how the number of operations grows relative to the input size.

#### Key Terms
| Term                            | Meaning                                                  |
| ------------------------------- | -------------------------------------------------------- |
| logarithmic time `O(log n)`     |Logarithmic time means that every step of the algorithm cuts the problem in half. So instead of checking every item, you’re skipping a big chunk with each move. Super fast even with big input sizes. Typical used in binary search|
| Quadratic time `O(n²)`          |Quadratic time means the work your code does grows a lot faster than the input size. Specifically, if you double the input, the work grows four times. If you triple it, it grows nine times — like squaring the size. Typically used in some sorting algorithms like bubble sort or selection sort|

#### Common Big *O* Examples (with C# context)
| Big O          | Meaning                                                  | C# Example                                                      |
| -------------- | -------------------------------------------------------- | --------------------------------------------------------------- |
| **O(1)**       | Constant time – super fast, doesn’t depend on input size | `list[0];` or `dictionary.ContainsKey("foo")`                   |
| **O(n)**       | Linear – time grows with input size                      | `foreach (var item in list) { ... }`                            |
| **O(n²)**      | Quadratic – nested loops, gets slow fast                 | `foreach (var a in list) foreach (var b in list) { ... }`       |
| **O(log n)**   | Logarithmic – very efficient                             | Binary search: `list.BinarySearch(item);`                       |
| **O(n log n)** | Typical of efficient sorts                               | `list.Sort();` (uses TimSort in .NET)                           |
| **O(2ⁿ)**      | Exponential – extremely slow for large inputs            | Recursive solutions like solving the Fibonacci sequence naively |

**TL;DR Rules of Thumb:**
- Favor O(1) and O(log n) when you can.
- Be cautious with O(n²) and worse – especially with nested loops.

#### Big *O* Code Examples
Linear: O(n) - Time grows linearly with the size of numbers.
```C#
bool Contains(int[] numbers, int target)
{
    foreach (var num in numbers)
    {
        if (num == target) return true;
    }
    return false;
}
```

Constant: O(1) - Fast, no matter how big the dictionary is.
```C#
bool HasValue(Dictionary<int, string> dict, int key)
{
    return dict.ContainsKey(key);
}
```

Quadratic: O(n²) - Gets very slow as numbers grows.
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
* **.NET Blogs**
  * [About Processes and Threads](https://learn.microsoft.com/en-us/windows/win32/procthread/about-processes-and-threads)
  * [All About Span: Exploring a New .NET Mainstay](https://learn.microsoft.com/en-us/archive/msdn-magazine/2018/january/csharp-all-about-span-exploring-a-new-net-mainstay)
  * [How Async/Await Really Works in C#](https://devblogs.microsoft.com/dotnet/how-async-await-really-works/)
  * [“Out Of Memory” Does Not Refer to Physical Memory](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/out-of-memory-does-not-refer-to-physical-memory)
  * [What is .NET, and why should you choose it?](https://devblogs.microsoft.com/dotnet/why-dotnet/)

* **Microsoft**
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

* **Wikipedia**
  * [CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)
  * [CIL Instructions](https://en.wikipedia.org/wiki/List_of_CIL_instructions)
  * [Message Loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows)


