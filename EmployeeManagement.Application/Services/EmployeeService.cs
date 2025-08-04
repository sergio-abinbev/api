using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Domain.ValueObjects;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }



        public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDto createDto)
        {

            if (await _employeeRepository.DocNumberExistsAsync(createDto.DocNumber))
            {
                throw new ArgumentException($"Employee with document number '{createDto.DocNumber}' already exists.");
            }
            if (await _employeeRepository.EmailExistsAsync(createDto.Email))
            {
                throw new ArgumentException($"Employee with email '{createDto.Email}' already exists.");
            }


            var passwordHash = _passwordHasher.HashPassword(createDto.Password);


            var employee = new Employee(
                createDto.FirstName,
                createDto.LastName,
                createDto.Email,
                createDto.DocNumber,
                createDto.DateOfBirth,
                passwordHash,
                createDto.ManagerName
            );


            foreach (var phoneDto in createDto.Phones)
            {
                employee.AddPhone(new PhoneNumber(phoneDto.Number, phoneDto.Type));
            }

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();


            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return null;
            }
            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        }

        public async Task UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException($"Employee with ID '{id}' not found.");

            if (updateDto.Email != employee.Email && await _employeeRepository.EmailExistsAsync(updateDto.Email))
                throw new ArgumentException($"Email '{updateDto.Email}' is already in use by another employee.");

            employee.UpdateDetails(updateDto.FirstName, updateDto.LastName, updateDto.Email, updateDto.ManagerName);

            var existingPhones = employee.Phones.ToList();
            foreach (var phone in existingPhones)
            {
                employee.RemovePhone(phone);
            }

            foreach (var phoneDto in updateDto.Phones)
            {
                employee.AddPhone(new PhoneNumber(phoneDto.Number, phoneDto.Type));
            }

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException($"Employee with ID '{id}' not found.");

            await _employeeRepository.DeleteAsync(id);
            await _employeeRepository.SaveChangesAsync();
        }
    }
}