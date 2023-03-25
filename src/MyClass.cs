namespace dotnetwhat.library
{
    public class MyClass
    {
        private decimal currentPrice = 123.45m;

        public decimal GetCurrentPrice()
        {
            return currentPrice;
        }

        public ref decimal GetCurrentPriceByRef()
        {
            return ref currentPrice; 
        }

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