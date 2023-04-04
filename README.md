# dotnetwhat

*Programming is communicating instructions. Communication is all about meaning, semantics matter.*

### Table of Contents
- [Overview](#overview)
- [Value Types, Reference Types and Variables](#value-types-reference-types-and-variables)
- [Memory](#memory)
  - [Memory Allocation](#memory-allocation)
  - [Releasing Memory](#releasing-memory)
  - [Releasing Unmanaged Resources](#releasing-unmanaged-resources)
  - [OutOfMemoryException](#outofmemoryexception)
  - [Accessing Memory underlying a Variable](#accessing-memory-underlying-a-variable)  
      - [unsafe and fixed](#unsafe-and-fixed)
      - [Memory\<T> and Span\<T>](#memoryt-and-spant)
  - [Manually Allocating Memory on the Stack](#manually-allocating-memory-on-the-stack)
- [What's in the CIL](#whats-in-the-cil)
  - [Method Parameters](#method-parameters)
  - [Boxing and Unboxing](#boxing-and-unboxing)
  - [Ref](#ref)
  - [Ref Locals](#ref-locals)
  - [Ref Returns](#ref-returns)
  - [Lambda](#lambda)
  - [Captured Variable](#captured-variable)
  - [Closing Over a Loop Variable](#closing-over-a-loop-variable)
     - [for](#for)
     - [foreach](#foreach)     
- [Performance](#performance)
  - [Span\<T>](#spant)
  - [StringBuilder](#stringbuilder)
- [Glossary](#glossary)
- [References](#references)


## Overview

The pillars of the .NET stack is the runtime, libraries and languages.

.NET is known as [**managed**](https://learn.microsoft.com/en-us/dotnet/standard/managed-code) because it provides a runtime environment called the **Common Language Runtime ([CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr))** to [**manage code execution**](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process). The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** is a set of libraries for running .NET applications and is responsible for things like enforcing type safety and memory management. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** also **Just In Time ([JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code))** compiles managed code into native processor-specific code on demand at runtime. Only code that is used gets **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to avoid wasting resources.

The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also has a library containing the basic primitive types including char, bool, byte etc. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** also defines two main kinds of types that must be supported: value types and reference types.
The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** also includes rules for inheritance, interfaces, and virtual methods etc. that enables an object-oriented programming model.
The **[CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** is a subset of the **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** and defines a set of common features needed by applications.

The **[.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)** is a set of libraries and tools for developing .NET applications. .NET also has a large set of libraries called the **Base Class Library ([BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries))**, which provides implementation for many general types, algorithms, and utility functionality.

.NET applications can be written in different languages and the language compiler must adhere to the rules laid out in the **Common Type System ([CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))** and **Common Language Specification ([CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))**.

Code is compiled into **Common Intermediate language ([CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language))**, in the form of Portable Executable files such as *.exe* and *.dll* files. **[CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)** is CPU-independent [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) for loading, storing, initializing, and calling methods on objects, arithmetic and logical operations, control flow, direct memory access, exception handling etc. **[CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)** is **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to native (CPU-specific) code by the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** as runtime.

When a .NET application is initialised the operating system loads the **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)**. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** then loads the application assemblies into memory and reserves a contiguous region of virtual address space for the application called the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management#allocating-memory), with a default size of 2GB for 32-bit syatems. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** also creates the main application domain, which in turn creates the main thread with a default stack size of 1MB on a 32-bit system and 4MB on a 64-bit system. Every thread is allocated it's own stack memory which provides the thread context. The main thread executes the application's entry point, typically the static Main method, and the application starts running. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** continues to provide services such as memory management, garbage collection, exception handling, and **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiling **[CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)** code into native code.

The main thread creates the GUI and executes the [**message loop**](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows), which is responsible for processing and dispatching messages queued by the operating system, such as key presses and mouse clicks. Each user control is bound to the thread that created it, typically the main thread, and cannot be updated by another. This is to ensure the [**integrity of UI components**](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8). 

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

The [message loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows) calls `GetMessage(&msg, NULL, 0, 0)` to check the message queue. If there is no message the thread is blocked until one arrives e.g. mouse move, mouse click or key press etc. When a message is placed in the queue the thread picks it off and calls `TranslateMessage(&msg);` to translate it into something meaningful. The message is then passed into `DispatchMessage(&msg);`, which routes it to the applicable even handler for processing e.g. `Button1_Click(object sender, EventArgs e)`. When the event has finished processing `GetMessage(&msg, NULL, 0, 0)` and the process is repeated until the application shuts down.

## Value Types, Reference Types and Variables

The main difference between [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) and [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) are the way they are represented and how they get assigned between variables.

[**Variables**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables) represent storage locations. C# is a type-safe language and each variable has a type that determines what values can be stored in the variable.

[**Value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) objects are represented by the value of the object. When the value is assigned from one variable to another the value is copied and both variables will each contain their own copy of the value. Changing the value of one variable will not impact the value of the other variable.

[**Reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) objects are represented by a reference to the actual object i.e. the object is stored at an address in memory and the reference points to the object. When the reference is assigned from one variable to another the reference is copied and both variables will point to the same object. Unlike variables for [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), multiple variables can point to the same [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object therefore operations on one variable can affect the object referenced by the other variable.
<br>

>  **Note** 
>  
>  *A pimped up version of an analogy about [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) by Jon Skeet on [**.NET Rocks!**](https://www.dotnetrocks.com/details/881) (34m 42s)*
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

## Memory

#### Memory Allocation
When code execution enters a method, parameters passed into the method and local variables are allocated on the threads **stack** memory. For [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) variables the value of the type is stored on the **stack**. For [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) variables the reference to the object is stored on the **stack**, while the object is stored on the [**heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#memory-allocation). 
<br>

>  **Note**
> 
>  **[**Value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) live where they are created.**
>
> While local variables and parameters that are [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) will be stored on the **stack**, if a [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object contains a member that is a [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) then that [**value type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) member will be stored on the heap with that [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) object.
<br>

Local variables and parameters are pushed onto the **stack** in the order they are created and popped off the **stack** on a last in first out (LIFO) basis. Local variables and parameters are scoped to the method in which they are created. The **stack** is self-maintaining so when the executing code leaves the method they are popped off the **stack**.

Local variables and parameters that are [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) push the reference, or pointer, to the object onto the stack, however, the object itself is always stored on the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap). While each thread has it's own stack memory, all threads share the same [**heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) memory.

The [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) consists of two heaps, the small object heap and the [**large object heap (LOH)**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap) for objects that are 85,000 bytes and larger, which are usually arrays.
The small object heap is divided into three generations, 0, 1, and 2, so it can handle short-lived and long-lived objects separately lfor optimization reasons.
- Gen 0 - newly allocated objects that are short lived. Garbage collection is most frequent on Gen 0. 
- Gen 1 - objects that survive a collection of Gen 0 are promoted to Gen 1, which serves as a buffer between short-lived objects and long-lived objects.
- Gen 2 - objects that survive a collection of Gen 1 are considered long-lived objects and promoted to Gen 2.

The [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap) is sometimes referred to as generation 3. If an object is greater than or equal to 85,000 bytes in size, it's considered a large object and allocated on the [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap). This number was determined by performance tuning.

#### Releasing Memory
[**Garbage collection**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#what-happens-during-a-garbage-collection) is the process of releasing and compacting [**heap memory**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap) and occurs most frequently in Gen0. The [**LOH**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap#loh-performance-implications) and Gen 2 are collected together, if either one's threshold is exceeded, a generation 2 collection is triggered.

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

>  **Warning** 
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
            Dispose(disposing true);
            
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
            Dispose(disposing false);
        }
    }
```

#### OutOfMemoryException
[**OutOfMemoryException**](https://learn.microsoft.com/en-us/dotnet/api/system.outofmemoryexception) is thrown when there isn't enough memory to continue the execution of a program. [“Out Of Memory” Does Not Refer to Physical Memory](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/out-of-memory-does-not-refer-to-physical-memory). The most common reason is there isn't a contiguous block of memory large enough for the required allocation size. Another common reason is attempting to expand a `StringBuilder` object beyond the length defined by its `StringBuilder.MaxCapacity` property.

#### Accessing Memory underlying a Variable 
C# code is called "verifiably safe code" because .NET tools can verify that the code is safe. Safe code creates managed objects and doesn't allow you to access memory directly. C# does, however, still allow direct memory access. `.NET Core 2.1` introduced `Memory<T>` and `Span<T>` which provide a type safe way to work with a contiguous block of memory. Prior to that, memory could be directly accessed by writing unsafe code using `unsafe` and `fixed`. The examples below show how, despite being [immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings), a [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) can be modified by directly accessing the memory storing it. The first example uses [unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) code with the `unsafe` and `fixed` keywords. The second example uses `Memory<T>` and `Span<T>`.

A [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) is a reference type with value type semantics. [Strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) store text as a readonly collection of [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char) objects. [Strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) are [immutable](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#immutability-of-strings) i.e. once created they cannot be modified. If a [strings](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) variable is updated, a new [string](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings) is created and the original is released for disposal by the garabage collector. 

##### unsafe and fixed
[Unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) code is written with the `unsafe` keyword, where you can directly access memory using [pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types). A [pointer](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types) is simply a variable that holds the memory address of another type or variable. The variable also needs to be [fixed](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/fixed) or "pinned", so the garbage collector can't move it while compacting the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap). 

[Unsafe code](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) isn't necessarily dangerous; it's just code whose safety cannot be verified.

>  **Note**
> 
>  In order to use the `unsafe` block you must set [AllowUnsafeBlocks](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/language#allowunsafeblocks) in the project file to `true`.
>  ```XML
>  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
>  ```

In the following [C# code](https://github.com/grantcolley/dotnetwhat/blob/15f618ccf2d8f0eef09fa42f3971b1e03aa0108d/tests/TestCases.cs#L46) an immutable string is mutated by directly accessing it's values in memory using `unsafe` and `fixed`. The `unsafe` keyword allows us to create a [pointer](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types) `char* ptr` using the `fixed` statement, which gives us direct access to the value in the variable `source`, allowing us to directly replace each character in memory with a character from the variable `target`.
>  **Warning** this example works because the number of characters in `source` and `target` are equal.
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

>  **Warning** 
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

## What's in the CIL

#### Method Parameters
Arguments can be passed to [**method parameters**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters) by value or by reference.
**Passing by value**, which is the default for both [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) and [**reference types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types), means the argument passes a copy of the variable into the method. **Passing by reference**, using the [ref](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref) keyword, means the argument passes the address of the variable into the method.

>  **Note**
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
[Lambdas](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#capture-of-outer-variables-and-variable-scope-in-lambda-expressions) can refer to outer variables. Outer variables are local variables within the method that contains the lambda expression. Outer variables consumed by a lambda expression are called captured variables. [Captured variables](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#capture-of-outer-variables-and-variable-scope-in-lambda-expressions) won't be garbage-collected until the delegate that references it becomes eligible for garbage collection. 

>  **Note** A lambda expression can't directly capturea parameter that has been passed by [ref](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref).

In the following [example](https://github.com/grantcolley/dotnetwhat/blob/main/src/CapturedVariable.cs) a lambda expression increments a captured variable and returns the result. We can see in [IL Disassembler](https://learn.microsoft.com/en-us/dotnet/framework/tools/ildasm-exe-il-disassembler) the compiler converts the [lambda](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions) expression into a private nested container class (inside the red box). The container class contains a public field `myLocalValue : public int32` i.e. this is where the compiler moves the captured variable that is to be incremented, thereby ensuring the captured variable won't be garbage-collected until the containing class is garbage collected.

```C#
    public class CapturedVariable
    {
        public int IncrementLocalVariable()
        {
            int myLocalValue = 0;

            Func<int> increment = () => myLocalValue++;

            increment();

            return myLocalValue;
        }
    }
```

![CIL for incrementing a Captured Variable](/readme-images/Captured_variable.png?raw=true "CIL for incrementing a Captured Variable")

#### Closing Over a Loop Variable
<!-- 
https://ericlippert.com/2009/11/12/closing-over-the-loop-variable-considered-harmful-part-one/#more-1441
https://csharpindepth.com/articles/Closures
-->
##### for

##### foreach

## Performance
<!--
        https://blog.marcgravell.com/2017/04/spans-and-ref-part-1-ref.html
        // https://blog.marcgravell.com/2022/05/unusual-optimizations-ref-foreach-and.html
-->

#### Span\<T>
[Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) is a [ref struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) that provides type-safe access to a contiguous region of memory. [Ref structs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) can only be allocated on the stack and not the heap. [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) can, however, point to heap memory, stack memory and unmanaged memory. [Span\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) can wrap an entire contiguous block of memory or it can point to any contiguous range within it, using [slicing](https://learn.microsoft.com/en-us/dotnet/api/system.span-1?view=net-7.0#spant-and-slices).

>  **Note**
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
* **System.Object** *- the base class of all .NET classes*
* **Unboxing** *- the process of explicitly converting an objects value, or interface type, to a value type*
* **Unmanaged resources** *- common types include files, windows, network connections, or database connections*
* **Unsafe code** *- allows direct access to memory using pointers*
* **Value types** *- objects represented by the value of the object*
* **Variables** *- represent storage locations*

## References
* **.NET Blogs**
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
  * [System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)
  * [Unmanaged Resources](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/unmanaged)
  * [Unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code)
  * [Value Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types)
  * [Variables](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables)
  * [Workstation GC](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#workstation-gc)

* **Wikipedia**
  * [CIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language)
  * [CIL Instructions](https://en.wikipedia.org/wiki/List_of_CIL_instructions)
  * [Message Loop](https://en.wikipedia.org/wiki/Message_loop_in_Microsoft_Windows)


