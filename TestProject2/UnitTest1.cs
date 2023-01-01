using NumericMethodsEquMSV;
using NUnit.Framework;
using System;
using Moq;

[TestFixture]
public class MethodTests
{
    private const double Epsilon = 1e-4;

    [SetUp]
    public void Setup()
    {
        Method.ApostRelax = 0;
        Method.ApostNewton = 0;
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
    }

    [Test, Category("Group1")]
    [Category("ParallelTests")]
    public void TestFu()
    {
        Assert.AreEqual(-1, Method.Fu(0), 1e-5);
        Assert.That(Method.Fu(1), Is.Not.EqualTo(0).Within(1e-5));
        Assert.Greater(Method.Fu(2), 0);
        Assert.Less(Method.Fu(-2), 0);
    }

    [Test, Category("Group1")]
    [Category("ParallelTests")]
    public void TestFu_App()
    {
        double result = Method.Fu_App(0);
        Assert.That(result, Is.EqualTo(5).Within(Epsilon));
    }

    [Test, Category("Group1")]
    [Category("ParallelTests")]
    public void TestFu_App2()
    {
        double result = Method.Fu_App2(0);
        Assert.That(result, Is.EqualTo(-3).Within(Epsilon));
    }

    [Test, Category("Group1")]
    public void TestFiFu()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Method.FiFu(3));
    }

    [Test, Category("Group2")]
    public void TestFiFuApp()
    {
        double result = Method.FiFuApp(1);
        Assert.That(result, Is.EqualTo(0).Within(Epsilon));
    }

    [TestCase(0.13, -2.1, ExpectedResult = -2.227, TestName = "TestRelaxation_Case1", Category = "Group2")]
    [TestCase(0.15, -2.2, ExpectedResult = -2.227, TestName = "TestRelaxation_Case2", Category = "Group1")]
    public double TestRelaxation(double tau, double x0)
    {
        double result = Method.Relaxation(tau, x0);
        Assert.That(Method.ApostRelax, Is.GreaterThanOrEqualTo(1));
        return Math.Round(result, 3);
    }

    [TestCase(-2.5, 0, ExpectedResult = -2.2269999999999999d, TestName = "TestMNewton_Case1")]
    [TestCase(-2.5, -0.5, ExpectedResult = -2.2269999999999999d, TestName = "TestMNewton_Case2")]
    public double TestMNewton(double a, double b)
    {
        double result = Method.MNewton(a, b);
        Assert.That(Method.ApostNewton, Is.GreaterThanOrEqualTo(1));
        return Math.Round(result, 3);
    }

    [Test]
    public void TestApriorRelax()
    {
        int result = Method.ApriorRelax(0.2, -2.1);
        Assert.That(result, Is.GreaterThanOrEqualTo(1));
    }


    [TestCase(0.13, -2.1, ExpectedResult = -2.2269999999999999d)]
    [TestCase(0.2, -2, ExpectedResult = -2.2269999999999999d)]
    public double TestRelaxation_Parametrized(double tau, double x0)
    {
        return Method.Relaxation(tau, x0);
    }

    [Test]
    public void TestException()
    {
        Assert.Throws<DivideByZeroException>(() => Method.ApriorRelax(0, -2.1));
    }

    [Test]
    public void TestComplexMatchers()
    {
        Assert.That(Method.Fu(0), Is.EqualTo(-1).Within(1e-5));
        Assert.That(Method.Fu(2), Is.GreaterThan(0).And.LessThan(10));
        Assert.That(Method.MNewton(-2.5, 0), Is.EqualTo(-2.227288088464884d).Within(1e-5));
        Assert.That(Method.MNewton(-2.5, -0.5), Is.EqualTo(-2.227288088464884d).Within(1e-5));
    }

    [Test]
    public void TestCollectionMatchers()
    {
        double[] results = new double[] {
        Method.Relaxation(0.13, -2.1),
        Method.Relaxation(0.15, -2.2),
        Method.MNewton(-2.5, 0),
        Method.MNewton(-2.5, -0.5)
    };
        Assert.That(results, Has.Exactly(4).EqualTo(-2.227288088464884d).Within(1e-1));
    }

    [Test]
    public void TestSomeMethod_WithMock()
    {
        var mockDataProvider = new Mock<IDataProvider>();
        mockDataProvider.Setup(x => x.GetData()).Returns(21.0);

        Method.DataProvider = mockDataProvider.Object;

        double result = Method.SomeMethod();
        Assert.AreEqual(21.0, result);

        mockDataProvider.Verify(x => x.GetData(), Times.Once());
    }

    [Test]
    public void TestSomeMethod_WithMock_ThrowsException()
    {
        var mockDataProvider = new Mock<IDataProvider>();
        mockDataProvider.Setup(x => x.GetData()).Throws<InvalidOperationException>();

        Method.DataProvider = mockDataProvider.Object;

        Assert.Throws<InvalidOperationException>(() => Method.SomeMethod());

        mockDataProvider.Verify(x => x.GetData(), Times.Once());
    }

    [Test]
    public void TestUseDoSomething_WithSpy()
    {
        var someClassMock = new Mock<DataProvider> { CallBase = true };

        someClassMock.Setup(x => x.GetData()).Returns(21.0);

        double result = someClassMock.Object.UseGetData();

        Assert.AreEqual(42, result);
    }

    [Test]
    public void TestSomeMethodThatThrowsException_WithMock()
    {
        var mockDataProvider = new Mock<IDataProvider>();
        mockDataProvider.Setup(x => x.GetData()).Returns(-1.0);
        Method.DataProvider = mockDataProvider.Object;

        Assert.Throws<InvalidOperationException>(() => Method.SomeMethodThatThrowsException());

        mockDataProvider.Verify(x => x.GetData(), Times.Once());
    }


}