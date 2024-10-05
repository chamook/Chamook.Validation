using Chamook.Validation.Numbers;

namespace Chamook.Validation.Tests;

public sealed class NonEmptyStringTests
{
    [Theory]
    [InlineData("")]
    [InlineData("           ")]
    public void NonEmptyStringReturnsExpectedErrorForBlankInput(string input)
    {
        var error = Guid.NewGuid().ToString();

        var result =
            NonEmptyString
                .Validate(input, () => error)
                .Match(
                    ifValid: x => "fail",
                    ifError: e => e);

        Assert.Equal(error, result);
    }

    [Fact]
    public void NonEmptyStringIsEqualToValidInput()
    {
        var input = Guid.NewGuid().ToString();

        var result =
            NonEmptyString
                .Validate(input, Guid.NewGuid().ToString)
                .Match(
                    ifValid: x => x,
                    ifError: e => e);

        Assert.Equal(input, result);
    }
}

public sealed class BoundedStringTests
{
    [Fact]
    public void BoundedStringReturnsExpectedErrorForInputTooShort()
    {
        var input = Guid.NewGuid().ToString().Substring(0, 3);
        var error = Guid.NewGuid().ToString();

        var result =
            BoundedString<N5, N100>
                .Validate(input, () => error)
                .Match(
                    ifValid: x => x,
                    ifError: e => e);

        Assert.Equal(error, result);
    }

    [Fact]
    public void BoundedStringReturnsExpectedErrorForInputTooLong()
    {
        var input = Guid.NewGuid().ToString();
        var error = Guid.NewGuid().ToString();

        var result =
            BoundedString<N1, N7>
                .Validate(input, () => error)
                .Match(
                    ifValid: x => x,
                    ifError: e => e);

        Assert.Equal(error, result);
    }

    [Fact]
    public void BoundedStringIsEqualToValidInput()
    {
        var input = Guid.NewGuid().ToString();
        var error = Guid.NewGuid().ToString();

        var result =
            BoundedString<N10, N70>
                .Validate(input, () => error)
                .Match(
                    ifValid: x => x,
                    ifError: e => e);

        Assert.Equal(input, result);
    }
}
