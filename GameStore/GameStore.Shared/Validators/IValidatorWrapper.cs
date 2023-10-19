namespace GameStore.Shared.Validators;

public interface IValidatorWrapper<T>
    where T : class
{
    void ValidateAndThrow(T instance);
}