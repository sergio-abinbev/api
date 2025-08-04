using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Data;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {

            return await _context.Employees
                                 .Include(e => e.Phones)
                                 .Where(e => e.IsActive)
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> GetByDocNumberAsync(string docNumber)
        {
            return await _context.Employees
                                 .Include(e => e.Phones)
                                 .Where(e => e.IsActive)
                                 .FirstOrDefaultAsync(e => e.DocNumber == docNumber);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                                 .Include(e => e.Phones)
                                 .Where(e => e.IsActive)
                                 .ToListAsync();
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employeeToDelete = await _context.Employees.FindAsync(id);
            if (employeeToDelete != null)
            {
                employeeToDelete.Deactivate();
            }
        }

        public async Task<bool> DocNumberExistsAsync(string docNumber)
        {
            return await _context.Employees.AnyAsync(e => e.DocNumber == docNumber);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Employees.AnyAsync(e => e.Email == email);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}