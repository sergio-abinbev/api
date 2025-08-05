using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Entities;
using AutoMapper;
using System.Collections.Generic;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockMapper = new Mock<IMapper>();

        _employeeService = new EmployeeService(
            _mockEmployeeRepository.Object,
            _mockPasswordHasher.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task CreateEmployeeAsync_ShouldCreateEmployee_WhenDataIsValid()
    {
        var creatorRole = Role.Director;
        var createDto = new CreateEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            DocNumber = "12345678900",
            DateOfBirth = DateTime.Now.AddYears(-25),
            Password = "password123",
            Role = Role.Employee,
            Phones = new List<PhoneNumberDto> { new PhoneNumberDto { Number = "11987654321", Type = "Mobile" } }
        };

        _mockEmployeeRepository.Setup(r => r.DocNumberExistsAsync(It.IsAny<string>()))
                               .ReturnsAsync(false);
        _mockEmployeeRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                               .ReturnsAsync(false);
        _mockPasswordHasher.Setup(h => h.HashPassword(It.IsAny<string>()))
                           .Returns("hashed_password");
        _mockMapper.Setup(m => m.Map<EmployeeResponseDto>(It.IsAny<Employee>()))
                   .Returns(new EmployeeResponseDto());

        var result = await _employeeService.CreateEmployeeAsync(createDto, creatorRole);

        Assert.NotNull(result);
        _mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
        _mockEmployeeRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ShouldThrowException_WhenDocNumberAlreadyExists()
    {

        var creatorRole = Role.Director;
        var createDto = new CreateEmployeeDto
        {
            DocNumber = "12345678900",
            Role = Role.Employee
        };

        _mockEmployeeRepository.Setup(r => r.DocNumberExistsAsync(It.IsAny<string>()))
                               .ReturnsAsync(true);


        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _employeeService.CreateEmployeeAsync(createDto, creatorRole));

        Assert.Equal($"Employee with document number '{createDto.DocNumber}' already exists.", exception.Message);
        _mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
        _mockEmployeeRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {

        var creatorRole = Role.Director;
        var createDto = new CreateEmployeeDto
        {
            Email = "existing.email@test.com",
            FirstName = "Jane",
            LastName = "Doe",
            DocNumber = "11122233344",
            DateOfBirth = DateTime.Now.AddYears(-25),
            Password = "password123",
            Role = Role.Employee
        };

        _mockEmployeeRepository.Setup(r => r.DocNumberExistsAsync(It.IsAny<string>()))
                           .ReturnsAsync(false);
        _mockEmployeeRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                               .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _employeeService.CreateEmployeeAsync(createDto, creatorRole));

        Assert.Equal($"Employee with email '{createDto.Email}' already exists.", exception.Message);
        _mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
        _mockEmployeeRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ShouldThrowException_WhenEmployeeIsUnder18()
    {
        var creatorRole = Role.Director;
        var createDto = new CreateEmployeeDto
        {
            FirstName = "Junior",
            LastName = "Dev",
            Email = "junior.dev@test.com",
            DocNumber = "99988877766",
            DateOfBirth = DateTime.Now.AddYears(-17),
            Password = "password123",
            Role = Role.Employee
        };

        _mockEmployeeRepository.Setup(r => r.DocNumberExistsAsync(It.IsAny<string>()))
                               .ReturnsAsync(false);
        _mockEmployeeRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                               .ReturnsAsync(false);
        _mockPasswordHasher.Setup(h => h.HashPassword(It.IsAny<string>()))
            .Returns("mocked_hashed_password");

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _employeeService.CreateEmployeeAsync(createDto, creatorRole));

        Assert.Equal("Employee must be at least 18 years old.", exception.Message);
        _mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
        _mockEmployeeRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ShouldThrowException_WhenCreatorRoleIsInsufficient()
    {

        var creatorRole = Role.Employee;
        var createDto = new CreateEmployeeDto
        {
            DocNumber = "12345678900",
            Role = Role.Director
        };

        _mockEmployeeRepository.Setup(r => r.DocNumberExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _mockEmployeeRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _employeeService.CreateEmployeeAsync(createDto, creatorRole));

        Assert.Equal("You cannot create a user with higher permissions than your own.", exception.Message);
        _mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
        _mockEmployeeRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}