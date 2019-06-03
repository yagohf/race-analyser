using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;

namespace Yagohf.Gympass.RaceAnalyser.Services.Domain
{
    public class RaceService : IRaceService
    {
        public Task<RaceResultDTO> AnalyseAsync(CreateRaceDTO createData, string uploader)
        {
            throw new System.NotImplementedException();
        }

        public Task<RaceResultDTO> GetResultByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Listing<RaceSummaryDTO>> ListSummaryAsync(string description, string uploader, int? page)
        {
            throw new System.NotImplementedException();
        }
    }
}
