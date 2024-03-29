using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;

namespace dotnetwhat.tests
{
    [TestClass]
    public class TestCases
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
        public void Direct_Memory_Unsafe_Fixed()
        {
            // Arrange
            string source = "Hello";
            string target = "World";

            // Act
            MutateString.Mutate_Using_Fixed(source, target);

            // Assert
            Assert.AreEqual(target, source);
        }

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

        [TestMethod]
        public void Multiplier()
        {
            // Arrange
            Multiplier multiplier = new Multiplier();

            // Act
            var result = multiplier.Multiply(2, 2);

            // Assert
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void CapturedVariable()
        {
            // Arrange
            CapturedVariable capturedVariable = new CapturedVariable();

            // Act
            var result = capturedVariable.IncrementLocalVariable();

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Looping_ForEach()
        {
            // Arrange
            var looping = new Looping_Foreach();

            // Act
            var result = looping.Loop();

            // Assert
            Assert.AreEqual("12", result);
        }

        [TestMethod]
        public void Looping_For()
        {
            // Arrange
            var looping = new Looping_For();

            // Act
            var result = looping.Loop();

            // Assert
            Assert.AreEqual("22", result);
        }

        [TestMethod]
        public void Looping_While()
        {
            // Arrange
            var looping = new Looping_While();

            // Act
            var result = looping.Loop();

            // Assert
            Assert.AreEqual("22", result);
        }

        [TestMethod]
        public void Thread_Execute()
        {
            // Arrange
            var threads = new Threads();

            // Act
            threads.RunThread();

            // Assert

        }

        [TestMethod]
        public void ThreadPool_Execute()
        {
            // Arrange
            var theThreadPool = new TheThreadPool();

            // Act
            theThreadPool.RunThreadFromThreadPool();

            // Assert

        }

        [TestMethod]
        public void Tasks_Execute()
        {
            // Arrange
            var tasks = new Tasks();

            // Act
            tasks.RunTask();

            // Assert

        }

        [TestMethod]
        public void CapturedVariables()
        {
            int seed = 0;
            Func<int> seedIncrementer = () => seed++;
            var res1 = seedIncrementer();
            var res2 = seedIncrementer();
            var res3 = seedIncrementer();

            // NOTE: seed++ is post-increment
            //       i.e. the increment happens AFTER
            //            the return is returned!

            Assert.AreEqual(0, res1);
            Assert.AreEqual(1, res2);
            Assert.AreEqual(2, res3);

            Assert.AreEqual(3, seed);
        }
    }
}