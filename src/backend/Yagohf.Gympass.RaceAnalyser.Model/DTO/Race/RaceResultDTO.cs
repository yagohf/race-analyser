using System;
using System.Collections.Generic;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO.Race
{
    public class RaceResultDTO
    {
        public int RaceId { get; set; }
        public string RaceDescription { get; set; }
        public DateTime RaceDate { get; set; }
        public string Winner { get; set; }
        public string Uploader { get; set; }
        public DateTime UploadDate { get; set; }
        public BestLapDTO BestLap { get; set; }
        public IEnumerable<DriverResultDTO> Results { get; set; }
    }
}
