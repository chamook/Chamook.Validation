namespace Chamook.Validation.Tests;

public sealed class ValidatedTests
{
    [Fact]
    public void CanRoundTripSuccessValue()
    {
        var input = Guid.NewGuid().ToString();

        var sut = Validated<string, string>.Valid(input);

        var result =
            sut.Match(
                ifValid: v => v,
                ifError: e => "Shouldn't go here");
        Assert.Equal(input, result);
    }

    [Fact]
    public void CanRoundTripFailureValue()
    {
        var input = Guid.NewGuid().ToString();

        var sut = Validated<string, string>.Error(input);

        var result =
            sut.Match(
                ifValid: v => "Shouldn't go here",
                ifError: e => e);
        Assert.Equal(input, result);
    }
}
