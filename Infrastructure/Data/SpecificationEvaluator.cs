using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        if (spec.Criteria is not null)
            inputQuery.Where(spec.Criteria);

        inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

        return inputQuery;
    }
}
