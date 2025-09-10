using static Neutrino.Syntax;

namespace Neutrino.Tests;

public class SyntaxIntValueParserTests
{
    [Fact]
    public void Int_NoConstraints_ParsesValidInteger()
    {
        var parser = Int();
        var result = parser.Parse("42");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(42, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_NoConstraints_ParsesNegativeInteger()
    {
        var parser = Int();
        var result = parser.Parse("-123");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(-123, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_NoConstraints_ParsesZero()
    {
        var parser = Int();
        var result = parser.Parse("0");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(0, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_NoConstraints_ParsesMaxValue()
    {
        var parser = Int();
        var result = parser.Parse(int.MaxValue.ToString());
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(int.MaxValue, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_NoConstraints_ParsesMinValue()
    {
        var parser = Int();
        var result = parser.Parse(int.MinValue.ToString());
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(int.MinValue, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_NoConstraints_FailsOnInvalidString()
    {
        var parser = Int();
        var result = parser.Parse("not a number");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_NoConstraints_FailsOnEmptyString()
    {
        var parser = Int();
        var result = parser.Parse("");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_NoConstraints_FailsOnDecimalNumber()
    {
        var parser = Int();
        var result = parser.Parse("42.5");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_NoConstraints_FailsOnNumberTooLarge()
    {
        var parser = Int();
        var result = parser.Parse("99999999999999999999");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_WithConstraints_ParsesValueInRange()
    {
        var parser = Int(10, 20);
        var result = parser.Parse("15");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(15, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_WithConstraints_ParsesMinValue()
    {
        var parser = Int(10, 20);
        var result = parser.Parse("10");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(10, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_WithConstraints_ParsesMaxValue()
    {
        var parser = Int(10, 20);
        var result = parser.Parse("20");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(20, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_WithConstraints_FailsOnValueTooSmall()
    {
        var parser = Int(10, 20);
        var result = parser.Parse("5");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_WithConstraints_FailsOnValueTooLarge()
    {
        var parser = Int(10, 20);
        var result = parser.Parse("25");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_WithConstraints_FailsOnInvalidString()
    {
        var parser = Int(10, 20);
        var result = parser.Parse("not a number");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_WithNegativeRange_ParsesNegativeValue()
    {
        var parser = Int(-50, -10);
        var result = parser.Parse("-30");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(-30, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_WithNegativeRange_FailsOnPositiveValue()
    {
        var parser = Int(-50, -10);
        var result = parser.Parse("5");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }

    [Fact]
    public void Int_WithMixedRange_ParsesZero()
    {
        var parser = Int(-5, 5);
        var result = parser.Parse("0");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(0, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_WithSingleValueRange_ParsesExactValue()
    {
        var parser = Int(42, 42);
        var result = parser.Parse("42");
        Assert.IsType<ValueParserResult<int>.Success>(result);
        Assert.Equal(42, ((ValueParserResult<int>.Success)result).Value);
    }

    [Fact]
    public void Int_WithSingleValueRange_FailsOnDifferentValue()
    {
        var parser = Int(42, 42);
        var result = parser.Parse("43");
        Assert.IsType<ValueParserResult<int>.Failure>(result);
    }
}