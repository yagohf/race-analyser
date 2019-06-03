using System;
using System.Collections.Generic;
using System.Text;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO
{
    public class DriverResultDTO
    {
        public int Position { get; set; }
        public int DriverCode { get; set; }
        public string DriverName { get; set; }
        public int Laps { get; set; }
        public TimeSpan TotalRaceTime { get; set; }
        public TimeSpan BestLap { get; set; }
        public decimal AverageSpeed { get; set; }
        public TimeSpan? Gap { get; set; }
    }
}