using task11;
using Xunit;
using System;
namespace task11tests;

public class CalculatorTests
{
    [Fact]
    public void Add_ReturnCorrectAnswer()
    {
        dynamic calculator = ClassGenerator.CreateCalculator();
        int result = calculator.Add(10, 10);
        Assert.Equal(20, result);
    }

    [Fact]
    public void Minus_ReturnCorrectAnswer()
    {
        dynamic calculator = ClassGenerator.CreateCalculator();
        int result = calculator.Minus(10, 9);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Mul_ReturnCorrectAnswer()
    {
        dynamic calculator = ClassGenerator.CreateCalculator();
        int result = calculator.Mul(10, 10);
        Assert.Equal(100, result);
    }

    [Fact]
    public void Div_ReturnCorrectAnswer()
    {
        dynamic calculator = ClassGenerator.CreateCalculator();
        int result = calculator.Div(10, 10);
        Assert.Equal(1, result);
    }

    [Fact]
    public void DivByZero_ReturnError()
    {
        dynamic calculator = ClassGenerator.CreateCalculator();
        bool threw = false;
        try
        {
            calculator.Div(1, 0);
        }
        catch (DivideByZeroException)
        {
            threw = true;
        }
        Assert.True(threw);
    }
}
