using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications;

public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductWithTypesAndBrandsSpecification()
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }

    public ProductWithTypesAndBrandsSpecification(int id) 
        : base(GetById(id))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }

    private static Expression<Func<Product, bool>> GetById(int id)
    {
        return p => p.Id == id;
    }
}
