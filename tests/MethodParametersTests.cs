namespace dotnetwhat.tests
{
    [TestClass]
    public class MethodParametersTests
    {
        [TestMethod]
        public void Pass_Args_ByValue_And_ByRef()
        {
            // Arrange
            MyClass myClass = new MyClass();
            int param = 123;
            Foo foo = new Foo();

            // Act
            myClass.Method1(param, foo);
            myClass.Method2(ref param, ref foo);

            // Assert
            Assert.AreEqual(124, param);
            Assert.AreEqual(1, foo.Value1);
            Assert.AreEqual(100, foo.Value2);
        }

        [TestMethod]
        public void Boxing_Value_Types()
        {
            // Arrange
            int localInt = 5;

            // Act
            string string1 = string.Format("{0}", localInt);            // boxing
            string string2 = string.Format("{0}", localInt.ToString()); // no boxing
            string string3 = string.Concat("Foo", localInt);            // boxing
            string string4 = string.Concat("Foo", localInt.ToString()); // no boxing
            string string5 = $"{localInt}";                             // no boxing

            // Assert
            Assert.AreEqual("5", string1);
            Assert.AreEqual("5", string2);
            Assert.AreEqual("Foo5", string3);
            Assert.AreEqual("Foo5", string4);
            Assert.AreEqual("5", string5);
        }

        [TestMethod]
        public void Direct_Memory_Unsafe_Code()
        {
            // Arrange
            string source = "Hello";
            string target = "World";

            // Act
            MutateString.Mutate(source, target);

            // Assert
            Assert.AreEqual(target, source);
        }

        [TestMethod]
        public void Ref_Local()
        {
            // Arrange
            int a = 5;

            // Act
            int b = a;
            ref int c = ref a;

            c = 7;

            // Assert
            Assert.AreEqual(7, a);
            Assert.AreEqual(5, b);
        }

        [TestMethod]
        public void Ref_Return()
        {
            // Arrange
            MyClass myClass = new MyClass();

            // Act
            decimal a = myClass.GetCurrentPrice();
            ref decimal b = ref myClass.GetCurrentPriceByRef();
            b = 567.89m;

            a = myClass.GetCurrentPriceByRef();

            // Assert
            Assert.AreEqual(567.89m, a);
            Assert.AreEqual(567.89m, myClass.GetCurrentPrice());
        }

        [TestMethod]
        public void Stackalloc_Unsafe()
        {
            // Arrange
            int length = 3;

            // Act
            unsafe
            {
                int* numbers = stackalloc int[length];
                for (var i = 0; i < length; i++)
                {
                    numbers[i] = i;
                }

                // Assert
                Assert.AreEqual(0, numbers[0]);
                Assert.AreEqual(1, numbers[1]);
                Assert.AreEqual(2, numbers[2]);
            }
        }

        [TestMethod]
        public void Stackalloc_Safe()
        {
            // Arrange
            int length = 3;

            // Act
            Span<int> numbers = stackalloc int[length];
            for (var i = 0; i < length; i++)
            {
                numbers[i] = i;
            }

            // Assert
            Assert.AreEqual(0, numbers[0]);
            Assert.AreEqual(1, numbers[1]);
            Assert.AreEqual(2, numbers[2]);
        }
    }
}