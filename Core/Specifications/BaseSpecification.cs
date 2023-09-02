using System.Linq.Expressions;

namespace Core.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = new();

    public BaseSpecification()
    {

    }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>> Criteria { get; }

    public IReadOnlyCollection<Expression<Func<T, object>>> Includes => _includes.AsReadOnly();

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }
}
