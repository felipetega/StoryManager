using API.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<bool> Create(UserDTO userDTO);
        Task<bool> Update(UserDTO userDTO, int id);
        Task<bool> Delete(int id);
    }
}
