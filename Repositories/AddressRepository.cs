using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shoezy.Data;
using Shoezy.DTOs;
using Shoezy.Models;

namespace Shoezy.Repositories
    
{
    public interface IAddressRepository {
        Task<Result<object>> AddAddress(int userId, AddressCreateDTO newAddress);
        Task<Result<List<GetAddressDTO>>> GetAddress(int userId);

        Task<bool> RemoveAddress(Guid addressid);
            }
    public class AddressRepository:IAddressRepository
    {
        private readonly ShoezyDbContext context;
        private readonly IMapper mapper;
        public AddressRepository(ShoezyDbContext _context,IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }
        public async Task<Result<object>> AddAddress(int userId, AddressCreateDTO newAddress)
        {
            try
            {
                if (userId == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "userId is not valid" };
        
                }
                if (newAddress == null)
                {
                    return new Result<object> { StatusCode = 400, Message = "Address cannot be null" };
                }
                var userAddress = await context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();
                if (userAddress.Count > 5)
                {
                    return new Result<object> { StatusCode = 400, Message = "Maximum limit of Address reached" };
                }

                var address = mapper.Map<Address>(newAddress);

                address.UserId= userId;
                await context.Addresses.AddAsync(address);
                await context.SaveChangesAsync();
                 return new Result<object> { StatusCode = 200, Message = "Address added" };

            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<List<GetAddressDTO>>> GetAddress(int userId)
        {
            try
            {
                var addresses = await context.Addresses
                    .Where(a => a.UserId == userId).ToListAsync();
                var mappedaddress = mapper.Map<List<GetAddressDTO>>(addresses);


                if (addresses.Count == 0)
                {
                    return new Result<List<GetAddressDTO>> { StatusCode = 404, Message = "No addresses found" };
                }

                return new Result<List<GetAddressDTO>> { StatusCode = 200, Message = "Addresses retrieved", Data = mappedaddress };
            }
            catch (Exception ex)
            {
                return new Result<List<GetAddressDTO>> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<bool> RemoveAddress(Guid addressid)
        {
            var data=await context.Addresses.FirstOrDefaultAsync(a=>a.AddressId== addressid);
            context.Addresses.Remove(data);
            context.SaveChanges();
            return true;
        }
    }
}
