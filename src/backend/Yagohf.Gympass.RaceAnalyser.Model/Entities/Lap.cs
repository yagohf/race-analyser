using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class Lap : EntityBase
    {
        public int RaceId { get; set; }
        public DateTime Date { get; set; }
        public int DriverNumber { get; set; }
        public string DriverName { get; set; }
        public int Number { get; set; }
        public long TimeTicks { get; set; }
        public decimal AverageSpeed { get; set; }

        public TimeSpan Time
        {
            get
            {
                return new TimeSpan(TimeTicks);
            }
            set
            {
                TimeTicks = value.Ticks;
            }
        }

        //Relacionamentos.
        public Race Race { get; set; }
    }
}
