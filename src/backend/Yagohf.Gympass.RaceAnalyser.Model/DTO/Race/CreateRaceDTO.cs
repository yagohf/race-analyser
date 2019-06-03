using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
