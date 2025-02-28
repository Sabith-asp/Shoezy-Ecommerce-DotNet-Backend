using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Repositories;

namespace Shoezy.Services
{
    public interface IAddressService {
        Task<Result<object>> AddAddress(int userId, AddressCreateDTO newAddress);
        Task<Result<List<GetAddressDTO>>> GetAddress(int userId);

        Task<Result<object>> RemoveAddress(int userId, Guid addressid);
    }
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressrepository;
        public AddressService(IAddressRepository adressrepo) {
            addressrepository= adressrepo;
        }
        public async Task<Result<object>> AddAddress(int userId, AddressCreateDTO newAddress)
        { 
            return await addressrepository.AddAddress(userId, newAddress);
        }

        public async Task<Result<List<GetAddressDTO>>> GetAddress(int userId) { 
            return await addressrepository.GetAddress(userId);
        }

        public async Task<Result<object>> RemoveAddress(int userid,Guid addressid) {
            try { 
            
                var address= addressrepository.GetAddress(userid).Result.Data.FirstOrDefault(a=>a.AddressId== addressid);
                if (address == null) { 
                    return new Result<object> { StatusCode=404,Message="Address not exist"};
                }
                await addressrepository.RemoveAddress(addressid);
                return new Result<object> { StatusCode=200,Message="Address removed"};
                
            }
            catch (Exception ex) {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }
    }
    
}
