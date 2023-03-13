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
    }
}