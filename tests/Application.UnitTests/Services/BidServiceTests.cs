using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Domain.Interfaces;
using Application.Services;
using Application.DTOs;
using FluentValidation;

namespace Application.UnitTests.Services
{
    public class BidServiceTests
    {
        private readonly Mock<IBidRepository> _bidRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILotRepository> _lotRepositoryMock;
        private readonly Mock<IValidator<BidCreateRequest>> _bidCreateValidator;
        private readonly BidService _bidService;

        public BidServiceTests()
        {
            _bidRepositoryMock = new Mock<IBidRepository>();
            _lotRepositoryMock = new Mock<ILotRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _bidCreateValidator = new Mock<IValidator<BidCreateRequest>>();

            _bidService = new BidService(
                _bidRepositoryMock.Object,
                _currentUserServiceMock.Object,
                _userRepositoryMock.Object,
                _lotRepositoryMock.Object,
                _bidCreateValidator.Object
            );
        }

        [Fact]
        public async Task PlaceBidAsync_WithValidData_ShouldCreateBid()
        {
            
        }
    }
}