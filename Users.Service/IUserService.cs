﻿using Common.Domain;
using Users.Service.Dto;

namespace Users.Service
{
    public interface IUserService
    {
        //IReadOnlyCollection<User> GetAllUsers(int? offset, string? labelFreeText, int? limit);
        Task<IReadOnlyCollection<GetUserDto>> GetAllUsersAsync(int? offset, string? labelFreeText, int? limit);
        Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        //User AddUser(CreateUserDto user);
        Task<GetUserDto?> AddUserAsync(CreateUserDto user);
        //User? UpdateUser(UpdateUserDto newUser);
        Task<GetUserDto?> UpdateUserAsync(UpdateUserDto newUser);
        //int Count(string? nameFreeText);
        Task<int> CountAsync(string? nameFreeText);
        //bool RemoveUser(int id);
        Task<bool> RemoveUserAsync(int id);


    }
}
