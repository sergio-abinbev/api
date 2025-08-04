using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);

        Task<Employee> GetByIdAsync(Guid id);

        Task<Employee> GetByDocNumberAsync(string docNumber);

        Task<IEnumerable<Employee>> GetAllAsync();

        void Update(Employee employee);

        Task DeleteAsync(Guid id);

        Task<bool> DocNumberExistsAsync(string docNumber);

        Task<bool> EmailExistsAsync(string email);

        Task<int> SaveChangesAsync();
    }
}