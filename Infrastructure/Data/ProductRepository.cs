﻿using Core.Entities;
using Core.Interfaces;
using Infrastructue.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandAsync()
    {
        return await _context.ProductBrands.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.ProdutcBrand)
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _context.Products
            .Include(p => p.ProdutcBrand)
            .Include(p => p.ProductType)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypeAsync()
    {
        return await _context.ProductTypes.ToListAsync();
    }
}
