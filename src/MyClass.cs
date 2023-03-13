namespace dotnetwhat.library
{
    public class MyClass
    {
        public void Method1(int value, Foo foo)
        {
            value++;
            foo.Value1++;
            foo = new Foo { Value2 = 50 };
        }

        public void Method2(ref int value, ref Foo foo)
        {
            value++;
            var value1 = foo.Value1;
            foo = new Foo { Value1 = value1, Value2 = 100 };
        }
    }
}