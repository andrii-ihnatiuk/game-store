using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace GameStore.Shared.Validators;

[ExcludeFromCodeCoverage]
public class ValidatorWrapper<T> : IValidatorWrapper<T>
    where T : class
{
    private readonly IValidator<T> _validator;

    public ValidatorWrapper(IValidator<T> validator)
    {
        _validator = validator;
    }

    public void ValidateAndThrow(T instance)
    {
        _validator.ValidateAndThrow(instance);
    }
}