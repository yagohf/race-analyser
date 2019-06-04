using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO.Race
{
    public class DriverResultDTO
    {
        public int Position { get; set; }
        public int DriverNumber { get; set; }
        public string DriverName { get; set; }
        public int Laps { get; set; }
        public TimeSpan TotalRaceTime { get; set; }
        public TimeSpan BestLap { get; set; }
        public decimal AverageSpeed { get; set; }
        public TimeSpan? Gap { get; set; }
    }
}