namespace Chamook.Validation;

///<summary>
///Represents both potential outcomes from a validation operation.
///Either a Valid value or an Error.
///</summary>
public readonly struct Validated<TValid, TError>
{
    private readonly TValid _validValue { get; init; }
    private readonly TError _errorValue { get; init; }
    private readonly bool _isValid { get; init; }

    ///<summary>
    ///Create a new Validated instance for a successfully validated type
    ///</summary>
    public static Validated<TValid, TError> Valid(TValid valid) =>
        new Validated<TValid, TError>() { _validValue = valid, _isValid = true };

    ///<summary>
    ///Create a new Validated instance for an error
    ///</summary>
    public static Validated<TValid, TError> Error(TError error) =>
        new Validated<TValid, TError>() { _errorValue = error, _isValid = false };

    ///<summary>
    ///Provide functions to handle both potential cases of a Validated object
    ///and return a common outcome type.
    ///</summary>
    public TResult Match<TResult>(
        Func<TValid, TResult> ifValid,
        Func<TError, TResult> ifError) =>
        _isValid ? ifValid(_validValue) : ifError(_errorValue);
}
