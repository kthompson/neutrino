using static Neutrino.Syntax;

namespace Neutrino.Tests;

public class FloatValueParserTests
{
    [Fact]
    public void Float_NoConstraints_ParsesValidFloat()
    {
        var parser = Float();
        var result = parser.Parse("42.5");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(42.5f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesInteger()
    {
        var parser = Float();
        var result = parser.Parse("42");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(42.0f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesNegativeFloat()
    {
        var parser = Float();
        var result = parser.Parse("-123.45");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(-123.45f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesZero()
    {
        var parser = Float();
        var result = parser.Parse("0");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(0.0f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesZeroWithDecimals()
    {
        var parser = Float();
        var result = parser.Parse("0.0");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(0.0f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesMaxValue()
    {
        var parser = Float();
        var result = parser.Parse(float.MaxValue.ToString());
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(float.MaxValue, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesMinValue()
    {
        var parser = Float();
        var result = parser.Parse(float.MinValue.ToString());
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(float.MinValue, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesScientificNotation()
    {
        var parser = Float();
        var result = parser.Parse("1.23E+5");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(123000.0f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_NoConstraints_ParsesNegativeScientificNotation()
    {
        var parser = Float();
        var result = parser.Parse("-1.23E-2");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(-0.0123f, ((ValueParserResult<float>.Success)result).Value, 6);
    }

    [Fact]
    public void Float_NoConstraints_FailsOnInvalidString()
    {
        var parser = Float();
        var result = parser.Parse("not a number");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_NoConstraints_FailsOnEmptyString()
    {
        var parser = Float();
        var result = parser.Parse("");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_NoConstraints_FailsOnMultipleDecimals()
    {
        var parser = Float();
        var result = parser.Parse("42.5.3");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_WithConstraints_ParsesValueInRange()
    {
        var parser = Float(10.0f, 20.0f);
        var result = parser.Parse("15.5");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(15.5f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_WithConstraints_ParsesMinValue()
    {
        var parser = Float(10.0f, 20.0f);
        var result = parser.Parse("10.0");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(10.0f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_WithConstraints_ParsesMaxValue()
    {
        var parser = Float(10.0f, 20.0f);
        var result = parser.Parse("20.0");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(20.0f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_WithConstraints_FailsOnValueTooSmall()
    {
        var parser = Float(10.0f, 20.0f);
        var result = parser.Parse("5.0");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_WithConstraints_FailsOnValueTooLarge()
    {
        var parser = Float(10.0f, 20.0f);
        var result = parser.Parse("25.0");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_WithConstraints_FailsOnInvalidString()
    {
        var parser = Float(10.0f, 20.0f);
        var result = parser.Parse("not a number");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_WithNegativeRange_ParsesNegativeValue()
    {
        var parser = Float(-50.0f, -10.0f);
        var result = parser.Parse("-30.5");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(-30.5f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_WithNegativeRange_FailsOnPositiveValue()
    {
        var parser = Float(-50.0f, -10.0f);
        var result = parser.Parse("5.0");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_WithMixedRange_ParsesZero()
    {
        var parser = Float(-5.0f, 5.0f);
        var result = parser.Parse("0.0");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(0.0f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_WithSingleValueRange_ParsesExactValue()
    {
        var parser = Float(42.5f, 42.5f);
        var result = parser.Parse("42.5");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(42.5f, ((ValueParserResult<float>.Success)result).Value);
    }

    [Fact]
    public void Float_WithSingleValueRange_FailsOnDifferentValue()
    {
        var parser = Float(42.5f, 42.5f);
        var result = parser.Parse("42.6");
        Assert.IsType<ValueParserResult<float>.Failure>(result);
    }

    [Fact]
    public void Float_WithEqualMinMax_DoesNotThrow()
    {
        var parser = Float(5.0f, 5.0f);
        Assert.NotNull(parser);
    }

    [Fact]
    public void Float_WithMaxLessThanMin_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Float(10.0f, 5.0f));
    }

    [Fact]
    public void Float_WithVerySmallRange_ParsesValueInRange()
    {
        var parser = Float(0.001f, 0.002f);
        var result = parser.Parse("0.0015");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(0.0015f, ((ValueParserResult<float>.Success)result).Value, 6);
    }

    [Fact]
    public void Float_WithLargeRange_ParsesLargeValue()
    {
        var parser = Float(1000000.0f, 2000000.0f);
        var result = parser.Parse("1500000.5");
        Assert.IsType<ValueParserResult<float>.Success>(result);
        Assert.Equal(1500000.5f, ((ValueParserResult<float>.Success)result).Value);
    }
}
