using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.DTOs
{
    public class UpdateEmployeeDto
    {
        // public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        // public string DocNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public List<PhoneNumberDto> Phones { get; set; } = new List<PhoneNumberDto>();

        [MaxLength(200, ErrorMessage = "Manager name cannot exceed 200 characters.")]
        public string ManagerName { get; set; }
    }
}