using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications;
public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFiltersForCountSpecification(ProductsSpecParams productsParams)
        : base(Filters(productsParams))
    {
        
    }

    private static Expression<Func<Product, bool>> Filters(ProductsSpecParams productsParams)
    {
        return p =>
            (string.IsNullOrEmpty(productsParams.Search) || p.Name.ToLower().Contains(productsParams.Search)) &&
            (!productsParams.BrandId.HasValue || p.ProductBrandId == productsParams.BrandId.Value) &&
            (!productsParams.TypeId.HasValue || p.ProductTypeId == productsParams.TypeId.Value);
    }
}
