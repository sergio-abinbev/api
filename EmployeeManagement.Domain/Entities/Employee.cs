using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Domain.ValueObjects;

namespace EmployeeManagement.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; private set; }
        public bool IsActive { get; private set; } = true;
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string DocNumber { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string PasswordHash { get; private set; }
        public string ManagerName { get; private set; }
        private readonly List<PhoneNumber> _phones;
        public IReadOnlyCollection<PhoneNumber> Phones => _phones.AsReadOnly();

        public Employee(
            string firstName,
            string lastName,
            string email,
            string docNumber,
            DateTime dateOfBirth,
            string passwordHash,
            string managerName = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(docNumber))
                throw new ArgumentException("Document number is required.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required.");

            if (CalculateAge(dateOfBirth) < 18)
                throw new ArgumentException("Employee must be at least 18 years old.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DocNumber = docNumber;
            DateOfBirth = dateOfBirth;
            PasswordHash = passwordHash;
            ManagerName = managerName;
            _phones = [];
        }

        public void AddPhone(PhoneNumber phoneNumber)
        {
            ArgumentNullException.ThrowIfNull(phoneNumber);

            if (_phones.Any(p => p == phoneNumber))
                throw new InvalidOperationException("This phone number already exists for the employee.");

            _phones.Add(phoneNumber);
        }

        public void RemovePhone(PhoneNumber phoneNumber)
        {
            ArgumentNullException.ThrowIfNull(phoneNumber);

            var phoneToRemove = _phones.FirstOrDefault(p => p == phoneNumber);
            if (phoneToRemove is not null)
            {
                _phones.Remove(phoneToRemove);
            }
            else
            {
                throw new InvalidOperationException("Phone number not found for this employee.");
            }
        }

        public void UpdateDetails(string firstName, string lastName, string email, string managerName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ManagerName = managerName;
        }

        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Employee is already deactivated.");

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Employee is already active.");

            IsActive = true;
        }
    }
}