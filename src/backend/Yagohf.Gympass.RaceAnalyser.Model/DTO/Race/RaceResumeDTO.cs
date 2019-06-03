using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO.Race
{
    public class RaceResumeDTO
    {
        public int RaceId { get; set; }
        public string RaceName { get; set; }
        public DateTime RaceDate { get; set; }
        public string Winner { get; set; }
    }
}
