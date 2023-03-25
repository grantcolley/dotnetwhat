namespace dotnetwhat.library
{
    public static class MutateString
    {
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
    }
}
