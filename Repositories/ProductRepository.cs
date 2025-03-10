﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shoezy.Repositories
{
    public interface IProductRepository {
        Task<Result<object>> AddProduct(Product newproduct);
        Task<Result<List<ProductGetDTO>>>  GetAllProduct ();
        Task<Result<List<ProductGetAdminDTO>>> GetAllProductByAdmin();

        Task<Result<Product>> GetProductById(Guid Id);
        Task<Result<ProductGetDTO>> GetUserProductById(Guid Id);

        Task<Result<List<ProductGetAdminDTO>>> GetProductByCategory(string category);
        Task<List<Product>> GetProductByBrand(string brand);
        Task<Result<List<ProductGetDTO>>> GetPaginatedProduct(int pageNumber,int pageSize);

        Task<Result<List<ProductGetDTO>>> SearchProduct(string param);
        Task<List<Product>> GetRelatedProducts(Guid id,string category);
        Task<Category?> GetCategoryByIdAsync(Guid categoryId);

        Task SaveChangesAsync();

        Task<Result<object>> DeleteProduct(Guid productid);

        Task<bool> AddCategory(Category category);

        Task<List<Category>> GetCategory();
    }
    public class ProductRepository:IProductRepository
    {
        private readonly ShoezyDbContext context;
        private readonly IMapper mapper;

        public ProductRepository(ShoezyDbContext _context,IMapper _mapper) { 
            context = _context;
            mapper = _mapper;
        }
        public async Task<Result<object>> AddProduct(Product newproduct) {
            try {
                await context.Products.AddAsync(newproduct);
                await context.SaveChangesAsync();
                return new Result<object> { StatusCode = 201, Message = "Product Added Successfully", Data = newproduct };
            }
            catch (Exception ex) {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<List<ProductGetDTO>>> GetAllProduct()
        {
            try {
                var data =await context.Products.Include(p=>p.Category).Where(p=>p.isDeleted==false).ToListAsync();
                var mappeddata = mapper.Map<List<ProductGetDTO>>(data);
                return new Result<List<ProductGetDTO>> { StatusCode=200,Message="Product Loaded Successfully",Data= mappeddata };
            } 
            catch (Exception ex) {
                return new Result<List<ProductGetDTO>> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<List<ProductGetAdminDTO>>> GetAllProductByAdmin()
        {
            try
            {
                var data = await context.Products.Include(p => p.Category).ToListAsync();
                var mappeddata = mapper.Map<List<ProductGetAdminDTO>>(data);
                return new Result<List<ProductGetAdminDTO>> { StatusCode = 200, Message = "Product Loaded Successfully", Data = mappeddata };
            }
            catch (Exception ex)
            {
                return new Result<List<ProductGetAdminDTO>> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<Product>> GetProductById(Guid Id)
        {
            try {
                var response = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);
                if (response == null) {
                    return new Result<Product> { StatusCode = 404, Message = "Product not found" };
                }
                return new Result<Product> { StatusCode = 200, Message = "GetUserByID Successful", Data = response };
            }
            catch (Exception ex) {
                return new Result<Product> { StatusCode = 500, Message = ex.Message };
                    }
        }

        public async Task<Result<ProductGetDTO>> GetUserProductById(Guid Id)
        {
            try
            {
                var response = await context.Products.Include(p=>p.Category).FirstOrDefaultAsync(x => x.Id == Id);
                if (response == null)
                {
                    return new Result<ProductGetDTO> { StatusCode = 404, Message = "Product not found" };
                }
                var result=mapper.Map<ProductGetDTO>(response);
                return new Result<ProductGetDTO> { StatusCode = 200, Message = "GetUserProductByID Successful", Data = result };
            }
            catch (Exception ex)
            {
                return new Result<ProductGetDTO> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<List<ProductGetAdminDTO>>> GetProductByCategory(string category)
        {
            try {
                var data = await context.Products.Include(x => x.Category).Where(x => x.Category.Name.ToLower() == category.ToLower() &&  x.isDeleted == false).ToListAsync();
                if (data.Count < 1) {
                    return new Result<List<ProductGetAdminDTO>> { StatusCode = 404, Message = "No products found in this category" };
                        }
                var result=mapper.Map<List<ProductGetAdminDTO>>(data);
                return new Result<List<ProductGetAdminDTO>> { StatusCode = 200, Message = "ProductByCategory success",Data= result };
            } catch (Exception ex) {
                return new Result<List<ProductGetAdminDTO>> { StatusCode = 500, Message = ex.Message };
            }
        }
        public async Task<List<Product>> GetProductByBrand(string brand)
        {
            return await context.Products.Include(x => x.Category).Where(x => x.Brand.ToLower() == brand.ToLower() && x.isDeleted == false).ToListAsync();
            //try
            //{
            //    var data = await context.Products.Include(x => x.Category).Where(x => x.Brand== brand).ToListAsync();
            //    if (data.Count < 1)
            //    {
            //        return new Result<List<ProductGetDTO>> { StatusCode = 404, Message = "No products found in this brand" };
            //    }
            //    var result = mapper.Map<List<ProductGetDTO>>(data);
            //    return new Result<List<ProductGetDTO>> { StatusCode = 200, Message = "product by brand success", Data = result };
            //}
            //catch (Exception ex)
            //{
            //    return new Result<List<ProductGetDTO>> { StatusCode = 500, Message = ex.Message };
            //}
        }

        public async Task<Result<List<ProductGetDTO>>> GetPaginatedProduct(int pageNumber, int pageSize)
        {
            try
            {
                var products = await context.Products.Include(p=>p.Category).Where(p => p.isDeleted == false)
            .Skip((pageNumber - 1) * pageSize) 
            .Take(pageSize)                     
            .ToListAsync();

                if (products == null || !products.Any())
                {
                    return new Result<List<ProductGetDTO>> {StatusCode=404,Message="No products found" };
                };
                var result = mapper.Map<List<ProductGetDTO>>(products);
                return new Result<List<ProductGetDTO>> {StatusCode = 200,Message="paginated successfully",Data= result };
            }
            catch (Exception ex) {
                return new Result<List<ProductGetDTO>> { StatusCode=500,Message=ex.Message};
            }
        }

        public async Task<Result<List<ProductGetDTO>>> SearchProduct(string param) {
            try { 
                var data = context.Products.Include(p=>p.Category).Where(x => x.Title.Contains(param) && x.isDeleted == false).ToList();
                if (data == null || data.Count<1) {
                    return new Result<List<ProductGetDTO>> { StatusCode = 200, Message = "No products found" };
                }
                var result = mapper.Map<List<ProductGetDTO>>(data);
                return new Result<List<ProductGetDTO>> {StatusCode=200,Message="Searching product success",Data = result};
            } 
            catch (Exception ex) {
                return new Result<List<ProductGetDTO>> { StatusCode = 500, Message = ex.Message };
            }

        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<Result<object>> DeleteProduct(Guid productid) {

            try
            {

                var product = await context.Products.FirstOrDefaultAsync(p=>p.Id==productid);
                if (product == null)
                {
                    return new Result<object> { StatusCode = 400, Message = "Product not found" };
                }
                Console.WriteLine(product.Title);

                //context.Products.Remove(product);
                product.isDeleted = true;
                await context.SaveChangesAsync();
                return new Result<object> { StatusCode = 200, Message = "Product deleted successfully" };

            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
           

        }

        public async Task<List<Product>> GetRelatedProducts(Guid id,string category) {
            return await context.Products.Include(p=>p.Category).Where(x=>x.Category.Name.Contains(category) && x.Id!=id && x.isDeleted == false).ToListAsync();
        }

        //public async Task<Result<ProductGetDTO>> GetUserProductById(Guid Id) { 
        //    return await context.Produ
        //}

        public async Task<bool> AddCategory(Category category) {
           await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> GetCategory() {
            return await context.Categories.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }


    }
}
