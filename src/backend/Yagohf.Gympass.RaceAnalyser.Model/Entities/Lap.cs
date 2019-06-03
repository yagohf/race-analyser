using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class Lap : EntityBase
    {
        public DateTime Date { get; set; }
        public string Pilot { get; set; }
        public int Number { get; set; }
        public TimeSpan Time { get; set; }
        public decimal AverageSpeed { get; set; }
    }
}
