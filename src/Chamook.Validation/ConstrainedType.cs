namespace Chamook.Validation;

///<summary>
///A constraint to determine the validity of a type
///</summary>
public interface IConstraint<T>
{
    bool IsValid(T candidate);
}

///<summary>
///A building block type that represents an instance of TInput that is valid according to TConstraint
///</summary>
public class ConstrainedType<TConstraint, TInput>
    where TConstraint : IConstraint<TInput>, new()
{
    protected static TConstraint _constraint;
    protected TInput _value;

    ///<remarks>
    ///This will be run once for each version of the class (i.e. once per set of type parameters)
    ///</remarks>
    static ConstrainedType()
    {
        _constraint = new TConstraint();
    }

    protected ConstrainedType(TInput value)
    {
        _value = value;
    }

    public static implicit operator TInput(ConstrainedType<TConstraint, TInput> constrained) =>
        constrained._value;

    ///<summary>
    ///Checks the input value against the constraints specified in TConstraint and returns Valid
    ///if it passes or Error with the value produced by errorCreator if not
    ///</summary>
    public static Validated<ConstrainedType<TConstraint, TInput>, TError> Validate<TError>(
        TInput input,
        Func<TError> errorCreator) =>
    _constraint.IsValid(input)
    ? Validated<ConstrainedType<TConstraint, TInput>, TError>.Valid(new(input))
    : Validated<ConstrainedType<TConstraint, TInput>, TError>.Error(errorCreator());

    ///<summary>
    ///This is just an alias for Validate so that subtypes can use it when providing their own version
    ///of Validate
    ///</summary>
    ///<remarks>
    ///Subtypes need to provide their own version of Validate to ensure that the correct success type
    ///is returned.
    ///</remarks>
    public static Validated<ConstrainedType<TConstraint, TInput>, TError> DoValidate<TError>(
        TInput input,
        Func<TError> errorCreator) =>
        Validate(input, errorCreator);
}

