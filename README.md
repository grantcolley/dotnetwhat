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
- [What's in the CIL](#whats-in-the-cil)
  - [Method Parameters](#method-parameters)
  - [Boxing and Unboxing](#boxing-and-unboxing)
  - [Ref Locals](#ref-locals)
  - [Ref Returns](#ref-returns)
- [Performance](#performance)
- [Glossary](#glossary)
- [References](#references)


## Overview

The pillars of the .NET stack is the runtime, libraries and languages.

.NET is known as [**managed**](https://learn.microsoft.com/en-us/dotnet/standard/managed-code) because it provides a runtime environment called the **Common Language Runtime ([CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr))** to [**manage code execution**](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process). The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** is a set of libraries for running .NET applications and is responsible for things like enforcing type safety and memory management. The **[CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)** also **Just In Time ([JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code))** compiles managed code into native processor-specific code on demand at runtime. Only code that is used gets **[JIT](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_msil_to_native_code)** compiled to avoid wasting resources.

.NET applications can be written in different languages and the language compiler must adhere to the rules laid out in the **Common Type System ([CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))** and **Common Language Specification ([CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system))**. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** establishes a framework for cross language execution by defining rules all languages must follow when it comes to working with types. It also has a library containing the basic primitive types including char, bool, byte etc. The **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** also defines two main kinds of types that must be supported: value types and reference types. The **[CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** is a subset of the **[CTS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)** and defines a set of common features needed by applications.

The **[.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)** is a set of libraries and tools for developing .NET applications. .NET also has a large set of libraries called the **Base Class Library ([BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries))**, which provides implementation for many general types, algorithms, and utility functionality.
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

The main difference between [**value types**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) and [**reference type**](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) are the way they are represented and how they get assigned between variables.

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
C# code is called "verifiably safe code" because .NET tools can verify that the code is safe. Safe code creates managed objects and doesn't allow you to access memory directly using pointers. C# does, however, allow for [unsafe](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) code to be written using the `unsafe` keyword, where you can directly access memory using [pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types). A [pointer](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types) is simply a variable that holds the memory address of another type or variable. The variable also needs to be [fixed](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/fixed) or "pinned", so the garbage collector can't move it while compacting the [**managed heap**](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap). 

[Unsafe code](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code) isn't necessarily dangerous; it's just code whose safety cannot be verified.

>  **Note**
> 
>  In order to use the `unsafe` block you must set [AllowUnsafeBlocks](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/language#allowunsafeblocks) in the project file to `true`.
>  ```XML
>  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
>  ```

The following example shows how an immutable string, can actually be mutated by directly accessing it in memory. The `unsafe` keyword allows us to create a pointer `char* ptr` using the `fixed` statement, which gives us direct access to the value in the variable `source`, allowing us to directly replace each character in memory with a character from the variable `target`.
>  **Warning** this example works because the number of characters in `source` and `target` are equal.
```C#
        [TestMethod]
        public void Unsafe()
        {
            // Arrange
            string source = "Hello";
            string target = "World";

            // Act
            Mutate(source, target);

            // Assert
            Assert.AreEqual(target, source);
        }

        public static void Mutate(string source, string target)
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

Example C# code passing arguments to method parameters **by value** and **by reference** and the compiled [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions):
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

Example C# code comparing writing the value of an integer to a string, both with and without calling `Int32.ToString()` and using string interpolation, and the compiled [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions):
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

#### Ref Locals
A [ref local](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals) is a variable that refers to other storage.

In this code listing variable `b` holds a copy of `a`. Variable `c`, however, refers to the same storage location as `c`. When we set `c` to 7 then `a` is now also 7 because they are both refering to the same storage location. `b` on the other hand is still 5 because it has its own copy. We can see the [**CIL instructions**](https://en.wikipedia.org/wiki/List_of_CIL_instructions) below.
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

### Ref Returns
[Ref return](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values) values are returned by a method by reference i.e. the address of the value is returned rather than the value itself. If the returned value is stored in a [ref local](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals) it can be modifed and the change is reflected in the called method. If a [ref return](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values) value returned by a method isn't stored in a [ref local](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals) then it stores a copy of the value stored at the address in the [ref return](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values).

In the code listing below `decimal a = myClass.GetCurrentPrice()` returns the current price by value i.e. `a` is only a copy of the current price returned by `myClass.GetCurrentPrice()`. Changes to `a` will only be applied to itself.

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

## Performance

## Glossary
* **Background GC** *- applies only to generation 2 collections and is enabled by default*
* **Base Class Library  (BCL)** *- a standard set of class libraries providing implementation for general functionality*
* **Boxing** *- the process of converting value types to objects or an interface implemented by the value type*
* **Common Intermediate Language (CIL)** *- instructions for loading, storing, initializing, and calling methods on objects, arithmetic and logical operations, control flow, direct memory access, exception handling etc*
* **Common Language Runtime (CLR)** *- .NET runtime responsible for managing code execution, memory and type safety etc.*
* **Common Language Specification (CLS)** *- subset of CTS that defines a set of common features needed by applications*
* **Common Type System (CTS)** *- defines rules all languages must follow when it comes to working with types*
* **Fixed** *- declares a pointer to a variable and fixes or "pins" it, so the garbage collection can't relocate it*
* **Garbage Collection** *- the process of releasing and compacting heap memory*
* **in Keyword** *- an argument is passed by reference, however it cannot be modified in the called method*
* **Just-In-Time compilation (JIT)** *- at runtime the JIT compiler translates MSIL into native code, which is processor specific code*
* **Large Object Heap (LOH)** *- contains objects that are 85,000 bytes and larger, which are usually arrays*
* **Managed Code** *- code whose execution is managed by a runtime*
* **Managed Heap** *- a segment of memory for storing and managing objects. All threads share the same heap*
* **Message Loop** *- responsible for processing and dispatching messages queued by the operating system, such as key presses and mouse clicks*
* **Method Parameters** *- arguments passed my value or by reference. Default is by value.*
* **.NET SDK** *-a set of libraries and tools for developing .NET applications*
* **out Keyword** *- an argument is passed by reference, however a value must be assigned to it in the called method*
* **OutOfMemoryException** *- is thrown when there is not enough memory to continue the execution of a program*
* **Pointers** *- a variable that holds the memory address of another type or variable, allowing direct access to it in memory.*
* **ref Keyword** *- an argument passes a variables address into a method, rather than a copy of the variable*
* **Reference types** *- objects represented by a reference that points to where the object is stored in memory*
* **Ref Locals** *- variables that refers to other storage i.e. reference another variables storage*
* **Ref Returns** *- values returned by a method by reference i.e. modifying it will change the value in the called code*
* **Safe Handle** *- represents a wrapper class for operating system handles*
* **Stack** *- stores local variables and method parameters. Each thread has it's own stack memory which gives it context* 
* **System.Object** *- the base class of all .NET classes*
* **Unboxing** *- the process of explicitly converting an objects value, or interface type, to a value type*
* **Unmanaged resources** *- common types include files, windows, network connections, or database connections*
* **Unsafe code** *- allows direct access to memory using pointers*
* **Value types** *- objects represented by the value of the object*
* **Variables** *- represent storage locations*

## References
* **.NET Blogs**
  * [How Async/Await Really Works in C#](https://devblogs.microsoft.com/dotnet/how-async-await-really-works/)
  * [“Out Of Memory” Does Not Refer to Physical Memory](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/out-of-memory-does-not-refer-to-physical-memory)
  * [What is .NET, and why should you choose it?](https://devblogs.microsoft.com/dotnet/why-dotnet/)

* **Microsoft**
  * [Background GC](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/background-gc)
  * [**Boxing and Unboxing**](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing)
  * [BCL](https://learn.microsoft.com/en-us/dotnet/standard/framework-libraries)
  * [CIL](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process#compiling_to_msil)
  * [CLR](https://learn.microsoft.com/en-us/dotnet/standard/clr)
  * [CTS & CLS](https://learn.microsoft.com/en-us/dotnet/standard/common-type-system)
  * [Dispose Pattern](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose)
  * [Fixed](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/fixed)
  * [Garbage Collection](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#what-happens-during-a-garbage-collection)
  * [in Keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier)
  * [Integrity of UI Components](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8)
  * [LOH](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/large-object-heap)
  * [Managed Code](https://learn.microsoft.com/en-us/dotnet/standard/managed-code)
  * [Managed Execution Process](https://learn.microsoft.com/en-us/dotnet/standard/managed-execution-process)
  * [Managed Heap](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#the-managed-heap)
  * [Memory Management](https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management)
  * [Method Parameters](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters)
  * [out Keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier)
  * [“Out Of Memory” Does Not Refer to Physical Memory](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/out-of-memory-does-not-refer-to-physical-memory)
  * [OutOfMemoryException](https://learn.microsoft.com/en-us/dotnet/api/system.outofmemoryexception)
  * [Performance](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance)
  * [Pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types)
  * [ref Keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref)
  * [Reference Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types)
  * [Ref Locals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#ref-locals)
  * [Ref returns](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#reference-return-values)
  * [Safe Handle](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.safehandle)
  * [SDK](https://learn.microsoft.com/en-us/dotnet/core/sdk)
  * [Server GC](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc#server-gc)
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


