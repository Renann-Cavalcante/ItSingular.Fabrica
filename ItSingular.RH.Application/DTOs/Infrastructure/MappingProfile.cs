using AutoMapper;
using ItSingular.RH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItSingular.RH.Application.DTOs.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Profissionais
            CreateMap<Profissionais, ProfissionaisDTO>();
            CreateMap<ProfissionaisDTO, Profissionais>();

        }
    }
}
