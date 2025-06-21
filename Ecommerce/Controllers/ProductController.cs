using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Spacifications;
using Ecommerce.DTO;
using Ecommerce.Helper;
using Infrastrucure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;  
        private readonly IGenericRepository<ProductType>  _ProductTypeRepository;  
        private readonly IGenericRepository<Product> _productRepo;  
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository,
            IGenericRepository<ProductBrand> productBrandRepository,
            IGenericRepository<ProductType> ProductTypeRepository,
            IGenericRepository<Product> productRepo,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _ProductTypeRepository = ProductTypeRepository;
            _productRepo = productRepo;
            _mapper = mapper;   
        }
        [HttpGet]  //FromQuery becouse it get method not FromBody
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            ProductWithTypesAndBrandsSpacification spac = new ProductWithTypesAndBrandsSpacification(productParams);
            ProductWithFiltersForCountSpacification countSpec = new ProductWithFiltersForCountSpacification(productParams);

            var pro = await _productRepo.ListAsync(spac);
            var totalItems = await _productRepo.CountAsync(countSpec);
            // var products = await _productRepo.GetAllAsync();

            /* return Ok(pro.Select(product => new ProductDto
             {
                 Id = product.Id,
                 Name = product.Name,
                 Description = product.Description,
                 Price = product.Price,
                 PictureUrl = product.PictureUrl,
                 ProductType = product.ProductType.Name,
                 ProductBrand = product.ProductBrand.Name
             }).ToList());
            */
            //replace above with mapper  
            //  return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(pro));
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(pro);
            return Ok(new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
        [HttpGet("{Id}")]//api/product/1
        public async Task<IActionResult> GetProduct(int Id)
        {
            ProductWithTypesAndBrandsSpacification spac = new ProductWithTypesAndBrandsSpacification(Id);
            var product = await _productRepo.GetEntityWithSpec(spac);
            // var product = await _productRepo.GetByIdAsync(Id);
            // return Ok(product);

            /* return Ok(new ProductDto
             {
                 Id = product.Id,
                 Name = product.Name,
                 Description = product.Description,
                 Price = product.Price,
                 PictureUrl = product.PictureUrl,
                 ProductType = product.ProductType.Name,
                 ProductBrand = product.ProductBrand.Name
             });
            */
            // replace above with mapper
            return Ok(_mapper.Map<Product, ProductDto>(product));
        }
        [HttpGet("brands")]//api/product/brands
        public async Task<IActionResult> GetProductBrands()
        {
            var brands = await _productBrandRepository.GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("types")]//api/product/types
        public async Task<IActionResult> GetProductTypes()
        {
            var types = await _ProductTypeRepository.GetAllAsync();
            return Ok(types);
        }
    }

   
}
