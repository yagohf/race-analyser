using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO.Race
{
    public class CreateRaceDTO
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int RaceTypeId { get; set; }
        public int TotalLaps { get; set; }
    }
}
