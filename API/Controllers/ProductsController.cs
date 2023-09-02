using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
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

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Product>>> GetProducts()
    {
        var spec = new ProductWithTypesAndBrandsSpecification();

        var products = await _productRepository.ListAsync(spec);

        var productsDto = _mapper.Map<IReadOnlyCollection<Product>, IReadOnlyCollection<ProductsToReturnDto>>(products);

        return Ok(productsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductsToReturnDto>> GetProduct(int id)
    {
        var spec = new ProductWithTypesAndBrandsSpecification(id);

        var product = await _productRepository.GetEntityWithSpecAsync(spec);

        return _mapper.Map<Product, ProductsToReturnDto>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyCollection<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandRepository.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyCollection<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypeRepository.ListAllAsync());
    }
}