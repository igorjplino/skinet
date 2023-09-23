using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        if (spec.Criteria is not null)
            inputQuery = inputQuery.Where(spec.Criteria);

        if (spec.OrderBy is not null)
            inputQuery = inputQuery.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending is not null)
            inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPagingEnabled)
            inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);

        inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

        return inputQuery;
    }
}
