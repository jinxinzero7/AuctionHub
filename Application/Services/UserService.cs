using Application.DTOs;
using Domain.Interfaces.DTOInterfaces;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.PassHasher;
using Domain.Interfaces.User;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHasher;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IPasswordHashService passwordHasher, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<IUserResponse> RegisterAsync(IRegisterUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            {
                throw new InvalidOperationException($"User with email '{request.Email}' already exists.");
            }
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Username,
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            // Сохранение пользователя в базе данных
            await _userRepository.CreateUserAsync(newUser);

            var userResponse = new UserResponse
            {
                Id = newUser.Id,
                Email = newUser.Email,
                Username = newUser.Username
            };

            return userResponse;

        }
        public async Task<string> LoginAsync(ILoginUserRequest request)
        {
            //проверка пустого req
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            //проверка существования user с email из request
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with email '{request.Email}' doesn't exist.");
            }

            if (_passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.Password))
            {
                return _jwtService.GenerateToken(existingUser);
            }
            else
            {
                throw new InvalidOperationException("Wrong password.");
            }
        }

        public async Task<IUserResponse> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
        }

        public async Task<IUserResponse> UpdateUserAsync(Guid userId, IUpdateUserRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (request.Username != null)
                user.Username = request.Username;
            if (request.Email != null)
                user.Email = request.Email;
            if (request.Password != null)
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.UpdateUserAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
        }

        public async Task<IEnumerable<IUserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            }).ToList();
        }
    }
}

