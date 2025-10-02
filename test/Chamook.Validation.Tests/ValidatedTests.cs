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

    [Fact]
    public void AndReturnsBothValidValues()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();

        var result =
            Validated<Guid, string>.Valid(first)
            .And(Validated<Guid, string>.Valid(second));

        Assert.True(result.IsValid);
        Assert.Equal(first, result.GetValidValueOrThrow().Item1);
        Assert.Equal(second, result.GetValidValueOrThrow().Item2);
    }

    [Fact]
    public void AndReturnsErrorForFirstFailure()
    {
        var error = Guid.NewGuid().ToString();

        var result =
            Validated<Guid, string>.Error(error)
            .And(Validated<Guid, string>.Valid(Guid.NewGuid()));

        Assert.False(result.IsValid);
        var returnedError = Assert.Single(result.GetErrorOrThrow());
        Assert.Equal(error, returnedError);
    }

    [Fact]
    public void AndReturnsErrorForSecondFailure()
    {
        var error = Guid.NewGuid().ToString();

        var result =
            Validated<Guid, string>.Valid(Guid.NewGuid())
            .And(Validated<Guid, string>.Error(error));

        Assert.False(result.IsValid);
        var returnedError = Assert.Single(result.GetErrorOrThrow());
        Assert.Equal(error, returnedError);
    }

    [Fact]
    public void AndReturnsBothErrors()
    {
        var error1 = Guid.NewGuid().ToString();
        var error2 = Guid.NewGuid().ToString();

        var result =
            Validated<Guid, string>.Error(error1)
            .And(Validated<Guid, string>.Error(error2));

        Assert.False(result.IsValid);
        var errors = result.GetErrorOrThrow();
        Assert.Contains(error1, errors);
        Assert.Contains(error2, errors);
    }

    [Fact]
    public void AndReturnsMultipleErrors()
    {
        var error1 = Guid.NewGuid().ToString();
        var error2 = Guid.NewGuid().ToString();
        var error3 = Guid.NewGuid().ToString();
        var error4 = Guid.NewGuid().ToString();
        var error5 = Guid.NewGuid().ToString();
        var error6 = Guid.NewGuid().ToString();

        var result =
            Validated<Guid, string>.Error(error1)
            .And(Validated<Guid, string>.Error(error2))
            .And(Validated<Guid, string>.Error(error3))
            .And(Validated<Guid, string>.Error(error4))
            .And(Validated<Guid, string>.Error(error5))
            .And(Validated<Guid, string>.Error(error6));

        Assert.False(result.IsValid);
        var errors = result.GetErrorOrThrow();
        Assert.Contains(error1, errors);
        Assert.Contains(error2, errors);
        Assert.Contains(error3, errors);
        Assert.Contains(error4, errors);
        Assert.Contains(error5, errors);
        Assert.Contains(error6, errors);
    }
}
