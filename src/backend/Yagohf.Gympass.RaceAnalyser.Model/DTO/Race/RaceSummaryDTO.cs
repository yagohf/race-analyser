using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO.Race
{
    public class RaceSummaryDTO
    {
        public int RaceId { get; set; }
        public string RaceDescription { get; set; }
        public DateTime RaceDate { get; set; }
        public int RaceTypeId { get; set; }
        public string RaceTypeDescription { get; set; }
        public string Uploader { get; set; }
        public string Winner { get; set; }
    }
}
