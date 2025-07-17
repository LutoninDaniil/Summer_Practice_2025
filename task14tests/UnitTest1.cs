namespace task14tests;
using Xunit;
using task14;

public class DefiniteIntegralTests
{
    [Fact]
    public void DefiniteIntegral_ReturnCorrectValue()
    {
        var A = (double a) => a;

        var Sin = (double x) => Math.Sin(x);

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, A, 1e-4, 2), 1e-4);

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, Sin, 1e-5, 8), 1e-4);

        Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, A, 1e-6, 8), 1e-5);
    }
    [Fact]
    public void ConstantFunction_ReturnCorrectValue()
    {
        var Const = (double a) => 3.0;

        Assert.Equal(30, DefiniteIntegral.Solve(0, 10, Const, 1e-5, 4), precision: 5);
    }

    [Fact]
    public void QuadraticFunction_ReturnCorrectValue()
    {
        var Square = (double a) => a * a;

        Assert.Equal(9, DefiniteIntegral.Solve(0, 3, Square, 1e-6, 4), precision: 5);
    }
}
