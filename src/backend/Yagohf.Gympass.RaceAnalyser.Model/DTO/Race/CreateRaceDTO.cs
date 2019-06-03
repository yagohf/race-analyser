using System;
using System.IO;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO.Race
{
    public class CreateRaceDTO
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int RaceTypeId { get; set; }
        public int TotalLaps { get; set; }
        public Stream ResultsFile { get; set; }
    }
}
