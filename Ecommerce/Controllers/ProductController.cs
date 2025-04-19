using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Spacifications;
using Ecommerce.DTO;
using Infrastrucure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
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
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            ProductWithTypesAndBrandsSpacification spac = new ProductWithTypesAndBrandsSpacification();

            var pro = await _productRepo.ListAsync(spac);
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
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(pro));
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
