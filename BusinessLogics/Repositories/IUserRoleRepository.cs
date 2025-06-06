﻿using DataAccesses.DTOs.PaginationModels.GroupChats;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        public List<UserRole> GetUserRolesByUserId(int userId);
        public NumberOfUserByEachRole GetNumberOfUserByEachRole(int groupChatId, int page, int itemsPerPage, string? keyword);
        public Task<GetNumberOfUserByEachRoleDTO> GetNumberOfUserByRole(int groupChatId, int roleId);
        public Task<List<GetUsersByEachRoleDTO>> GetUsersByEachRole(int groupChatId, int roleId);
        public Task<List<GetUsersNotInRoleDTO>> GetUsersNotInRole(int groupChatId, int roleId);
        public Task<List<UserRoleDTO>> GetRolesByUserInGroupChat(int groupChatId, int userId);
        public List<GetAllRolesByUserDTO> GetAllRolesByUser(int userId);
        public Task<List<GetUserDTO>> GetUsersByRole(int roleId);
    }
}
