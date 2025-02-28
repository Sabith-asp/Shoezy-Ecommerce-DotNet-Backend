using AutoMapper;
using Shoezy.DTOs;
using Shoezy.Models;
using Shoezy.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shoezy.Services
{
    public interface IUserService {

        Task<Result<List<GetUserDTO>>> GetUsers();
        Task<Result<GetUserDTO>> GetUserById(int id);
        Task<Result<object>> BlockOrUnblockUser(int id);
        Task<Result<GetUserDTO>> GetUser(int userid);
    }
    public class UserService: IUserService
    {
        private readonly UserRepository userrepo;
        private readonly IMapper mapper;
        public UserService(UserRepository _userrepo,IMapper _mapper) {
            userrepo= _userrepo;
            mapper = _mapper;
        }

        public async Task<Result<GetUserDTO>> GetUser(int userid)
        {
            try {
                var response= await userrepo.GetUser(userid);
                if (response == null) {
                    return new Result<GetUserDTO> { StatusCode=404,Message="User not found"};
                }
                var result=mapper.Map<GetUserDTO>(response);
                return new Result<GetUserDTO> {StatusCode=200,Message="User retrived successfully",Data=result };
            
            }
            catch (Exception ex) {

                return new Result<GetUserDTO> { StatusCode = 500, Message = ex.Message };
            }
        }
        public async Task<Result<List<GetUserDTO>>> GetUsers() {
            try {
                var data=await userrepo.GetUsers();
                if (data== null || !data.Any()) { 
                    return new Result<List<GetUserDTO>> {StatusCode=404,Message="No users found" };
                }
                var mappeddata = mapper.Map<List<GetUserDTO>>(data);
                return new Result<List<GetUserDTO>> { StatusCode = 200 ,Message="Users retrieved successfully",Data=mappeddata};
            } catch (Exception ex) { 
                return new Result<List<GetUserDTO>> {StatusCode= 500,Message=ex.Message};
            }
        }

        public async Task<Result<GetUserDTO>> GetUserById(int id) {
            try { 
                var data= await userrepo.GetUserById(id);
                if (data == null)
                {
                    return new Result<GetUserDTO> { StatusCode = 404, Message = "No user found" };
                }
                var mappeddata = mapper.Map<GetUserDTO>(data);
                return new Result<GetUserDTO> { StatusCode = 200, Message = "User retrieved successfully", Data = mappeddata };
            } catch (Exception ex) {
                return new Result<GetUserDTO> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Result<object>> BlockOrUnblockUser(int id)
        {
            try
            {
                var user = await userrepo.GetUserById(id);
                if (user == null)
                {
                    return new Result<object> { StatusCode = 404, Message = "User not found" };
                }
                bool success = await userrepo.BlockOrUnblockUser(id);
                if (!success)
                {
                    return new Result<object> { StatusCode = 500, Message = "Failed to update user status" };
                }
                return new Result<object> { StatusCode = 200, Message = "User status changed successfully" };
            }
            catch (Exception ex)
            {
                return new Result<object> { StatusCode = 500, Message = ex.Message };
            }
        }

    }
}
