using AutoMapper;
using System.Linq;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;
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
            this.MapDTOsToEntities();
            this.MapEntitiesToDTOs();
        }

        private void MapEntitiesToDTOs()
        {
            CreateMap<User, UserDTO>();
            CreateMap<DriverResult, DriverResultDTO>();
            CreateMap<Race, RaceSummaryDTO>()
                .ForMember(dto => dto.RaceId, opt => opt.MapFrom(race => race.Id))
                .ForMember(dto => dto.RaceDescription, opt => opt.MapFrom(race => race.Description))
                .ForMember(dto => dto.RaceDate, opt => opt.MapFrom(race => race.Date))
                .ForMember(dto => dto.Winner, opt => opt.MapFrom(race => race.DriverResults.OrderBy(dr => dr.Position).Select(x => $"{x.DriverNumber} - {x.DriverName}")));
        }

        private void MapDTOsToEntities()
        {
            CreateMap<RegistrationDTO, User>();
        }
    }
}
