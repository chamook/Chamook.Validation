using Chamook.Validation.Numbers;

namespace Chamook.Validation;

///<summary>
///A string that can't be empty or whitespace
///</summary>
public sealed class NonEmptyString: ConstrainedType<NonEmptyString.Constraint, string>
{
    public sealed class Constraint : IConstraint<string>
    {
        public Constraint() {}
        public bool IsValid(string candidate) => !string.IsNullOrWhiteSpace(candidate);
    }

    private NonEmptyString(string valid) : base(valid) {}
    public new static Validated<NonEmptyString, TError> Validate<TError>(
        string? candidate,
        Func<TError> errorCreator) =>
        DoValidate(candidate ?? "", errorCreator).Map(x => new NonEmptyString(x));
}

///<summary>
///A string with a length between 2 bounds
///</summary>
public class BoundedString<TLowerBound, TUpperBound>
    : ConstrainedType<BoundedString<TLowerBound, TUpperBound>.Constraint, string>
    where TLowerBound : NumberValue, new()
    where TUpperBound: NumberValue, new()
{
    private static int _lowerBound;
    private static int _upperBound;

    static BoundedString()
    {
        _lowerBound = new TLowerBound().Value;
        _upperBound = new TUpperBound().Value;
    }

    public sealed class Constraint: IConstraint<string>
    {
        public Constraint() {}
        public bool IsValid(string candidate) =>
            candidate.Length >= _lowerBound
            && candidate.Length <= _upperBound;
    }

    private BoundedString(string valid) : base(valid) {}
    public new static Validated<BoundedString<TLowerBound, TUpperBound>, TError> Validate<TError>(
        string candidate,
        Func<TError> errorCreator) =>
        DoValidate(candidate, errorCreator)
        .Map(x => new BoundedString<TLowerBound, TUpperBound>(x));
}
