using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.ValueObjects;

namespace EmployeeManagement.Application.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones.Select(p => new PhoneNumberDto { Number = p.Number, Type = p.Type }).ToList()));

            CreateMap<PhoneNumberDto, PhoneNumber>()
                .ConstructUsing(src => new PhoneNumber(src.Number, src.Type));

            // Note: O mapeamento de CreateEmployeeDto/UpdateEmployeeDto para Employee
            // não é direto aqui porque a entidade Employee tem um construtor que valida
            // e é preferível criar a entidade via esse construtor para garantir invariantes.
            // O serviço de aplicação cuida dessa criação/atualização.
        }
    }
}