using System.Runtime.InteropServices;

namespace dotnetwhat.library
{
    public static class MutateString
    {
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

        public static void Mutate_Using_Memory_Span(string source, string target)
        {
            var memory = MemoryMarshal.AsMemory(source.AsMemory());

            for (int i = 0; i < source.Length; i++)
            {
                ref char c = ref memory.Span[i];
                c = target[i];
            }
        }
    }
}
