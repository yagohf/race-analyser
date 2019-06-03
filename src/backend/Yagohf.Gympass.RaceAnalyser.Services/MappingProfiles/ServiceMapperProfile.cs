﻿using AutoMapper;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Services.MappingProfiles
{
    public class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile() : this("ServiceMapperProfile")
        {
        }

        protected ServiceMapperProfile(string profileName) : base(profileName)
        {
            this.MapearDTOsParaEntidades();
            this.MapearEntidadesParaDTOs();
        }

        private void MapearEntidadesParaDTOs()
        {
            CreateMap<User, UserDTO>();
        }

        private void MapearDTOsParaEntidades()
        {
            CreateMap<RegistrationDTO, User>();
        }
    }
}
