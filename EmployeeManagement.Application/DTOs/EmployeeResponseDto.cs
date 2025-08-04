using System;
using System.Collections.Generic;

namespace EmployeeManagement.Application.DTOs
{
    public class EmployeeResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<PhoneNumberDto> Phones { get; set; }
        public string ManagerName { get; set; }
    }
}