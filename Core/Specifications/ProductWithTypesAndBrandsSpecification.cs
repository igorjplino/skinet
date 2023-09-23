using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications;

public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductWithTypesAndBrandsSpecification(ProductsSpecParams productsParams)
        : base(Filters(productsParams))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
        ApplyPage(productsParams.PageSize * (productsParams.PageIndex - 1), productsParams.PageSize);

        if (!string.IsNullOrEmpty(productsParams.Sort))
        {
            switch (productsParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            };
        }
    }

    public ProductWithTypesAndBrandsSpecification(int id) 
        : base(GetById(id))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }

    private static Expression<Func<Product, bool>> Filters(ProductsSpecParams productsParams)
    {
        return p => 
            (string.IsNullOrEmpty(productsParams.Search) || p.Name.ToLower().Contains(productsParams.Search)) &&
            (!productsParams.BrandId.HasValue || p.ProductBrandId == productsParams.BrandId.Value) &&
            (!productsParams.TypeId.HasValue || p.ProductTypeId == productsParams.TypeId.Value);
    }

    private static Expression<Func<Product, bool>> GetById(int id)
    {
        return p => p.Id == id;
    }
}
