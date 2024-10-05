namespace Chamook.Validation.Tests;

public sealed class ConstrainedTypeTests
{
    [Fact]
    public void ValidateReturnsExpectedErrorForFailingConstraint()
    {
        var error = Guid.NewGuid().ToString();
        var result =
            ConstrainedType<FailingConstraint<string>, string>
                .Validate(
                    Guid.NewGuid().ToString(),
                    () => error)
                .Match(
                    ifValid: x => x,
                    ifError: x => x);

        Assert.Equal(error, result);
    }

    [Fact]
    public void ValidateReturnsInputForPassingConstraint()
    {
        var input = Guid.NewGuid().ToString();
        var result =
            ConstrainedType<PassingConstraint<string>, string>
                .Validate(
                    input,
                    () => Guid.NewGuid().ToString())
                .Match(
                    ifValid: x => x,
                    ifError: x => x);

        Assert.Equal(input, result);
    }
}

public sealed class FailingConstraint<T> : IConstraint<T>
{
    public bool IsValid(T candidate) => false;
}

public sealed class PassingConstraint<T> : IConstraint<T>
{
    public bool IsValid(T candidate) => true;
}
