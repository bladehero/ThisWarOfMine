using System.Text;
using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Abstraction;

public class Error : ValueObject, ICombine
{
    private string _message = null!;
    public virtual string Message => Exception.HasValue ? Exception.Value.Message : _message;
    public Maybe<Exception> Exception { get; private init; }

    private Error() { }

    ICombine ICombine.Combine(ICombine combine)
    {
        if (combine is not Error error)
        {
            throw new InvalidCastException($"Cannot combine `{combine.GetType()}` as {nameof(Error)}");
        }

        return Combine(error);
    }

    protected virtual bool IsComplex() => false;

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Message;
    }

    public static Error Because(string message) => new() { _message = message };

    public static Error Because(Exception exception) => new() { Exception = exception };

    public virtual Error Combine(Error error) => ComplexError.Wrap(this).Combine(error);

    private sealed class ComplexError : Error
    {
        private Error _current = null!;
        public Maybe<ComplexError> Next { get; private set; }
        public override string Message => BuildMessage();

        private ComplexError() { }

        public override Error Combine(Error error)
        {
            var next = error.IsComplex() ? (ComplexError)error : Wrap(error);
            var last = Last();
            last.Next = next;
            return last;
        }

        protected override bool IsComplex() => true;

        public static ComplexError Wrap(Error error)
        {
            ThrowIfComplexError(error);

            return new ComplexError { _current = error };
        }

        #region Helpers

        private ComplexError Last()
        {
            var error = Next.GetValueOrThrow(
                $"{nameof(ComplexError)} should never has no next value for first chain part"
            );
            while (error.Next.HasValue)
            {
                error = error.Next.Value;
            }

            return error;
        }

        private string BuildMessage()
        {
            var stack = new Stack<Error>();
            CaptureTo(stack, this);

            var builder = new StringBuilder();
            while (stack.TryPop(out var error))
            {
                builder.Append(error._message).Append(',').Append(' ');
            }

            return builder.ToString();
        }

        private static void CaptureTo(Stack<Error> stack, ComplexError error)
        {
            while (true)
            {
                if (error.Next.HasNoValue)
                {
                    return;
                }

                stack.Push(error._current);
                error = error.Next.Value;
            }
        }

        private static void ThrowIfComplexError(Error error)
        {
            if (error.IsComplex())
            {
                throw new InvalidOperationException(
                    $"Cannot start {nameof(ComplexError)} with another complex error: `{error}`"
                );
            }
        }

        #endregion
    }

    public ICombine Combine(ICombine value)
    {
        throw new NotImplementedException();
    }
}
