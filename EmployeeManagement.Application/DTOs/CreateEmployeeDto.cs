using System.ComponentModel.DataAnnotations; // Para validações

namespace EmployeeManagement.Application.DTOs
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Document number is required.")]
        [MinLength(5, ErrorMessage = "Document number must be at least 5 characters.")] // Ajuste conforme o formato do documento (CPF, RG, etc.)
        public string DocNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } // Não é o hash, é a senha em texto puro que será hashizada

        public List<PhoneNumberDto> Phones { get; set; } = new List<PhoneNumberDto>();

        [MaxLength(200, ErrorMessage = "Manager name cannot exceed 200 characters.")]
        public string ManagerName { get; set; }
    }
}