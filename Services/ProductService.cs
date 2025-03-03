using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Repositories;

namespace Shoezy.Services
{
    public interface IProductService {
        Task <Result<object>> AddProduct(Product newproduct);
        Task <Result<List<ProductGetDTO>>> GetAllProduct();
        Task<Result<List<ProductGetAdminDTO>>> GetAllProductByAdmin();
        Task <Result<Product>> GetProductById(Guid Id);
        Task<Result<ProductGetDTO>> GetUserProductById(Guid Id);
        Task <Result<List<ProductGetAdminDTO>>> GetProductByCategory(string category);
        Task<Result<List<ProductGetDTO>>> GetProductByBrand(string brand);
        Task <Result<List<ProductGetDTO>>> GetPaginatedProduct(int pageNumber, int pageSize);
        Task <Result<List<ProductGetDTO>>> SearchProduct(string param);

        Task<Result<List<ProductGetDTO>>> GetRelatedProducts(Guid id,string category);

        Task <Result<object>> UpdateProduct(Guid productid, AddProductDTO editdata);

        Task<Result<object>> DeleteProduct(Guid productid);

        Task<Result<object>> AddCategory(AddCategoryDTO category);

        Task<Result<List<GetCategoryDTO>>> GetCategory();
    }
    public class ProductService:IProductService
    {
        private readonly IProductRepository repository;
        private readonly CloudinaryService cloudinary;
        private readonly IMapper mapper;
        public ProductService(IProductRepository productRepository, CloudinaryService _cloudinary,IMapper _mapper) { 
            repository = productRepository;
            cloudinary = _cloudinary;
            mapper = _mapper;
        }
        public async Task<Result<object>> AddProduct(Product newproduct) { 
            return await repository.AddProduct(newproduct);
        }

        public async Task<Result<List<ProductGetDTO>>> GetAllProduct()
        {
            return await repository.GetAllProduct();
        }

        public async Task<Result<List<ProductGetAdminDTO>>> GetAllProductByAdmin()
        {
            return await repository.GetAllProductByAdmin();
        }
        public async Task<Result<Product>> GetProductById(Guid Id) { 
            return await repository.GetProductById(Id);
        }
        public async Task<Result<ProductGetDTO>> GetUserProductById(Guid Id)
        {
            return await repository.GetUserProductById(Id);
        }
        public async Task<Result<List<ProductGetAdminDTO>>> GetProductByCategory(string category) { 
            return await repository.GetProductByCategory(category);
        }

        public async Task<Result<List<ProductGetDTO>>> GetPaginatedProduct(int pageNumber, int pageSize) { 
            return await repository.GetPaginatedProduct(pageNumber, pageSize);
        }

        public async Task<Result<List<ProductGetDTO>>> SearchProduct(string param) { 
            return await repository.SearchProduct(param);
        }

        public async Task<Result<object>> UpdateProduct(Guid productid,AddProductDTO editdata) {
            try
            {
                var product = await repository.GetProductById(productid);
                if (product.Data == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "Product not found" };
                }
                var categorycheck = await repository.GetCategoryByIdAsync(editdata.CategoryId);
                if (categorycheck.Id == null)
                {
                    return new Result<object> { StatusCode = 400, Message = "Category not exist" };
                }

                

                product.Data.Title = editdata.Title;
                product.Data.Color = editdata.Color;
                product.Data.Description = editdata.Description;
                product.Data.CategoryId = categorycheck.Id;
                product.Data.Color=editdata.Color;
                product.Data.Brand = editdata.Brand;
                product.Data.Discount = editdata.Discount;
                product.Data.Price = editdata.Price;
                
                product.Data.Quantity = editdata.Quantity;
                if (editdata.Image != null && editdata.Image.Length > 0)
                {
                    using var stream = editdata.Image.OpenReadStream();
                    var imgURL = await cloudinary.UploadImageAsync(stream, editdata.Image.FileName);
                    product.Data.Image = imgURL;
                }
                await repository.SaveChangesAsync();

                return new Result<object> {StatusCode=200,Message = "Product updated successfully"};


            } 
            catch (Exception ex) {
                return new Result<object> { StatusCode=500, Error = ex.Message };
            }
        }

        public async Task<Result<object>> DeleteProduct(Guid productid)
        {
            return await repository.DeleteProduct(productid);
        }

        public async Task<Result<List<ProductGetDTO>>> GetRelatedProducts(Guid id,string category) {
            try
            {
                if (category == null || category == "")
                {
                    return new Result<List<ProductGetDTO>> { StatusCode = 400, Message = "No related product found" };
                }
                var data= await repository.GetRelatedProducts(id,category);
                var result=mapper.Map<List<ProductGetDTO>>(data);
                return new Result<List<ProductGetDTO>> { StatusCode = 200, Message = "Related products retrieved successfully",Data= result };
            }
            catch (Exception ex) {

                return new Result<List<ProductGetDTO>> { StatusCode=500,Message=ex.Message}; }
        }

        public async Task<Result<List<ProductGetDTO>>> GetProductByBrand(string brand) {

            try
            {
                var data = await repository.GetProductByBrand(brand);
                if (data.Count < 1)
                {
                    return new Result<List<ProductGetDTO>> { StatusCode = 404, Message = "No products found in this brand" };
                }
                var result = mapper.Map<List<ProductGetDTO>>(data);
                return new Result<List<ProductGetDTO>> { StatusCode = 200, Message = "product by brand success", Data = result };
            }
            catch (Exception ex)
            {
                return new Result<List<ProductGetDTO>> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<object>> AddCategory(AddCategoryDTO category)
        {
            try {
                var data=mapper.Map<Category>(category);
                var response=await repository.AddCategory(data);
                    return new Result<object> { StatusCode = 200, Message = "Category added" };

            } 
            catch (Exception ex) { 
                return new Result<object> { StatusCode=500, Message = ex.Message };
            }
        }

        public async Task<Result<List<GetCategoryDTO>>> GetCategory()
        {
            try { 
                var response=await repository.GetCategory();
                var data=mapper.Map<List<GetCategoryDTO>>(response);
                return new Result<List<GetCategoryDTO>> { StatusCode = 200,Message="Category retrieved successfully", Data = data };
            
            } catch (Exception ex) { return new Result<List<GetCategoryDTO>> { StatusCode = 500, Message = ex.Message }; }
        }

    }
}
