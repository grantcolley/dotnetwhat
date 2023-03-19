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
    }
}