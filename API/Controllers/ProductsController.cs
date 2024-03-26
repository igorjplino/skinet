using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductBrand> _productBrandRepository;
    private readonly IGenericRepository<ProductType> _productTypeRepository;
    
    private readonly IMapper _mapper;

    public ProductsController(
        IGenericRepository<Product> productRepository,
        IGenericRepository<ProductBrand> productBrandRepository,
        IGenericRepository<ProductType> productTypeRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _productBrandRepository = productBrandRepository;
        _productTypeRepository = productTypeRepository;
        _mapper = mapper;
    }

    [Cached(60)]
    [HttpGet]
    public async Task<ActionResult<Pagination<ProductsToReturnDto>>> GetProducts([FromQuery] ProductsSpecParams productsParams)
    {
        var spec = new ProductWithTypesAndBrandsSpecification(productsParams);

        var countSpec = new ProductWithFiltersForCountSpecification(productsParams);

        var totalItems = await _productRepository.CountAsync(countSpec);

        var products = await _productRepository.ListAsync(spec);

        var productsDto = _mapper.Map<IReadOnlyCollection<Product>, IReadOnlyCollection<ProductsToReturnDto>>(products);

        return Ok(new Pagination<ProductsToReturnDto>(
            productsParams.PageIndex,
            productsParams.PageSize,
            totalItems,
            productsDto));
    }

    [Cached(60)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductsToReturnDto>> GetProduct(int id)
    {
        var spec = new ProductWithTypesAndBrandsSpecification(id);

        var product = await _productRepository.GetEntityWithSpecAsync(spec);

        if (product is null)
            return NotFound(new ApiResponse(404));

        return _mapper.Map<Product, ProductsToReturnDto>(product);
    }

    [Cached(600)]
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyCollection<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandRepository.ListAllAsync());
    }

    [Cached(600)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyCollection<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypeRepository.ListAllAsync());
    }
}