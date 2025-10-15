using Application.DTOs;
using Application.Mappings;
using Domain.Interfaces;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.PassHasher;
using Domain.Models;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Validators;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Application.Services
{
    public class UserService : IUserService
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IValidator<RegisterUserRequest> _registerValidator;
        private readonly IValidator<LoginUserRequest> _loginValidator;
        private readonly IValidator<UpdateUserRequest> _updateValidator;

        public UserService(IUserRepository userRepository,
            IPasswordHashService passwordHasher,
            IJwtService jwtService,
            IValidator<RegisterUserRequest> registerValidator,
            IValidator<LoginUserRequest> loginValidator,
            IValidator<UpdateUserRequest> updateValidator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result<UserResponse>> RegisterAsync(RegisterUserRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<UserResponse>($"Validation failed: {errors}");
            }

            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            {
                return Result.Failure<UserResponse>($"User with email '{request.Email}' already exists.");
            }

            // Mapping с помощью UserMappings
            var newUser = UserMappings.ToUser(request);

            // Хеширование пароля
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            // Сохранение пользователя в базе данных
            await _userRepository.CreateUserAsync(newUser);

            return Result.Success<UserResponse>(newUser.ToUserResponse());

        }
        public async Task<Result<string>> LoginAsync(LoginUserRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<string>($"Validation failed: {errors}");
            }

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

        public async Task<Result<UserResponse>> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure<UserResponse>("User not found");
            }
            
            return Result.Success<UserResponse>(user.ToUserResponse());
        }

        public async Task<Result<UserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<UserResponse>($"Validation failed: {errors}");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure<UserResponse>("User not found");
            }

            // Проверка занятости email, если он меняется
            if (request.Email != null && request.Email != user.Email)
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                    return Result.Failure<UserResponse>($"Email '{request.Email}' is already taken");
            }
            if (request.Username != null)
                user.Username = request.Username;
            if (request.Email != null)
                user.Email = request.Email;
            if (request.Password != null)
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.UpdateUserAsync(user);

            return Result.Success<UserResponse>(user.ToUserResponse());
        }

        public async Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Result.Success<IEnumerable<UserResponse>>(users.ToUserResponse());
        }
    }
}

