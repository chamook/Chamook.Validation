namespace Chamook.Validation;

///<summary>
///An exception thrown when trying to directly access one of the potential values
///in a Validated type when it is in the wrong state.
///</summary>
public class ValidationException : Exception
{
    public ValidationException() : base() {}
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception inner) : base(message, inner) {}
}

///<summary>
///Represents both potential outcomes from a validation operation.
///Either a Valid value or an Error.
///</summary>
public readonly struct Validated<TValid, TError>
{
    private readonly TValid _validValue { get; init; }
    private readonly TError _errorValue { get; init; }
    public readonly bool IsValid { get; init; }

    ///<summary>
    ///Create a new Validated instance for a successfully validated type
    ///</summary>
    public static Validated<TValid, TError> Valid(TValid valid) =>
        new Validated<TValid, TError>() { _validValue = valid, IsValid = true };

    ///<summary>
    ///Create a new Validated instance for an error
    ///</summary>
    public static Validated<TValid, TError> Error(TError error) =>
        new Validated<TValid, TError>() { _errorValue = error, IsValid = false };

    ///<summary>
    ///Provide functions to handle both potential cases of a Validated object
    ///and return a common outcome type.
    ///</summary>
    public TResult Match<TResult>(
        Func<TValid, TResult> ifValid,
        Func<TError, TResult> ifError) =>
        IsValid ? ifValid(_validValue) : ifError(_errorValue);

    public void Match(
        Action<TValid> ifValid,
        Action<TError> ifError)
    {
        if (IsValid)
            ifValid(_validValue);
        else
            ifError(_errorValue);
    }

    ///<summary>
    ///Attempt to extract the valid value from this Validated instance, throw if it contains an
    ///error.
    ///</summary>
    ///<remarks>
    ///You should probably use Match instead of this.
    ///</remarks>
    public TValid GetValidValueOrThrow() =>
        IsValid
        ? _validValue
        : throw new ValidationException(
            "Attempted to retrieve a valid value from a Validated object that contained an error");

    ///<summary>
    ///Attempt to extract the error from this Validated instance, throw if it contains a
    ///valid value.
    ///</summary>
    ///<remarks>
    ///You should probably use Match instead of this.
    ///</remarks>
    public TError GetErrorOrThrow() =>
        (!IsValid)
        ? _errorValue
        : throw new ValidationException(
            "Attempted to retrieve an error from a Validated object that contained a valid value");
}

public static class ValidatedExtensions
{
    // Regular map and mapError - allow applying a transformation function
    // to the respective value inside a Validated object and short circuit if
    // it's in the other state

    public static Validated<TNew, TError> Map<TValid, TNew, TError>(
        this Validated<TValid, TError> validated,
        Func<TValid, TNew> mapper) =>
        validated.Match(
            ifValid: v => Validated<TNew, TError>.Valid(mapper(v)),
            ifError: e => Validated<TNew, TError>.Error(e));

    public static Validated<TValid, TNewError> MapError<TValid, TError, TNewError>(
        this Validated<TValid, TError> validated,
        Func<TError, TNewError> mapper) =>
        validated.Match(
            ifValid: v => Validated<TValid, TNewError>.Valid(v),
            ifError: e => Validated<TValid, TNewError>.Error(mapper(e)));

    // And is for combining two validations - any errors put the whole thing into
    // an error state, but multiple errors are collected together

    public static Validated<(A, B), List<TError>> And<A, B, TError>(
        this Validated<A, TError> first,
        Validated<B, TError> second)
    {
        if (first.IsValid && second.IsValid)
            return Validated<(A, B), List<TError>>.Valid(
                (first.GetValidValueOrThrow(), second.GetValidValueOrThrow()));

        var errors = new List<TError>(2);
        if (!first.IsValid)
            errors.Add(first.GetErrorOrThrow());
        if (!second.IsValid)
            errors.Add(second.GetErrorOrThrow());

        return Validated<(A, B), List<TError>>.Error(errors);
    }

    public static Validated<(A, B), List<TError>> And<A, B, TError>(
        this Validated<A, List<TError>> first,
        Validated<B, TError> second)
    {
        if (first.IsValid && second.IsValid)
            return Validated<(A, B), List<TError>>.Valid(
                (first.GetValidValueOrThrow(), second.GetValidValueOrThrow()));

        var errors = new List<TError>();
        if (!first.IsValid)
            errors.Concat(first.GetErrorOrThrow());
        if (!second.IsValid)
            errors.Add(second.GetErrorOrThrow());

        return Validated<(A, B), List<TError>>.Error(errors);
    }

    public static Validated<(A, B), List<TError>> And<A, B, TError>(
        this Validated<A, TError> first,
        Validated<B, List<TError>> second)
    {
        if (first.IsValid && second.IsValid)
            return Validated<(A, B), List<TError>>.Valid(
                (first.GetValidValueOrThrow(), second.GetValidValueOrThrow()));

        var errors = new List<TError>();
        if (!first.IsValid)
            errors.Add(first.GetErrorOrThrow());
        if (!second.IsValid)
            errors.Concat(second.GetErrorOrThrow());

        return Validated<(A, B), List<TError>>.Error(errors);
    }

    public static Validated<(A, B), List<TError>> And<A, B, TError>(
        this Validated<A, List<TError>> first,
        Validated<B, List<TError>> second)
    {
        if (first.IsValid && second.IsValid)
            return Validated<(A, B), List<TError>>.Valid(
                (first.GetValidValueOrThrow(), second.GetValidValueOrThrow()));

        var errors = new List<TError>();
        if (!first.IsValid)
            errors.Concat(first.GetErrorOrThrow());
        if (!second.IsValid)
            errors.Concat(second.GetErrorOrThrow());

        return Validated<(A, B), List<TError>>.Error(errors);
    }

    // Some fancy overloads for map to provide a nicer API when working
    // with combined objects
    // This is useful for building more complicated types out of the
    // small building blocks - e.g. a record made up of several NonEmptyStrings

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C>(
        this Validated<((A, B), C), TError> validated,
        Func<A, B, C, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c) = x.Flatten();
            return mapper(a, b, c);
        });

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C, D>(
        this Validated<(((A, B), C), D), TError> validated,
        Func<A, B, C, D, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c, d) = x.Flatten();
            return mapper(a, b, c, d);
        });

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C, D, E>(
        this Validated<((((A, B), C), D), E), TError> validated,
        Func<A, B, C, D, E, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c, d, e) = x.Flatten();
            return mapper(a, b, c, d, e);
        });

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C, D, E, F>(
        this Validated<(((((A, B), C), D), E), F), TError> validated,
        Func<A, B, C, D, E, F, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c, d, e, f) = x.Flatten();
            return mapper(a, b, c, d, e, f);
        });

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C, D, E, F, G>(
        this Validated<((((((A, B), C), D), E), F), G), TError> validated,
        Func<A, B, C, D, E, F, G, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c, d, e, f, g) = x.Flatten();
            return mapper(a, b, c, d, e, f, g);
        });

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C, D, E, F, G, H>(
        this Validated<(((((((A, B), C), D), E), F), G), H), TError> validated,
        Func<A, B, C, D, E, F, G, H, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c, d, e, f, g, h) = x.Flatten();
            return mapper(a, b, c, d, e, f, g, h);
        });

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C, D, E, F, G, H, I>(
        this Validated<((((((((A, B), C), D), E), F), G), H), I), TError> validated,
        Func<A, B, C, D, E, F, G, H, I, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c, d, e, f, g, h, i) = x.Flatten();
            return mapper(a, b, c, d, e, f, g, h, i);
        });

    public static Validated<TNew, TError> Map<TNew, TError, A, B, C, D, E, F, G, H, I, J>(
        this Validated<(((((((((A, B), C), D), E), F), G), H), I), J), TError> validated,
        Func<A, B, C, D, E, F, G, H, I, J, TNew> mapper) =>
        validated.Map(x =>
        {
            var (a, b, c, d, e, f, g, h, i, j) = x.Flatten();
            return mapper(a, b, c, d, e, f, g, h, i, j);
        });
}
