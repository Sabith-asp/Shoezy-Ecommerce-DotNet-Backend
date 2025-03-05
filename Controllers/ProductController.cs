using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Services;

namespace Shoezy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        private readonly IMapper mapper;
        private readonly CloudinaryService cloudinaryService;

        public ProductController(CloudinaryService _cloudinaryService, IMapper _mapper, IProductService _productService) {
            service = _productService;
            cloudinaryService = _cloudinaryService;
            mapper = _mapper;
        }
        
        [HttpPost("add-product")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> AddProduct([FromForm]AddProductDTO newproduct) {
            if (newproduct.Image == null || newproduct.Image.Length == 0) {
                return BadRequest("Image is required");
            }
            using var stream = newproduct.Image.OpenReadStream();

            var ImageUrl = await cloudinaryService.UploadImageAsync(stream, newproduct.Image.FileName);

            var productdatas = mapper.Map<ProductDTO>(newproduct);
            productdatas.Image = ImageUrl;
            var productdata = mapper.Map<Shoezy.Models.Product>(productdatas);
            var response = await service.AddProduct(productdata);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProduct() {
            var response = await service.GetAllProduct();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-all-admin")]
        public async Task<IActionResult> GetAllProductByAdmin()
        {
            var response = await service.GetAllProductByAdmin();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProductById(Guid Id) {
            var response= await service.GetProductById(Id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("user/{Id}")]
        public async Task<IActionResult> GetUserProductById(Guid Id)
        {
            var response = await service.GetUserProductById(Id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("category/{category}")]

        public async Task<IActionResult> GetProductByCategory(string category)
        {
            var response= await service.GetProductByCategory(category);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("brand/{brand}")]

        public async Task<IActionResult> GetProductByBrand(string brand)
        {
            var response = await service.GetProductByBrand(brand);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedProduct(int pageNumber, int pageSize) {
            var response = await service.GetPaginatedProduct(pageNumber, pageSize);
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProduct(string param) { 
         var response=await service.SearchProduct(param);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update/{productid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(Guid productid, [FromForm]AddProductDTO editdata) {
            var response = await service.UpdateProduct(productid, editdata);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("delete/{productid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteProduct(Guid productid) { 
            var response=await service.DeleteProduct(productid);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("get-related")]
        public async Task<IActionResult> GetRelatedProducts(Guid id,string category) { 
            var response =await service.GetRelatedProducts(id,category);
            return StatusCode(response.StatusCode,response);
        }

        [HttpPost("add-category")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddCategory( string category)
        {
            var response = await service.AddCategory(category);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-category")]
        public async Task<IActionResult> GetCategory() { 
            var response= await service.GetCategory();
            return StatusCode(response.StatusCode, response);
        }
    }
}
