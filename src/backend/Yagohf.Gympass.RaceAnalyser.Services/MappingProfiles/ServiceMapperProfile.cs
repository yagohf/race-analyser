using AutoMapper;
using Yagohf.Gympass.RaceAnalyser.Model.DTO;
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
            CreateMap<Race, RaceDTO>();
            CreateMap<Lap, LapDTO>();
        }

        private void MapearDTOsParaEntidades()
        {
            CreateMap<RaceDTO, Race>();
            CreateMap<RegistrationDTO, User>();
        }
    }
}
