using System.Linq.Expressions;

namespace Core.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    IReadOnlyCollection<Expression<Func<T, object>>> Includes { get; }
}