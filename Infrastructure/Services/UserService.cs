using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.DTOInterfaces;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.PassHasher;
using Domain.Models;
using Domain.Results;
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

        public UserService(IUserRepository userRepository, IPasswordHashService passwordHasher, 
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<Result<IUserResponse>> RegisterAsync(IRegisterUserRequest request)
        {
            if (request == null)
                return Result.Failure<IUserResponse>("Request cannot be null");

            if (string.IsNullOrWhiteSpace(request.Username))
                return Result.Failure<IUserResponse>("Username is required");
                
            if (string.IsNullOrWhiteSpace(request.Email))
                return Result.Failure<IUserResponse>("Email is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result.Failure<IUserResponse>("Password is required");

            if (request.Password.Length < 6)
                return Result.Failure<IUserResponse>("Password must be at least 6 characters");

            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
                {
                    return Result.Failure<IUserResponse>($"User with email '{request.Email}' already exists.");
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

            return Result.Success<IUserResponse>(userResponse);

        }
        public async Task<Result<string>> LoginAsync(ILoginUserRequest request)
        {
            //проверка пустого req
            if (request == null)
                return Result.Failure<string>("Request cannot be null");
                
            if (string.IsNullOrWhiteSpace(request.Email))
                return Result.Failure<string>("Invalid email or password");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result.Failure<string>("Invalid email or password");

            //проверка существования user с email из request
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return Result.Failure<string>("Invalid email or password");
            }

            if (_passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.Password))
            {
                string token = _jwtService.GenerateToken(existingUser);
                return Result.Success(token);
            }
            else
            {
                return Result.Failure<string>("Invalid email or password");
            }
        }

        public async Task<Result<IUserResponse>> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure<IUserResponse>("User not found");
            }
            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
            return Result.Success<IUserResponse>(userResponse);
        }

        public async Task<Result<IUserResponse>> UpdateUserAsync(Guid userId, IUpdateUserRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure<IUserResponse>("User not found");
            }

            // Проверка занятости email, если он меняется
            if (request.Email != null && request.Email != user.Email)
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                    return Result.Failure<IUserResponse>($"Email '{request.Email}' is already taken");
            }

            if (request.Username != null)
                user.Username = request.Username;
            if (request.Email != null)
                user.Email = request.Email;
            if (request.Password != null)
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.UpdateUserAsync(user);

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
            return Result.Success<IUserResponse>(userResponse);
        }

        public async Task<Result<IEnumerable<IUserResponse>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var usersResponse = users.Select(user => new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            }).ToList();

            return Result.Success<IEnumerable<IUserResponse>>(usersResponse);
        }
    }
}

