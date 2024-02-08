using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Context;
using API.Infrastructure.Models;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ApiContext _context;

        public UserService(ApiContext apiContext)
        {
            _context = apiContext;
        }

        public async Task<List<UserDTO>> GetAll()
        {
            return await _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Votes = u.Votes.Select(v => new VoteDTO
                    {
                        Id = v.Id,
                        UserId = v.UserId,
                    })
                })
                .ToListAsync();
        }

        public async Task<UserDTO> Create(UserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return userDTO;
        }

        public async Task<UserDTO> Update(UserDTO userDTO, int id)
        {
            User userById = await FindId(id);

            if (userById == null)
            {
                throw new Exception("User not found");
            }

            userById.Name = userDTO.Name;

            _context.Users.Update(userById);
            await _context.SaveChangesAsync();

            var updatedUserDTO = new UserDTO
            {
                Id = userById.Id,
                Name = userById.Name,
            };

            return updatedUserDTO;
        }

        public async Task<bool> Delete(int id)
        {
            User userById = await FindId(id);

            if (userById == null)
            {
                throw new Exception("User not found");
            }

            _context.Users.Remove(userById);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> FindId(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
