using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class Lap : EntityBase
    {
        public DateTime Date { get; set; }
        public int DriverNumber { get; set; }
        public string DriverName { get; set; }
        public int Number { get; set; }
        public TimeSpan Time { get; set; }
        public decimal AverageSpeed { get; set; }
    }
}
