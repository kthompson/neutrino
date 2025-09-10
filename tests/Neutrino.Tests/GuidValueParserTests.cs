using static Neutrino.Syntax;
using GUID = System.Guid;
namespace Neutrino.Tests;

public class GuidValueParserTests
{
    [Fact]
    public void Parse_ValidGuid_ReturnsSuccess()
    {
        var parser = Uuid();
        var guid = GUID.NewGuid();
        var result = parser.Parse(guid.ToString());
        Assert.IsType<ValueParserResult<Guid>.Success>(result);
        Assert.Equal(guid, ((ValueParserResult<Guid>.Success)result).Value);
    }

    [Fact]
    public void Parse_InvalidGuid_ReturnsFailure()
    {
        var parser = Uuid();
        var result = parser.Parse("not-a-guid");
        Assert.IsType<ValueParserResult<Guid>.Failure>(result);
    }

    [Fact]
    public void Format_Guid_ReturnsStringRepresentation()
    {
        var parser = Uuid();
        var guid = GUID.NewGuid();
        var formatted = parser.Format(guid);
        Assert.Equal(guid.ToString(), formatted);
    }
}